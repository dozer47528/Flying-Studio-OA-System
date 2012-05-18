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
    public class ProcessController : BaseController
    {
        public ActionResult Index()
        {
            var result = new List<BaseProcess>();
            var user = UserService.GetUserByCookie();
            var list = LeaveProcessService.GetAllNeedToProcess(user).Select(u => u as BaseProcess);
            var list2 = ProjectProcessService.GetAllNeedToProcess(user).Select(u => u as BaseProcess);
            result.AddRange(list);
            result.AddRange(list2);
            return View(result);
        }

        public ActionResult Finished()
        {
            var result = new List<BaseProcess>();
            var user = UserService.GetUserByCookie();
            var list = LeaveProcessService.GetAllFinishedProcess(user).Select(u => u as BaseProcess);
            var list2 = ProjectProcessService.GetAllFinishedProcess(user).Select(u => u as BaseProcess);
            result.AddRange(list);
            result.AddRange(list2);
            return View(result);
        }

        #region LeaveProcess
        public ActionResult CreateLeaveProcess()
        {
            ConvertFromUrl();
            return View(new LeaveProcess());
        }
        [HttpPost]
        public ActionResult CreateLeaveProcess(LeaveProcess leaveProcess)
        {
            if (leaveProcess.StartDate.Value > leaveProcess.EndDate.Value)
            {
                ModelState.AddModelError("StartDate", "开始时间不能晚于结束时间！");
            }

            if (ModelState.IsValid)
            {
                WorkFlowContext.CreateAndRun_LeaveProcess(leaveProcess, UserService.GetUserByCookie());
                return RedirectToAction("Index");
            }
            return View(leaveProcess);
        }
        public ActionResult ProcessLeaveProcess(int? id)
        {
            ConvertFromUrl();
            return View(LeaveProcessService.GetById(id ?? 0));
        }
        [HttpPost]
        public ActionResult ProcessLeaveProcess(int? id, bool agree)
        {
            var item = LeaveProcessService.GetById(id ?? 0);
            WorkFlowContext.RunInstance_LeaveProcess(item, agree);
            return RedirectFrom();
        }
        #endregion

        #region ProjectPrecess
        public ActionResult CreateProjectProcess()
        {
            ConvertFromUrl();
            return View(new ProjectProcess());
        }
        [HttpPost]
        public ActionResult CreateProjectProcess(ProjectProcess project)
        {
            project.Owner = UserService.GetUserByCookie();
            ProjectProcessService.Create(project);
            var guid = WorkFlowContext.CreateAndRun_ProjectProcess(project.ID);
            return RedirectFrom();
        }

        public ActionResult ProcessProjectProcess(int id)
        {
            ConvertFromUrl();

            var item = ProjectProcessService.GetByIdAndAppraisal(id);

            if (item.ProjectProcessActivity == (int)ProjectProcessActivity.可行性评估)
            {
                ViewBag.Agree = item.AppraisalResult.Count(a => a.IsAgree);
                ViewBag.Disagree = item.AppraisalResult.Count(a => !a.IsAgree);
                ViewBag.Other = UserService.Count() - (int)ViewBag.Agree - (int)ViewBag.Disagree;
                return View("AppraisalProjectProcess", item);
            }
            else
            {
                return View(item);
            }
        }
        [HttpPost]
        public ActionResult ProcessProjectProcess(int id, bool agree)
        {
            var user = UserService.GetUserByCookie();
            var item = ProjectProcessService.GetById(id);
            if (item.ProjectProcessActivity == (int)ProjectProcessActivity.可行性评估)
            {
                if (!ProjectProcessService.SetAppraisalResult(id, agree, user))
                {
                    ModelState.AddModelError("agree", "您已经投过票了!");
                }
            }
            else if (item.ProjectProcessActivity == (int)ProjectProcessActivity.技术组编写)
            {
                //上传文件
            }
            else if (item.ProjectProcessActivity == (int)ProjectProcessActivity.运营组设计)
            {
                //上传文件
            }






            WorkFlowContext.RunInstance_ProjectProcess(item, agree);

            if (ModelState.IsValid)
            {
                return RedirectFrom();
            }
            else
            {
                if (item.ProjectProcessActivity == (int)ProjectProcessActivity.可行性评估)
                {
                    return View("AppraisalProjectProcess", item);
                }
                else
                {
                    return View(item);
                }
            }
        }

        public ActionResult ProjectProcess2(int id, bool agree)
        {
            var user = UserService.GetUserByCookie();
            var project = ProjectProcessService.GetById(id);
            WorkFlowContext.RunInstance_ProjectProcess(project, agree);
            return Content("ok");
        }

        #endregion

    }
}
