using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using MODEL;
using BLL;

namespace WF.ProjectProcess
{

    public sealed class CodingReview : NativeActivity
    {
        [RequiredArgument]
        public InArgument<int> ID { get; set; }
        public OutArgument<bool> Rusult { get; set; }
        private ProjectProcessService ProjectProcessService = new ProjectProcessService();
        protected override void Execute(NativeActivityContext context)
        {
            var id = ID.Get(context);
            var bookmark = UserRoleEnum.技术组组长.ToString();
            ProjectProcessService.SetBookmark(id, bookmark);


            //通知所有用户


            context.CreateBookmark(bookmark, new BookmarkCallback(this.Continue));
        }

        private void Continue(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            //完成
            Rusult.Set(context, true);
        }
    }
}
