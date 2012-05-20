using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MODEL;
using Newtonsoft.Json;
using WEB.Filters;

namespace WEB.Controllers
{
    [TheAuthorizationFilter(AllowRoles = UserRoleEnum.全员)]
    public class AttendanceController : BaseController
    {
        //
        // GET: /Attendance/

        public ActionResult Index()
        {
            var result = CheckInService.GetCheckInJsonData(UserService.GetUserByCookie(), new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1), DateTime.Now);
            ViewBag.All = result.Item1;
            ViewBag.Late = result.Item2;
            ViewBag.Early = result.Item3;
            return View();
        }

        public ActionResult All()
        {
            ViewBag.Users = UserService.GetSubordinates(UserService.GetUserByCookie()).Select(u => new SelectListItem { Text = u.NickName, Value = u.ID.ToString() });
            ViewBag.Start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ViewBag.End = DateTime.Now;
            return View();
        }

        [HttpPost]
        public ActionResult All(int users, DateTime start, DateTime end)
        {
            if (start > end)
            {
                ModelState.AddModelError("date", "开始时间不能晚于结束时间！");
                return View();
            }

            ViewBag.Users = UserService.GetSubordinates(UserService.GetUserByCookie()).Select(u => new SelectListItem { Text = u.NickName, Value = u.ID.ToString(), Selected = u.ID == users });
            var result = CheckInService.GetCheckInJsonData(UserService.GetById(users), start, end);
            ViewBag.All = result.Item1;
            ViewBag.Late = result.Item2;
            ViewBag.Early = result.Item3;

            ViewBag.Start = start;
            ViewBag.End = end;
            return View();
        }

        [HttpPost]
        public ActionResult CheckIn()
        {
            return Json(new { Result = CheckInService.CheckIn(UserService.GetUserByCookie()) });
        }

        [HttpPost]
        public ActionResult CheckOut()
        {
            return Json(new { Result = CheckInService.CheckOut(UserService.GetUserByCookie()) });
        }
    }
}
