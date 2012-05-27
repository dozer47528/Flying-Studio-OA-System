using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using MODEL;
using BLL;

namespace WF.ProjectProcess
{

    public sealed class CodingReview : BaseActivity
    {
        protected override bool CanInduceIdle { get { return true; } }
        [RequiredArgument]
        public InArgument<int> ID { get; set; }
        public InOutArgument<int> UserID { get; set; }
        public OutArgument<bool> Rusult { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            var id = ID.Get(context);
            var bookmark = UserRoleEnum.技术组组长.ToString();

            var item = ProjectProcessService.GetById(id);
            item.NextProcessAuthority = (int)UserRoleEnum.技术组组长;
            item.Bookmark = bookmark;
            item.ProjectProcessActivity = (int)ProjectProcessActivity.技术组组长评估;
            ProjectProcessService.Save();


            context.CreateBookmark(bookmark, new BookmarkCallback(this.Continue));
        }

        private void Continue(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            var userID = (int)obj;
            UserID.Set(context, userID);


            var result = (bool)obj;
            if (result)
            {
                //通知通过
            }
            else
            {
                //通知未通过
            }
            Rusult.Set(context, result);
        }
    }
}
