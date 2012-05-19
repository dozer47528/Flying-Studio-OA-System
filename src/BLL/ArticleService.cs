using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using MODEL;
using Webdiyer.WebControls.Mvc;
using Utility;
namespace BLL
{
    public class ArticleService : BaseService
    {
        public ArticleService(OAContext db) : base(db) { }
        public Article GetItemById(int id)
        {
            return db.Articles.Single(a => a.ID == id);
        }

        public Article GetItemByIdWithAttachment(int id)
        {
            var item = db.Articles.Include("Attachment").Single(a => a.ID == id);
            return item;
        }
        public PagedList<Article> GetList(int page, int pageSize = 10, string search = null)
        {
            var q = db.Articles.OrderByDescending(a => a.AddDate);
            Filter(q, search);
            return q.ToPagedList(page, pageSize);
        }
        public Article Create(Article article, string autority)
        {
            article.Authority = string.IsNullOrEmpty(autority) ? 0 : AuthorityHelper.GetAuthority(autority.Split(','));
            article.AddDate = DateTime.Now;
            db.Articles.Add(article);
            db.SaveChanges();
            return article;
        }
        public Article Edit(Article article, string autority)
        {
            var old = db.Articles.Single(a => a.ID == article.ID);
            old.Authority = string.IsNullOrEmpty(autority) ? 0 : AuthorityHelper.GetAuthority(autority.Split(','));
            old.Title = article.Title;
            old.Content = article.Content;
            old.TempID = article.TempID;
            db.SaveChanges();
            return old;
        }
        public bool Delete(int id, User user)
        {
            var article = db.Articles.Single(a => a.ID == id);
            if (!CheckMyOwnOrAdmin(article, user)) return false;
            db.Articles.Remove(article);
            db.SaveChanges();
            return true;
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

        public bool CheckMyOwnOrAdmin(Article article, User user)
        {
            //执行站长可以修改所有
            if (user.Role.RoleEnum == (int)UserRoleEnum.执行站长) return true;

            LoadReference(article, a => a.Owner);
            var owner = article.Owner;

            //如果是自己，也可以修改
            if (owner.ID == user.ID) return true;

            //上级主管也有权限修改
            LoadReference(owner, u => u.Role);
            LoadReference(owner.Role, r => r.FatherRole);
            if (owner.Role.FatherRole.ID == user.Role.ID) return true;

            return false;
        }
    }
}
