using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CamajanSport.App_Start
{
    public class SessionHandle : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext.HttpContext.Session["Token"] == null)
            {
                // check if a new session id was generated
                filterContext.Result = new RedirectResult("~/Users/Login");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}