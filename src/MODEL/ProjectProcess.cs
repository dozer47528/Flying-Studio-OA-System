using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Display(Name = "项目描述"), Required]
        public string Memo { get; set; }

        [Display(Name = "设计稿")]
        public string File1 { get; set; }
        [Display(Name = "项目代码")]
        public string File2 { get; set; }
        public string ActivityName { get; set; }
        [Display(Name = "流程阶段")]
        public int ProjectProcessActivity { get; set; }
        public ICollection<AppraisalResult> AppraisalResult { get; set; }
    }

    public class AppraisalResult
    {
        public int ID { get; set; }
        public ProjectProcess ProjectProcess { get; set; }
        public User User { get; set; }
        public bool IsAgree { get; set; }
    }

    public enum AppraisalEnum
    {
        Agree = 1,
        Disagree = -1,
        Waiting = 0,
    }

    public enum ProjectProcessActivity
    {
        申请 = 0,
        可行性评估 = 1,
        运营组设计 = 2,
        运营组组长评估 = 3,
        技术组编写 = 4,
        技术组组长评估 = 5,
        完成 = 6,
    }
}
