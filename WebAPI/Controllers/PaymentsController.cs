using Azure.Storage.Blobs.Models;
using BusinesEntities;
using Evernote.EDAM.Type;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.UI.WebControls;
using WebAPI.Models;
using ZXing;

namespace WebAPI.Controllers
{
    public class PaymentsController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        //[Route("api/payments/createorder")]
        //[HttpPost]
        //public async Task<IHttpActionResult> CreateOrder(PaymentsOrderRequest model)
        //{
        //    PaymentOrderAPIRequest paymentOrderAPIRequest = new PaymentOrderAPIRequest
        //    {
        //        signature = "3FsqMOJCHWdsUfImXPNKYbTi",
        //        amount = model.amount,
        //        operative = "AUTHORIZATION",
        //        secure = false,
        //        customer_ext_id = "test",
        //        service = "3B2D0F75-B2BF-433B-8BFC-26B4085A33A8",
        //        description = model.description,
        //        additional = null,
        //        url_post = "https://tiqarte.azurewebsites.net/api/payments/urlpost",
        //        url_ok = "https://tiqarte.azurewebsites.net/api/payments/urlok",
        //        url_ko = "https://tiqarte.azurewebsites.net/api/payments/urlko",
        //        template_uuid = "6412549E-933E-4DFE-A225-2E87FBF7623E",
        //        dcc_template_uuid = "BF418CA6-7043-4864-B36F-F02C2CF2B76B",
        //        source_uuid = null,
        //        save_card = true,
        //        reference = "50620",
        //        dynamic_descriptor = "Tiqarte Payments",
        //        expires_in = 3600
        //    };

        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            var jsonString = JsonConvert.SerializeObject(paymentOrderAPIRequest);
        //            var data = new StringContent(jsonString, Encoding.UTF8, "application/json");
        //            ServicePointManager.Expect100Continue = true;
        //            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "YjExYzNmOWRmOGFkNGM3MTg1Mjc4MDY3ZTFiZjU4MGM=");
        //            var response = await client.PostAsync($"https://api.paylands.com/v1/sandbox/payment", data);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var result = response.Content.ReadAsStringAsync().Result;
        //                PaymentOrderAPIResponse myDeserializedClass = JsonConvert.DeserializeObject<PaymentOrderAPIResponse>(result);

        //                var responseReturn1 = new
        //                {
        //                    isSuccess = true,
        //                    Message = "Payment Order Created",
        //                    token = "https://webr-tiqarte.azurewebsites.net/Payland.html?token=" + myDeserializedClass.order.token
        //                };
        //                return Json(responseReturn1);
        //            }
        //            else
        //            {

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //    var responseReturn = new
        //    {
        //        isSuccess = false,
        //        Message = "Payment Order Not Created",
        //        token = ""
        //    };
        //    return Json(responseReturn);
        //}

        [Route("api/payments/urlpost")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult url_post(PaymentOrderAPIResult result)
        {
            var jsonString = JsonConvert.SerializeObject(result);
            db.AppLogs.Add(new BusinesEntities.AppLogs
            {
                Logs = "Ticket | " + jsonString
            });
            db.SaveChanges();
            return null;
        }

        [Route("api/payments/urlok")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult url_ok(int ticketid)
        {
            string htmlString = string.Format(@"<!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Success Page</title>
                <!-- Include Bootstrap CSS -->
                <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
            </head>
            <body>
                <div class=""container text-center mt-5"">
                    <h1 class=""text-success"">Success!</h1>
                    <p>Your payment was successful.</p>
                    <a href=""https://tiqarte.azurewebsites.net/api/payments/success?ticketid={0}"" target=""_blank"" class=""btn btn-primary"">Back to Home</a>
                </div>
                <!-- Include Bootstrap JS (optional) -->
                <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js""></script>
            </body>
            </html>
            "
            , ticketid);

            return new HtmlActionResult(htmlString, HttpStatusCode.OK);
        }

        [Route("api/payments/urlko")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult url_ko(int ticketid)
        {
            string htmlString = string.Format(@"<!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Failure Page</title>
                <!-- Include Bootstrap CSS -->
                <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
            </head>
            <body>
                <div class=""container text-center mt-5"">
                    <h1 class=""text-danger"">Failure!</h1>
                    <p>Sorry, something went wrong.</p>
                     <a href=""https://tiqarte.azurewebsites.net/api/payments/failer?ticketid={0}"" target=""_blank"" class=""btn btn-primary"">Back to Home</a>
                </div>failer
                <!-- Include Bootstrap JS (optional) -->
                <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js""></script>
            </body>
            </html>
            ", ticketid);
            return new HtmlActionResult(htmlString, HttpStatusCode.OK);
        }

        [Route("api/payments/success")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult PaymentSuccess(int ticketid)
        {
            string _BarCodeURL = "";
            string _QRCodeURL = "";
            TicketBookingResponse response = new TicketBookingResponse();
            List<TicketBookingDetailResponse> ticketBookingDetailResponse = new List<TicketBookingDetailResponse>();

            var _TicketBooking = db.TicketBookings.Where(a => a.TicketUniqueNumber == ticketid).ToList();
            var _Ticket1 = _TicketBooking.FirstOrDefault();
            var _Event = db.Event.FirstOrDefault(a => a.EventId == _Ticket1.EventId && a.isActive == true);
            var _Organizer = db.Organizers.FirstOrDefault(a => a.Id == _Event.OrganizerID);
            var _CustomerContactInfo = db.CustomerContacts.FirstOrDefault(a => a.Id == _Ticket1.UserId);

            if (_Event != null)
            {
                _BarCodeURL = _Ticket1.BarCodeURL;
                _QRCodeURL = _Ticket1.QRCodeURL;

                response.Event = _Event.Name;
                response.EventDate = Convert.ToString(_Event.EventDate);
                response.Location = _Event.Location;
                response.Organizer = _Organizer.Name;
                response.FullName = _CustomerContactInfo.FullName;
                response.NickName = _CustomerContactInfo.NickName;
                response.Gender = _CustomerContactInfo.Gender == 0 ? "Male" : "Female";
                response.DOB = _CustomerContactInfo.DOB.ToShortDateString();
                response.Email = _CustomerContactInfo.Email;
                response.MobileNo = _CustomerContactInfo.MobileNumber;
                response.Country = db.Countries.FirstOrDefault(a => a.Id == _CustomerContactInfo.CountryId).CountryName;

                foreach (var item in _TicketBooking)
                {
                    var _TicketDetails = db.TicketDetails.FirstOrDefault(a => a.Id == _Ticket1.TicketId);
                    ticketBookingDetailResponse.Add(new TicketBookingDetailResponse
                    {
                        TicketType = _TicketDetails.TicketType,
                        TicketCount = item.TicketCount,
                        TicketPrice = _TicketDetails.TicketPrice,
                        TaxAmount = 0
                    });

                    item.PaymentStatus = "Paid";
                    db.SaveChanges();
                }

                response.TicketBookingDetail = ticketBookingDetailResponse;
                response.PaymentMethod = "MasterCard";
                response.OrderId = ticketid;
                response.Status = "Paid";
                response.BarcodeURL = _BarCodeURL;
                response.QRcodeURL = _QRCodeURL;
            }

            return Json(response);
        }

        [Route("api/payments/failer")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult PaymentFailer(int ticketid)
        {
            var _TicketBooking = db.TicketBookings.Where(a => a.TicketUniqueNumber == ticketid).ToList();
            foreach (var item in _TicketBooking)
            {
                item.PaymentStatus = "Failed";
            }
            db.SaveChanges();

            var response = new
            {
                message = "Payment Failed",
                isSuccess = false,
                OrderId = ticketid
            };

            return Json(response);
        }

        [Route("api/payments/urlpostshop")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult url_post_shop(PaymentOrderAPIResult result)
        {
            var jsonString = JsonConvert.SerializeObject(result);
            db.AppLogs.Add(new BusinesEntities.AppLogs
            {
                Logs = "Shop | " + jsonString
            });
            db.SaveChanges();
            return null;
        }

        [Route("api/payments/urlokshop")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult url_ok_shop(int ticketid)
        {
            string htmlString = string.Format(@"<!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Success Page</title>
                <!-- Include Bootstrap CSS -->
                <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
            </head>
            <body>
                <div class=""container text-center mt-5"">
                    <h1 class=""text-success"">Success!</h1>
                    <p>Your payment was successful.</p>
                    <a href=""https://tiqarte.azurewebsites.net/api/payments/successshop?ticketid={0}"" target=""_blank"" class=""btn btn-primary"">Back to Home</a>
                </div>
                <!-- Include Bootstrap JS (optional) -->
                <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js""></script>
            </body>
            </html>
            "
            , ticketid);

            return new HtmlActionResult(htmlString, HttpStatusCode.OK);
        }

        [Route("api/payments/urlkoshop")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult url_ko_shop(int ticketid)
        {
            string htmlString = string.Format(@"<!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Failure Page</title>
                <!-- Include Bootstrap CSS -->
                <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
            </head>
            <body>
                <div class=""container text-center mt-5"">
                    <h1 class=""text-danger"">Failure!</h1>
                    <p>Sorry, something went wrong.</p>
                     <a href=""https://tiqarte.azurewebsites.net/api/payments/failershop?ticketid={0}"" target=""_blank"" class=""btn btn-primary"">Back to Home</a>
                </div>failer
                <!-- Include Bootstrap JS (optional) -->
                <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js""></script>
            </body>
            </html>
            ", ticketid);
            return new HtmlActionResult(htmlString, HttpStatusCode.OK);
        }

        [Route("api/payments/successshop")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult PaymentSuccessShop(int ticketid)
        {
            var shopcheckout = db.ShopCheckOut.FirstOrDefault(a => a.OrderNo == ticketid);
            shopcheckout.PaymentStatus = "Paid";
            db.SaveChanges();

            var response = new
            {
                message = "Paid Successfully",
                isSuccess = true,
                OrderId = ticketid
            };

            return Json(response);
        }

        [Route("api/payments/failershop")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult PaymentFailerShop(int ticketid)
        {
            var shopcheckout = db.ShopCheckOut.FirstOrDefault(a => a.OrderNo == ticketid);
            shopcheckout.PaymentStatus = "Failed";
            db.SaveChanges();

            var response = new
            {
                message = "Payment Failed",
                isSuccess = false,
                OrderId = ticketid
            };

            return Json(response);
        }


        //============================== WEB ==============================\\


        [Route("api/payments/urlpost_web")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult url_post_web(PaymentOrderAPIResult result)
        {
            var jsonString = JsonConvert.SerializeObject(result);
            db.AppLogs.Add(new BusinesEntities.AppLogs
            {
                Logs = "Ticket | " + jsonString
            });
            db.SaveChanges();
            return null;
        }

        [Route("api/payments/urlok_web")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult url_ok_web(int ticketid)
        {
            string htmlString = string.Format(@"<!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Success Page</title>
                <!-- Include Bootstrap CSS -->
                <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
            </head>
            <body>
                <div class=""container text-center mt-5"">
                    <h1 class=""text-success"">Success!</h1>
                    <p>Your payment was successful.</p>
                    <a href=""https://webr-tiqarte.azurewebsites.net"" target=""_blank"" class=""btn btn-primary"">Back to Home</a>
                </div>
                <!-- Include Bootstrap JS (optional) -->
                <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js""></script>
            </body>
            </html>
            "
            , ticketid);

            return new HtmlActionResult(htmlString, HttpStatusCode.OK);
        }

        [Route("api/payments/urlko_web")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult url_ko_web(int ticketid)
        {
            string htmlString = string.Format(@"<!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Failure Page</title>
                <!-- Include Bootstrap CSS -->
                <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
            </head>
            <body>
                <div class=""container text-center mt-5"">
                    <h1 class=""text-danger"">Failure!</h1>
                    <p>Sorry, something went wrong.</p>
                     <a href=""https://tiqarte.azurewebsites.net/api/payments/failer_web?ticketid={0}"" target=""_blank"" class=""btn btn-primary"">Back to Home</a>
                </div>failer
                <!-- Include Bootstrap JS (optional) -->
                <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js""></script>
            </body>
            </html>
            ", ticketid);
            return new HtmlActionResult(htmlString, HttpStatusCode.OK);
        }

        [Route("api/payments/success_web")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult PaymentSuccess_web(int ticketid)
        {
            string _BarCodeURL = "";
            string _QRCodeURL = "";
            TicketBookingResponse response = new TicketBookingResponse();
            List<TicketBookingDetailResponse> ticketBookingDetailResponse = new List<TicketBookingDetailResponse>();

            var _TicketBooking = db.TicketBookings.Where(a => a.TicketUniqueNumber == ticketid).ToList();
            var _Ticket1 = _TicketBooking.FirstOrDefault();
            var _Event = db.Event.FirstOrDefault(a => a.EventId == _Ticket1.EventId && a.isActive == true);
            var _Organizer = db.Organizers.FirstOrDefault(a => a.Id == _Event.OrganizerID);
            var _CustomerContactInfo = db.CustomerContacts.FirstOrDefault(a => a.Id == _Ticket1.UserId);

            if (_Event != null)
            {
                _BarCodeURL = _Ticket1.BarCodeURL;
                _QRCodeURL = _Ticket1.QRCodeURL;

                response.Event = _Event.Name;
                response.EventDate = Convert.ToString(_Event.EventDate);
                response.Location = _Event.Location;
                response.Organizer = _Organizer.Name;
                response.FullName = _CustomerContactInfo.FullName;
                response.NickName = _CustomerContactInfo.NickName;
                response.Gender = _CustomerContactInfo.Gender == 0 ? "Male" : "Female";
                response.DOB = _CustomerContactInfo.DOB.ToShortDateString();
                response.Email = _CustomerContactInfo.Email;
                response.MobileNo = _CustomerContactInfo.MobileNumber;
                response.Country = db.Countries.FirstOrDefault(a => a.Id == _CustomerContactInfo.CountryId).CountryName;

                foreach (var item in _TicketBooking)
                {
                    var _TicketDetails = db.TicketDetails.FirstOrDefault(a => a.Id == _Ticket1.TicketId);
                    ticketBookingDetailResponse.Add(new TicketBookingDetailResponse
                    {
                        TicketType = _TicketDetails.TicketType,
                        TicketCount = item.TicketCount,
                        TicketPrice = _TicketDetails.TicketPrice,
                        TaxAmount = 0
                    });

                    item.PaymentStatus = "Paid";
                    db.SaveChanges();
                }

                response.TicketBookingDetail = ticketBookingDetailResponse;
                response.PaymentMethod = "MasterCard";
                response.OrderId = ticketid;
                response.Status = "Paid";
                response.BarcodeURL = _BarCodeURL;
                response.QRcodeURL = _QRCodeURL;
            }

            return Json(response);
        }

        [Route("api/payments/failer_web")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult PaymentFailer_web(int ticketid)
        {
            var _TicketBooking = db.TicketBookings.Where(a => a.TicketUniqueNumber == ticketid).ToList();
            foreach (var item in _TicketBooking)
            {
                item.PaymentStatus = "Failed";
            }
            db.SaveChanges();

            var response = new
            {
                message = "Payment Failed",
                isSuccess = false,
                OrderId = ticketid
            };

            return Json(response);
        }

        [Route("api/payments/urlpostshop_web")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult url_post_shop_web(PaymentOrderAPIResult result)
        {
            var jsonString = JsonConvert.SerializeObject(result);
            db.AppLogs.Add(new BusinesEntities.AppLogs
            {
                Logs = "Shop | " + jsonString
            });
            db.SaveChanges();
            return null;
        }

        [Route("api/payments/urlokshop_web")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult url_ok_shop_web(int ticketid)
        {
            string htmlString = string.Format(@"<!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Success Page</title>
                <!-- Include Bootstrap CSS -->
                <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
            </head>
            <body>
                <div class=""container text-center mt-5"">
                    <h1 class=""text-success"">Success!</h1>
                    <p>Your payment was successful.</p>
                    <a href=""https://webr-tiqarte.azurewebsites.net"" target=""_blank"" class=""btn btn-primary"">Back to Home</a>
                </div>
                <!-- Include Bootstrap JS (optional) -->
                <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js""></script>
            </body>
            </html>
            "
            , ticketid);

            return new HtmlActionResult(htmlString, HttpStatusCode.OK);
        }

        [Route("api/payments/urlkoshop_web")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult url_ko_shop_web(int ticketid)
        {
            string htmlString = string.Format(@"<!DOCTYPE html>
            <html lang=""en"">
            <head>
                <meta charset=""UTF-8"">
                <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                <title>Failure Page</title>
                <!-- Include Bootstrap CSS -->
                <link href=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css"" rel=""stylesheet"">
            </head>
            <body>
                <div class=""container text-center mt-5"">
                    <h1 class=""text-danger"">Failure!</h1>
                    <p>Sorry, something went wrong.</p>
                     <a href=""https://tiqarte.azurewebsites.net/api/payments/failershop_web?ticketid={0}"" target=""_blank"" class=""btn btn-primary"">Back to Home</a>
                </div>failer
                <!-- Include Bootstrap JS (optional) -->
                <script src=""https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js""></script>
            </body>
            </html>
            ", ticketid);
            return new HtmlActionResult(htmlString, HttpStatusCode.OK);
        }

        [Route("api/payments/successshop_web")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult PaymentSuccessShop_web(int ticketid)
        {
            var shopcheckout = db.ShopCheckOut.FirstOrDefault(a => a.OrderNo == ticketid);
            shopcheckout.PaymentStatus = "Paid";
            db.SaveChanges();

            var response = new
            {
                message = "Paid Successfully",
                isSuccess = true,
                OrderId = ticketid
            };

            return Json(response);
        }

        [Route("api/payments/failershop_web")]
        [HttpGet]
        [AllowAnonymous]
        public IHttpActionResult PaymentFailerShop_web(int ticketid)
        {
            var shopcheckout = db.ShopCheckOut.FirstOrDefault(a => a.OrderNo == ticketid);
            shopcheckout.PaymentStatus = "Failed";
            db.SaveChanges();

            var response = new
            {
                message = "Payment Failed",
                isSuccess = false,
                OrderId = ticketid
            };

            return Json(response);
        }
    }
}


public class PaymentOrderAPIRequest
{
    public string signature { get; set; }
    public decimal amount { get; set; }
    public string operative { get; set; }
    public bool secure { get; set; }
    public string customer_ext_id { get; set; }
    public string service { get; set; }
    public string description { get; set; }
    public object additional { get; set; }
    public string url_post { get; set; }
    public string url_ok { get; set; }
    public string url_ko { get; set; }
    public string template_uuid { get; set; }
    public string dcc_template_uuid { get; set; }
    public object source_uuid { get; set; }
    public bool save_card { get; set; }
    public string reference { get; set; }
    public string dynamic_descriptor { get; set; }
    public int expires_in { get; set; }
}


// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Client
{
    public string uuid { get; set; }
}

public class Order
{
    public string uuid { get; set; }
    public DateTime created { get; set; }
    public DateTime created_from_client_timezone { get; set; }
    public int amount { get; set; }
    public string currency { get; set; }
    public bool paid { get; set; }
    public string status { get; set; }
    public bool safe { get; set; }
    public int refunded { get; set; }
    public object additional { get; set; }
    public string service { get; set; }
    public string service_uuid { get; set; }
    public string customer { get; set; }
    public object cof_txnid { get; set; }
    public List<object> transactions { get; set; }
    public string token { get; set; }
    public object ip { get; set; }
    public string reference { get; set; }
    public string dynamic_descriptor { get; set; }
    public object threeds_data { get; set; }
}

public class PaymentOrderAPIResponse
{
    public string message { get; set; }
    public int code { get; set; }
    public DateTime current_time { get; set; }
    public Order order { get; set; }
    public Client client { get; set; }
    public string validation_hash { get; set; }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

public class Cof
{
    public bool is_available { get; set; }
}

public class PaymentOrderAPIResult
{
    public string message { get; set; }
    public int code { get; set; }
    public DateTime current_time { get; set; }
    public Order order { get; set; }
    public Client client { get; set; }
    public string validation_hash { get; set; }
}

public class Source
{
    public string @object { get; set; }
    public string uuid { get; set; }
    public string type { get; set; }
    public string token { get; set; }
    public string brand { get; set; }
    public string country { get; set; }
    public string holder { get; set; }
    public int bin { get; set; }
    public string last4 { get; set; }
    public bool is_saved { get; set; }
    public string expire_month { get; set; }
    public string expire_year { get; set; }
    public string additional { get; set; }
    public string bank { get; set; }
    public string prepaid { get; set; }
    public string validation_date { get; set; }
    public string creation_date { get; set; }
    public object brand_description { get; set; }
    public string origin { get; set; }
    public Cof cof { get; set; }
}

public class Transaction
{
    public string uuid { get; set; }
    public DateTime created { get; set; }
    public DateTime created_from_client_timezone { get; set; }
    public string operative { get; set; }
    public int amount { get; set; }
    public string authorization { get; set; }
    public string processor_id { get; set; }
    public string status { get; set; }
    public string error { get; set; }
    public Source source { get; set; }
    public object antifraud { get; set; }
    public object device { get; set; }
    public object error_details { get; set; }
}

