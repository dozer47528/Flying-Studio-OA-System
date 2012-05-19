using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DAL;
using System.Linq.Expressions;
namespace BLL
{
    abstract public class BaseService
    {
        protected OAContext db;
        public BaseService(OAContext db)
        {
            this.db = db;
        }

        public void Save()
        {
            db.SaveChanges();
        }
        public void LoadCollection<T>(T item, Expression<Func<T, ICollection<T>>> exp) where T : class
        {
            var colle = db.Entry(item).Collection(exp);
            if (!colle.IsLoaded)
            {
                colle.Load();
            }
        }

        public void LoadReference<T, TProperty>(T item, Expression<Func<T, TProperty>> exp)
            where T : class
            where TProperty : class
        {
            var prop = db.Entry(item).Reference(exp);
            if (!prop.IsLoaded)
            {
                prop.Load();
            }
        }
    }
}
