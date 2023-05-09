using Newtonsoft.Json;
using System;
using System.Collections.Generic; 
using System.Linq; 
using System.Xml;
using TicketManagement.Common;
using TicketManagement.Model;

namespace TicketManagement.Service
{
    public interface IBaseService
    {

        #region Role
          
        Result<List<BLL.sRole>> GetRoles();
        Result<List<BLL.sRole>> GetRolesByFilter(int p_roleID, string p_roleName, int p_estado);
        Result<BLL.sRole> GetRoleByCode(int p_roleID);
        Result<bool> CreateNewRole(string p_roleName, string p_description, string p_creationUser);
        Result<bool> UpdateRole(int p_roleID, string p_roleName, string p_description, string p_changeUser);
        Result<bool> DeleteRole(int p_roleID);

        #endregion


        #region User

        Result<List<BLL.sUser>> GetUsers(int p_estado);
        Result<List<BLL.User>> GetUsersByFilter(string p_userName, int p_estado);
        Result<BLL.User> GetUserByCode(string p_userCode);
        Result<BLL.sUser> GetCredentialsByEmail(string p_userCode);
        Result<List<BLL.sRole>> GetNotAssignedRoles(string p_userCode);
        Result<List<BLL.sUser>> GetUsersByRole(int p_roleID);
        Result<string> CreateNewUser(string p_userName, string p_password, string p_email, string p_creationUser);
        Result<bool> UpdateUser(string p_userCode, string p_userName, string p_email, string p_changeUser);
        Result<bool> ActivateUser(string p_userCode, string p_changeUser);
        Result<bool> UpdateUserImage(string p_userCode, string p_guidImage1, string p_changeUser);
        Result<bool> UpdateProfile(string p_userCode, string p_userName, string p_changeUser);
        Result<bool> UpdateUserPassword(string p_userCode, string p_password, string p_changeUser);
        Result<bool> DeleteUser(string p_userCode);

        #endregion


        #region User Role

        Result<bool> AssignUserRole(string p_userCode, int p_roleID, string p_creationUser);
        Result<bool> RemoveUserRole(string p_userCode, int p_roleID, string p_changeUser);

        #endregion


        #region Request

        Result<List<BLL.tRequest>> GetRequestsByFilter(int p_requestID, string p_requestTitle, int p_status, int p_situation, string p_requestUser);
        Result<BLL.Request> GetRequestByID(int p_requestID);
        Result<int> CreateNewRequest(string p_requestTitle, string p_description, int p_requestType, string p_location, string p_creationUser); 
        Result<bool> ReassignRequest(int p_requestID, string p_assignedUser, string p_changeUser);
        Result<bool> ProcessRequest(int p_requestID, int p_situation, string p_comments, string p_changeUser);
        Result<List<BLL.tRequestComment>> GetCommentsByRequestID(int p_requestID);
        Result<bool> CreateNewComment(int p_requestID, string p_comment, string p_creationUser);

        #endregion 


        #region Parameters

        Result<List<BLL.pParameter>> GetParametersByList(List<int> p_list);
        Result<List<BLL.pParameter>> GetParametersByID(int p_parameterID, int p_estado);
        Result<BLL.pParameter> GetParametersByCode(int p_parameterID, int p_itemID);
        Result<bool> CreateNewParameter(int p_parameterID, int p_itemID, string p_value, string p_text, string p_auxiliar1, string p_auxiliar2, string p_auxiliar3, string p_auxiliar4, string p_auxiliar5, string p_creationUser);
        Result<bool> UpdateParameter(int p_parameterID, int p_itemID, string p_value, string p_text, string p_auxiliar1, string p_auxiliar2, string p_auxiliar3, string p_auxiliar4, string p_auxiliar5, string p_changeUser);
        Result<bool> ActivateParameter(int p_parameterID, int p_itemID, string p_changeUser);

        #endregion


        #region Reports

        Result<List<Model.ChartData>> GetRequestsGroupedByStatus();
        Result<MSBar2DChart> GetRequestsGroupedByManagerAndStatus();

        #endregion

    }

    public class BaseService : IBaseService
    {
        private readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        #region Role 

        // OK - Obtener la lista de roles configurados
        public Result<List<BLL.sRole>> GetRoles()
        {
            var _Result = new Result<List<BLL.sRole>>() { IsSuccess = false, Data = null };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var output = db.sRole.ToList();
                    _Result.IsSuccess = true;
                    _Result.Data = output;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // OK - get the roles by filter
        public Result<List<BLL.sRole>> GetRolesByFilter(int p_roleID, string p_roleName, int p_estado)
        {
            var _Result = new Result<List<BLL.sRole>>() { IsSuccess = false, Data = null };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var predicate = PredicateBuilder.True<BLL.sRole>();
                    if (p_roleID > 0) predicate = predicate.And(c => c.roleID == p_roleID);
                    if (p_roleName != "") predicate = predicate.And(c => c.roleName.ToLower().Contains(p_roleName.ToLower()));
                    if (p_estado >= 0) predicate = predicate.And(c => c.status == p_estado);

                    var output = db.sRole.Where(predicate).ToList(); 
                    _Result.IsSuccess = true;
                    _Result.Data = output;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // OK - get role by code
        public Result<BLL.sRole> GetRoleByCode(int p_roleID)
        {
            var _Result = new Result<BLL.sRole>() { IsSuccess = false, Data = null };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                { 
                    var output = db.sRole.Where(c => c.roleID == p_roleID).FirstOrDefault();
                    _Result.IsSuccess = true;
                    _Result.Data = output;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // create new profile
        public Result<bool> CreateNewRole(string p_roleName, string p_description, string p_creationUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.sRole.Where(c => c.roleName == p_roleName).FirstOrDefault();
                    if (entity != null) throw new Exception("Role name just exists");

                    var roleID = db.sRole.Max(c => c.roleID);
                    roleID = (Functions.ToInt(roleID, 0) + 1);

                    entity = new BLL.sRole
                    {
                        roleID = roleID,
                        roleName = Functions.ToString(p_roleName),
                        description = Functions.ToString(p_description),
                        status = 1,
                        creationDate = DateTime.Now,
                        creationUser = p_creationUser
                    };
                    db.sRole.Add(entity);
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Update Role 
        public Result<bool> UpdateRole(int p_roleID, string p_roleName, string p_description, string p_changeUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.sRole.Where(c => c.roleID == p_roleID).FirstOrDefault();
                    if (entity == null) throw new Exception("Role not exists");

                    entity.roleName = Functions.ToString(p_roleName);
                    entity.description = Functions.ToString(p_description);
                    entity.changeDate = DateTime.Now;
                    entity.changeUser = p_changeUser;
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // delete profile
        public Result<bool> DeleteRole(int p_roleID)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.sRole.Where(c => c.roleID == p_roleID).FirstOrDefault();
                    if (entity == null) throw new Exception("Element not exists");
                    db.sRole.Remove(entity);
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        #endregion


        #region User  

        // get user
        public Result<List<BLL.sUser>> GetUsers(int p_estado)
        {
            var _Result = new Result<List<BLL.sUser>>() { IsSuccess = false, Data = null };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var predicate = PredicateBuilder.True<BLL.sUser>();
                    if (p_estado >= 0) predicate = predicate.And(c => c.status == p_estado);

                    var output = db.sUser.Where(predicate).ToList();
                    _Result.IsSuccess = true;
                    _Result.Data = output;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Get users by filter
        public Result<List<BLL.User>> GetUsersByFilter(string p_userName, int p_estado)
        {
            var _Result = new Result<List<BLL.User>>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var output = new List<BLL.User>();

                    // Users
                    var predicate = PredicateBuilder.True<BLL.sUser>();
                    if (p_userName != "") predicate = predicate.And(c => c.userName.ToLower().Contains(p_userName.ToLower()));
                    if (p_estado >= 0) predicate = predicate.And(c => c.status == p_estado);
                    var users = db.sUser.Where(predicate).ToList();

                    // Roles
                    var roles = (from up in db.sUserRole
                                 join p in db.sRole on up.roleID equals p.roleID
                                 join u in db.sUser on up.userCode equals u.userCode
                                 where up.status == 1
                                 select new { up, p })
                                .ToList();

                    foreach (var xuser in users)
                    {
                        var xroles = new List<BLL.sRole>();
                        var xlist = roles.FindAll(c => c.up.userCode == xuser.userCode);
                        foreach (var xrole in xlist)
                        {
                            xroles.Add(new BLL.sRole() { roleID = xrole.p.roleID, roleName = xrole.p.roleName });
                        }
                        output.Add(new BLL.User() { usuario = xuser, perfiles = xroles });
                    }

                    _Result.IsSuccess = true;
                    _Result.Data = output;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Get users by code
        public Result<BLL.User> GetUserByCode(string p_userCode)
        {
            var _Result = new Result<BLL.User>() { IsSuccess = false, Data = null };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var user = db.sUser.Where(c => c.userCode == p_userCode).FirstOrDefault();
                    if (user != null)
                    {
                        var roles = (from up in db.sUserRole
                                     join p in db.sRole on up.roleID equals p.roleID
                                     where up.userCode ==  p_userCode
                                     select p)
                                    .ToList();

                        var output = new BLL.User() { usuario = user, perfiles = roles };
                        _Result.IsSuccess = true;
                        _Result.Data = output;
                    }
                    else throw new Exception("The user is not registered");
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Get user credentials
        public Result<BLL.sUser> GetCredentialsByEmail(string p_email)
        {
            var _Result = new Result<BLL.sUser>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var user = db.sUser.Where(c => c.email == p_email).FirstOrDefault();
                    if (user != null)
                    { 
                        _Result.IsSuccess = true;
                        _Result.Data = user;
                    }
                    else throw new Exception("The user is not registered");
                }
            }
            catch (Exception ex)
            {
                _Result.IsSuccess = false;
                _Result.Message = ex.Message;
                _Result.Exception = ex;
            }
            return _Result;
        }

        // Get not assigned roles
        public Result<List<BLL.sRole>> GetNotAssignedRoles(string p_userCode)
        {
            var _Result = new Result<List<BLL.sRole>>() { IsSuccess = false, Data = null };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var assigned = (from up in db.sUserRole
                                    join p in db.sRole on up.roleID equals p.roleID
                                    where up.userCode == p_userCode
                                    select p).ToList();

                    var roles = (from r in db.sRole
                                 //where assigned.All(a => a.roleID != r.roleID)
                                 select r).ToList();

                    _Result.IsSuccess = true;
                    _Result.Data = roles;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Get users by role
        public Result<List<BLL.sUser>> GetUsersByRole(int p_roleID)
        {
            var _Result = new Result<List<BLL.sUser>>() { IsSuccess = false, Data = null };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var users = (from u in db.sUser
                                join ur in db.sUserRole on u.userCode equals ur.userCode
                                where ur.roleID == p_roleID
                                select u)
                                .ToList();
                    _Result.IsSuccess = true;
                    _Result.Data = users;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Create new user
        public Result<string> CreateNewUser(string p_userName, string p_password, string p_email, string p_creationUser)
        {
            var _Result = new Result<string>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.sUser.Where(c => c.email == p_email).FirstOrDefault();
                    if (entity != null) throw new Exception("User email just exists");

                    // Add new user
                    var userCode = db.sUser.Max(c => c.userCode);
                    userCode = (Functions.ToInt(userCode, 0) + 1).ToString();
                    entity = new BLL.sUser
                    {
                        userCode = userCode,
                        userName = Functions.ToString(p_userName),
                        password = Functions.ToString(p_password),
                        email = Functions.ToString(p_email),
                        status = 1,
                        creationDate = DateTime.Now,
                        creationUser = p_creationUser
                    };
                    db.sUser.Add(entity);
                    db.SaveChanges();

                    // Add new role
                    var roleID = 4; // -> Requester
                    var xentity = new BLL.sUserRole
                    {
                        userCode = userCode,
                        roleID = roleID,
                        status = 1,
                        creationDate = DateTime.Now,
                        creationUser = p_creationUser
                    };
                    db.sUserRole.Add(xentity);
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = userCode;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Update user
        public Result<bool> UpdateUser(string p_userCode, string p_userName, string p_email, string p_changeUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.sUser.Where(c => c.userCode == p_userCode).FirstOrDefault();
                    if (entity == null) throw new Exception("User not exists");

                    entity.userName = Functions.ToString(p_userName);
                    entity.email = Functions.ToString(p_email);
                    entity.changeDate = DateTime.Now;
                    entity.changeUser = p_changeUser;
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Update user
        public Result<bool> ActivateUser(string p_userCode, string p_changeUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.sUser.Where(c => c.userCode == p_userCode).FirstOrDefault();
                    if (entity == null) throw new Exception("User not exists");

                    var status = 1;
                    if (entity.status == 1) status = 0;
                    else status = 1;

                    entity.status = status; 
                    entity.changeDate = DateTime.Now;
                    entity.changeUser = p_changeUser;
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Update user image
        public Result<bool> UpdateUserImage(string p_userCode, string p_guidImage1, string p_changeUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.sUser.Where(c => c.userCode == p_userCode).FirstOrDefault();
                    if (entity == null) throw new Exception("User not exists");

                    entity.guidImage1 = Functions.ToString(p_guidImage1); 
                    entity.changeDate = DateTime.Now;
                    entity.changeUser = p_changeUser;
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Update profile
        public Result<bool> UpdateProfile(string p_userCode, string p_userName, string p_changeUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.sUser.Where(c => c.userCode == p_userCode).FirstOrDefault();
                    if (entity == null) throw new Exception("User not exists");

                    entity.userName = Functions.ToString(p_userName);
                    entity.changeDate = DateTime.Now;
                    entity.changeUser = p_changeUser;
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Update user password
        public Result<bool> UpdateUserPassword(string p_userCode, string p_password, string p_changeUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.sUser.Where(c => c.userCode == p_userCode).FirstOrDefault();
                    if (entity == null) throw new Exception("User not exists");

                    entity.password = Functions.ToString(p_password);
                    entity.changeDate = DateTime.Now;
                    entity.changeUser = p_changeUser;
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Delete user
        public Result<bool> DeleteUser(string p_userCode)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.sUser.Where(c => c.userCode == p_userCode).FirstOrDefault();
                    if (entity == null) throw new Exception("Element not exists");
                    db.sUser.Remove(entity);
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        #endregion


        #region User Role

        // Assign role to user
        public Result<bool> AssignUserRole(string p_userCode, int p_roleID, string p_creationUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.sUserRole.Where(c => c.userCode == p_userCode && c.roleID == p_roleID).FirstOrDefault();
                    if (entity != null) throw new Exception("Role is just assigned");

                    // Add new user role
                    var xentity = new BLL.sUserRole
                    {
                        userCode = p_userCode, 
                        roleID = p_roleID,
                        status = 1,
                        creationDate = DateTime.Now,
                        creationUser = p_creationUser
                    };
                    db.sUserRole.Add(xentity);
                    db.SaveChanges(); 

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Remove role from user
        public Result<bool> RemoveUserRole(string p_userCode, int p_roleID, string p_changeUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.sUserRole.Where(c => c.userCode == p_userCode && c.roleID == p_roleID).FirstOrDefault();
                    if (entity == null) throw new Exception("Element not exists");
                    db.sUserRole.Remove(entity);
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        #endregion


        #region  Request

        // Get requests by filter
        public Result<List<BLL.tRequest>> GetRequestsByFilter(int p_requestID, string p_requestTitle, int p_status, int p_situation, string p_requestUser)
        {
            var _Result = new Result<List<BLL.tRequest>>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var p1s = db.pParameter.Where(c => c.parameterID == 1).ToList();
                    var p2s = db.pParameter.Where(c => c.parameterID == 2).ToList();
                    var users = db.sUser.ToList();
                    var roles = db.sUserRole.Where(c => c.userCode == p_requestUser).ToList();
                    var predicate = PredicateBuilder.True<BLL.tRequest>();

                    if (roles.Where(c => new List<int> { 1, 2 }.Contains(c.roleID)).Any())
                    {
                        if (p_requestID > 0) predicate = predicate.And(c => c.requestID == p_requestID);
                        if (p_requestTitle != "") predicate = predicate.And(c => c.requestTitle.ToLower().Contains(p_requestTitle.ToLower()));
                        if (p_status >= 0) predicate = predicate.And(c => c.status == p_status);
                        if (p_situation >= 0) predicate = predicate.And(c => c.situation == p_situation);
                    }
                    else if (roles.Where(c => new List<int> { 3 }.Contains(c.roleID)).Any())
                    { 
                        predicate = predicate.And(c => c.assignedRole == 3);
                        predicate = predicate.And(c => c.assignedUser == p_requestUser);
                        if (p_requestID > 0) predicate = predicate.And(c => c.requestID == p_requestID);
                        if (p_requestTitle != "") predicate = predicate.And(c => c.requestTitle.ToLower().Contains(p_requestTitle.ToLower()));
                        if (p_status >= 0) predicate = predicate.And(c => c.status == p_status);
                        if (p_situation >= 0) predicate = predicate.And(c => c.situation == p_situation); 
                    }
                    else if (roles.Where(c => new List<int> { 3 }.Contains(c.roleID)).Any())
                    { 
                        predicate = predicate.And(c => c.creationUser == p_requestUser);
                        if (p_requestID > 0) predicate = predicate.And(c => c.requestID == p_requestID);
                        if (p_requestTitle != "") predicate = predicate.And(c => c.requestTitle.ToLower().Contains(p_requestTitle.ToLower()));
                        if (p_status >= 0) predicate = predicate.And(c => c.status == p_status);
                        if (p_situation >= 0) predicate = predicate.And(c => c.situation == p_situation);
                    }

                    var output = db.tRequest.Where(predicate).ToList();
                    foreach (var item in output)
                    {
                        var p1 = p1s.Where(c => c.value == item.requestType.ToString()).FirstOrDefault();
                        if (p1 != null) item.requestTypeName = p1.text;

                        var p2 = p2s.Where(c => c.value == item.situation.ToString()).FirstOrDefault();
                        if (p2 != null) item.situationName = p2.text;

                        var user = users.Where(c => c.userCode == item.assignedUser).FirstOrDefault();
                        if (user != null) item.assignedUserName = user.userName;
                    }

                    _Result.IsSuccess = true;
                    _Result.Data = output;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Get request by ID
        public Result<BLL.Request> GetRequestByID(int p_requestID)
        {
            var _Result = new Result<BLL.Request>() { IsSuccess = false, Data = null };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var request = db.tRequest.Where(c => c.requestID == p_requestID).FirstOrDefault();
                    if (request != null)
                    {
                        var comments = db.tRequestComment.Where(c => c.requestID == p_requestID).ToList();

                        var output = new BLL.Request() { request = request, comments = comments };
                        _Result.IsSuccess = true;
                        _Result.Data = output;
                    }
                    else throw new Exception("The user is not registered");
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Create new request
        public Result<int> CreateNewRequest(string p_requestTitle, string p_description, int p_requestType, string p_location, string p_creationUser)
        {
            var _Result = new Result<int>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var xassignedRole = 3; //--> MANAGER
                    var xsituation = 1;

                    // Exists
                    string xassignedUser = "";
                    var exists = (from up in db.tRequest
                                  join u in db.sUser on up.assignedUser equals u.userCode
                                  join ur in db.sUserRole on up.assignedUser equals ur.userCode
                                  where ur.roleID == xassignedRole
                                  select u).Any();
                    if (exists)
                    {
                        var user = (from up in db.tRequest
                                    join u in db.sUser on up.assignedUser equals u.userCode
                                    join ur in db.sUserRole on up.assignedUser equals ur.userCode
                                    where ur.roleID == xassignedRole
                                    group u by new { u.userCode, u.userName } into g
                                    select new
                                    {
                                        userCode = g.Key.userCode,
                                        userName = g.Key.userName,
                                        count = g.Count()
                                    })
                                    .OrderBy(c => c.userName)
                                    .OrderBy(c => c.count)
                                    .FirstOrDefault();

                        xassignedUser = user.userCode;
                    }
                    else
                    {
                        xassignedUser = (from up in db.sUser
                                         join ur in db.sUserRole on up.userCode equals ur.userCode
                                         where ur.roleID == xassignedRole
                                         select ur)
                                         .FirstOrDefault().userCode;
                    }

                    // Add new user role
                    var xentity = new BLL.tRequest
                    {
                        requestTitle = p_requestTitle,
                        description = p_description,
                        requestType = p_requestType,
                        location = p_location,
                        assignedRole = xassignedRole,
                        assignedUser = xassignedUser,
                        situation = xsituation,
                        status = 1,
                        creationDate = DateTime.Now,
                        creationUser = p_creationUser
                    };
                    db.tRequest.Add(xentity);
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = xentity.requestID;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        } 

        // Reassign the request
        public Result<bool> ReassignRequest(int p_requestID, string p_assignedUser, string p_changeUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.tRequest.Where(c => c.requestID == p_requestID).FirstOrDefault();
                    if (entity == null) throw new Exception("Request not exists");

                    entity.assignedUser = Functions.ToString(p_assignedUser);
                    entity.changeDate = DateTime.Now;
                    entity.changeUser = p_changeUser;
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Process the request
        public Result<bool> ProcessRequest(int p_requestID, int p_situation, string p_comments, string p_changeUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.tRequest.Where(c => c.requestID == p_requestID).FirstOrDefault();
                    if (entity == null) throw new Exception("Request not exists");

                    entity.situation = Functions.ToInt(p_situation);
                    entity.changeDate = DateTime.Now;
                    entity.changeUser = p_changeUser;
                    db.SaveChanges();

                    if (!string.IsNullOrEmpty(p_comments))
                    {
                        var xentity = new BLL.tRequestComment();
                        xentity.requestID = p_requestID;
                        xentity.comment = p_comments;
                        xentity.status = 1;
                        xentity.creationDate = DateTime.Now;
                        xentity.creationUser = p_changeUser;
                        db.tRequestComment.Add(xentity);
                        db.SaveChanges();
                    }    

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Get comments
        public Result<List<BLL.tRequestComment>> GetCommentsByRequestID(int p_requestID)
        {
            var _Result = new Result<List<BLL.tRequestComment>>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var users = db.sUser.ToList();
                    var output = db.tRequestComment.Where(c => c.requestID == p_requestID).ToList();

                    foreach (var item in output)
                    {
                        if (item.creationDate != null) item.creationDateT = ((DateTime)item.creationDate).ToString("yyyy-MM-dd hh:mm");

                        var user = users.Where(c => c.userCode == item.creationUser).FirstOrDefault();
                        if (user != null) item.creationUserName = user.userName;
                    }

                    _Result.IsSuccess = true;
                    _Result.Data = output;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Create new comments
        public Result<bool> CreateNewComment(int p_requestID, string p_comment, string p_creationUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                { 
                    var entity = new BLL.tRequestComment
                    {
                        requestID = p_requestID,
                        comment = p_comment, 
                        status = 1,
                        creationDate = DateTime.Now,
                        creationUser = p_creationUser
                    };
                    db.tRequestComment.Add(entity);
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }



        #endregion


        #region Parameters

        // Get parameters by list
        public Result<List<BLL.pParameter>> GetParametersByList(List<int> p_list)
        {
            var _Result = new Result<List<BLL.pParameter>>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var output = db.pParameter.Where(c => p_list.Contains(c.parameterID)).ToList();
                    _Result.IsSuccess = true;
                    _Result.Data = output;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Get parameters list
        public Result<List<BLL.pParameter>> GetParametersByID(int p_parameterID, int p_estado)
        {
            var _Result = new Result<List<BLL.pParameter>>() { IsSuccess = false, Data = null };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var predicate = PredicateBuilder.True<BLL.pParameter>();
                    if (p_parameterID >= 0) predicate = predicate.And(c => c.parameterID == p_parameterID); 
                    if (p_estado >= 0) predicate = predicate.And(c => c.status == p_estado);

                    var output = db.pParameter.Where(predicate).ToList();
                    _Result.IsSuccess = true;
                    _Result.Data = output;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Get parameters by code
        public Result<BLL.pParameter> GetParametersByCode(int p_parameterID, int p_itemID)
        {
            var _Result = new Result<BLL.pParameter>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var output = db.pParameter.Where(c => c.parameterID == p_parameterID && c.itemID == p_itemID).FirstOrDefault();
                    _Result.IsSuccess = true;
                    _Result.Data = output;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Create new parameter
        public Result<bool> CreateNewParameter(int p_parameterID, int p_itemID, string p_value, string p_text, string p_auxiliar1, string p_auxiliar2, string p_auxiliar3, string p_auxiliar4, string p_auxiliar5, string p_creationUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.pParameter.Where(c => c.parameterID == p_parameterID && c.itemID == p_itemID).FirstOrDefault();
                    if (entity != null) throw new Exception("Parameter just exists"); 

                    entity = new BLL.pParameter
                    {
                        parameterID = p_parameterID,
                        itemID = p_itemID,
                        value = p_value,
                        text = p_text,
                        auxiliar1 = p_auxiliar1,
                        auxiliar2 = p_auxiliar2,
                        auxiliar3 = p_auxiliar3,
                        auxiliar4 = p_auxiliar4,
                        auxiliar5 = p_auxiliar5,
                        status = 1,
                        creationDate = DateTime.Now,
                        creationUser = p_creationUser,
                    };
                    db.pParameter.Add(entity);
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Update parameter
        public Result<bool> UpdateParameter(int p_parameterID, int p_itemID, string p_value, string p_text, string p_auxiliar1, string p_auxiliar2, string p_auxiliar3, string p_auxiliar4, string p_auxiliar5, string p_changeUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.pParameter.Where(c => c.parameterID == p_parameterID && c.itemID == p_itemID).FirstOrDefault();
                    if (entity == null) throw new Exception("Parameter not exists");

                    entity.value = p_value;
                    entity.text = p_text;
                    entity.auxiliar1 = p_auxiliar1;
                    entity.auxiliar2 = p_auxiliar2;
                    entity.auxiliar3 = p_auxiliar3;
                    entity.auxiliar4 = p_auxiliar4;
                    entity.auxiliar5 = p_auxiliar5;
                    entity.changeDate = DateTime.Now;
                    entity.changeUser = p_changeUser;
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Activate parameter
        public Result<bool> ActivateParameter(int p_parameterID, int p_itemID, string p_changeUser)
        {
            var _Result = new Result<bool>() { IsSuccess = false };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var entity = db.pParameter.Where(c => c.parameterID == p_parameterID && c.itemID == p_itemID).FirstOrDefault();
                    if (entity == null) throw new Exception("Parameter not exists");

                    var status = 1;
                    if (entity.status == 1) status = 0;
                    else status = 1;

                    entity.status = status;
                    entity.changeDate = DateTime.Now;
                    entity.changeUser = p_changeUser;
                    db.SaveChanges();

                    _Result.IsSuccess = true;
                    _Result.Data = true;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        #endregion


        #region Reports

        // Get requests by status
        public Result<List<Model.ChartData>> GetRequestsGroupedByStatus()
        {
            var _Result = new Result<List<Model.ChartData>>() { IsSuccess = false, Data = null };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var data = (from up in db.tRequest
                                join p2 in db.pParameter on up.situation.ToString() equals p2.value
                                where p2.parameterID == 2
                                group p2 by p2.text into g
                                select new
                                {
                                    label = g.Key,
                                    value = g.Count()
                                });

                    var chartData = new List<Model.ChartData>();
                    foreach (var item in data)
                    {
                        chartData.Add(new ChartData()
                        {
                            label = item.label,
                            value = item.value.ToString(),  
                        });
                    }

                    _Result.IsSuccess = true;
                    _Result.Data = chartData;
                }
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        // Get requests by manager and status
        public Result<MSBar2DChart> GetRequestsGroupedByManagerAndStatus()
        {
            var _Result = new Result<MSBar2DChart>() { IsSuccess = false, Data = null };
            try
            {
                using (var db = new BLL.TicketManagementEntities())
                {
                    var output = (from up in db.tRequest
                                  join u in db.sUser on up.assignedUser equals u.userCode
                                  join p2 in db.pParameter on up.situation.ToString() equals p2.value
                                  where p2.parameterID == 2
                                  group u by new { u.userName, p2.text } into g
                                  select new
                                  {
                                      label = g.Key.userName,
                                      serieName = g.Key.text,
                                      value = g.Count()
                                  });

                    var chartData = new MSBar2DChart();

                    // Chart
                    var chart = new SChart()
                    {
                        caption = "Requests by Manager and Status",
                        placevaluesinside = "1",
                        showvalues = "0",
                        plottooltext = "<b>$dataValue</b> visitors from $label in $seriesName",
                        theme = "fusion"
                    };
                    chartData.chart = chart;

                    // Categories
                    var category = new SCategory();
                    var labels = output.GroupBy(i => i.label)
                                       .Select(grp => new { label = grp.Key })
                                       .ToArray();
                    foreach (var item in labels)
                    {
                        category.category.Add(new SLabel() { label = item.label });
                    }
                    chartData.categories.Add(category);

                    // Datasets
                    var series = output.GroupBy(i => i.serieName)
                                       .Select(grp => new { serieName = grp.Key })
                                       .ToArray();
                    foreach (var xserieName in series)
                    {
                        var dataset = new SDataset() { seriesname = xserieName.serieName };
                        foreach (var xlabel in labels)
                        {
                            var value = new SValue();
                            var xvalue = output.Where(c => c.serieName == xserieName.serieName && c.label == xlabel.label).ToList();
                            if (xvalue != null && xvalue.Count() > 0)
                            {
                                value.value = xvalue.FirstOrDefault().value.ToString();
                            }
                            else
                            {
                                value.value = "0";
                            }
                            dataset.data.Add(value);
                        }
                        chartData.dataset.Add(dataset);
                    }

                    var jsonData = JsonConvert.SerializeObject(chartData);

                    _Result.IsSuccess = true;
                    _Result.Data = chartData;
                } 
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                _Result.IsSuccess = false;
                _Result.Exception = ex;
                _Result.Message = ex.Message;
            }
            return _Result;
        }

        #endregion

    }
}
