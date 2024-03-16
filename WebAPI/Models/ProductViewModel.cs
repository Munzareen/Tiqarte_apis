using BusinesEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class ProductAddRequestModel
    {
        public int PromotorId { get; set; }
        public string ProductName { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        public string DeliveryDetails { get; set; }
        public decimal Price { get; set; }
        public int CatagoryId { get; set; } // T Shirts, Trousers, Caps
        public string ProductFor { get; set; }
        public string[] ProductImageURLs { get; set; }
        public List<ProductAttributes> Attributes { get; set; }
        public int EventId { get; set; }
    }

    public class EditProductAddRequestModel
    {
        public int ProductId { get; set; }
        public int PromotorId { get; set; }
        public string Sku { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string DeliveryDetails { get; set; }
        public decimal Price { get; set; }
        public int CatagoryId { get; set; } // T Shirts, Trousers, Caps
        public string ProductFor { get; set; }
        public string[] ProductImageURLs { get; set; }
        public List<ProductAttributes> Attributes { get; set; }
    }

    public partial class ProductAttributes
    {
        public string AttributeName { get; set; }
        public string AttributeDescription { get; set; }
        public List<ProductVariations> Variations { get; set; }
    }

    public partial class ProductVariations
    {
        public string VariationName { get; set; }
        public string VariationDescription { get; set; }
        public double AvailableQuantity { get; set; }
    }

    public class ProductResponseViewAll
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string DeliveryDetails { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int CatagoryId { get; set; } // T Shirts, Trousers, Caps
        public string ProductFor { get; set; } // 0 For Male and 1 For Female
        public bool isActive { get; set; }
        public int PromotorId { get; set; }
        public string ImageURL { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class ProductResponseViewSingle
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string DeliveryDetails { get; set; }
        public decimal Price { get; set; }
        public int CatagoryId { get; set; } // T Shirts, Trousers, Caps
        public string ProductFor { get; set; } // 0 For Male and 1 For Female
        public bool isActive { get; set; }
        public int PromotorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string[] ProductImageURLs { get; set; }
        public List<Attributes> Attributes { get; set; }
    }

    public partial class ShopCheckOutViewModel
    {
        public int Id { get; set; }
        public long OrderNo { get; set; }
        public List<CheckOutProductsViewModel> CheckOutProducts { get; set; }
        public int UserId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string MobileNumber { get; set; }
        public DateTime PurchaseDate { get; set; }
    }

    public partial class CheckOutProductsViewModel
    {
        public int Id { get; set; }
        public int CheckOutId { get; set; }
        public string ProductName { get; set; }
        public List<AddToCartAttributesName> AttributeNames { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public List<string> ProductImageURLs{ get; set; }
    }

    public class ShopCheckOutNew
    {
        public List<int> AddToCartIds { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string MobileNumber { get; set; }
        public DateTime PurchaseDate { get; set; }
    }

    public class CheckOutProductsNew
    {
        public int CheckOutId { get; set; }
        public int ProductId { get; set; }
        public int AttributeId { get; set; }
        public int VariationId { get; set; }
        public int Quantity { get; set; }
    }

    public class AddToCartRequest
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ProductURLs { get; set; }
        public List<AddToCartAttributes> Attributes { get; set; }
        public int Quantity { get; set; }
    }

    public class AddToCartResponse
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ProductURLs { get; set; }
        public List<AddToCartAttributesName> Attributes { get; set; }
        public int Quantity { get; set; }
    }

    public class AddToCartAttributes
    {
        public int AttributeId { get; set; }
        public int VariationId { get; set; }
    }

    public class AddToCartAttributesName
    {
        public string AttributeName { get; set; }
        public string VariationName { get; set; }
    }

}