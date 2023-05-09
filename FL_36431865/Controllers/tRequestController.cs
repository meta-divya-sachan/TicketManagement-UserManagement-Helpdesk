using TicketManagement.Filtros;
using TicketManagement.Helpers;
using TicketManagement.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TicketManagement.Controllers
{
    [SessionExpire]
    public class tRequestController : Controller
    {
        private readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IBaseService _BaseService = new BaseService();

        // Requests
        public ActionResult Requests(string requestID, string requestTitle, string status, string situation)
        {
            try
            {
                // Parameters
                var list = new List<int>()
                {
                    Constants.Tables.RequestTypes,
                };
                var _ResultA = _BaseService.GetParametersByList(list);
                if (_ResultA.IsSuccess)
                {
                    ViewBag.ListOfRequestTypes = _ResultA.Data.FindAll(c => c.parameterID == Constants.Tables.RequestTypes);
                }
                else throw new Exception(_ResultA.Exception.Message);


                // Managers
                int roleID = 3;
                var _Result0A = _BaseService.GetUsersByRole(roleID);
                if (_Result0A.IsSuccess) ViewBag.ListOfManagers = _Result0A.Data;
                else throw new Exception(_Result0A.Exception.Message); 


                // Filters 
                int xrequestID = Functions.ToInt(requestID);
                string xrequestTitle = Functions.ToString(requestTitle);
                int xstatus = Functions.ToInt(status, 1);
                int xsituation = Functions.ToInt(situation, -1);

                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                var data = new BLL.tRequest() { requestID = xrequestID, requestTitle = xrequestTitle, status = xstatus, situation = xsituation };
                var result = _BaseService.GetRequestsByFilter(xrequestID, xrequestTitle, xstatus, xsituation, user.userCode);
                if (result.IsSuccess)
                {
                    var output = new Model.Base<BLL.tRequest>()
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

        // POST: tRequestController/Requests
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Requests(FormCollection collection)
        {
            try
            {
                // Parámeters
                var list = new List<int>()
                {
                    Constants.Tables.RequestTypes,
                };
                var _ResultA = _BaseService.GetParametersByList(list);
                if (_ResultA.IsSuccess)
                {
                    ViewBag.ListOfRequestTypes = _ResultA.Data.FindAll(c => c.parameterID == Constants.Tables.RequestTypes);
                }
                else throw new Exception(_ResultA.Exception.Message);


                // Managers
                int roleID = 3;
                var _Result0A = _BaseService.GetUsersByRole(roleID);
                if (_Result0A.IsSuccess) ViewBag.ListOfManagers = _Result0A.Data;
                else throw new Exception(_Result0A.Exception.Message); 


                //--------------------
                // Transaction Begin
                //-------------------- 
                int requestID = Functions.ToInt(collection["Data.requestID"]);
                string requestTitle = Functions.ToString(collection["Data.requestTitle"]);
                int status = Functions.ToInt(collection["Data.status"]);
                int situation = Functions.ToInt(collection["Data.situation"]);

                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                var data = new BLL.tRequest() { requestID = requestID, requestTitle = requestTitle, status = status, situation = situation };
                var result = _BaseService.GetRequestsByFilter(requestID, requestTitle, status, situation, user.userCode);
                if (result.IsSuccess)
                {
                    var output = new Model.Base<BLL.tRequest>()
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


        #region Web Methods

        // POST: GetRequestByID/
        [HttpPost]
        public ActionResult GetRequestByID(int requestID)
        {
            try
            {
                var result = _BaseService.GetRequestByID(requestID);
                if (result.IsSuccess)
                {
                    return Json(new { error = 0, message = "Request found", output = result.Data });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: GetCommentsByRequestID/
        [HttpPost]
        public ActionResult GetCommentsByRequestID(int requestID)
        {
            try
            {
                var result = _BaseService.GetCommentsByRequestID(requestID);
                if (result.IsSuccess)
                {
                    return Json(new { error = 0, message = "Request found", output = result.Data });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: CrateNewRequest/
        [HttpPost]
        public ActionResult CreateNewRequest(string requestTitle, string description, int requestType, string location)
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString()); 
                var xrequestTitle = Functions.ToString(requestTitle);
                var xdescription = Functions.ToString(description);
                var xrequestType = Functions.ToInt(requestType);
                var xlocation = Functions.ToString(location); 
                var result = _BaseService.CreateNewRequest(xrequestTitle, xdescription, xrequestType,xlocation, user.userCode);
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


        // POST: ReassignRequest/
        [HttpPost]
        public ActionResult ReassignRequest(int requestID, string assignedUser)
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                var result = _BaseService.ReassignRequest(requestID, assignedUser, user.userCode);
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

        // POST: ProcessRequest/
        [HttpPost]
        public ActionResult ProcessRequest(int requestID, int situation, string comments)
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                var result = _BaseService.ProcessRequest(requestID, situation, comments, user.userCode);
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

        // POST: CreateNewComment/
        [HttpPost]
        public ActionResult CreateNewComment(int requestID, string comment)
        {
            try
            {
                var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                var xcomment = Functions.ToString(comment); 
                var result = _BaseService.CreateNewComment(requestID, xcomment, user.userCode);
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
