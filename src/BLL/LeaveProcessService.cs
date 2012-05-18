using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DAL;
using MODEL;
using Ninject;
using Webdiyer.WebControls.Mvc;

namespace BLL
{
    public class LeaveProcessService : BaseService
    {
        public LeaveProcessService(OAContext db) : base(db) { }
        public LeaveProcess GetById(int id)
        {
            return db.LeaveProcesses.Single(l => l.ID == id);
        }
        public void Create(LeaveProcess model)
        {
            model.Adddate = DateTime.Now;
            model.Owner = db.Users.Single(u => u.ID == model.Owner.ID);
            db.LeaveProcesses.Add(model);
            db.SaveChanges();
        }

        public void Update(LeaveProcess model)
        {
            var old = db.LeaveProcesses.Single(l => l.InstanceID == model.InstanceID);
            old.Bookmark = model.Bookmark;
            old.NextProcessAuthority = model.NextProcessAuthority;
            old.Result = model.Result;
            old.Finished = model.Finished;
            old.Passed = model.Passed;
            db.SaveChanges();
        }

        public List<LeaveProcess> GetAllNeedToProcess(User user)
        {
            var q = from l in db.LeaveProcesses.Include("Owner")
                    where (l.NextProcessAuthority & user.ID) == user.ID && l.Finished == false
                    select l;
            return q.ToList();
        }
        public List<LeaveProcess> GetAllFinishedProcess(User user)
        {
            var q = from l in db.LeaveProcesses.Include("Owner")
                    where l.Owner.ID == user.ID && l.Finished == true
                    select l;
            return q.ToList();
        }

        /// <summary>
        /// 得到这个月的请假天数
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int GetThisMonthTimes(User user)
        {
            var thisMonth = DateTime.Now.Date.AddDays(-DateTime.Now.Day);
            var list = db.LeaveProcesses.Where(l =>
                l.Owner.ID == user.ID &&
                l.Finished == true &&
                l.Passed == true &&
                l.StartDate.Value >= thisMonth //当月的请假申请
                ).ToList();
            var result = 0;
            foreach (var l in list)
            {
                result += (int)(l.EndDate.Value - l.StartDate.Value).TotalDays + 1;
            }

            return result;
        }
    }
}
