using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using DAL;
using MODEL;
using Ninject;
using Webdiyer.WebControls.Mvc;

namespace BLL
{
    public class UserRoleService : BaseService
    {
        public UserRoleService(OAContext db) : base(db) { }

        public List<UserRole> GetList()
        {
            return db.UserRoles.ToList();
        }
    }
}
