using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public int UserId  { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
    }

    public class CustomerSignup
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string DOB { get; set; }
        public string MobileNumber { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Location { get; set; }
        public string ImageURL { get; set; }

    }
}