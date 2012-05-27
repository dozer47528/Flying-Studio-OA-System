using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MODEL
{
    public class Inbox
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int RedirectType { get; set; }
        public int RedirectID { get; set; }
        public bool IsRead { get; set; }
        public ICollection<User> Users { get; set; }
    }

    public enum RedirectType
    {
        请假流程 = 1,
        项目流程 = 2,
        采购流程 = 3,
    }
}
