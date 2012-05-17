﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MODEL
{
    public class ProjectProcess : BaseProcess
    {
        public ProjectProcess()
        {
            ProcessName = "项目流程";
        }
        public string Memo { get; set; }
        public string File1 { get; set; }
        public string File2 { get; set; }
        public string ActivityName { get; set; }
        public ICollection<AppraisalResult> AppraisalResult { get; set; }
    }

    public class AppraisalResult
    {
        public int ID { get; set; }
        public int ProcessID { get; set; }
        public User User { get; set; }
        public bool IsAgree { get; set; }
    }

    public enum ProjectProcessActivity
    {
        可行性评估,
    }
}
