﻿using System;
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
        public LeaveProcess GetById(int id)
        {
            return db.LeaveProcessSet.Single(l => l.ID == id);
        }
        public void Create(LeaveProcess model)
        {
            model.Adddate = DateTime.Now;
            model.Owner = db.UserSet.Single(u => u.ID == model.Owner.ID);
            db.LeaveProcessSet.Add(model);
            db.SaveChanges();
        }

        public void Update(LeaveProcess model)
        {
            var old = db.LeaveProcessSet.Single(l => l.InstanceID == model.InstanceID);
            old.Bookmark = model.Bookmark;
            old.NextProcessAuthority = model.NextProcessAuthority;
            old.Result = model.Result;
            old.Finished = model.Finished;
            old.Passed = model.Passed;
            db.SaveChanges();
        }

        public List<LeaveProcess> GetAllNeedToProcess(User user)
        {
            var q = from l in db.LeaveProcessSet
                    where (l.NextProcessAuthority & user.ID) == user.ID && l.Finished == false
                    select l;
            return q.ToList();
        }
        public List<LeaveProcess> GetAllFinishedProcess(User user)
        {
            var q = from l in db.LeaveProcessSet
                    where l.Owner.ID == user.ID && l.Finished == true
                    select l;
            return q.ToList();
        }
    }
}