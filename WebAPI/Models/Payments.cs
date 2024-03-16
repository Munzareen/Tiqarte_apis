using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class PaymentsOrderRequest
    {
        public decimal amount { get; set; }
        public string description { get; set; }
    }

    public class PromotorPaymentRequest
    {
        public int PromotorId { get; set; }
        public string PromotorName { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
    }

    public class PromotorPaymentUpdate
    {
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string PromotorName { get; set; }
        public string Currency { get; set; }
        public decimal? Amount { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public string InvoiceNumber { get; set; }
        public string Description { get; set; }
    }
}