using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using DAL;
using MODEL;
using Ninject;
using Utility;
using Newtonsoft.Json;

namespace BLL
{
    public class UploadFileService : BaseService
    {

        public UploadFileService(OAContext db) : base(db) { }
        private HttpContextBase context;
        protected HttpContextBase Context
        {
            get { return context ?? (context = new HttpContextWrapper(HttpContext.Current)); }
        }

        public string TempUrl
        {
            get
            {
                return "~/temp";
            }
        }
        public string TempPath
        {
            get
            {
                var path = Context.Server.MapPath(TempUrl);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return path;
            }
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

        public void InitArticleFiles(string guid, int id)
        {
            var saved = db.UploadFiles.Where(f => f.Article.ID == id).ToList();
            saved.ForEach(f => f.Article = null);//可以优化
            CookieHelper.PostTemp(guid, JsonConvert.SerializeObject(saved));
        }

        public void SaveArticleFile(Article article)
        {
            var items = JsonConvert.DeserializeObject<List<UploadFile>>(CookieHelper.Get(article.TempID.ToString())) ?? new List<UploadFile>();
            var saved = db.UploadFiles.Where(f => f.Article.ID == article.ID).ToList();

            foreach (var i in items)
            {
                if (i.ID == 0 || !saved.Any(s => s.ID == i.ID))
                {
                    var tempPath = Context.Server.MapPath(i.Path);
                    var path = Path.Combine(FilePath, Path.GetFileName(tempPath));
                    File.Move(tempPath, path);
                    i.Path = string.Format("{0}/{1}", FileUrl, Path.GetFileName(tempPath));
                    i.Article = db.Articles.Single(a => a.ID == article.ID);
                    db.UploadFiles.Add(i);
                    db.SaveChanges();
                }
            }
            foreach (var s in saved)
            {
                if (!items.Any(i => i.ID == s.ID))
                {
                    var fullPath = Context.Server.MapPath(s.Path);
                    var item = db.UploadFiles.FirstOrDefault(f => f.ID == s.ID);
                    if (item != null)
                    {
                        db.UploadFiles.Remove(item);
                        db.SaveChanges();
                    }
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }
            }
        }


        public void RemoveFile(string guid, string id)
        {
            var items = JsonConvert.DeserializeObject<List<UploadFile>>(CookieHelper.Get(guid)) ?? new List<UploadFile>();
            var item = items.FirstOrDefault(i => i.TempID.ToString() == id);
            if (item == null) return;
            items.Remove(item);
            var fullPath = Context.Server.MapPath(item.Path);
            if (File.Exists(fullPath) && item.ID == 0)
            {
                File.Delete(fullPath);
            }
            CookieHelper.PostTemp(guid, JsonConvert.SerializeObject(items));
        }
        public void AppendFile(string guid, HttpPostedFileBase file)
        {
            var items = JsonConvert.DeserializeObject<List<UploadFile>>(CookieHelper.Get(guid)) ?? new List<UploadFile>();

            var fileName = SaveFile(file);

            var fileInfo = new UploadFile
            {
                AddDate = DateTime.Now,
                Extension = fileName.Item3,
                FileName = Uri.EscapeDataString(fileName.Item2),
                Path = string.Format("{0}/{1}", TempUrl, fileName.Item1),
            };

            items.Add(fileInfo);
            CookieHelper.PostTemp(guid, JsonConvert.SerializeObject(items));
        }
        private Tuple<string, string, string> SaveFile(HttpPostedFileBase file)
        {
            var ext = Path.GetExtension(file.FileName);
            var name = Path.GetFileNameWithoutExtension(file.FileName);
            var fullName = Path.Combine(TempPath, Guid.NewGuid().ToString() + ext);
            file.SaveAs(fullName);
            return new Tuple<string, string, string>(Path.GetFileName(fullName), name, ext);
        }
    }
}