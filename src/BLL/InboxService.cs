using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using MODEL;
using Webdiyer.WebControls.Mvc;

namespace BLL
{
    public class InboxService : BaseService
    {
        public InboxService(OAContext db) : base(db) { }

        #region LeaveProcessString
        public const string NEED_TO_PROCESS_LEAVE = "[{0}]由 {1} 申请的请假流程等待您审批!";
        public const string WAIT_TO_PROCESS_LEAVE = "[{0}]你的请假流程正等待 {1} 审批!";
        public const string FINISH_PROCESS_LEAVE = "[{0}]你的请假流程通过了 {1} 审批,审批结果是 {2}!";
        #endregion

        #region ProjectProcessString
        public const string APPRAISAL_PROCESS_PROJECT = "[{0}]{1} 提议了一个项目，大家看看是否可行吧!";
        public const string APPRAISAL_FINISH_PROCESS_PROJECT = "[{0}]{4} 提议了一个项目评审完毕! {1} 人同意, {2} 人不同意。最终{3}!";

        public const string DESIGN_PROCESS_PROJECT = "[{0}]{1} 提议的项目通过评审了，赶快开始设计吧!";
        public const string DESIGN_AGAIN_PROCESS_PROJECT = "[{0}]{1} 提议的项目设计未通过，请再次加工!";
        public const string DESIGN_FINISH_PROCESS_PROJECT = "[{0}]{1} 提议的项目设计完成了，需要您审核!";

        public const string CODE_PROCESS_PROJECT = "[{0}]{1} 提议的项目设计完成了，赶快开始编写吧!";
        public const string CODE_AGAIN_PROCESS_PROJECT = "[{0}]{1} 提议的项目编写未通过，请再次加工!";
        public const string CODE_FINISH_PROCESS_PROJECT = "[{0}]{1} 提议的项目编写完成了，需要您审核!";

        public const string FINISH_PROCESS_PROJECT = "[{0}]你提议的项目开发完成了!";
        #endregion

        public void Create(UserRoleEnum role, int RedirectID, RedirectType type, string format, params string[] args)
        {
            var roleInt = (int)role;
            var users = db.Users.Where(u => (u.Role.RoleEnum & roleInt) == u.Role.RoleEnum).ToList();
            foreach (var u in users)
            {
                Create(u, RedirectID, type, format, args);
            }
        }

        public void Create(User user, int RedirectID, RedirectType type, string format, params string[] args)
        {
            var inbox = new Inbox
            {
                RedirectID = RedirectID,
                RedirectType = (int)type,
                Title = string.Format(format, args),
                User = user,
            };
            db.Inboxes.Add(inbox);
            db.SaveChanges();
        }

        public void Create(int userID, int RedirectID, RedirectType type, string format, params string[] args)
        {
            var user = db.Users.Single(u => u.ID == userID);
            Create(user, RedirectID, type, format, args);
        }

        public IEnumerable<Inbox> GetByList(User user, int limit = 0)
        {
            var q = db.Inboxes.Where(i => i.User.ID == user.ID && !i.IsRead);

            if (limit > 0)
            {
                q = q.Take(limit);
            }
            return q;
        }

        public void SetRead(int inboxID)
        {
            var inbox = db.Inboxes.FirstOrDefault(i => i.ID == inboxID);
            if (inbox != null)
            {
                inbox.IsRead = true;
                db.SaveChanges();
            }
        }
    }
}
