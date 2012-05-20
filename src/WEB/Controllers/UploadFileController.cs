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
    public class UploadFileController : BaseController
    {

        [HttpPost]
        public ActionResult Upload(string id)
        {
            var file = Request.Files[0];
            UploadFileService.AppendFile(id, file);
            return PartialView("_UploadFileContent", id);
        }

        [HttpPost]
        public ActionResult Delete(string id, string fileID)
        {
            UploadFileService.RemoveFile(id, fileID);
            return PartialView("_UploadFileContent", id);
        }

    }
}
