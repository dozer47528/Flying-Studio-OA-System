using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using MODEL;
using BLL;
namespace WF.ProjectProcess
{

    public sealed class Appraisal : BaseActivity
    {
        protected override bool CanInduceIdle { get { return true; } }
        [RequiredArgument]
        public InArgument<int> ID { get; set; }
        public InOutArgument<int> UserID { get; set; }
        public OutArgument<int> Rusult { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            var id = ID.Get(context);
            var bookmark = UserRoleEnum.全员.ToString();
            var item = ProjectProcessService.GetById(id);
            item.NextProcessAuthority = (int)UserRoleEnum.全员;
            item.Bookmark = bookmark;
            item.InstanceID = context.WorkflowInstanceId;
            item.ProjectProcessActivity = (int)ProjectProcessActivity.可行性评估;
            ProjectProcessService.Save();

            //通知所有用户
            context.CreateBookmark(bookmark, new BookmarkCallback(this.Continue));
        }


        private void Continue(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            var userID = (int)obj;
            UserID.Set(context, userID);

            var result = 0;
            var id = ID.Get(context);
            var item = ProjectProcessService.GetById(id);
            var appraisalResult = ProjectProcessService.GetAppraisalResult(item);
            var userCount = UserService.Count();

            if (appraisalResult.Item1 + appraisalResult.Item2 < userCount * 0.8)//投票人数未达到所有人的 80%
            {
                result = 0;
            }
            else
            {
                if (appraisalResult.Item1 > appraisalResult.Item2 * 2)//同意的人数大于不同意人数的两倍
                {
                    result = 1;
                }
                else
                {
                    result = -1;
                }
            }
            Rusult.Set(context, result);
        }
    }
}
