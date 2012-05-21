using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using MODEL;
using WF;

namespace WEB.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login() {
            Response.Write("<script>alert('请点右上角的“登录”按钮登录！');</script>");
            return View("Index");
        }
    }
}
