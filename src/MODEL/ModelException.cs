using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace MODEL
{
    //ModelExceptions
    //必须继承自Exception
    public class ModelExceptions : Exception
    {
        //存放错误信息的List
        List<string[]> errors = new List<string[]>();

        //判断是否有错误
        public bool IsValid
        {
            get
            {
                return errors.Count == 0 ? true : false;
            }
        }

        //增加错误信息
        public void AddError(string name, string message)
        {
            this.errors.Add(new string[] { name, message });
        }

        //填充ModelState
        public void FillModelState(ModelStateDictionary modelstate)
        {
            foreach (var e in this.errors)
            {
                modelstate.AddModelError(e[0], e[1]);
            }
        }
    }
}
