using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DAL;
using MODEL;
using Utility;
namespace BLL
{
    public class UserService : BaseService
    {
        public UserService(OAContext db) : base(db) { }
        public const string USER_ID_NAME = "user_id";

        public bool Login(string username, string password)
        {
            password = password.MD5();
            var user = db.Users.SingleOrDefault(u => u.Username == username && u.Password == password);
            if (user == null) return false;
            CookieHelper.Post(USER_ID_NAME, user.ID.ToString());
            return true;
        }
        public void Logout()
        {
            CookieHelper.Delete(USER_ID_NAME);
        }

        public User GetUserByCookie()
        {
            var context = new HttpContextWrapper(System.Web.HttpContext.Current);
            if (context.Items["User"] == null)
            {
                int id;
                if (!int.TryParse(CookieHelper.Get(USER_ID_NAME), out id)) id = 0;
                CookieHelper.Post(USER_ID_NAME, id.ToString());
                var user = db.Users.Include("Role").SingleOrDefault(u => u.ID == id);
                context.Items["User"] = user;
            }
            return context.Items["User"] as User;
        }

        public User GetById(int id) {
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

        public List<User> GetSubordinates(User user)
        {
            LoadReference(user, u => u.Role);
            return db.Users.Where(u => u.Role.FatherRole.ID == user.Role.ID).ToList();
        }
    }
}
