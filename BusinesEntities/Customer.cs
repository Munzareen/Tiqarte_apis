using System.ComponentModel.DataAnnotations;
using System;

namespace BusinesEntities
{
    public partial class CustomerContact
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public int Gender { get; set; } // 0 For Male and 1 For Female
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int CountryId { get; set; } // Based On Country Table
    }

    public partial class Country
    {
        [Key]
        public int Id { get; set; }
        public string CountryName { get; set; }
    }

    public class CustomerBillingDetails
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string BillingName { get; set; }
        public string BillingCountry { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }
        public string BillingTown { get; set; }
        public string BillingPostalCode { get; set; }
        public string BillingEmail { get; set; }
        public string BillingPhone { get; set; }
    }
}