using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MODEL
{
    public class UploadFile
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public DateTime AddDate { get; set; }
        public Article Article { get; set; }

        private Guid? tempID;
        public Guid TempID
        {
            get
            {
                if (tempID == null) { tempID = Guid.NewGuid(); }
                return tempID.Value;
            }
            set {
                tempID = value;
            }
        }
    }
}
