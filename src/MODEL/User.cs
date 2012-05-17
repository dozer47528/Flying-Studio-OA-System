using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MODEL
{
    [Serializable]
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }//名
        public string LastName { get; set; }//姓
        public string NickName { get; set; }//昵称
        public string Username { get; set; }//用户名
        public string Password { get; set; }//密码
        public string Email { get; set; }//邮箱
        public DateTime FirstLoginDate { get; set; }//首次登陆时间
        public DateTime LastLoginDate { get; set; }//上次登录时间
        public int LoginTimes { get; set; }//登录次数
        public UserRole Role { get; set; }//角色
    }
}
