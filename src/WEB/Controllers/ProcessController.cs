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
            var list = LeaveProcessService.GetAllNeedToProcess(UserService.GetUserByCookie()).Select(u => u as BaseProcess);
            result.AddRange(list);
            return View(result);
        }

        public ActionResult Finished()
        {
            var result = new List<BaseProcess>();
            var list = LeaveProcessService.GetAllFinishedProcess(UserService.GetUserByCookie()).Select(u => u as BaseProcess);
            result.AddRange(list);
            return View(result);
        }

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
        public ActionResult PostLeaveProcess(int? id)
        {
            var item = LeaveProcessService.GetById(id ?? 0);
            WorkFlowContext.RunInstance_LeaveProcess(item, (Request["agree"] ?? "") == "1");
            return RedirectToAction("Index");
        }
    }
}
