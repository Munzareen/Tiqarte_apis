using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class OrdersViewModel
    {
        public int Id { get; set; }
        public long OrderNo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public decimal  TotalAmount { get; set; }
        public DateTime CompletedAt { get; set; }
        public string Status { get; set; }
        public string Payment { get; set; }

        public string BillingName { get; set; }
        public string BillingEmail { get; set; }
        public string BillingTelephone { get; set; }
        public string BillingCountry { get; set; }
        public string BillingTown { get; set; }
        public string BillingPostalCode { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }
        public string BillingState { get; set; }
    }

    public class OrderCustomer
    {
        public int Id { get; set; }
        public string BillingName { get; set; }
        public string BillingEmail { get; set; }
        public string BillingTelephone { get; set; }
        public string BillingCountry { get; set; }
        public string BillingTown { get; set; }
        public string BillingPostalCode { get; set; }
        public string BillingAddressLine1 { get; set; }
        public string BillingAddressLine2 { get; set; }
        public string BillingState { get; set; }
    }
}