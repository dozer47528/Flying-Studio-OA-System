using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using MODEL;
using BLL;

namespace WF.ProjectProcess
{

    public sealed class Designing : BaseActivity
    {
        protected override bool CanInduceIdle { get { return true; } }
        [RequiredArgument]
        public InOutArgument<int> UserID { get; set; }
        public InArgument<int> ID { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            var id = ID.Get(context);
            var bookmark = UserRoleEnum.运营组成员.ToString();
            var item = ProjectProcessService.GetById(id);
            item.NextProcessAuthority = (int)UserRoleEnum.运营组成员;
            item.Bookmark = bookmark;
            item.ProjectProcessActivity = (int)ProjectProcessActivity.运营组设计;
            ProjectProcessService.Save();

            context.CreateBookmark(bookmark, new BookmarkCallback(this.Continue));
        }

        private void Continue(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            var userID = (int)obj;
            UserID.Set(context, userID);

        }
    }
}
