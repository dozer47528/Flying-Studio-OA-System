using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class UserController : BaseController
    {
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var result = UserService.Login(username, password);
            return Json(new { Result = result });
        }
        [HttpPost]
        public ActionResult Logout()
        {
            UserService.Logout();
            return Json(new { Result = true });
        }
    }
}
