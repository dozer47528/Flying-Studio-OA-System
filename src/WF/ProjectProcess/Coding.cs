using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using MODEL;
using BLL;

namespace WF.ProjectProcess
{

    public sealed class Coding : BaseActivity
    {
        protected override bool CanInduceIdle { get { return true; } }
        [RequiredArgument]
        public InOutArgument<int> UserID { get; set; }
        public InArgument<int> ID { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            var id = ID.Get(context);
            var bookmark = UserRoleEnum.技术组成员.ToString();

            var item = ProjectProcessService.GetById(id);
            item.NextProcessAuthority = (int)UserRoleEnum.技术组成员;
            item.Bookmark = bookmark;
            item.ProjectProcessActivity = (int)ProjectProcessActivity.技术组编写;
            ProjectProcessService.Save();


            context.CreateBookmark(bookmark, new BookmarkCallback(this.Continue));
        }

        private void Continue(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            var args = (object[])obj;
            var userID = (int)args[0];
            UserID.Set(context, userID);

        }
    }
}
