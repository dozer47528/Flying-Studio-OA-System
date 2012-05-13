using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WEB.Controllers
{
    public class UploadFileController : Controller
    {
        [HttpPost]
        public ActionResult Upload(string id)
        {
            var file = Request.Files[0];
            return Content("asss");
        }
    }
}
