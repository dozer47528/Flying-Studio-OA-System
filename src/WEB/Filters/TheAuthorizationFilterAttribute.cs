using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BLL;
using DAL;
using MODEL;
using Utility;

namespace WEB.Filters
{
    public class TheAuthorizationFilterAttribute : AuthorizeAttribute
    {
        protected OAContext db
        {
            get
            {
                var context = new HttpContextWrapper(System.Web.HttpContext.Current);
                if (context.Items["OAContext"] == null)
                {
                    context.Items["OAContext"] = new OAContext();
                }
                return context.Items["OAContext"] as OAContext;
            }
        }
        protected readonly UserService UserService;


        protected RouteValueDictionary RouteValues { get; set; }
        protected HttpSessionStateBase Session { get { return new HttpContextWrapper(HttpContext.Current).Session; } }
        protected HttpRequestBase Request { get { return new HttpContextWrapper(HttpContext.Current).Request; } }

        protected int allowRoles { get { return (int)AllowRoles; } }
        public UserRoleEnum AllowRoles { get; set; }


        public TheAuthorizationFilterAttribute()
        {
            UserService = new UserService(db);
            setRouteValues("Login", "Home", "", new string[0]);
        }
        private void setRouteValues(string action, string controller, string area, string[] args)
        {
            RouteValues = new RouteValueDictionary { { "action", action } };
            if (!string.IsNullOrEmpty(controller)) RouteValues.Add("controller", controller);
            if (!string.IsNullOrEmpty(area)) RouteValues.Add("area", area);
            for (var k = 0; k < args.Length; k += 2)
            {
                RouteValues.Add(args[k], args[k + 1]);
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = UserService.GetUserByCookie();
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(RouteValues);
                return;
            }

            var roleEnum = user.Role.RoleEnum;
            if ((allowRoles & roleEnum) == roleEnum) return;

            filterContext.Result = new RedirectToRouteResult(RouteValues);
        }
    }
}