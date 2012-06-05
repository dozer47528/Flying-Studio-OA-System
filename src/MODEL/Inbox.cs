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
        public User User { get; set; }
    }

    public enum RedirectType
    {
        请假流程处理 = 1,
        项目流程处理 = 2,
        请假流程查看 = 3,
        项目流程查看 = 4,
    }
}
