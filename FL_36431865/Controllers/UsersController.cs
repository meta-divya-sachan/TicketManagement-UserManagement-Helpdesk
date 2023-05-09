using TicketManagement.Helpers; 
using TicketManagement.Service;  
using Newtonsoft.Json;
using System;
using System.Web.Mvc;

namespace TicketManagement.Controllers
{
    public class UsersController : Controller
    {
        private readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IBaseService _BaseService = new BaseService();
        private readonly IUserService _UserService = new UserService();


        private const string SessionUserRoles = "_USERROLES";
        private const string SessionUserData = "_USERDATA";
        private const string SessionUserName = "_USERNAME";
         
        private class LoginActions
        {
            public const int OnlyUser = 0;
            public const int OnlyPassword = 1;
            public const int PasswordAndClientID = 2;
        }


        // Login
        public ActionResult Login()
        {
            return View();
        }


        // Register
        public ActionResult Register()
        {
            return View();
        }


        // Profile
        public ActionResult Profile()
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                var result = _BaseService.GetUserByCode(user.userCode);
                if (result.IsSuccess)
                {
                    return View(result.Data);
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                TempData["Success"] = "0";
                TempData["Message"] = ex.Message;
                return View();
            }
        }


        // Logout
        public ActionResult Logout()
        {
            HttpContext.Session.Remove(SessionUserRoles);
            HttpContext.Session.Remove(SessionUserData);
            HttpContext.Session.Remove(SessionUserName);
            //string returnUrl = Url.Content("~/Home/Index");
            //return RedirectToAction(returnUrl);
            return RedirectToAction("Login", "Users");
        }

        // POST: CheckLogin/eMail
        [HttpPost]
        public ActionResult CheckLogin(string email, string password, string codigoCliente)
        {
            try
            {
                string xipAcceso = "";
                var xcodigoCliente = Functions.ToInt(codigoCliente);
                var result = _BaseService.GetCredentialsByEmail(email); //, xcodigoCliente);
                if (result.IsSuccess)
                {
                    var user = result.Data;
                    var result1U = _UserService.CheckLogin(user.userCode, password, user.password, xipAcceso);
                    if (result1U.IsSuccess)
                    {
                        //_Logger.LogInformation("Access granted");
                        var usuario = result1U.Data;
                        HttpContext.Session[SessionUserName] = usuario.usuario.userName;

                        var userData = JsonConvert.SerializeObject(usuario.usuario);
                        HttpContext.Session[SessionUserData] = userData;

                        var userRoles = JsonConvert.SerializeObject(usuario.perfiles);
                        HttpContext.Session[SessionUserRoles] = userRoles;
                         
                        TempData["Success"] = "1";
                        TempData["Message"] = "Access granted";

                        return Json(new { error = 0, message = "Access granted" });
                    }
                    else throw new Exception(result1U.Exception.Message);
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: GetCredentialsByEmail/eMail
        [HttpPost]
        public ActionResult GetCredentialsByEmail(string email)
        {
            try
            {
                var result = _BaseService.GetCredentialsByEmail(email);
                if (result.IsSuccess)
                {
                    return Json(new { error = 0, message = "User alias found (1)", output = new { action = LoginActions.OnlyPassword } });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: CreateAccount/eMail
        [HttpPost]
        public ActionResult CreateAccount(string email, string userName, string password, string codigoCliente)
        {
            try
            {
                string xipAcceso = ""; 
                var result1U = _UserService.CreateAccount(email, userName, password, xipAcceso);
                if (result1U.IsSuccess)
                {
                    var result = _BaseService.GetCredentialsByEmail(email); //, xcodigoCliente);
                    if (result.IsSuccess)
                    {
                        //_Logger.LogInformation("Access granted");
                        var usuario = result1U.Data;
                        HttpContext.Session[SessionUserName] = usuario.userName;

                        var userData = JsonConvert.SerializeObject(usuario);
                        HttpContext.Session[SessionUserData] = userData;

                        TempData["Success"] = "1";
                        TempData["Message"] = "Access granted";

                        return Json(new { error = 0, message = "Access granted" });
                    }
                    else throw new Exception(result.Exception.Message);
                }
                else throw new Exception(result1U.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: UpdateProfile/eMail
        [HttpPost]
        public ActionResult UpdateProfile(string userCode, string userName)
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                var result = _BaseService.UpdateProfile(userCode, userName, user.userCode);
                if (result.IsSuccess)
                {
                    TempData["Success"] = "1";
                    TempData["Message"] = "Profile updated";
                    return Json(new { error = 0, message = "Profile updated" });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: UpdateProfile/eMail
        [HttpPost]
        public ActionResult UpdateUserPassword(string userCode, string password)
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                string hash = SecurityHelper.HashPassword(Functions.ToString(password), SecurityHelper.GenerateSalt(5));
                var result = _BaseService.UpdateUserPassword(userCode, hash, user.userCode);
                if (result.IsSuccess)
                {
                    TempData["Success"] = "1";
                    TempData["Message"] = "Password updated";
                    return Json(new { error = 0, message = "Password updated" });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

    }
}
