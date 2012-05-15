using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;

namespace WF
{

    public sealed class LeaveProcess_Disagree : CodeActivity
    {
        // 如果活动返回值，则从 CodeActivity<TResult>
        // 派生并从 Execute 方法返回该值。
        protected override void Execute(CodeActivityContext context)
        {
            // 获取 Text 输入参数的运行时值
        }
    }
}
