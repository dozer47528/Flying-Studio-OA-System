using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utility;

namespace WEB.Controllers
{
    public class BaseController : Controller
    {
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
