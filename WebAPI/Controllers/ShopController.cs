using BusinesEntities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Web.Http;
using WebAPI.Models;
using System.Security.Claims;
using Newtonsoft.Json;
using System.Web.Helpers;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;

namespace WebAPI.Controllers
{
    public class ShopController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        NotificationController notificationController = new NotificationController();

        [HttpPost]
        [Route("api/addProduct")]
        public IHttpActionResult AddProduct(ProductAddRequestModel model)
        {
            try
            {
                var product = db.ShopProduct.Add(new ShopProduct
                {
                    Sku = model.Sku,
                    ProductName = model.ProductName,
                    Description = model.Description,
                    DeliveryDetails = model.DeliveryDetails,
                    Price = model.Price,
                    CatagoryId = model.CatagoryId,
                    ProductFor = model.ProductFor,
                    isActive = true,
                    PromotorId = model.PromotorId,
                    CreatedDate = DateTime.Now,
                    EventId = model.EventId
                });
                db.SaveChanges();

                foreach (var item in model.ProductImageURLs)
                {
                    var prodImage = db.ShopProductImages.Add(new ShopProductImages
                    {
                        ProductId = product.Id,
                        ImageURL = item
                    });
                    db.SaveChanges();
                }

                foreach (var attr in model.Attributes)
                {
                    var prodAttribute = db.Attributes.Add(new Attributes
                    {
                        ProductId = product.Id,
                        AttributeName = attr.AttributeName,
                        AttributeDescription = attr.AttributeDescription,
                    });
                    db.SaveChanges();

                    foreach (var vari in attr.Variations)
                    {
                        db.Variations.Add(new Variations
                        {
                            ProductId = product.Id,
                            AttributeId = prodAttribute.Id,
                            VariationName = vari.VariationName,
                            VariationDescription = vari.VariationDescription,
                            AvailableQuantity = vari.AvailableQuantity
                        });
                        db.SaveChanges();
                    }
                }

                return Json(model);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpPost]
        [Route("api/deleteproduct")]
        public IHttpActionResult DeleteProduct(int productId)
        {
            try
            {
                var _addtocart = db.ShopProduct.FirstOrDefault(a => a.Id == productId);
                _addtocart.isActive = false;

                db.SaveChanges();

                return Json(_addtocart);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpPost]
        [Route("api/editproduct")]
        public IHttpActionResult EditProduct(EditProductAddRequestModel model)
        {
            try
            {
                var ShopProduct = db.ShopProduct.FirstOrDefault(a => a.Id == model.ProductId && a.isActive == true);
                if (ShopProduct != null)
                {
                    ShopProduct.Sku = model.Sku;
                    ShopProduct.ProductName = model.ProductName;
                    ShopProduct.Description = model.Description;
                    ShopProduct.DeliveryDetails = model.DeliveryDetails;
                    ShopProduct.Price = model.Price;
                    ShopProduct.CatagoryId = model.CatagoryId;
                    ShopProduct.ProductFor = model.ProductFor;
                    db.SaveChanges();

                    return Json(ShopProduct);
                }
                else
                    return Json(new { result = "Error" });
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getAllProductList")]
        public IHttpActionResult GetAllShopProductList(int PromotorId, int? sPrice = 0, int? ePrice = 1000)
        {
            try
            {
                List<ProductResponseViewAll> evr = new List<ProductResponseViewAll>();
                var prod = db.ShopProduct.Where(a => a.PromotorId == PromotorId && a.isActive == true && a.Price >= sPrice && a.Price <= ePrice).ToList();
                foreach (var item in prod)
                {
                    var prodImage = db.ShopProductImages.FirstOrDefault(a => a.ProductId == item.Id);
                    evr.Add(new ProductResponseViewAll
                    {
                        Id = item.Id,
                        Sku = item.Sku,
                        ProductName = item.ProductName,
                        Description = item.Description,
                        DeliveryDetails = item.DeliveryDetails,
                        Price = item.Price,
                        CatagoryId = item.CatagoryId,
                        ProductFor = item.ProductFor,
                        PromotorId = item.PromotorId,
                        ImageURL = prodImage != null ? prodImage.ImageURL : "https://cdn-icons-png.flaticon.com/512/138/138572.png?w=826&t=st=1685608407~exp=1685609007~hmac=9c4d70dc4327190ecb062ebae2e6bb7d11ae41e3f71e03b66b2b3bf13bd1edb0",
                        CreatedDate = item.CreatedDate,
                        isActive = item.isActive
                    });
                }

                return Json(evr);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getSingleProductById")]
        public IHttpActionResult GetSingleProductById(int ProductId)
        {
            try
            {
                ProductResponseViewSingle evr = new ProductResponseViewSingle();
                List<Attributes> attributes = new List<Attributes>();
                var item = db.ShopProduct.FirstOrDefault(a => a.Id == ProductId && a.isActive == true);
                if (item != null)
                {
                    var prodImage = db.ShopProductImages.Where(a => a.ProductId == item.Id).Select(i => i.ImageURL).ToArray();
                    var prodAttr = db.Attributes.Where(a => a.ProductId.Equals(ProductId)).ToArray();
                    foreach (var attr in prodAttr)
                    {
                        var prodVari = db.Variations.Where(a => a.ProductId.Equals(ProductId) && a.AttributeId.Equals(attr.Id)).ToList();
                        attributes.Add(new Attributes
                        {
                            Id = attr.Id,
                            ProductId = attr.ProductId,
                            AttributeName = attr.AttributeName,
                            AttributeDescription = attr.AttributeDescription,
                            Variations = prodVari
                        });
                    }
                    evr = new ProductResponseViewSingle
                    {
                        Id = item.Id,
                        Sku = item.Sku,
                        ProductName = item.ProductName,
                        Description = item.Description,
                        DeliveryDetails = item.DeliveryDetails,
                        Price = item.Price,
                        CatagoryId = item.CatagoryId,
                        ProductFor = item.ProductFor,
                        PromotorId = item.PromotorId,
                        CreatedDate = item.CreatedDate,
                        isActive = item.isActive,
                        ProductImageURLs = prodImage,
                        Attributes = attributes,
                    };
                }

                return Json(evr);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getAllProductListByCatagoryId")]
        public IHttpActionResult GetAllProductListByCatagoryId(int PromotorId, int CatagoryId, int? sPrice = 0, int? ePrice = 1000)
        {
            try
            {
                AddDBLogs("CatagoryId: " + CatagoryId);
                List<ProductResponseViewAll> evr = new List<ProductResponseViewAll>();

                if (CatagoryId == 1)
                {
                    var prod = db.ShopProduct.Where(a => a.PromotorId == PromotorId && a.isActive == true && a.Price >= sPrice && a.Price <= ePrice).ToList();
                    foreach (var item in prod)
                    {
                        var prodImage = db.ShopProductImages.FirstOrDefault(a => a.ProductId == item.Id);
                        evr.Add(new ProductResponseViewAll
                        {
                            Id = item.Id,
                            Sku = item.Sku,
                            ProductName = item.ProductName,
                            Description = item.Description,
                            DeliveryDetails = item.DeliveryDetails,
                            Price = item.Price,
                            CatagoryId = item.CatagoryId,
                            ProductFor = item.ProductFor,
                            PromotorId = item.PromotorId,
                            ImageURL = prodImage != null ? prodImage.ImageURL : "https://cdn-icons-png.flaticon.com/512/138/138572.png?w=826&t=st=1685608407~exp=1685609007~hmac=9c4d70dc4327190ecb062ebae2e6bb7d11ae41e3f71e03b66b2b3bf13bd1edb0",
                            CreatedDate = item.CreatedDate,
                            isActive = item.isActive
                        });
                    }
                }
                else
                {
                    var prod = db.ShopProduct.Where(a => a.CatagoryId == CatagoryId && a.PromotorId == PromotorId && a.isActive == true).ToList();
                    foreach (var item in prod)
                    {
                        var prodImage = db.ShopProductImages.FirstOrDefault(a => a.ProductId == item.Id);
                        evr.Add(new ProductResponseViewAll
                        {
                            Id = item.Id,
                            Sku = item.Sku,
                            ProductName = item.ProductName,
                            Description = item.Description,
                            DeliveryDetails = item.DeliveryDetails,
                            Price = item.Price,
                            CatagoryId = item.CatagoryId,
                            ProductFor = item.ProductFor,
                            PromotorId = item.PromotorId,
                            ImageURL = prodImage != null ? prodImage.ImageURL : "https://cdn-icons-png.flaticon.com/512/138/138572.png?w=826&t=st=1685608407~exp=1685609007~hmac=9c4d70dc4327190ecb062ebae2e6bb7d11ae41e3f71e03b66b2b3bf13bd1edb0",
                            CreatedDate = item.CreatedDate,
                            isActive = item.isActive
                        });
                    }
                }

                return Json(evr);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getAllProductListByUser")]
        public IHttpActionResult GetAllProductListByUser(int? sPrice = 0, int? ePrice = 1000)
        {
            try
            {
                List<ShopCheckOutViewModel> shopCheckOut = new List<ShopCheckOutViewModel>();
                List<CheckOutProductsViewModel> checkOutProducts = new List<CheckOutProductsViewModel>();
                List<AddToCartAttributesName> addToCartAttributesName = new List<AddToCartAttributesName>();
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var CustomerID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var checkOut = db.ShopCheckOut.Where(a => a.UserId == CustomerID).ToList();
                foreach (var co in checkOut)
                {
                    checkOutProducts = new List<CheckOutProductsViewModel>();
                    var checkoutProducts = db.CheckOutProducts.Where(a => a.CheckOutId == co.Id).ToList();
                    foreach (var coP in checkoutProducts)
                    {
                        addToCartAttributesName = new List<AddToCartAttributesName>();

                        var addtocart = db.AddToCart.FirstOrDefault(a => a.Id == coP.AddToCartId);
                        var pQ = addtocart.Quantity;
                        var pA = JsonConvert.DeserializeObject<List<AddToCartAttributes>>(addtocart.Attributes);
                        foreach (var item in pA)
                        {
                            var pAN = db.Attributes.FirstOrDefault(a => a.Id == item.AttributeId).AttributeName;
                            var pVN = db.Variations.FirstOrDefault(a => a.Id == item.VariationId).VariationName;

                            addToCartAttributesName.Add(new AddToCartAttributesName { AttributeName = pAN, VariationName = pVN });
                        }

                        var pname = db.ShopProduct.FirstOrDefault(a => a.Id == addtocart.ProductId && a.isActive == true && a.Price >= sPrice && a.Price <= ePrice);
                        var pImages = db.ShopProductImages.Where(a => a.ProductId == addtocart.ProductId).ToList();
                        checkOutProducts.Add(new CheckOutProductsViewModel
                        {
                            Id = coP.Id,
                            CheckOutId = coP.CheckOutId,
                            ProductName = pname != null ? pname.ProductName : "",
                            Quantity = addtocart.Quantity,
                            AttributeNames = addToCartAttributesName,
                            Price = pname != null ? pname.Price : 0,
                            ProductImageURLs = pImages != null ? pImages.Select(a => a.ImageURL).ToList() : null
                        });
                    }

                    shopCheckOut.Add(new ShopCheckOutViewModel
                    {
                        Id = co.Id,
                        OrderNo = co.OrderNo,
                        UserId = co.UserId,
                        CustomerName = co.CustomerName,
                        CustomerEmail = co.CustomerEmail,
                        State = co.State,
                        City = co.City,
                        PostalCode = co.PostalCode,
                        MobileNumber = co.MobileNumber,
                        PurchaseDate = co.PurchaseDate,
                        CheckOutProducts = checkOutProducts,

                    });
                }

                return Json(shopCheckOut);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpPost]
        [Route("api/addToCart")]
        public IHttpActionResult AddToCart(AddToCartRequest model)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var CustomerID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
                var Products = db.ShopProduct.FirstOrDefault(a => a.Id == model.ProductId);
                var _addtocart = db.AddToCart.Add(new AddToCart
                {
                    UserId = CustomerID,
                    ProductId = model.ProductId,
                    Attributes = Newtonsoft.Json.JsonConvert.SerializeObject(model.Attributes),
                    Quantity = model.Quantity,
                    isActive = true,
                    PromotorId = Products.PromotorId,
                    CreatedDate = DateTime.Now,
                });

                db.SaveChanges();

                return Json(_addtocart);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpPost]
        [Route("api/addToCartDelete")]
        public IHttpActionResult AddToCartDelete(int Id)
        {
            try
            {
                var _addtocart = db.AddToCart.FirstOrDefault(a => a.Id == Id);
                _addtocart.isActive = false;

                db.SaveChanges();

                return Json(_addtocart);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getAddToCartByUser")]
        public IHttpActionResult GetAddToCartByUser()
        {
            try
            {
                List<AddToCartAttributes> Attributes = new List<AddToCartAttributes>();
                List<AddToCartResponse> AddToCartRequest = new List<AddToCartResponse>();
                List<AddToCartAttributesName> addToCartAttributesName = new List<AddToCartAttributesName>();
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var CustomerID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var _addtocart = db.AddToCart.Where(a => a.UserId == CustomerID && a.isActive == true).ToList();

                foreach (var item in _addtocart)
                {
                    Attributes = new List<AddToCartAttributes>();
                    addToCartAttributesName = new List<AddToCartAttributesName>();

                    var _prod = db.ShopProduct.FirstOrDefault(a => a.Id == item.ProductId && a.isActive == true);
                    var _prodImage = db.ShopProductImages.FirstOrDefault(a => a.ProductId == item.ProductId).ImageURL;
                    var _pA = JsonConvert.DeserializeObject<List<AddToCartAttributes>>(item.Attributes);
                    foreach (var pA in _pA)
                    {
                        var pAN = db.Attributes.FirstOrDefault(a => a.Id == pA.AttributeId);
                        var pVN = db.Variations.FirstOrDefault(a => a.Id == pA.VariationId);

                        addToCartAttributesName.Add(new AddToCartAttributesName { AttributeName = pAN == null ? "" : pAN.AttributeName, VariationName = pVN == null ? "" : pVN.VariationName });
                    }

                    AddToCartRequest.Add(new Models.AddToCartResponse
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = _prod.ProductName,
                        Description = _prod.Description,
                        Price = _prod.Price,
                        Quantity = item.Quantity,
                        ProductURLs = _prodImage,
                        Attributes = addToCartAttributesName,
                    });
                }

                return Json(AddToCartRequest);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpPost]
        [Route("api/shopCheckOut")]
        public async Task<IHttpActionResult> ShopCheckOut(ShopCheckOutNew model)
        {
            try
            {
                Random rnd = new Random();
                List<CheckOutProducts> CheckOutProducts = new List<CheckOutProducts>();
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var CustomerID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
                var Checkout = db.ShopCheckOut.Add(new ShopCheckOut
                {
                    OrderNo = rnd.Next(100000, 999999),
                    UserId = CustomerID,
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    State = model.State,
                    City = model.City,
                    PostalCode = model.PostalCode,
                    MobileNumber = model.MobileNumber,
                    PurchaseDate = DateTime.Now,
                    PaymentStatus = "Pending"
                });
                db.SaveChanges();

                int PromotorId = 0;
                decimal TotalPrice = 0;
                foreach (var Id in model.AddToCartIds)
                {
                    var _addtocart = db.AddToCart.FirstOrDefault(a => a.Id == Id);
                    var _shopProduct = db.ShopProduct.FirstOrDefault(a => a.Id.Equals(_addtocart.ProductId));
                    TotalPrice += _addtocart.Quantity * _shopProduct.Price;
                    _addtocart.isActive = false;

                    var _CheckOutProducts = db.CheckOutProducts.Add(new CheckOutProducts
                    {
                        CheckOutId = Checkout.Id,
                        AddToCartId = Id,
                        PromotorId = _addtocart.PromotorId
                    });

                    PromotorId = _addtocart.PromotorId;

                    db.SaveChanges();
                    CheckOutProducts.Add(_CheckOutProducts);
                }

                var cCheckout = db.ShopCheckOut.FirstOrDefault(a => a.Id == Checkout.Id);
                cCheckout.PromotorId = PromotorId; db.SaveChanges();

                Checkout.CheckOutProducts = CheckOutProducts;

                PaymentOrderAPIRequest paymentOrderAPIRequest = new PaymentOrderAPIRequest
                {
                    signature = "3FsqMOJCHWdsUfImXPNKYbTi",
                    amount = TotalPrice,
                    operative = "AUTHORIZATION",
                    secure = false,
                    customer_ext_id = "test",
                    service = "3B2D0F75-B2BF-433B-8BFC-26B4085A33A8",
                    description = Checkout.OrderNo.ToString(),
                    additional = null,
                    url_post = "https://tiqarte.azurewebsites.net/api/payments/urlpostshop",
                    url_ok = "https://tiqarte.azurewebsites.net/api/payments/urlokshop?ticketid=" + Checkout.OrderNo.ToString(),
                    url_ko = "https://tiqarte.azurewebsites.net/api/payments/urlkoshop?ticketid=" + Checkout.OrderNo.ToString(),
                    template_uuid = "6412549E-933E-4DFE-A225-2E87FBF7623E",
                    dcc_template_uuid = "BF418CA6-7043-4864-B36F-F02C2CF2B76B",
                    source_uuid = null,
                    save_card = true,
                    reference = "50620",
                    dynamic_descriptor = "Tiqarte Payments for order number: " + Checkout.OrderNo.ToString(),
                    expires_in = 3600
                };

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var jsonString = JsonConvert.SerializeObject(paymentOrderAPIRequest);
                        var data = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "YjExYzNmOWRmOGFkNGM3MTg1Mjc4MDY3ZTFiZjU4MGM=");
                        var response = await client.PostAsync($"https://api.paylands.com/v1/sandbox/payment", data);

                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            PaymentOrderAPIResponse myDeserializedClass = JsonConvert.DeserializeObject<PaymentOrderAPIResponse>(result);

                            var responseReturn1 = new
                            {
                                isSuccess = true,
                                Message = "Payment Order Created",
                                token = "https://webr-tiqarte.azurewebsites.net/Payland.html?token=" + myDeserializedClass.order.token,
                                ticketId = Checkout.OrderNo.ToString()
                            };
                            return Json(responseReturn1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

                var responseReturn = new
                {
                    isSuccess = false,
                    Message = "Payment Order Not Created",
                    token = "",
                    ticketId = 0
                };
                return Json(responseReturn);

            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpPost]
        [Route("api/shopCheckOut_Web")]
        public async Task<IHttpActionResult> ShopCheckOut_Web(ShopCheckOutNew model)
        {
            try
            {
                Random rnd = new Random();
                List<CheckOutProducts> CheckOutProducts = new List<CheckOutProducts>();
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var CustomerID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
                var Checkout = db.ShopCheckOut.Add(new ShopCheckOut
                {
                    OrderNo = rnd.Next(100000, 999999),
                    UserId = CustomerID,
                    CustomerName = model.CustomerName,
                    CustomerEmail = model.CustomerEmail,
                    State = model.State,
                    City = model.City,
                    PostalCode = model.PostalCode,
                    MobileNumber = model.MobileNumber,
                    PurchaseDate = DateTime.Now,
                    PaymentStatus = "Pending"
                });
                db.SaveChanges();

                int PromotorId = 0;
                decimal TotalPrice = 0;
                foreach (var Id in model.AddToCartIds)
                {
                    var _addtocart = db.AddToCart.FirstOrDefault(a => a.Id == Id);
                    var _shopProduct = db.ShopProduct.FirstOrDefault(a => a.Id.Equals(_addtocart.ProductId));
                    TotalPrice += _addtocart.Quantity * _shopProduct.Price;
                    _addtocart.isActive = false;

                    var _CheckOutProducts = db.CheckOutProducts.Add(new CheckOutProducts
                    {
                        CheckOutId = Checkout.Id,
                        AddToCartId = Id,
                        PromotorId = _addtocart.PromotorId
                    });

                    PromotorId = _addtocart.PromotorId;

                    db.SaveChanges();
                    CheckOutProducts.Add(_CheckOutProducts);
                }

                var cCheckout = db.ShopCheckOut.FirstOrDefault(a => a.Id == Checkout.Id);
                cCheckout.PromotorId = PromotorId; db.SaveChanges();

                Checkout.CheckOutProducts = CheckOutProducts;

                PaymentOrderAPIRequest paymentOrderAPIRequest = new PaymentOrderAPIRequest
                {
                    signature = "3FsqMOJCHWdsUfImXPNKYbTi",
                    amount = TotalPrice,
                    operative = "AUTHORIZATION",
                    secure = false,
                    customer_ext_id = "test",
                    service = "3B2D0F75-B2BF-433B-8BFC-26B4085A33A8",
                    description = Checkout.OrderNo.ToString(),
                    additional = null,
                    url_post = "https://tiqarte.azurewebsites.net/api/payments/urlpostshop_web",
                    url_ok = "https://tiqarte.azurewebsites.net/api/payments/urlokshop_web?ticketid=" + Checkout.OrderNo.ToString(),
                    url_ko = "https://tiqarte.azurewebsites.net/api/payments/urlkoshop_web?ticketid=" + Checkout.OrderNo.ToString(),
                    template_uuid = "6412549E-933E-4DFE-A225-2E87FBF7623E",
                    dcc_template_uuid = "BF418CA6-7043-4864-B36F-F02C2CF2B76B",
                    source_uuid = null,
                    save_card = true,
                    reference = "50620",
                    dynamic_descriptor = "Tiqarte Payments for order number: " + Checkout.OrderNo.ToString(),
                    expires_in = 3600
                };

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        var jsonString = JsonConvert.SerializeObject(paymentOrderAPIRequest);
                        var data = new StringContent(jsonString, Encoding.UTF8, "application/json");
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "YjExYzNmOWRmOGFkNGM3MTg1Mjc4MDY3ZTFiZjU4MGM=");
                        var response = await client.PostAsync($"https://api.paylands.com/v1/sandbox/payment", data);

                        if (response.IsSuccessStatusCode)
                        {
                            var result = response.Content.ReadAsStringAsync().Result;
                            PaymentOrderAPIResponse myDeserializedClass = JsonConvert.DeserializeObject<PaymentOrderAPIResponse>(result);

                            var responseReturn1 = new
                            {
                                isSuccess = true,
                                Message = "Payment Order Created",
                                token = "https://webr-tiqarte.azurewebsites.net/Payland.html?token=" + myDeserializedClass.order.token,
                                ticketId = Checkout.OrderNo.ToString()
                            };
                            return Json(responseReturn1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }

                notificationController.AddUserNotificationLocal(
                    new UserNotificationsRequest
                    {
                        NotificationHeader = "Purchase!",
                        NotificationText = "Your order was successful.",
                        NotificationType = "Purchase",
                        PromotorId = PromotorId,
                        UserId = CustomerID
                    }
                );

                var responseReturn = new
                {
                    isSuccess = false,
                    Message = "Payment Order Not Created",
                    token = "",
                    ticketId = 0
                };
                return Json(responseReturn);

            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        public void AddDBLogs(string Logs)
        {
            db.AppLogs.Add(new AppLogs
            {
                Logs = Logs
            });
            db.SaveChanges();
        }

        [HttpPost]
        [Route("api/addProductNew")]
        public IHttpActionResult AddProductNew(ProductAddRequestModel model)
        {
            try
            {
                var product = db.ShopProduct.Add(new ShopProduct
                {
                    Sku = model.Sku,
                    ProductName = model.ProductName,
                    Description = model.Description,
                    DeliveryDetails = model.DeliveryDetails,
                    Price = model.Price,
                    CatagoryId = model.CatagoryId,
                    ProductFor = model.ProductFor,
                    isActive = true,
                    PromotorId = model.PromotorId,
                    CreatedDate = DateTime.Now,
                    EventId = model.EventId
                });
                db.SaveChanges();

                foreach (var item in model.ProductImageURLs)
                {
                    var prodImage = db.ShopProductImages.Add(new ShopProductImages
                    {
                        ProductId = product.Id,
                        ImageURL = item
                    });
                    db.SaveChanges();
                }

                foreach (var attr in model.Attributes)
                {
                    var prodAttribute = db.Attributes.Add(new Attributes
                    {
                        ProductId = product.Id,
                        AttributeName = attr.AttributeName,
                        AttributeDescription = attr.AttributeDescription,
                    });
                    db.SaveChanges();

                    foreach (var vari in attr.Variations)
                    {
                        db.Variations.Add(new Variations
                        {
                            ProductId = product.Id,
                            AttributeId = prodAttribute.Id,
                            VariationName = vari.VariationName,
                            VariationDescription = vari.VariationDescription,
                            AvailableQuantity = vari.AvailableQuantity
                        });
                        db.SaveChanges();
                    }
                }

                return Json(model);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }


    }
}