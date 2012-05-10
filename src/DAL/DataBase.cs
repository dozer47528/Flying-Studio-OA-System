using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using MODEL;

namespace DAL
{
    public class DataBase : DbContext
    {
        public DataBase() : base("DefaultConnection") { }
        public DbSet<User> UserSet { get; set; }
    }
}
