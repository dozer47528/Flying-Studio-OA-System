﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using BLL;
using MODEL;

namespace WF.ProjectProcess
{

    public sealed class End : BaseActivity
    {
        [RequiredArgument]
        public InOutArgument<int> UserID { get; set; }
        public InArgument<int> ID { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            var id = ID.Get(context);
            var item = ProjectProcessService.GetById(id);
            item.ProjectProcessActivity = (int)ProjectProcessActivity.完成;
            item.Finished = true;
            item.Passed = false;
            ProjectProcessService.Save();
        }
    }
}
