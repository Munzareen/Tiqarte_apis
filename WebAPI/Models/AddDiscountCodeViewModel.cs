using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class DiscountCodeViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Basis { get; set; }
        public decimal FixedAmount { get; set; }
        public bool IncludeBookingFee { get; set; }
        public int EventId { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime ValidFormTime { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime ExpiryTime { get; set; }
        public int UsageLimit { get; set; }
        public int UsageLimitPerCustomer { get; set; }
        public string PostalCodeDiscounts { get; set; }
        public bool AutoApply { get; set; }
        public bool isActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class AddDiscountCodeViewModel
    {
        public string Code { get; set; }
        public string Basis { get; set; }
        public decimal FixedAmount { get; set; }
        public bool IncludeBookingFee { get; set; }
        public List<int> EventId { get; set; }
        public DateTime ValidFromDate { get; set; }
        public DateTime ValidFormTime { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime ExpiryTime { get; set; }
        public int UsageLimit { get; set; }
        public int UsageLimitPerCustomer { get; set; }
        public string PostalCodeDiscounts { get; set; }
        public bool AutoApply { get; set; }
    }
}