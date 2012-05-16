using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using BLL;
using MODEL;
namespace WF.LeaveProcess
{

    public class Apply : CodeActivity
    {
        public OutArgument<bool> NeedTeamLeader { get; set; }
        public InOutArgument<MODEL.LeaveProcess> Model { get; set; }
        protected LeaveProcessService LeaveProcessService = new LeaveProcessService();

        protected override void Execute(CodeActivityContext context)
        {
            var result = false;
            var model = Model.Get(context);
            if ((model.EndDate.Value - model.StartDate.Value).TotalDays > 7) { result = true; }


            NeedTeamLeader.Set(context, result);
        }
    }
}
