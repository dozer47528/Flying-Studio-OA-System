using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using MODEL;
using BLL;
namespace WF.ProjectProcess
{

    public sealed class Appraisal : NativeActivity
    {
        [RequiredArgument]
        public InArgument<int> ID { get; set; }
        public OutArgument<int> Rusult { get; set; }

        private ProjectProcessService ProjectProcessService = new ProjectProcessService();
        protected override void Execute(NativeActivityContext context)
        {
            var id = ID.Get(context);
            var bookmark = UserRoleEnum.全员.ToString();
            ProjectProcessService.SetBookmark(id, bookmark);


            //通知所有用户
            
            
            context.CreateBookmark(bookmark, new BookmarkCallback(this.Continue));
        }


        private void Continue(NativeActivityContext context, Bookmark bookmark, object obj)
        {
            //判断是否最终意见
            Rusult.Set(context, 1);
        }
    }
}
