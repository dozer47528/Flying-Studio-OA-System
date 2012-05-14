using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MODEL
{
    public class Article
    {
        public int ID { get; set; }
        [Display(Name = "标题"), Required]
        public string Title { get; set; }
        private Guid? tempID;
        public Guid TempID
        {
            get
            {
                if (tempID == null) { tempID = Guid.NewGuid(); }
                return tempID.Value;
            }
            set
            {
                tempID = value;
            }
        }
        [Display(Name = "内容")]
        public string Content { get; set; }
        public DateTime AddDate { get; set; }
        public bool IsDelete { get; set; }
        public virtual User Owner { get; set; }
        [Display(Name = "权限")]
        public int Authority { get; set; }
        public virtual List<UploadFile> Attachment { get; set; }
    }
}
