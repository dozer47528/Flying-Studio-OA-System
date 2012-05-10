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
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string NickName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime FirstLoginDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int LoginTimes { get; set; }
    }
}
