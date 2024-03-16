using BusinesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class EventPromotorRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string ImageUrl { get; set; }
        public PaymentGatewayEnum PaymentGateway { get; set; }
        public string SecretKey { get; set; }
        public string APIKey { get; set; }
    }

    public class UpdateEventPromotorRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string ImageUrl { get; set; }
        public PaymentGatewayEnum PaymentGateway { get; set; }
        public string SecretKey { get; set; }
        public string APIKey { get; set; }
    }
}