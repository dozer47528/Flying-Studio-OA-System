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
            ProjectProcessService.LoadReference(item, i => i.Owner);
            item.NextProcessAuthority = (int)UserRoleEnum.技术组组长;
            item.Bookmark = bookmark;
            item.ProjectProcessActivity = (int)ProjectProcessActivity.技术组组长评估;
            ProjectProcessService.Save();

            InboxService.Create(UserRoleEnum.技术组组长, item.ID, RedirectType.项目流程处理, InboxService.CODE_FINISH_PROCESS_PROJECT, item.ID.ToString(), item.Owner.NickName);

            context.CreateBookmark(bookmark, new BookmarkCallback(this.Continue));
        }

        private void Continue(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            var args = (object[])obj;
            var userID = (int)args[0];
            UserID.Set(context, userID);
            var id = ID.Get(context);
            var item = ProjectProcessService.GetById(id);
            ProjectProcessService.LoadReference(item, i => i.Owner);

            var result = (bool)args[1];
            if (result)
            {
                //通知通过
                InboxService.Create(item.Owner, item.ID, RedirectType.项目流程处理, InboxService.FINISH_PROCESS_PROJECT, item.ID.ToString());
            }
            else
            {
                //通知未通过
                InboxService.Create(UserRoleEnum.技术组成员, item.ID, RedirectType.项目流程处理, InboxService.CODE_AGAIN_PROCESS_PROJECT, item.ID.ToString(), item.Owner.NickName);
            }
            Rusult.Set(context, result);
        }
    }
}
