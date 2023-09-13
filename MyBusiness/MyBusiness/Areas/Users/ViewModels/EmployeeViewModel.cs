using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyBusiness.Areas.Users.ViewModels
{
    public class EmployeeViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string lastName { get; set; }
        public string surName { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
    }
}