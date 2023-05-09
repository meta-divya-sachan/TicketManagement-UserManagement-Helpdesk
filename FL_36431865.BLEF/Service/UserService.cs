using TicketManagement.Common;
using TicketManagement.Model; 
using System; 

namespace TicketManagement.Service
{
    public interface IUserService
    {
        Result<BLL.User> CheckLogin(string p_userCode, string p_password, string p_storedPassword, string p_accessIP);
        Result<BLL.sUser> CreateAccount(string p_userCode, string p_userName, string p_password, string p_accessIP);
    }

    public class UserService: IUserService
    {
        private readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        // Check Login
        public Result<BLL.User> CheckLogin(string p_userCode, string p_password, string p_storedPassword, string p_accessIP)
        {
            var result = new Result<BLL.User>() { IsSuccess = false };
            try
            {
                if (!SecurityHelper.CheckPassword(p_password, p_storedPassword)) throw new Exception("The password is incorrect");

                var resultU1 = new BaseService().GetUserByCode(p_userCode);
                if (!resultU1.IsSuccess) throw new Exception(resultU1.Exception.Message);

                result.Data = resultU1.Data;
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                result.IsSuccess = false;
                result.Exception = ex;
                result.Message = ex.Message;
            }
            return result;
        }

        // Create account
        public Result<BLL.sUser> CreateAccount(string p_email, string p_userName, string p_password, string p_accessIP)
        {
            var result = new Result<BLL.sUser>() { IsSuccess = false };
            try
            {
                string hash = SecurityHelper.HashPassword(p_password, SecurityHelper.GenerateSalt(5));
                var resultU1 = new BaseService().CreateNewUser(p_userName, hash, p_email, "LOGIN");
                if (resultU1.IsSuccess)
                {
                    var userCode = resultU1.Data;
                    var usuario = new BLL.sUser()
                    {
                        userCode = userCode,
                        userName = p_userName,
                        email = p_email,
                    };
                    result.Data = usuario;
                    result.IsSuccess = true;
                }
                else throw new Exception(resultU1.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                result.IsSuccess = false;
                result.Exception = ex;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
