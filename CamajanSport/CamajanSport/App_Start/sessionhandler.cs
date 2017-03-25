using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CamajanSport.App_Start
{
    public class AuthorizationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (HttpContext.Current.Session["UserName"] == null)
            {
                HttpContext.Current.Session["UserName"] = 5;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}