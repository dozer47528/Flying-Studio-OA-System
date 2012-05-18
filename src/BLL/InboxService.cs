using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Ninject;
using Webdiyer.WebControls.Mvc;

namespace BLL
{
    public class InboxService : BaseService
    {

        public InboxService(OAContext db) : base(db) { }

    }
}
