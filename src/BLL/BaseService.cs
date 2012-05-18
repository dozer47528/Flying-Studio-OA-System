using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DAL;

namespace BLL
{
    abstract public class BaseService
    {
        protected OAContext db;
        public BaseService(OAContext db)
        {
            this.db = db;
        }
    }
}
