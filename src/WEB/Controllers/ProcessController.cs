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
    [TheAuthorizationFilter(AllowRoles = UserRoleEnum.全员)]
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
            return View(result.OrderBy(p => p.Adddate).ToList());
        }

        public ActionResult My()
        {
            var result = new List<BaseProcess>();
            var user = UserService.GetUserByCookie();
            var list = LeaveProcessService.GetMyProcess(user).Select(u => u as BaseProcess);
            var list2 = ProjectProcessService.GetMyProcess(user).Select(u => u as BaseProcess);
            result.AddRange(list);
            result.AddRange(list2);
            return View(result.OrderByDescending(p => p.Adddate).ToList());
        }

        #region LeaveProcess
        public ActionResult CreateLeaveProcess()
        {
            if (ConvertFromUrl()) return null;
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
            if (ConvertFromUrl()) return null;
            var item = LeaveProcessService.GetById(id ?? 0);
            var user = UserService.GetUserByCookie();
            if ((item.NextProcessAuthority & user.Role.RoleEnum) == user.Role.RoleEnum && !item.Finished)
            {
                return View(item);
            }
            else
            {
                return RedirectToAction("ViewLeaveProcess", new { id = id ?? 0 });
            }
        }
        [HttpPost]
        public ActionResult ProcessLeaveProcess(int? id, bool agree)
        {
            var item = LeaveProcessService.GetById(id ?? 0);
            WorkFlowContext.RunInstance_LeaveProcess(item, agree);
            return RedirectFrom();
        }

        public ActionResult ViewLeaveProcess(int id)
        {
            return View(LeaveProcessService.GetById(id));
        }
        #endregion

        #region ProjectPrecess
        public ActionResult ViewProjectProcess(int id)
        {
            return View(ProjectProcessService.GetById(id));
        }
        public ActionResult CreateProjectProcess()
        {
            if (ConvertFromUrl()) return null;
            return View(new ProjectProcess());
        }
        [HttpPost]
        public ActionResult CreateProjectProcess(ProjectProcess project)
        {
            project.Owner = UserService.GetUserByCookie();
            ProjectProcessService.Create(project);
            var guid = WorkFlowContext.CreateAndRun_ProjectProcess(project.ID, project.Owner.ID);
            return RedirectFrom();
        }

        public ActionResult ProcessProjectProcess(int id)
        {
            if (ConvertFromUrl()) return null;
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
                var user = UserService.GetUserByCookie();
                if ((item.NextProcessAuthority & user.Role.RoleEnum) == user.Role.RoleEnum && !item.Finished)
                {
                    return View(item);
                }
                else
                {
                    return RedirectToAction("ViewProjectProcess", new { id = id });
                }

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
            else if (item.ProjectProcessActivity == (int)ProjectProcessActivity.技术组编写 ||
                     item.ProjectProcessActivity == (int)ProjectProcessActivity.运营组设计)
            {
                if (Request.Files.Count == 0)
                {
                    ModelState.AddModelError("file", "请上传文件!");
                }
                else
                {
                    ProjectProcessService.UpLoadFile(item, Request.Files[0]);
                }
            }



            if (ModelState.IsValid)
            {
                WorkFlowContext.RunInstance_ProjectProcess(item, user.ID, agree);
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
        #endregion


        public ActionResult Redirect(int id, int type, int inboxID)
        {
            InboxService.SetRead(inboxID);

            var redirectType = (RedirectType)type;
            switch (redirectType)
            {
                case RedirectType.请假流程查看:
                    {
                        return RedirectToAction("ViewLeaveProcess", new { id = id });
                    }
                case RedirectType.请假流程处理:
                    {
                        return RedirectToAction("ProcessLeaveProcess", new { id = id });
                    }

                case RedirectType.项目流程查看:
                    {
                        return RedirectToAction("ViewProjectProcess", new { id = id });
                    }
                case RedirectType.项目流程处理:
                    {
                        return RedirectToAction("ProcessProjectProcess", new { id = id });
                    }
                default:
                    {
                        return RedirectToAction("Index");

                    }

            }
        }
    }
}
