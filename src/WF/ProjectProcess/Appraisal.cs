using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using MODEL;
namespace WF.ProjectProcess
{

    public sealed class Appraisal : NativeActivity
    {
        public OutArgument<int> Rusult { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            //通知所有用户
            context.CreateBookmark(UserRoleEnum.全员.ToString(), new BookmarkCallback(this.Continue));
        }


        private void Continue(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            var result = obj as AppraisalResult;
            //判断是否最终意见
            Rusult.Set(context, 1);
        }
    }
}
