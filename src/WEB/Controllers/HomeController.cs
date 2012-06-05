using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using MODEL;
using WEB.Filters;
using WF;

namespace WEB.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController()
        {
            ViewBag.Logined = UserService.GetUserByCookie() != null;
        }

        [TheAuthorizationFilter(AllowRoles = UserRoleEnum.全员)]
        public ActionResult Index()
        {
            ViewBag.Inboxes = InboxService.GetByList(UserService.GetUserByCookie(), 10);
            var result = new List<BaseProcess>();
            var user = UserService.GetUserByCookie();
            var list = LeaveProcessService.GetAllNeedToProcess(user).Select(u => u as BaseProcess);
            var list2 = ProjectProcessService.GetAllNeedToProcess(user).Select(u => u as BaseProcess);
            result.AddRange(list);
            result.AddRange(list2);
            ViewBag.Process = result.OrderBy(p => p.Adddate);
            return View();
        }

        public ActionResult Login()
        {
            Response.Write("<script>alert('请点右上角的“登录”按钮登录！');</script>");
            return View();
        }
    }
}
