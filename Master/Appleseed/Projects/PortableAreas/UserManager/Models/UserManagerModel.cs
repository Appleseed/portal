using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserManager.Models
{
    public class UserManagerModel
    {
        public Guid UserId { get; set; }
        public int id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserRol { get; set; }
        public string Edit { get; set; }

    }
}