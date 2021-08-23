using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ANTS.Authentication
{
    public class ManagerAuthentication : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
                if (filterContext.HttpContext.Session["user_type"] == null)
                {
                    filterContext.Controller.TempData["ErrorMessage"] = "Please Login In!";
                    filterContext.Result = new RedirectResult("/Login/Index");
                }
                else if (!filterContext.HttpContext.Session["user_type"].Equals("Manager"))
                {
                    filterContext.Controller.TempData["ErrorMessage"] = "Invalid Request!";
                    filterContext.Result = new RedirectResult("/Login/Index");
                }
                base.OnActionExecuting(filterContext);
        }
    }
}