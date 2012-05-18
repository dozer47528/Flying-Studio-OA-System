using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using MODEL;
using Utility;
namespace BLL
{
    public class UserService : BaseService
    {
        public UserService(OAContext db) : base(db) { }
        public const string USER_ID_NAME = "user_id";
        public User GetUserByCookie()
        {

            var id = int.Parse(CookieHelper.Get(USER_ID_NAME));
            return db.Users.Single(u => u.ID == id);
        }

        public UserRole LoadFartherRole(User user)
        {
            return (from u in db.Users
                    where u.ID == user.ID
                    select u.Role.FatherRole).Single();
        }

        public int Count()
        {
            return db.Users.Count();
        }
    }
}
