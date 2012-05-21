using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using MODEL;
using WEB.Filters;
namespace WEB.Controllers
{
    [TheAuthorizationFilter(AllowRoles = UserRoleEnum.全员)]
    public class InformationController : BaseController
    {

        public ActionResult Index(int? id)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Index", ArticleService.GetAuthorizedList(UserService.GetUserByCookie(), id ?? 1));
            }
            return View(ArticleService.GetAuthorizedList(UserService.GetUserByCookie(), id ?? 1));
        }

        public ActionResult Create()
        {
            if (ConvertFromUrl()) return null;
            ViewBag.UserRole = UserRoleService.GetList();
            return View(new Article { Authority = (int)UserRoleEnum.全员 });
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Article article)
        {
            var user = UserService.GetUserByCookie();
            article.Owner = user;
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
            if (ConvertFromUrl()) return null;
            var user = UserService.GetUserByCookie();
            var article = ArticleService.GetItemById(id ?? 0);
            if (!ArticleService.CheckMyOwnOrAdmin(article, user)) return AlertAndRedirect("你没有权限！", "Index", "Information");

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

        public ActionResult Delete(int id)
        {
            var user = UserService.GetUserByCookie();
            if (!ArticleService.Delete(id, user)) return AlertAndRedirect("你没有权限！", "Index", "Information");
            return RedirectFrom();
        }

        public ActionResult Detail(int? id)
        {
            return View(ArticleService.GetItemByIdWithAttachment(id ?? 0));
        }
    }
}
