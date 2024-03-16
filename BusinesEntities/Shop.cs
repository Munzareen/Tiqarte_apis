using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace BusinesEntities
{
	public partial class ShopProduct
	{
		[Key]
		public int Id { get; set; }
		public string Sku { get; set; }
		public string ProductName { get; set; }
		public string Description { get; set; }
		public string DeliveryDetails { get; set; }
		public decimal Price { get; set; }
		public int CatagoryId { get; set; }
		public string ProductFor { get; set; }
		public bool isActive { get; set; }
		public int PromotorId { get; set; }
		public DateTime CreatedDate { get; set; }
        [NotMapped]
        public List<ShopProductImages> ProductImages { get; set; }
		[NotMapped]
		public List<Attributes> Attributes { get; set; }
		public int EventId { get; set; }
	}

	public partial class ShopProductImages
	{
		[Key]
        public int Id { get; set; }
		public int ProductId { get; set; }
		public string ImageURL { get; set; }
    }

	public partial class ShopCheckOut
	{
        [Key]
        public int Id { get; set; }
		[NotMapped]
		public List<CheckOutProducts> CheckOutProducts { get; set; }
		public int UserId { get; set; }
		public string CustomerName { get; set; }
		public string CustomerEmail { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		public string PostalCode { get; set; }
		public string MobileNumber { get; set; }
		public DateTime PurchaseDate { get; set; }
		public long OrderNo { get; set; }
		public int PromotorId { get; set; }
		public bool isHide { get; set; } = false;
		public bool isActive { get; set; } = true;
		public string BillingCountry { get; set; }
		public string AddressLine1 { get; set; }
		public string AddressLine2 { get; set; }
		public bool isRefund { get; set; } = false;
		public DateTime? RefundTime { get; set; }
		public string PaymentStatus { get; set; }
	}

	public partial class CheckOutProducts
	{
        [Key]
        public int Id { get; set; }
		public int CheckOutId { get; set; }
		public int AddToCartId { get; set; }
		public int PromotorId { get; set; }
	}

	public partial class Attributes
	{
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
		public string AttributeName { get; set; }
        public string AttributeDescription { get; set; }
		[NotMapped]
		public List<Variations> Variations { get; set; }
	}

    public partial class Variations
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int AttributeId { get; set; }
		public string VariationName { get; set; }
        public string VariationDescription { get; set; }
		public double AvailableQuantity { get; set; }
	}

	public partial class ProductCatagory
	{
        [Key]
        public int Id { get; set; }
		public string CatagoryName { get; set; }
	}

	public partial class AddToCart
	{
		[Key]
		public int Id { get; set; }
		public int PromotorId { get; set; }
		public int UserId { get; set; }
		public int ProductId { get; set; }
        public string Attributes { get; set; }
        public int Quantity { get; set; }
        public bool isActive { get; set; }
		public DateTime? CreatedDate { get; set; } = DateTime.Now;
	}
}
