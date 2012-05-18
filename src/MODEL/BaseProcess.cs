using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MODEL
{
    public class BaseProcess
    {
        public string ProcessName { get; set; }
        public int ID { get; set; }
        public Guid InstanceID { get; set; }
        public string Bookmark { get; set; }
        public int NextProcessAuthority { get; set; }
        public User Owner { get; set; }
        public string Result { get; set; }
        public DateTime Adddate { get; set; }
        public bool Passed { get; set; }
        public bool Finished { get; set; }
    }

}
