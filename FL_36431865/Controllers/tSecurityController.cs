using TicketManagement.Filtros;
using TicketManagement.Helpers; 
using TicketManagement.Service;
using System;
using System.Web.Mvc;

namespace TicketManagement.Controllers
{
    [AdminRole]
    
    public class tSecurityController : Controller
    {
        private readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IBaseService _BaseService = new BaseService();


        // GET: tSecurityController/Roles
        public ActionResult Roles(string roleID, string roleName, string status)
        {
            try
            { 
                // Filters 
                int xroleID = Functions.ToInt(roleID);
                string xroleName = Functions.ToString(roleName);
                int xestado = Functions.ToInt(status, 1);

                var data = new BLL.sRole() { roleID = xroleID, roleName = xroleName, status = xestado };
                var result = _BaseService.GetRolesByFilter(xroleID, xroleName, xestado);
                if (result.IsSuccess)
                {
                    var output = new Model.Base<BLL.sRole>()
                    {
                        Data = data,
                        Items = result.Data,
                    };
                    return View(output);
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

        // POST: tSecurityController/Summary
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Roles(FormCollection collection)
        {
            try
            { 
                //--------------------
                // Transaction Begin
                //-------------------- 
                int roleID = Functions.ToInt(collection["Data.roleID"]);
                string roleName = Functions.ToString(collection["Data.roleName"]);
                int status = Functions.ToInt(collection["Data.status"]);

                var data = new BLL.sRole() { roleID = roleID, roleName = roleName, status = status };
                var result = _BaseService.GetRolesByFilter(roleID, roleName, status);
                if (result.IsSuccess)
                {
                    var output = new Model.Base<BLL.sRole>()
                    {
                        Data = data,
                        Items = result.Data,
                    };
                    return View(output);
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

        // GET: pUserController
        public ActionResult Users(string userName, string status)
        {
            try
            {
                string xuserName = Functions.ToString(userName);
                int xestado = Functions.ToInt(status, 1);

                var data = new BLL.User
                {
                    usuario = new BLL.sUser() { userName = xuserName, status = xestado }
                };

                var result = _BaseService.GetUsersByFilter(data.usuario.userName, (int)data.usuario.status);
                if (result.IsSuccess)
                {
                    var output = new Model.Base<BLL.User>()
                    {
                        Data = data,
                        Items = result.Data,
                    };
                    return View(output);
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

        // POST: pUserController/Users
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Users(FormCollection collection)
        {
            try
            { 
                // Roles
                var _Result10 = _BaseService.GetRoles();
                if (_Result10.IsSuccess) ViewBag.ListOfPerfiles = _Result10.Data;
                else throw new Exception(_Result10.Exception.Message);


                //--------------------
                // Transaction Begin
                //--------------------
                string userName = Functions.ToString(collection["Data.usuario.userName"]);
                int status = Functions.ToInt(collection["Data.usuario.status"]);

                var data = new BLL.User
                {
                    usuario = new BLL.sUser() { userName = userName, status = status }
                };

                var result = _BaseService.GetUsersByFilter(data.usuario.userName, (int)data.usuario.status);
                if (result.IsSuccess)
                {
                    var output = new Model.Base<BLL.User>()
                    {
                        Data = data,
                        Items = result.Data,
                    };
                    return View(output);
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


        #region Roles Web Methods

        // POST: GetRoleByCode/
        [HttpPost]
        public ActionResult GetRoleByCode(int roleID)
        {
            try
            {
                var result = _BaseService.GetRoleByCode(roleID);
                if (result.IsSuccess)
                { 
                    return Json(new { error = 0, message = "Role data found", output = result.Data });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: CreateNewRole/
        [HttpPost]
        public ActionResult CreateNewRole(string roleName, string description)
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                string xroleName = Functions.ToString(roleName);
                string xdescription = Functions.ToString(description);
                var result = _BaseService.CreateNewRole(xroleName, xdescription, user.userCode);
                if (result.IsSuccess)
                {
                    TempData["Success"] = "1";
                    TempData["Message"] = "Record created";
                    return Json(new { error = 0, message = "Record created" });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: UpdateRole/
        [HttpPost]
        public ActionResult UpdateRole(int roleID, string roleName, string description)
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                string xroleName = Functions.ToString(roleName);
                string xdescription = Functions.ToString(description);
                var result = _BaseService.UpdateRole(roleID, xroleName, xdescription, user.userCode);
                if (result.IsSuccess)
                {
                    TempData["Success"] = "1";
                    TempData["Message"] = "Record updated";
                    return Json(new { error = 0, message = "Record updated" });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: DeleteRole/
        [HttpPost]
        public ActionResult DeleteRole(int roleID)
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                var result = _BaseService.DeleteRole(roleID);
                if (result.IsSuccess)
                {
                    TempData["Success"] = "1";
                    TempData["Message"] = "Record deleted";
                    return Json(new { error = 0, message = "Record deleted" });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        //private class ObjectPermission
        //{
        //    public int objectOrder { get; set; }
        //    public int objectID { get; set; }
        //    public string objectName { get; set; }
        //    public int objectLevel { get; set; }
        //    public bool selected { get; set; }
        //}

        //// POST: GetPermissionsByClientAndRole/
        //[HttpPost]
        //public ActionResult GetPermissionsByClientAndRole(int roleID, int clientID)
        //{
        //    try
        //    {
        //        var result = _BaseService.GetPermissionsByClientAndRole(roleID, clientID);
        //        if (result.IsSuccess)
        //        {
        //            var list = Functions.ConvertTo<ObjectPermission>(result.Data);
        //            return Json(new { error = 0, message = "Registros obtenidos correctamente", output = list.OrderBy(c => c.objectOrder) });
        //        }
        //        else throw new Exception(result.Exception.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _Logger.Error(ex.Message, ex);
        //        return Json(new { error = 1, message = ex.Message });
        //    }
        //}

        //// POST: GetUsersByClientAndRole/
        //[HttpPost]
        //public ActionResult GetUsersByClientAndRole(int roleID, int clientID)
        //{
        //    try
        //    {
        //        var result = _BaseService.GetUsersByClientAndRole(roleID, clientID);
        //        if (result.IsSuccess)
        //        {
        //            var list = Functions.ConvertTo<t_User>(result.Data);
        //            return Json(new { error = 0, message = "Registros obtenidos correctamente", output = list });
        //        }
        //        else throw new Exception(result.Exception.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _Logger.Error(ex.Message, ex);
        //        return Json(new { error = 1, message = ex.Message });
        //    }
        //} 

        #endregion


        #region Users Web Methods

        // POST: GetUserByCode/
        [HttpPost]
        public ActionResult GetUserByCode(string userCode)
        {
            try
            {
                var result = _BaseService.GetUserByCode(userCode);
                if (result.IsSuccess)
                {
                    return Json(new { error = 0, message = "User data found", output = result.Data });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: GetNotAssignedRoles/
        [HttpPost]
        public ActionResult GetNotAssignedRoles(string userCode)
        {
            try
            {
                var result = _BaseService.GetNotAssignedRoles(userCode);
                if (result.IsSuccess)
                {
                    return Json(new { error = 0, message = "User data found", output = result.Data });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: CreateNewUser/
        [HttpPost]
        public ActionResult CreateNewUser(string userName, string email)
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                string password = DateTime.Now.ToString("yyyyMMdd");
                string hash = SecurityHelper.HashPassword(password, SecurityHelper.GenerateSalt(5));
                var result = _BaseService.CreateNewUser(userName, hash, email, user.userCode);
                if (result.IsSuccess)
                {
                    TempData["Success"] = "1";
                    TempData["Message"] = "Record created. User password is today in format yyyyMMdd";
                    return Json(new { error = 0, message = "Record created" });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: UpdateUser/
        [HttpPost]
        public ActionResult UpdateUser(string userCode, string userName, string email)
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                string xuserName = Functions.ToString(userName);
                string xemail = Functions.ToString(email); 
                var result = _BaseService.UpdateUser(userCode, xuserName , xemail, user.userCode);
                if (result.IsSuccess)
                {
                    TempData["Success"] = "1";
                    TempData["Message"] = "Record updated";
                    return Json(new { error = 0, message = "Record updated" });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: ActivateUser/
        [HttpPost]
        public ActionResult ActivateUser(string userCode)
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                var result = _BaseService.ActivateUser(userCode, user.userCode);
                if (result.IsSuccess)
                {
                    TempData["Success"] = "1";
                    TempData["Message"] = "Record updated";
                    return Json(new { error = 0, message = "Record updated" });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: RemoveUserRole/
        [HttpPost]
        public ActionResult RemoveUserRole(string userCode, int roleID)
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                var result = _BaseService.RemoveUserRole(userCode, roleID, user.userCode);
                if (result.IsSuccess)
                {
                    TempData["Success"] = "1";
                    TempData["Message"] = "Record updated";
                    return Json(new { error = 0, message = "Record updated" });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: CreateNewUser/
        [HttpPost]
        public ActionResult AssignUserRole(string userCode, int roleID)
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                var xuserCode = userCode;
                var xroleID = Functions.ToInt(roleID);
                var result = _BaseService.AssignUserRole(xuserCode, xroleID, user.userCode);
                if (result.IsSuccess)
                {
                    TempData["Success"] = "1";
                    TempData["Message"] = "Record created";
                    return Json(new { error = 0, message = "Record created" });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        #endregion

    }
}
