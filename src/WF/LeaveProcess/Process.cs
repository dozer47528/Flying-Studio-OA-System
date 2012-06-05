using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using MODEL;
using BLL;

namespace WF.LeaveProcess
{

    public class Process : BaseActivity
    {
        protected override bool CanInduceIdle
        {
            get
            {
                return true;
            }
        }
        public UserRoleEnum Role { get; set; }
        public InOutArgument<MODEL.User> User { get; set; }
        public OutArgument<bool> Agree { get; set; }
        public InOutArgument<MODEL.LeaveProcess> Model { get; set; }


        protected override void Execute(NativeActivityContext context)
        {
            var model = Model.Get(context);
            model.InstanceID = context.WorkflowInstanceId;
            model.Owner = User.Get(context);
            if (Role == UserRoleEnum.组长)
            {
                model.NextProcessAuthority = (int)UserService.LoadFartherRole(model.Owner).RoleEnum;
            }
            else
            {
                model.NextProcessAuthority = (int)Role;
            }
            model.Bookmark = Role.ToString();


            if (model.ID > 0)
            {
                LeaveProcessService.Update(model);
            }
            else
            {
                LeaveProcessService.Create(model);
            }
            Model.Set(context, model);

            InboxService.Create((UserRoleEnum)model.NextProcessAuthority, model.ID, RedirectType.请假流程处理, InboxService.NEED_TO_PROCESS_LEAVE, model.ID.ToString(), model.Owner.NickName);
            InboxService.Create(model.Owner.ID, model.ID, RedirectType.请假流程查看, InboxService.WAIT_TO_PROCESS_LEAVE, model.ID.ToString(), ((UserRoleEnum)model.NextProcessAuthority).ToString());
            context.CreateBookmark(Role.ToString(),
                new BookmarkCallback(this.Continue));
        }

        protected void Continue(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            var result = (bool)obj;
            Agree.Set(context, result);

         }
    }
}
