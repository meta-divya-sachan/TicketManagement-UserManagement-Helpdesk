using TicketManagement.Filtros;
using System.Web.Mvc; 

namespace TicketManagement.Controllers
{
    [SessionExpire]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Index2()
        {
            return View();
        }

        public ActionResult Index3()
        {
            return View();
        }
    }
}
