using TicketManagement.Helpers;
using TicketManagement.Service;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace TicketManagement.Controllers
{
    public class tUploadController : Controller
    {
        private readonly log4net.ILog _Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IBaseService _BaseService = new BaseService();


        // POST tUploadController/UploadFile
        [HttpPost]
        public ActionResult UploadFile(string code, int subModule, HttpPostedFileBase uploader)
        {
            try
            {
                if (uploader != null)
                {
                    var user = Functions.Deserialize<BLL.sUser>(Session["_USERDATA"].ToString());
                    string uploads = Server.MapPath("~/Uploads/");
                    if (uploader != null)
                    {
                        string fileName = Guid.NewGuid() + Path.GetExtension(uploader.FileName);
                        string filePath = Path.Combine(uploads, fileName); 
                        uploader.SaveAs(filePath); 

                        switch (subModule)
                        {
                            case 1:
                                var result1 = _BaseService.UpdateUserImage(code, fileName, user.userCode);
                                if (!result1.IsSuccess) throw new Exception(result1.Exception.Message);
                                break;
                        }
                        return Json(new { status = "success", documentName = fileName });
                    }
                    else throw new Exception("No files selected");
                }
                else throw new Exception("Uploader is null");
            }
            catch (Exception ex)
            {
                _Logger.Error(ex.Message, ex);
                return Json(new { status = "fail" });
            }
        }
    }
}
