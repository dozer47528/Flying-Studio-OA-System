using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAL;
using MODEL;
using System.IO;
namespace BLL
{
    public class ProjectProcessService : BaseService
    {
        private HttpContextBase context;
        protected HttpContextBase Context
        {
            get { return context ?? (context = new HttpContextWrapper(HttpContext.Current)); }
        }
        public string FileUrl
        {
            get
            {
                return "~/file/" + DateTime.Now.ToString("yyyy-MM-dd");
            }
        }
        public string FilePath
        {
            get
            {
                var path = Context.Server.MapPath(FileUrl);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
        }


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
                    where (l.NextProcessAuthority & user.Role.RoleEnum) == user.Role.RoleEnum && l.Finished == false
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

        public void UpLoadFile(ProjectProcess project, HttpPostedFileBase file)
        {
            var path = SaveFile(file);
            if (project.ProjectProcessActivity == (int)ProjectProcessActivity.运营组设计)
            {
                if (!string.IsNullOrEmpty(project.File1))
                {
                    DeleteFile(project.File1);
                }
                project.File1 = path;
            }
            else
            {
                if (!string.IsNullOrEmpty(project.File2))
                {
                    DeleteFile(project.File2);
                }
                project.File2 = path;
            }
            db.SaveChanges();
        }
        private string SaveFile(HttpPostedFileBase file)
        {
            var fileName = file.FileName;
            var fullName = Path.Combine(FilePath, Guid.NewGuid().ToString() + Path.GetExtension(fileName));
            var fullUrl = FileUrl + "/" + Path.GetFileName(fullName);
            file.SaveAs(fullName);
            return fullUrl;
        }

        private void DeleteFile(string path)
        {
            var filePath = Context.Server.MapPath(path);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
