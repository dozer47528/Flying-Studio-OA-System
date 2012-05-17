using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using BLL;

namespace WF.ProjectProcess
{

    public sealed class End : CodeActivity
    {
        [RequiredArgument]
        public InArgument<int> ID { get; set; }
        private ProjectProcessService ProjectProcessService = new ProjectProcessService();
        protected override void Execute(CodeActivityContext context)
        {
        }
    }
}
