using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TicketManagement.Helpers;

namespace TicketManagement.Filtros
{
    public class SessionExpire : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["_USERDATA"] == null || string.IsNullOrEmpty(HttpContext.Current.Session["_USERDATA"].ToString()))
            {
                filterContext.Result =
                    new RedirectToRouteResult(new RouteValueDictionary
                    {
                        { "action", "Login" },
                        { "controller", "Users" },
                        //{ "returnUrl", filterContext.HttpContext.Request.RawUrl}
                    });
                return;
            }
        }
    }
    public class AdminRole : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool isAdmin = false;
            if (HttpContext.Current.Session["_USERROLES"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["_USERROLES"].ToString()))
            {
                var roles = Functions.Deserialize<List<BLL.sRole>>(HttpContext.Current.Session["_USERROLES"].ToString());
                foreach (var role in roles)
                {
                    if (Functions.ToBool(role.isAdmin) || Functions.ToBool(role.isSuperadmin))
                    {
                        isAdmin = true;
                        break;
                    }

                }
            }
            if (!isAdmin)
            {
                filterContext.Result =
                    new RedirectToRouteResult(new RouteValueDictionary
                    {
                        { "action", "Login" },
                        { "controller", "Users" },
                    });
                return;
            }

        }
    }
}