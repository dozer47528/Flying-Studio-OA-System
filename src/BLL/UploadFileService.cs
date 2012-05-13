using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAL;
using Ninject;

namespace BLL
{
    public class UploadFileService
    {
        [Inject]
        public OAContext db { get; set; }

        public void InitArticleFiles(string guid, int id) { 
        
        }
        public void AppendFile(string guid, HttpPostedFileBase file)
        {

        }
    }
}