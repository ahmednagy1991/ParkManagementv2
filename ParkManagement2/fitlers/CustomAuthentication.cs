using ParkManagement2.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ParkManagement2.fitlers
{
    public class CustomAuthentication : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var cookie = httpContext.Request.Cookies["ParkManagement_token"];           
            if (cookie != null&& TokenManager.ValidateToken(cookie.Value)!=null)
            {
                return true;
            }
            return false;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
            new RouteValueDictionary {{ "Controller", "Account" },
                                      { "Action", "login" } });
        }
    }
}