﻿using System;
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
        public DbSet<User> Users { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<UploadFile> UploadFiles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<LeaveProcess> LeaveProcesses { get; set; }
        public DbSet<ProjectProcess> ProjectProcesses { get; set; }
        public DbSet<AppraisalResult> AppraisalResults { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }
        public DbSet<Inbox> Inboxes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>().Ignore(a => a.TempID);
            modelBuilder.Entity<UploadFile>().Ignore(a => a.TempID);
            modelBuilder.Entity<LeaveProcess>().Ignore(a => a.ProcessName);
            base.OnModelCreating(modelBuilder);
        }
    }

    public static class DataBaseEntension
    {
        public static void InitData(this OAContext db)
        {
            #region InitRole
            var admin = new UserRole { Name = UserRoleEnum.执行站长.ToString(), Describe = UserRoleEnum.执行站长.ToString(), RoleEnum = (int)UserRoleEnum.执行站长 };
            db.UserRoles.Add(admin);
            db.SaveChanges();

            var bangong = new UserRole { FatherRole = admin, Name = UserRoleEnum.办公组组长.ToString(), Describe = UserRoleEnum.办公组组长.ToString(), RoleEnum = (int)UserRoleEnum.办公组组长 };
            db.UserRoles.Add(bangong);
            db.SaveChanges();
            var bangong2 = new UserRole { FatherRole = bangong, Name = UserRoleEnum.办公组成员.ToString(), Describe = UserRoleEnum.办公组成员.ToString(), RoleEnum = (int)UserRoleEnum.办公组成员 };
            db.UserRoles.Add(bangong2);
            db.SaveChanges();


            var jishu = new UserRole { FatherRole = admin, Name = UserRoleEnum.技术组组长.ToString(), Describe = UserRoleEnum.技术组组长.ToString(), RoleEnum = (int)UserRoleEnum.技术组组长 };
            db.UserRoles.Add(jishu);
            db.SaveChanges();
            var jishu2 = new UserRole { FatherRole = jishu, Name = UserRoleEnum.技术组成员.ToString(), Describe = UserRoleEnum.技术组成员.ToString(), RoleEnum = (int)UserRoleEnum.技术组成员 };
            db.UserRoles.Add(jishu2);
            db.SaveChanges();


            var yunying = new UserRole { FatherRole = admin, Name = UserRoleEnum.运营组组长.ToString(), Describe = UserRoleEnum.运营组组长.ToString(), RoleEnum = (int)UserRoleEnum.运营组组长 };
            db.UserRoles.Add(yunying);
            db.SaveChanges();
            var yunying2 = new UserRole { FatherRole = yunying, Name = UserRoleEnum.运营组成员.ToString(), Describe = UserRoleEnum.运营组成员.ToString(), RoleEnum = (int)UserRoleEnum.运营组成员 };
            db.UserRoles.Add(yunying2);
            db.SaveChanges();
            #endregion
        }

        public static void InitTestData(this OAContext db)
        {
            #region User
            var role = db.UserRoles.First(u => u.RoleEnum == (int)UserRoleEnum.执行站长);
            var role1 = db.UserRoles.First(u => u.RoleEnum == (int)UserRoleEnum.办公组组长);
            var role2 = db.UserRoles.First(u => u.RoleEnum == (int)UserRoleEnum.办公组成员);
            var role3 = db.UserRoles.First(u => u.RoleEnum == (int)UserRoleEnum.技术组组长);
            var role4 = db.UserRoles.First(u => u.RoleEnum == (int)UserRoleEnum.技术组成员);
            var role5 = db.UserRoles.First(u => u.RoleEnum == (int)UserRoleEnum.运营组组长);
            var role6 = db.UserRoles.First(u => u.RoleEnum == (int)UserRoleEnum.运营组成员);
            var user = new User
            {
                NickName = "龚昱阳",
                FirstName = "昱阳",
                LastName = "龚",
                Email = "mail@dozer.cc",
                FirstLoginDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                LoginTimes = 0,
                Password = "1".MD5(),
                Role = role,
                Username = "gongyuyang",
            };

            var user1 = new User
            {
                NickName = "简小波",
                FirstName = "小波",
                LastName = "简",
                Email = "jianxiaobo@gmail.com",
                FirstLoginDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                LoginTimes = 0,
                Password = "1".MD5(),
                Role = role1,
                Username = "jianxiaobo",
            };

            var user2 = new User
            {
                NickName = "王洋",
                FirstName = "洋",
                LastName = "王",
                Email = "wangyang@gmail.com",
                FirstLoginDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                LoginTimes = 0,
                Password = "1".MD5(),
                Role = role2,
                Username = "wangyang",
            };

            var user3 = new User
            {
                NickName = "张翔",
                FirstName = "翔",
                LastName = "张",
                Email = "zhangxiang@gmail.com",
                FirstLoginDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                LoginTimes = 0,
                Password = "1".MD5(),
                Role = role3,
                Username = "zhangxiang",
            };

            var user4 = new User
            {
                NickName = "奚杉杉",
                FirstName = "杉杉",
                LastName = "奚",
                Email = "xishanshan@gmail.com",
                FirstLoginDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                LoginTimes = 0,
                Password = "1".MD5(),
                Role = role4,
                Username = "xishanshan",
            };


            var user5 = new User
            {
                NickName = "李准",
                FirstName = "准",
                LastName = "李",
                Email = "lizhun@gmail.com",
                FirstLoginDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                LoginTimes = 0,
                Password = "1".MD5(),
                Role = role5,
                Username = "lizhun",
            };
            var user6 = new User
            {
                NickName = "陈爽新",
                FirstName = "爽新",
                LastName = "陈",
                Email = "chenshuangxin@gmail.com",
                FirstLoginDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                LoginTimes = 0,
                Password = "1".MD5(),
                Role = role6,
                Username = "chenshuangxin",
            };


            db.Users.Add(user);
            db.Users.Add(user1);
            db.Users.Add(user2);
            db.Users.Add(user3);
            db.Users.Add(user4);
            db.Users.Add(user5);
            db.Users.Add(user6);
            #endregion

            db.SaveChanges();
        }

        public static void InitDebugData(this OAContext db)
        {
            //#region 文章
            //var user = db.Users.FirstOrDefault();
            //for (var k = 0; k < 10; k++)
            //{
            //    var article = new Article
            //    {
            //        AddDate = DateTime.Now,
            //        Authority = (int)UserRoleEnum.全员,
            //        Content = "text content",
            //        Owner = user,
            //        Title = "测试文章：" + DateTime.Now.Ticks.ToString().Substring(5),
            //    };
            //    db.Articles.Add(article);
            //}
            //#endregion

            #region 签到
            foreach (var user2 in db.Users)
            {
                var rnd = new Random();
                for (var k = 2; k < DateTime.Now.Day; k++)
                {
                    var checkInTime = new DateTime(2012, DateTime.Now.Month, k, 9 + rnd.Next(-2, 2), 0, 0);
                    var checkOutTime = new DateTime(2012, DateTime.Now.Month, k, 18 + rnd.Next(-1, 4), 0, 0);
                    db.CheckIns.Add(new CheckIn
                    {
                        CheckInTime = checkInTime,
                        CheckOutTime = checkOutTime,
                        IsHoliday = (checkInTime.DayOfWeek == DayOfWeek.Saturday || checkInTime.DayOfWeek == DayOfWeek.Sunday),
                        User = user2,
                        Memo = "test",
                    });
                }
            }
            #endregion
            db.SaveChanges();
        }
    }
}
