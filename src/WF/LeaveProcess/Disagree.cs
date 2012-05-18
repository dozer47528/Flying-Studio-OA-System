using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using BLL;

namespace WF.LeaveProcess
{

    public class Disagree : BaseActivity
    {
        public InOutArgument<MODEL.LeaveProcess> Model { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            var model = Model.Get(context);
            model.Result += "不同意";
            model.Finished = true;
            model.Passed = false;
            LeaveProcessService.Update(model);

        }
    }
}
