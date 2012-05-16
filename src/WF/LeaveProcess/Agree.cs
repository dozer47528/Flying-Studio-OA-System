using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using BLL;

namespace WF.LeaveProcess
{

    public class Agree : CodeActivity
    {
        public InOutArgument<MODEL.LeaveProcess> Model { get; set; }
        protected LeaveProcessService LeaveProcessService = new LeaveProcessService();
        protected override void Execute(CodeActivityContext context)
        {
            var model = Model.Get(context);
            model.Result += "同意";
            model.Passed = true;
            model.Finished = true;
            LeaveProcessService.Update(model);
        }
    }
}
