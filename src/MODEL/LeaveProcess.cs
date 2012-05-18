using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace MODEL
{
    public class LeaveProcess : BaseProcess
    {
        public LeaveProcess()
        {
            ProcessName = "请假流程";
        }
        [Display(Name = "请假理由"), Required]
        public string Memo { get; set; }
        [Display(Name = "开始时间"), Required]
        public DateTime? StartDate { get; set; }
        [Display(Name = "结束时间"), Required]
        public DateTime? EndDate { get; set; }
    }
}
