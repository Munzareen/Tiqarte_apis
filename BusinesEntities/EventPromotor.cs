using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public class EventPromotor
    {
        [Key]
        public int Id { get; set; }
        public int PromotoId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string ImageUrl { get; set; }
        public PaymentGatewayEnum PaymentGateway { get; set; }
        public string SecretKey { get; set; }
        public string APIKey { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isActive { get; set; }
    }

    public enum PaymentGatewayEnum
    {
        [EnumMember(Value = "Stripe")]
        Stripe = 1,
        [EnumMember(Value = "PayPal")]
        PayPal = 2,
        [EnumMember(Value = "Visa")]
        Visa = 3
    }
}
