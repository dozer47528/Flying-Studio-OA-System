using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using MODEL;

namespace WF.ProjectProcess
{

    public sealed class DesigningReview : NativeActivity
    {
        public OutArgument<bool> Rusult { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            //通知
            context.CreateBookmark(UserRoleEnum.运营组组长.ToString(), new BookmarkCallback(this.Continue));
        }

        private void Continue(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            //完成
            Rusult.Set(context, true);
        }
    }
}
