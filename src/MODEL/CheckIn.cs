using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MODEL
{
    public class CheckIn
    {
        public int ID { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public bool IsHoliday { get; set; }
        public string Memo { get; set; }
        public User User { get; set; }
    }
}
