using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using BLL;
using MODEL;
namespace WF.LeaveProcess
{

    public class Apply : BaseActivity
    {
        public OutArgument<bool> NeedTeamLeader { get; set; }
        public InOutArgument<MODEL.LeaveProcess> Model { get; set; }
        public InOutArgument<MODEL.User> User { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            var result = false;
            var model = Model.Get(context);
            if ((model.EndDate.Value - model.StartDate.Value).TotalDays > 7) { result = true; }
            if (LeaveProcessService.GetThisMonthTimes(User.Get(context)) > 5) { result = true; }

            NeedTeamLeader.Set(context, result);
        }
    }
}
