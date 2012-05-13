﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using MODEL;
using Ninject;
using Webdiyer.WebControls.Mvc;
using Utility;
namespace BLL
{
    public class ArticleService
    {
        [Inject]
        public OAContext db { get; set; }
        public Article GetItemById(int id)
        {
            return db.ArticleSet.Single(a => a.ID == id);
        }
        public PagedList<Article> GetList(int page, int pageSize = 10, string search = null)
        {
            var q = db.ArticleSet.OrderByDescending(a => a.AddDate);
            Filter(q, search);
            return q.ToPagedList(page, pageSize);
        }
        public void Create(Article article, string autority)
        {
            article.Authority = string.IsNullOrEmpty(autority) ? 0 : AuthorityHelper.GetAuthority(autority.Split(','));
            article.AddDate = DateTime.Now;
            //article.Attachment
            //article.Owner
            db.ArticleSet.Add(article);
            db.SaveChanges();
        }
        public void Edit(Article article, string autority)
        {
            var old = db.ArticleSet.Single(a => a.ID == article.ID);
            old.Authority = string.IsNullOrEmpty(autority) ? 0 : AuthorityHelper.GetAuthority(autority.Split(','));
            old.Title = article.Title;
            old.Content = article.Content;
            //old.Attachment
            db.SaveChanges();
        }
        public void Delete(int id)
        {
            db.ArticleSet.Remove(db.ArticleSet.Single(a => a.ID == id));
            db.SaveChanges();
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
