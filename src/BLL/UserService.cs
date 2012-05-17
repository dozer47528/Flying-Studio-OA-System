using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MODEL;
using Utility;
namespace BLL
{
    public class UserService : BaseService
    {
        public const string USER_ID_NAME = "user_id";
        public User GetUserByCookie()
        {
            var id = int.Parse(CookieHelper.Get(USER_ID_NAME));
            return db.UserSet.Single(u => u.ID == id);
        }

        public UserRole LoadFartherRole(User user)
        {
            return (from u in db.UserSet
                    where u.ID == user.ID
                    select u.Role.FatherRole).Single();
        }
    }
}
