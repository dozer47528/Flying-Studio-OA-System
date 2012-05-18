using System;
using System.Activities;
using System.Activities.DurableInstancing;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.DurableInstancing;
using System.Text;
using System.Threading;
using MODEL;

namespace WF
{
    public static class WorkFlowContext
    {
        public static string ConnectString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            }
        }

        #region LeaveProcess
        public static Guid CreateAndRun_LeaveProcess(MODEL.LeaveProcess model, MODEL.User user)
        {
            return CreateAndRun(
                new Dictionary<string, object> {
                { "LeaveProcessModel", model },
                { "User", user } },
                new LeaveProcess.LeaveProcess());
        }
        public static void RunInstance_LeaveProcess(MODEL.LeaveProcess model, bool isAgree)
        {
            RunInstance(model.InstanceID, model.Bookmark, new LeaveProcess.LeaveProcess(), isAgree);
        }
        #endregion

        #region ProjectProcess
        public static Guid CreateAndRun_ProjectProcess(int id)
        {
            return CreateAndRun(
                new Dictionary<string, object> { { "ProjectID", id } },
                new ProjectProcess.ProjectProcess());
        }
        public static void RunInstance_ProjectProcess(MODEL.ProjectProcess model, bool isAgree = false)
        {
            RunInstance(model.InstanceID, model.Bookmark, new ProjectProcess.ProjectProcess(), isAgree);
        }
        #endregion

        #region Private
        private static Guid CreateAndRun(IDictionary<string, object> data, Activity activity)
        {
            SqlWorkflowInstanceStore instanceStore = new SqlWorkflowInstanceStore(ConnectString);
            WorkflowApplication application = new WorkflowApplication(activity, data);
            application.InstanceStore = instanceStore;


            application.PersistableIdle = (e) =>
            {
                return PersistableIdleAction.Unload;
            };

            application.Persist();
            Guid id = application.Id;

            application.Run();
            return id;
        }
        private static void RunInstance(Guid guid, string bookmark, Activity activity, object args)
        {
            SqlWorkflowInstanceStore instanceStore = new SqlWorkflowInstanceStore(ConnectString);
            WorkflowApplication application = new WorkflowApplication(activity);
            application.InstanceStore = instanceStore;


            application.PersistableIdle = (e) =>
            {
                return PersistableIdleAction.Unload;
            };

            application.Load(guid);
            application.ResumeBookmark(bookmark, args);
        }

        #endregion
    }
}
