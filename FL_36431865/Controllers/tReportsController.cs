using TicketManagement.Filtros;
using TicketManagement.Service;
using System;
using System.Web.Mvc;

namespace TicketManagement.Controllers
{
    [SessionExpire]
    public class tReportsController : Controller
    {
        private readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IBaseService _BaseService = new BaseService();


        // GET: tReports
        public ActionResult Index()
        {
            return View();
        }


        // POST: GetRequestsGroupedByStatus/
        [HttpPost]
        public ActionResult GetRequestsGroupedByStatus()
        {
            try
            {
                var result = _BaseService.GetRequestsGroupedByStatus();
                if (result.IsSuccess)
                {
                    return Json(new { error = 0, message = "Data found", output = result.Data });
                }
                else throw new Exception(result.Exception.Message);
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { error = 1, message = ex.Message });
            }
        }

        // POST: GetRequestsGroupedByManagerAndStatus/
        [HttpPost]
        public ActionResult GetRequestsGroupedByManagerAndStatus()
        {
            try
            {
                var result = _BaseService.GetRequestsGroupedByManagerAndStatus();
                if (result.IsSuccess)
                {
                    return Json(new { error = 0, message = "Data found", output = result.Data });
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