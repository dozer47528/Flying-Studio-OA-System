using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using MODEL;
using Ninject;
using Webdiyer.WebControls.Mvc;

namespace BLL
{
    public class ArticleService
    {
        [Inject]
        public OAContext db { get; set; }

        public PagedList<Article> GetList(int page, int pageSize = 10, string search = null)
        {
            var q = db.ArticleSet.OrderByDescending(a => a.AddDate);
            Filter(q, search);
            return q.ToPagedList(page, pageSize);
        }



        private IQueryable<Article> Filter(IQueryable<Article> query, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                query.Where(a => a.Title.Contains(search) || a.Content.Contains(search) ||
                            a.Owner.Username.Contains(search) || a.Owner.LastName.Contains(search) ||
                            a.Owner.FirstName.Contains(search));
            }
            return query;
        }
    }
}
