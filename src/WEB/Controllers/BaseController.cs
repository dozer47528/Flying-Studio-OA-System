using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using Utility;

namespace WEB.Controllers
{
    public class BaseController : Controller
    {
        public UserService UserService = new UserService();
        public InboxService InboxService = new InboxService();
        public ArticleService ArticleService = new ArticleService();
        public UserRoleService UserRoleService = new UserRoleService();
        public UploadFileService UploadFileService = new UploadFileService();
        public LeaveProcessService LeaveProcessService = new LeaveProcessService();


        protected void ConvertFromUrl()
        {
            if (string.IsNullOrEmpty(Request["from"]) && Request.UrlReferrer != null)
            {
                Response.Redirect(MvcUrlHelper.GetFromUrl(Request.Url.ToString(), Request.UrlReferrer.ToString()));
            }
        }

        protected ActionResult RedirectFrom()
        {
            if (!string.IsNullOrEmpty(Request["from"]))
            {
                return Redirect(Uri.UnescapeDataString(Request["from"]));
            }
            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
