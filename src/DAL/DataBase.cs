using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using MODEL;
using Utility;

namespace DAL
{
    public class OAContext : DbContext
    {
        public OAContext() : base("DefaultConnection") { }
        public DbSet<User> UserSet { get; set; }
        public DbSet<Article> ArticleSet { get; set; }
        public DbSet<UploadFile> UploadFileSet { get; set; }
        public DbSet<UserRole> UserRoleSet { get; set; }
        public DbSet<Inbox> InboxSet { get; set; }
    }

    public static class DataBaseEntension
    {
        public static void InitData(this OAContext db)
        {
            #region InitRole
            var admin = new UserRole { Name = UserRoleEnum.执行站长.ToString(), Describe = UserRoleEnum.执行站长.ToString(), RoleEnum = (int)UserRoleEnum.执行站长 };
            db.UserRoleSet.Add(admin);
            db.SaveChanges();

            var bangong = new UserRole { FatherRole = admin, Name = UserRoleEnum.办公组组长.ToString(), Describe = UserRoleEnum.办公组组长.ToString(), RoleEnum = (int)UserRoleEnum.办公组组长 };
            db.UserRoleSet.Add(bangong);
            db.SaveChanges();
            var bangong2 = new UserRole { FatherRole = bangong, Name = UserRoleEnum.办公组成员.ToString(), Describe = UserRoleEnum.办公组成员.ToString(), RoleEnum = (int)UserRoleEnum.办公组成员 };
            db.UserRoleSet.Add(bangong2);
            db.SaveChanges();


            var jishu = new UserRole { FatherRole = admin, Name = UserRoleEnum.技术组组长.ToString(), Describe = UserRoleEnum.技术组组长.ToString(), RoleEnum = (int)UserRoleEnum.技术组组长 };
            db.UserRoleSet.Add(jishu);
            db.SaveChanges();
            var jishu2 = new UserRole { FatherRole = jishu, Name = UserRoleEnum.技术组成员.ToString(), Describe = UserRoleEnum.技术组成员.ToString(), RoleEnum = (int)UserRoleEnum.技术组成员 };
            db.UserRoleSet.Add(jishu2);
            db.SaveChanges();


            var yunying = new UserRole { FatherRole = admin, Name = UserRoleEnum.运营组组长.ToString(), Describe = UserRoleEnum.运营组组长.ToString(), RoleEnum = (int)UserRoleEnum.运营组组长 };
            db.UserRoleSet.Add(yunying);
            db.SaveChanges();
            var yunying2 = new UserRole { FatherRole = yunying, Name = UserRoleEnum.运营组成员.ToString(), Describe = UserRoleEnum.运营组成员.ToString(), RoleEnum = (int)UserRoleEnum.运营组成员 };
            db.UserRoleSet.Add(yunying2);
            db.SaveChanges();
            #endregion
        }

        public static void InitTestData(this OAContext db)
        {
            var role = db.UserRoleSet.First(u => u.RoleEnum == (int)UserRoleEnum.执行站长);
            var user = new User
            {
                NickName = "Dozer",
                FirstName = "Yuyang",
                LastName = "Gong",
                Email = "mail@dozer.cc",
                FirstLoginDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                LoginTimes = 0,
                Password = "dozer36937".MD5(),
                Role = role,
                Username = "dozer47528",
            };
            db.UserSet.Add(user);


            for (var k = 0; k < 100; k++)
            {
                var article = new Article
                {
                    AddDate = DateTime.Now,
                    Authority = user.Role.RoleEnum,
                    Content = "text content",
                    Owner = user,
                    Title = "title",
                };
                db.ArticleSet.Add(article);
            }
            db.SaveChanges();
        }
    }
}
