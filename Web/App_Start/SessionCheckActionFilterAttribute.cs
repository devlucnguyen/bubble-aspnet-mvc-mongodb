﻿using Web.WebUtilities;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web.App_Start
{
    public class SessionCheckActionFilterAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (NeedToCheckSession(filterContext))
            {
                if (SessionUtility.IsSessionAlive() == false)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        { "action", "Login" },
                        { "controller", "Account" }
                    });
                }
            }
        }
        private bool NeedToCheckSession(ActionExecutingContext filterContext)
        {
            var currentArea = filterContext.RouteData.DataTokens["area"] ?? string.Empty;
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            var actionName = filterContext.ActionDescriptor.ActionName;

            if ((controllerName.Equals("Account") && actionName.Equals("Login")) || (controllerName.Equals("Account") && actionName.Equals("Register")))
                return false;

            return true;
        }
    }
}