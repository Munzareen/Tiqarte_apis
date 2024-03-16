using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class OrdersController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("admin/getAllProductsByPromotor")]
        public IHttpActionResult getAllProductsByPromotor()
        {
            try
            {
                List<OrdersViewModel> lstOrdersViewModel = new List<OrdersViewModel>();
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotorID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var ShopCheckOut = db.ShopCheckOut.Where(a => a.PromotorId == PromotorID && a.isActive == true && a.isHide == false).ToList();
                foreach (var sco in ShopCheckOut)
                {
                    decimal TotalAmount = 0;
                    var CheckOutProducts = db.CheckOutProducts.Where(a => a.CheckOutId == sco.Id).ToList();
                    foreach (var cop in CheckOutProducts)
                    {
                        var AddToCart = db.AddToCart.Where(a => a.Id == cop.AddToCartId).ToList();
                        foreach (var atc in AddToCart)
                        {
                            var Product = db.ShopProduct.FirstOrDefault(a => a.Id == atc.ProductId);
                            TotalAmount += atc.Quantity * Product.Price;
                        }
                    }

                    lstOrdersViewModel.Add(new OrdersViewModel
                    {
                        Id = sco.Id,
                        Name = sco.CustomerName,
                        Email = sco.CustomerEmail,
                        Telephone = sco.MobileNumber,
                        CompletedAt = sco.PurchaseDate,
                        Status = "Unpaid",
                        Payment = "Online",
                        TotalAmount = TotalAmount,
                        OrderNo = sco.OrderNo,
                        BillingName = sco.CustomerName,
                        BillingEmail = sco.CustomerEmail,
                        BillingTelephone = sco.MobileNumber,
                        BillingCountry = sco.BillingCountry,
                        BillingTown = sco.City,
                        BillingPostalCode = sco.PostalCode,
                        BillingAddressLine1 = sco.AddressLine1,
                        BillingAddressLine2 = sco.AddressLine2,
                        BillingState = sco.State,
                    });
                }

                return Json(lstOrdersViewModel);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpPost]
        [Route("admin/setOrderHide")]
        public IHttpActionResult setOrderHide(int Id)
        {
            try
            {
                var ShopCheckOut = db.ShopCheckOut.FirstOrDefault(a => a.Id == Id);
                if (ShopCheckOut != null)
                {
                    ShopCheckOut.isHide = true;
                    db.SaveChanges();

                    return Json(true);
                }

                return Json(false);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpPost]
        [Route("admin/setOrderDelete")]
        public IHttpActionResult setOrderDelete(int Id)
        {
            try
            {
                var ShopCheckOut = db.ShopCheckOut.FirstOrDefault(a => a.Id == Id);
                if (ShopCheckOut != null)
                {
                    ShopCheckOut.isActive = false;
                    db.SaveChanges();

                    return Json(true);
                }

                return Json(false);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getOrderCustomerDetails")]
        public IHttpActionResult getOrderCustomerDetails(int Id)
        {
            try
            {
                var sco = db.ShopCheckOut.FirstOrDefault(a => a.Id == Id && a.isActive == true && a.isHide == false);
                var response = new OrderCustomer
                {
                    Id = sco.Id,
                    BillingName = sco.CustomerName,
                    BillingEmail = sco.CustomerEmail,
                    BillingTelephone = sco.MobileNumber,
                    BillingCountry = sco.BillingCountry,
                    BillingTown = sco.City,
                    BillingPostalCode = sco.PostalCode,
                    BillingAddressLine1 = sco.AddressLine1,
                    BillingAddressLine2 = sco.AddressLine2,
                    BillingState = sco.State
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

        [HttpPost]
        [Route("admin/updateOrderCustomer")]
        public IHttpActionResult updateOrderCustomer(OrderCustomer oc)
        {
            try
            {
                var ShopCheckOut = db.ShopCheckOut.FirstOrDefault(a => a.Id == oc.Id);
                if (ShopCheckOut != null)
                {
                    ShopCheckOut.CustomerName = oc.BillingName;
                    ShopCheckOut.CustomerEmail = oc.BillingEmail;
                    ShopCheckOut.State = oc.BillingState;
                    ShopCheckOut.AddressLine2 = oc.BillingAddressLine2;
                    ShopCheckOut.AddressLine1 = oc.BillingAddressLine1;
                    ShopCheckOut.PostalCode = oc.BillingPostalCode;
                    ShopCheckOut.City = oc.BillingTown;
                    ShopCheckOut.BillingCountry = oc.BillingCountry;
                    ShopCheckOut.MobileNumber = oc.BillingTelephone;
                    db.SaveChanges();

                    return Json(true);
                }

                return Json(false);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }

    }
}
