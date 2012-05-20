using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using MODEL;
using Webdiyer.WebControls.Mvc;

namespace BLL
{
    public class CheckInService : BaseService
    {
        public CheckInService(OAContext db) : base(db) { }

        public void Create(CheckIn checkIn)
        {
            db.CheckIns.Add(checkIn);
            db.SaveChanges();
        }
        public List<CheckIn> GetListThisMouth(User user, DateTime start, DateTime end)
        {
            var thisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            return db.CheckIns.Where(c => c.CheckInTime.Value <= end && c.CheckInTime.Value >= thisMonth && c.CheckOutTime != null && c.User.ID == user.ID).ToList();
        }

        public Tuple<double, double, double> GetCheckInJsonData(User user, DateTime start, DateTime end)
        {
            var list = GetListThisMouth(user, start, end);
            double all = 0, early = 0, late = 0;
            foreach (var item in list)
            {
                var date = item.CheckInTime.Value.ToString("yyyy-MM-dd");

                var workHour = (item.CheckOutTime.Value - item.CheckInTime.Value).TotalHours;

                var startTime = new DateTime(item.CheckInTime.Value.Year, item.CheckInTime.Value.Month, item.CheckInTime.Value.Day, 9, 0, 0);
                var endTime = new DateTime(item.CheckInTime.Value.Year, item.CheckInTime.Value.Month, item.CheckInTime.Value.Day, 18, 0, 0);
                var sHour = (startTime - item.CheckInTime.Value).TotalHours;
                var eHour = (endTime - item.CheckOutTime.Value).TotalHours;

                double lateHour = 0;
                double earlyHour = 0;
                if (item.IsHoliday)
                {
                    earlyHour = workHour;
                }
                else
                {
                    if (sHour > 0)
                    {
                        earlyHour += sHour;
                    }
                    else
                    {
                        lateHour += -sHour;
                    }

                    if (eHour > 0)
                    {
                        lateHour += eHour;
                    }
                    else
                    {
                        earlyHour += -eHour;
                    }
                }
                all += workHour;
                late += lateHour;
                early += earlyHour;
            }
            return new Tuple<double, double, double>(all, late, early);
        }

        public bool CheckIn(User user)
        {
            var today = DateTime.Now.Date;
            var tomorrow = DateTime.Now.Date.AddDays(1);
            if (db.CheckIns.Any(c => c.User.ID == user.ID && c.CheckInTime.Value >= today && c.CheckInTime.Value < tomorrow)) return false;
            var check = new CheckIn
            {
                CheckInTime = DateTime.Now,
                CheckOutTime = DateTime.Now,
                IsHoliday = DateTime.Now.DayOfWeek == DayOfWeek.Sunday || DateTime.Now.DayOfWeek == DayOfWeek.Saturday,
                User = user
            };
            Create(check);
            return true;
        }

        public bool CheckOut(User user)
        {
            var today = DateTime.Now.Date;
            var tomorrow = DateTime.Now.Date.AddDays(1);
            var check = db.CheckIns.SingleOrDefault(c => c.User.ID == user.ID && c.CheckInTime.Value >= today && c.CheckInTime.Value < tomorrow);
            if (check == null) return false;
            check.CheckOutTime = DateTime.Now;
            db.SaveChanges();
            return true;
        }
    }
}
