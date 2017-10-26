using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eCard.Models
{
    public class UserAccountModel
    {
        public Guid ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Department { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public string ShowCreatedBy { get; set; }
        public string Email { get; set; }
    }
}