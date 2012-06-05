using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLL;
using DAL;

namespace WF
{
    public abstract class BaseActivity : NativeActivity
    {
        protected OAContext db = new OAContext();
        protected readonly UserService UserService;
        protected readonly CheckInService CheckInService;
        protected readonly ArticleService ArticleService;
        protected readonly UserRoleService UserRoleService;
        protected readonly UploadFileService UploadFileService;
        protected readonly LeaveProcessService LeaveProcessService;
        protected readonly ProjectProcessService ProjectProcessService;
        protected readonly InboxService InboxService;

        public BaseActivity()
        {
            UserService = new UserService(db);
            CheckInService = new CheckInService(db);
            ArticleService = new ArticleService(db);
            UserRoleService = new UserRoleService(db);
            UploadFileService = new UploadFileService(db);
            LeaveProcessService = new LeaveProcessService(db);
            ProjectProcessService = new ProjectProcessService(db);
            InboxService = new InboxService(db);
        }
    }
}
