using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class AccountModel
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LoggedOn { get; set; }
        public string Role { get; set; }
        public string DepartmentId { get; set; }
        public string MaritalStatus { get; set; }
        public string FatherName { get; set; }
        public string RegNo { get; set; }
        public string Cnic { get; set; }
    }

    public class UpdateProfileViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string NickName { get; set; }
        public string DOB { get; set; }
    }

    public class ContactUsLogsRequest
    {
        public int PromotorId { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string Municipality { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
    }
}