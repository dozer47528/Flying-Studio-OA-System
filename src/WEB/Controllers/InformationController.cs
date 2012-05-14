using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using MODEL;
using Ninject;
using Utility;

namespace WEB.Controllers
{
    public class InformationController : BaseController
    {
        [Inject]
        public InboxService InboxService { get; set; }
        [Inject]
        public ArticleService ArticleService { get; set; }
        [Inject]
        public UserRoleService UserRoleService { get; set; }
        [Inject]
        public UploadFileService UploadFileService { get; set; }

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
            ConvertFromUrl();
            ViewBag.UserRole = UserRoleService.GetList();
            return View(new Article());
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Article article)
        {
            var saved = ArticleService.Create(article, Request["userrole"]);
            UploadFileService.SaveArticleFile(saved);
            if (ModelState.IsValid)
            {
                return RedirectFrom();
            }
            else
            {
                ViewBag.UserRole = UserRoleService.GetList();
                return View(article);
            }
        }

        public ActionResult Edit(int? id)
        {
            ConvertFromUrl();
            var article = ArticleService.GetItemById(id ?? 0);
            UploadFileService.InitArticleFiles(article.TempID.ToString(), article.ID);
            ViewBag.UserRole = UserRoleService.GetList();
            return View(article);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Article article)
        {
            var saved = ArticleService.Edit(article, Request["userrole"]);
            UploadFileService.SaveArticleFile(saved);

            if (ModelState.IsValid)
            {
                return RedirectFrom();
            }
            else
            {
                ViewBag.UserRole = UserRoleService.GetList();
                return View(article);
            }
        }

        public ActionResult Delete(int? id)
        {
            ArticleService.Delete(id ?? 0);
            return RedirectFrom();
        }

        public ActionResult Detail(int? id)
        {
            return View(ArticleService.GetItemById(id ?? 0));
        }
    }
}
