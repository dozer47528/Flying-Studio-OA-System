using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;

namespace BLL
{
    abstract public class BaseService
    {
        protected OAContext db = new OAContext();
    }
}
