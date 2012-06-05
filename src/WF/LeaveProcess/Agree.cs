using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using BLL;
using MODEL;

namespace WF.LeaveProcess
{

    public class Agree : BaseActivity
    {
        public InOutArgument<MODEL.LeaveProcess> Model { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            var model = Model.Get(context);
            model.Result += "同意";
            model.Passed = true;
            model.Finished = true;
            LeaveProcessService.Update(model);

            InboxService.Create(model.Owner.ID, model.ID, RedirectType.请假流程查看, InboxService.FINISH_PROCESS_LEAVE, model.ID.ToString(), ((UserRoleEnum)model.NextProcessAuthority).ToString(), "同意");

        }
    }
}
