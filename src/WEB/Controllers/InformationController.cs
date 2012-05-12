using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using MODEL;
using Ninject;

namespace WEB.Controllers
{
    public class InformationController : Controller
    {
        [Inject]
        public InboxService InboxService { get; set; }
        [Inject]
        public ArticleService ArticleService { get; set; }
        [Inject]
        public UserRoleService UserRoleService { get; set; }

        public ActionResult Index(int? id)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Index", ArticleService.GetList(id ?? 1));
            }
            return View(ArticleService.GetList(id ?? 1));
        }

        public ActionResult Create()
        {
            ViewBag.UserRole = UserRoleService.GetList();

            return View();
        }
        [HttpPost]
        public ActionResult Create(Article article)
        {
            return View();
        }
    }
}
