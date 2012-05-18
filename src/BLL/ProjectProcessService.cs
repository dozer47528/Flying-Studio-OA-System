using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using MODEL;

namespace BLL
{
    public class ProjectProcessService : BaseService
    {
        public ProjectProcessService(OAContext db) : base(db) { }

        public List<ProjectProcess> GetAllFinishedProcess(User user)
        {
            var q = from l in db.ProjectProcesses.Include("Owner")
                    where l.Owner.ID == user.ID && l.Finished == true
                    select l;
            return q.ToList();
        }
        public List<ProjectProcess> GetAllNeedToProcess(User user)
        {
            var q = from l in db.ProjectProcesses.Include("Owner")
                    where (l.NextProcessAuthority & user.ID) == user.ID && l.Finished == false
                    select l;
            return q.ToList();
        }

        public ProjectProcess GetById(int id)
        {
            return db.ProjectProcesses.Single(p => p.ID == id);
        }
        public ProjectProcess GetByIdAndAppraisal(int id)
        {
            return db.ProjectProcesses.Include("AppraisalResult").Single(p => p.ID == id);
        }


        public void Create(ProjectProcess project)
        {
            project.Adddate = DateTime.Now;
            db.ProjectProcesses.Add(project);
            db.SaveChanges();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public bool SetAppraisalResult(int id, bool isAgree, User user)
        {
            if (db.AppraisalResults.Any(a => a.User.ID == user.ID && a.ProjectProcess.ID == id))
            {
                return false;
            }

            var project = db.ProjectProcesses.Single(p => p.ID == id);
            var result = new AppraisalResult
            {
                IsAgree = isAgree,
                ProjectProcess = project,
                User = user,
            };
            db.AppraisalResults.Add(result);
            db.SaveChanges();
            return true;
        }

        public Tuple<int, int> GetAppraisalResult(ProjectProcess project)
        {
            db.Entry(project).Collection(p => p.AppraisalResult).Load();
            return new Tuple<int, int>(project.AppraisalResult.Count(p => p.IsAgree == true), project.AppraisalResult.Count(p => p.IsAgree == false));
        }
    }
}
