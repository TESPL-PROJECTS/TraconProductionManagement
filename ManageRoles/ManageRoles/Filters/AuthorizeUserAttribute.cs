using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ManageRoles.Models;
using ManageRoles.Repository;

namespace ManageRoles.Filters
{
    public class AuthorizeUserAttribute : ActionFilterAttribute, IAuthorizationFilter
    {

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                DatabaseContext context = new DatabaseContext();

                var role = Convert.ToString(filterContext.HttpContext.Session["Role"]);

                if (!string.IsNullOrEmpty(role))
                {
                    var roleValue = Convert.ToInt32(role);

                    var roleMasterDetails = (from rolemaster in context.RoleMasters
                                             where rolemaster.RoleId == roleValue
                                             select rolemaster).FirstOrDefault();

                    if (roleMasterDetails != null && !(string.Equals(roleMasterDetails.RoleName.ToLower(), "user")))
                    {
                        filterContext.HttpContext.Session.Abandon();

                        filterContext.Result = new RedirectToRouteResult
                        (
                            new RouteValueDictionary
                                (new
                                { controller = "Login", action = "Login" }
                            ));
                    }
                }
                else
                {
                    filterContext.HttpContext.Session.Abandon();

                    filterContext.Result = new RedirectToRouteResult
                    (
                        new RouteValueDictionary
                        (new
                        { controller = "Login", action = "Login" }
                        ));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            Controller controller = filterContext.Controller as Controller;

            if (controller != null)
            {
                if (session != null && session["UserID"] == null)
                {
                    filterContext.Result =
                           new RedirectToRouteResult(new RouteValueDictionary
                                (new { controller = "Login", action = "Login" } ));
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}