using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MODEL;

namespace BLL
{
    public class ProjectProcessService : BaseService
    {
        public void SetBookmark(int id, string bookmark)
        {
            var item = db.ProjectProcessSet
                .Where(p => p.ID == id)
                .Select(p => new ProjectProcess { ID = p.ID, Bookmark = bookmark })
                .Single();
            item.Bookmark = bookmark;
            db.SaveChanges();
        }

        public ProjectProcess GetById(int id)
        {
            return db.ProjectProcessSet.Single(p => p.ID == id);
        }
    }
}
