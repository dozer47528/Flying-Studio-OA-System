using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Ninject;
using Webdiyer.WebControls.Mvc;

namespace BLL
{
    public class InboxService
    {
        [Inject]
        public OAContext OAContext { get; set; }
    }
}
