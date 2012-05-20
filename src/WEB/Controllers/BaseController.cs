using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DAL;
using MODEL;
using Utility;

namespace WEB.Controllers
{
    public class BaseController : Controller
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
        protected readonly CheckInService InboxService;
        protected readonly ArticleService ArticleService;
        protected readonly UserRoleService UserRoleService;
        protected readonly UploadFileService UploadFileService;
        protected readonly LeaveProcessService LeaveProcessService;
        protected readonly ProjectProcessService ProjectProcessService;
        protected readonly CheckInService CheckInService;

        public BaseController()
        {
            UserService = new UserService(db);
            InboxService = new CheckInService(db);
            ArticleService = new ArticleService(db);
            UserRoleService = new UserRoleService(db);
            UploadFileService = new UploadFileService(db);
            LeaveProcessService = new LeaveProcessService(db);
            ProjectProcessService = new ProjectProcessService(db);
            CheckInService = new CheckInService(db);

            ViewBag.IsLogin = UserService.GetUserByCookie() != null;
        }

        protected bool ConvertFromUrl()
        {
            if (string.IsNullOrEmpty(Request["from"]) && Request.UrlReferrer != null && Request.UrlReferrer.ToString() != Request.Url.ToString())
            {
                Response.Redirect(MvcUrlHelper.GetFromUrl(Request.Url.ToString(), Request.UrlReferrer.ToString()));
                return true;
            }
            return false;
        }

        protected ActionResult RedirectFrom()
        {
            if (!string.IsNullOrEmpty(Request["from"]))
            {
                return Redirect(Uri.UnescapeDataString(Request["from"]));
            }
            return RedirectToAction("Index");
        }


        protected ActionResult AlertAndRedirect(string msg, string action, string controller)
        {
            return AlertAndRedirect(msg, Url.Action(action, controller));
        }
        protected ActionResult AlertAndRedirect(string msg, string url)
        {
            return Content(string.Format(@"<script>alert('{0}')</script>
                                           <script>location.href='{1}'</script>", msg, url));
        }


    }
}
