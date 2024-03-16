using BusinesEntities;
using HtmlParserSharp.Core;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Owin.Security.Providers.Orcid.Message;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;
using System.Drawing;
using ZXing;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using Azure.Storage.Blobs.Models;
using WebAPI.Services;
using System.Net.Sockets;
using CsQuery.Engine.PseudoClassSelectors;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Evernote.EDAM.Type;
using System.Web.UI.WebControls;
using System.Security.Policy;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    public class TicketController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        NotificationController notificationController = new NotificationController();

        [HttpPost]
        [Route("api/ticketBooking")]
        public async Task<IHttpActionResult> TicketBooking(TicketBookingRequest request)
        {
            try
            {
                var events = db.Event.FirstOrDefault(a => a.EventId == request.EventId && a.isActive == true);
                CustomerContact customerContact = new CustomerContact
                {
                    UserId = request.CustomerContactInfo.UserId,
                    FullName = request.CustomerContactInfo.FullName,
                    NickName = request.CustomerContactInfo.NickName,
                    Gender = request.CustomerContactInfo.Gender,
                    DOB = request.CustomerContactInfo.DOB,
                    Email = request.CustomerContactInfo.Email,
                    MobileNumber = request.CustomerContactInfo.MobileNumber,
                    CountryId = request.CustomerContactInfo.CountryId,
                };
                db.CustomerContacts.Add(customerContact);
                db.SaveChanges();

                Random rand = new Random();
                var TicketUniqueNumber = rand.Next(100001, 1000001);
                decimal TotalPrice = 0;
                foreach (var item in request.TicketDetails)
                {
                    TicketBooking ticketBooking = new TicketBooking
                    {
                        PromotorId = events != null ? events.OrganizerID : 0,
                        EventId = request.EventId,
                        TicketId = item.Id,
                        UserId = customerContact.Id,
                        TicketCount = item.TicketCount,
                        TicketUniqueNumber = TicketUniqueNumber,
                        BarCodeURL = CreateBarcode(TicketUniqueNumber),
                        QRCodeURL = CreateQRcode(TicketUniqueNumber),
                        CreatedDate = DateTime.Now,
                        PaymentStatus = "Pending",
                    };
                    db.TicketBookings.Add(ticketBooking);
                    db.SaveChanges();

                    TotalPrice += item.TicketPrice * item.TicketCount;
                }

                PaymentOrderAPIRequest paymentOrderAPIRequest = new PaymentOrderAPIRequest
                {
                    signature = "3FsqMOJCHWdsUfImXPNKYbTi",
                    amount = TotalPrice,
                    operative = "AUTHORIZATION",
                    secure = false,
                    customer_ext_id = "test",
                    service = "3B2D0F75-B2BF-433B-8BFC-26B4085A33A8",
                    description = TicketUniqueNumber.ToString(),
                    additional = null,
                    url_post = "https://tiqarte.azurewebsites.net/api/payments/urlpost",
                    url_ok = "https://tiqarte.azurewebsites.net/api/payments/urlok?ticketid=" + TicketUniqueNumber,
                    url_ko = "https://tiqarte.azurewebsites.net/api/payments/urlko?ticketid=" + TicketUniqueNumber,
                    template_uuid = "6412549E-933E-4DFE-A225-2E87FBF7623E",
                    dcc_template_uuid = "BF418CA6-7043-4864-B36F-F02C2CF2B76B",
                    source_uuid = null,
                    save_card = true,
                    reference = "50620",
                    dynamic_descriptor = "Tiqarte Payments for order number: " + TicketUniqueNumber,
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
                                ticketId = TicketUniqueNumber
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
                        NotificationHeader = "Booking Successful!",
                        NotificationText = "You have successfully booked the " + events.Name + ". The event will be held on " + events.EventDate + ". Don't forget to activate your reminder. Enjoy the event!" ,
                        NotificationType = "Booking",
                        PromotorId = events != null ? events.OrganizerID : 0,
                        UserId = request.CustomerContactInfo.UserId
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

        [HttpGet]
        [Route("api/getCustomerTicketList")]
        public IHttpActionResult GetCustomerTicketList()
        {
            try
            {
                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var CustomerID = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                List<TicketTabList> ticketTabLists = new List<TicketTabList>();
                var ticketBookings = db.TicketBookings.Where(ui => ui.UserId == CustomerID).ToList();
                if (ticketBookings.Count == 0) { return Json(ticketTabLists); }

                foreach (var item in ticketBookings)
                {
                    var _event = db.Event.FirstOrDefault(a => a.EventId == item.EventId && a.isActive == true);
                    DateTimeOffset dto = new DateTimeOffset(_event.EventDate);

                    string Status = string.Empty;
                    if (item.isCancelled == true)
                        Status = "Cancelled";
                    else if (_event.EventDate.Date >= DateTime.Now.Date)
                        Status = "Upcoming";
                    else if (_event.EventDate.Date < DateTime.Now.Date)
                        Status = "Completed";
                    else
                        Status = "Unknown";

                    ticketTabLists.Add(new TicketTabList
                    {
                        EventId = _event.EventId,
                        ImageURL = db.EventImages.FirstOrDefault(d => d.eventid == item.EventId && d.Type == 1).url,
                        EventName = _event.Name,
                        Location = _event.Location,
                        City = _event.City,
                        EventDate = _event.EventDate.ToShortDateString(),
                        EventDateTimeStamp = dto.ToUnixTimeSeconds(),
                        TicketId = item.TicketId,
                        Status = Status,
                        TicketCount = item.TicketCount,
                        TicketUniqueNumber = item.TicketUniqueNumber,
                        isReviewed = _event.isReviewed
                    });
                }

                return Json(ticketTabLists);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getETicket")]
        public IHttpActionResult GetETicket(int TicketUniqueNumber)
        {
            try
            {
                var bookings = db.TicketBookings.Where(a => a.TicketUniqueNumber == TicketUniqueNumber).ToList();
                var bookingdetails = db.TicketBookings.FirstOrDefault(a => a.TicketUniqueNumber == TicketUniqueNumber);

                var _Event = db.Event.FirstOrDefault(a => a.EventId == bookingdetails.EventId && a.isActive == true);
                var _Organizer = db.Organizers.FirstOrDefault(a => a.Id == _Event.OrganizerID);
                var request = db.CustomerContacts.FirstOrDefault(a => a.UserId == bookingdetails.UserId);
                TicketBookingResponse response = new TicketBookingResponse();
                List<TicketBookingDetailResponse> ticketBookingDetailResponse = new List<TicketBookingDetailResponse>();
                DateTimeOffset dto = new DateTimeOffset(_Event.EventDate);

                response.Event = _Event.Name;
                response.EventDate = Convert.ToString(_Event.EventDate);
                response.EventDateTimeStamp = dto.ToUnixTimeSeconds();
                response.Location = _Event.Location;
                response.Organizer = _Organizer.Name;
                response.FullName = request.FullName;
                response.NickName = request.NickName;
                response.Gender = request.Gender == 0 ? "Male" : "Female";
                response.DOB = request.DOB.ToShortDateString();
                response.Email = request.Email;
                response.MobileNo = request.MobileNumber;
                response.Country = db.Countries.FirstOrDefault(a => a.Id == request.CountryId).CountryName;

                foreach (var item in bookings)
                {
                    var ticketdetails = db.TicketDetails.FirstOrDefault(a => a.Id == item.TicketId);
                    ticketBookingDetailResponse.Add(new TicketBookingDetailResponse
                    {
                        TicketType = ticketdetails.TicketType,
                        TicketCount = item.TicketCount,
                        TicketPrice = ticketdetails.TicketPrice,
                        TaxAmount = 0
                    });
                }

                response.TicketBookingDetail = ticketBookingDetailResponse;
                response.PaymentMethod = "MasterCard";
                response.OrderId = TicketUniqueNumber;
                response.Status = "Paid";
                response.BarcodeURL = bookingdetails.BarCodeURL;
                response.QRcodeURL = bookingdetails.QRCodeURL;
                return Json(response);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpPost]
        [Route("api/eventCancel")]
        public IHttpActionResult EventCancel(int TicketUniqueNumber, string reason)
        {
            try
            {
                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var CustomerID = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var Ticekts = db.TicketBookings.FirstOrDefault(a => a.TicketUniqueNumber == TicketUniqueNumber);
                if (Ticekts != null)
                {
                    Ticekts.isCancelled = true;
                    Ticekts.CancelReason = reason;
                    Ticekts.CancelDate = DateTime.Now;
                }

                notificationController.AddUserNotificationLocal(
                    new UserNotificationsRequest
                    {
                        NotificationHeader = "Event Cancel!",
                        NotificationText = "You have successfully canceled the booking.",
                        NotificationType = "Cancel Booking",
                        PromotorId = 0,
                        UserId = CustomerID
                    }
                );

                db.SaveChanges();

                return Json(Ticekts);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        public string CreateBarcode(int Id)
        {
            string barcodeText = Id.ToString("X");
            Bitmap barcodeImage = GenerateBarCode(barcodeText);

            MemoryStream ms = new MemoryStream();
            barcodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            var blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=tiqarteblob;AccountKey=n4JlNEkWc5KzFSPd02dkNKLJqbWjbxH8LHQqrk8zBAd4B8RUtnI9XJetMyo4wtbOJddQ++e93Emd+AStYPy3Vw==;EndpointSuffix=core.windows.net";
            BlobService blobService = new BlobService(blobStorageConnectionString);
            var URI = blobService.UploadFileBlobAsync("tiqarteblob", barcodeText + "_b.png", ms).Result;

            return URI.ToString();
        }

        public string CreateQRcode(int Id)
        {
            string barcodeText = Id.ToString("X");
            Bitmap qrcodeImage = GenerateQRCode(barcodeText);

            MemoryStream ms = new MemoryStream();
            qrcodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            var blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=tiqarteblob;AccountKey=n4JlNEkWc5KzFSPd02dkNKLJqbWjbxH8LHQqrk8zBAd4B8RUtnI9XJetMyo4wtbOJddQ++e93Emd+AStYPy3Vw==;EndpointSuffix=core.windows.net";
            BlobService blobService = new BlobService(blobStorageConnectionString);
            var URI = blobService.UploadFileBlobAsync("tiqarteblob", barcodeText + "_q.png", ms).Result;

            return URI.ToString();
        }

        public static Bitmap GenerateBarCode(string barcodeText)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128, // Choose the barcode format you want
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300, // Set the width and height of the barcode image
                    Height = 100,
                    Margin = 10 // Set the margin around the barcode
                }
            };

            Bitmap barcodeBitmap = writer.Write(barcodeText);
            return barcodeBitmap;
        }

        public static Bitmap GenerateQRCode(string barcodeText)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE, // Choose the barcode format you want
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300, // Set the width and height of the barcode image
                    Height = 300,
                    Margin = 10 // Set the margin around the barcode
                }
            };

            Bitmap barcodeBitmap = writer.Write(barcodeText);
            return barcodeBitmap;
        }

        public void AddDBLogs(string Logs)
        {
            db.AppLogs.Add(new AppLogs
            {
                Logs = Logs
            });
            db.SaveChanges();
        }

        //=============================================================================================//

        [HttpPost]
        [Route("admin/addTicketDetails")]
        public IHttpActionResult AddTicketDetails(TicketDetailsRequest model)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var TicketDetails = db.TicketDetails.Add(new BusinesEntities.TicketDetails
                {
                    PromotorId = PromotoID,
                    EventId = model.EventId,
                    TicketType = model.TicketType,
                    TicketPrice = model.TicketPrice,
                    BookingFee = model.BookingFee,
                    AvailableTickets = model.AvailableTickets,
                    SeasonTicketId = model.SeasonTicketId,
                    AttendeeAge = model.AttendeeAge,
                    HideFromFrontend = model.HideFromFrontend,
                    ExcludeFromOverallCapacity = model.ExcludeFromOverallCapacity,
                    MaximumTickets = model.MaximumTickets,
                    MinimumTickets = model.MinimumTickets,
                    UnitCost = model.UnitCost,
                    RequiredTicketHolderDetails = model.RequiredTicketHolderDetails,
                    TicketDescription = model.TicketDescription,
                    DocumentURL = model.DocumentURL,
                    AcknowledgementURL = model.AcknowledgementURL,
                    MetaDataURL = model.MetaDataURL,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                });

                db.SaveChanges();
                return Json("Record Added Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        [Route("admin/editTicketDetails")]
        public IHttpActionResult EditTicketDetails(UpdateTicketDetailsRequest model)
        {
            var _dc = db.TicketDetails.FirstOrDefault(a => a.Id == model.Id);
            if (_dc != null)
            {
                _dc.EventId = model.EventId;
                _dc.TicketType = model.TicketType;
                _dc.TicketPrice = model.TicketPrice;
                _dc.BookingFee = model.BookingFee;
                _dc.AvailableTickets = model.AvailableTickets;
                _dc.SeasonTicketId = model.SeasonTicketId;
                _dc.AttendeeAge = model.AttendeeAge;
                _dc.HideFromFrontend = model.HideFromFrontend;
                _dc.ExcludeFromOverallCapacity = model.ExcludeFromOverallCapacity;
                _dc.MaximumTickets = model.MaximumTickets;
                _dc.MinimumTickets = model.MinimumTickets;
                _dc.UnitCost = model.UnitCost;
                _dc.RequiredTicketHolderDetails = model.RequiredTicketHolderDetails;
                _dc.TicketDescription = model.TicketDescription;
                _dc.DocumentURL = model.DocumentURL;
                _dc.AcknowledgementURL = model.AcknowledgementURL;
                _dc.MetaDataURL = model.MetaDataURL;
            }

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpDelete]
        [Route("admin/deleteTicketDetails")]
        public IHttpActionResult DeleteTicketDetails(int Id)
        {
            var _et = db.EventTickets.FirstOrDefault(a => a.Id == Id);
            if (_et != null)
                _et.isActive = false;

            var _td = db.TicketDetails.FirstOrDefault(a => a.InitialTicketID == _et.Id);
            if (_td != null)
                _td.isActive = false;

            var _sst = db.StandardSeatTicket.FirstOrDefault(a => a.EventTicketId == _et.Id);
            if (_sst != null)
                _sst.isActive = false;

            var _tpp = db.TicketPasswordProtection.FirstOrDefault(a => a.EventTicketsId == _et.Id);
            if (_tpp != null)
                _tpp.isActive = false;

            db.SaveChanges();

            return Json(_et);
        }

        [HttpGet]
        [Route("admin/getTicketDetailsById")]
        public IHttpActionResult GetTicketDetailsById(int Id)
        {
            var data = db.TicketDetails.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllTicketDetailsByPromotor")]
        public IHttpActionResult GetAllTicketDetailsByPromotor(int PromotorId)
        {
            var data = db.TicketDetails.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllTicketDetails")]
        public IHttpActionResult GetAllTicketDetails()
        {
            var data = db.TicketDetails.Where(a => a.isActive == true).ToList();
            return Json(data);
        }

        //=============================================================================================//

        [HttpPost]
        [Route("admin/addEventTickets")]
        public IHttpActionResult AddEventTickets(EventTicketsRequest model)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var EventTickets = db.EventTickets.Add(new BusinesEntities.EventTickets
                {
                    PromotorId = PromotoID,
                    Date = model.Date,
                    Time = model.Time,
                    EventRunTime = model.EventRunTime,
                    DisplayEventTime = model.DisplayEventTime,
                    Location = model.Location,
                    ManagementFeeType = model.ManagementFeeType,
                    Amount = model.Amount,
                    Add1EuroBookingFeeUnder10 = model.Add1EuroBookingFeeUnder10,
                    Copy = model.Copy,
                    OverrideCapacityScheduleSoldOut = model.OverrideCapacityScheduleSoldOut,
                    MinimumAge = model.MinimumAge,
                    ProductURL = model.ProductURL,
                    isItBuyable = model.isItBuyable,
                    MarkAsSold = model.MarkAsSold,
                    Venue = model.Venue,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                });

                db.SaveChanges();
                return Json("Record Added Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        [Route("admin/editEventTickets")]
        public IHttpActionResult EditEventTickets(UpdateEventTicketsRequest model)
        {
            var _dc = db.EventTickets.FirstOrDefault(a => a.Id == model.Id);
            if (_dc != null)
            {
                _dc.Date = model.Date;
                _dc.Time = model.Time;
                _dc.EventRunTime = model.EventRunTime;
                _dc.DisplayEventTime = model.DisplayEventTime;
                _dc.Location = model.Location;
                _dc.ManagementFeeType = model.ManagementFeeType;
                _dc.Amount = model.Amount;
                _dc.Add1EuroBookingFeeUnder10 = model.Add1EuroBookingFeeUnder10;
                _dc.Copy = model.Copy;
                _dc.OverrideCapacityScheduleSoldOut = model.OverrideCapacityScheduleSoldOut;
                _dc.MinimumAge = model.MinimumAge;
                _dc.ProductURL = model.ProductURL;
                _dc.isItBuyable = model.isItBuyable;
                _dc.MarkAsSold = model.MarkAsSold;
                _dc.Venue = model.Venue;
            }

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpDelete]
        [Route("admin/deleteEventTickets")]
        public IHttpActionResult DeleteEventTickets(int Id)
        {
            var _dc = db.EventTickets.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("admin/getEventTicketsById")]
        public IHttpActionResult GetEventTicketsById(int Id)
        {
            var data = db.EventTickets.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllEventTicketsByPromotor")]
        public IHttpActionResult GetAllEventTicketsByPromotor(int PromotorId)
        {
            List<object> lstResult = new List<object>();
            var data = db.EventTickets.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
            foreach (var item in data)
            {
                var ticketDetails = db.TicketDetails.Where(a => a.InitialTicketID == item.Id).ToList();
                if (ticketDetails.Count > 0)
                {
                    var eventId = ticketDetails[0].EventId;
                    var eventDetails = db.Event.FirstOrDefault(a => a.EventId == eventId);
                    var ticketBookings = db.TicketBookings.Where(a => a.EventId == eventId).ToList();

                    var minPrice = ticketDetails.Min(a => a.TicketPrice);
                    var maxPrice = ticketDetails.Max(a => a.TicketPrice);

                    var eventTicektDetails = new
                    {
                        item.Venue,
                        minPrice,
                        maxPrice,
                        visibility = "Public",
                        eventDate = eventDetails.EventDate.ToShortDateString(),
                        soldTickets = ticketBookings.Sum(a => a.TicketCount),
                        totalTickets = ticketDetails.Sum(a => a.AvailableTickets),
                    };

                    var result = new
                    {
                        EventTicektDetails = eventTicektDetails,
                        EventTickets = item
                    };

                    lstResult.Add(result);
                }
                else
                {
                    var result = new
                    {
                        EventTicektDetails = "",
                        EventTickets = item
                    };

                    lstResult.Add(result);
                }

            }


            return Json(lstResult);
        }

        [HttpGet]
        [Route("admin/getAllEventTickets")]
        public IHttpActionResult GetAllEventTickets()
        {
            var data = db.EventTickets.Where(a => a.isActive == true).ToList();
            return Json(data);
        }

        //=============================================================================================//

        [HttpPost]
        [Route("admin/addTicketPasswordProtection")]
        public IHttpActionResult AddTicketPasswordProtection(TicketPasswordProtectionRequest model)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var TicketPasswordProtection = db.TicketPasswordProtection.Add(new BusinesEntities.TicketPasswordProtection
                {
                    PromotorId = PromotoID,
                    EventTicketsId = model.EventTicketsId,
                    Password = model.Password,
                    isEnablePasswordProtection = model.isEnablePasswordProtection,
                    AutoGeneratedLink = model.AutoGeneratedLink,
                    Visibility = model.Visibility,
                    Slug = model.Slug,
                    URL = model.URL,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                });

                db.SaveChanges();
                return Json("Record Added Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        [Route("admin/editTicketPasswordProtection")]
        public IHttpActionResult EditTicketPasswordProtection(UpdateTicketPasswordProtectionRequest model)
        {
            var _dc = db.TicketPasswordProtection.FirstOrDefault(a => a.Id == model.Id);
            if (_dc != null)
            {
                _dc.Password = model.Password;
                _dc.isEnablePasswordProtection = model.isEnablePasswordProtection;
                _dc.AutoGeneratedLink = model.AutoGeneratedLink;
                _dc.Visibility = model.Visibility;
                _dc.Slug = model.Slug;
                _dc.URL = model.URL;
            }

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpDelete]
        [Route("admin/deleteTicketPasswordProtection")]
        public IHttpActionResult DeleteTicketPasswordProtection(int Id)
        {
            var _dc = db.TicketPasswordProtection.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("admin/getTicketPasswordProtectionById")]
        public IHttpActionResult GetTicketPasswordProtectionById(int Id)
        {
            var data = db.TicketPasswordProtection.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllTicketPasswordProtectionByPromotor")]
        public IHttpActionResult GetAllTicketPasswordProtectionByPromotor(int PromotorId)
        {
            var data = db.TicketPasswordProtection.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllTicketPasswordProtection")]
        public IHttpActionResult GetAllTicketPasswordProtection()
        {
            var data = db.TicketPasswordProtection.Where(a => a.isActive == true).ToList();
            return Json(data);
        }

        //=============================================================================================//

        [HttpPost]
        [Route("admin/addBlockStands")]
        public IHttpActionResult AddBlockStands(BlockStandsRequest model)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var BlockStands = db.BlockStands.Add(new BusinesEntities.BlockStands
                {
                    PromotorId = PromotoID,
                    StadiumId = model.StadiumId,
                    BlockStandName = model.BlockStandName,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                });

                db.SaveChanges();
                return Json("Record Added Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        [Route("admin/editBlockStands")]
        public IHttpActionResult EditBlockStands(UpdateBlockStandsRequest model)
        {
            var _dc = db.BlockStands.FirstOrDefault(a => a.Id == model.Id);
            if (_dc != null)
            {
                _dc.StadiumId = model.StadiumId;
                _dc.BlockStandName = model.BlockStandName;
            }

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpDelete]
        [Route("admin/deleteBlockStands")]
        public IHttpActionResult DeleteBlockStands(int Id)
        {
            var _dc = db.BlockStands.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("admin/getBlockStandsById")]
        public IHttpActionResult GetBlockStandsById(int Id)
        {
            var data = db.BlockStands.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllBlockStandsByPromotor")]
        public IHttpActionResult GetAllBlockStandsByPromotor(int PromotorId)
        {
            var data = db.BlockStands.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllBlockStands")]
        public IHttpActionResult GetAllBlockStands()
        {
            var data = db.BlockStands.Where(a => a.isActive == true).ToList();
            return Json(data);
        }

        //=============================================================================================//

        [HttpPost]
        [Route("admin/addRowsInBlockStands")]
        public IHttpActionResult AddRowsInBlockStands(RowsInBlockStandsRequest model)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var RowsInBlockStands = db.RowsInBlockStands.Add(new BusinesEntities.RowsInBlockStands
                {
                    PromotorId = PromotoID,
                    BlockStandsId = model.BlockStandsId,
                    RowName = model.RowName,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                });

                db.SaveChanges();
                return Json("Record Added Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        [Route("admin/editRowsInBlockStands")]
        public IHttpActionResult EditRowsInBlockStands(UpdateRowsInBlockStandsRequest model)
        {
            var _dc = db.RowsInBlockStands.FirstOrDefault(a => a.Id == model.Id);
            if (_dc != null)
            {
                _dc.RowName = model.RowName;
                _dc.BlockStandsId = model.BlockStandsId;
            }

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpDelete]
        [Route("admin/deleteRowsInBlockStands")]
        public IHttpActionResult DeleteRowsInBlockStands(int Id)
        {
            var _dc = db.RowsInBlockStands.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("admin/getRowsInBlockStandsById")]
        public IHttpActionResult GetRowsInBlockStandsById(int Id)
        {
            var data = db.RowsInBlockStands.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllRowsInBlockStandsByPromotor")]
        public IHttpActionResult GetAllRowsInBlockStandsByPromotor(int PromotorId)
        {
            var data = db.RowsInBlockStands.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllRowsInBlockStands")]
        public IHttpActionResult GetAllRowsInBlockStands()
        {
            var data = db.RowsInBlockStands.Where(a => a.isActive == true).ToList();
            return Json(data);
        }

        //=============================================================================================//

        [HttpPost]
        [Route("admin/addSeatsInRowBlockStands")]
        public IHttpActionResult AddSeatsInRowBlockStands(SeatsInRowBlockStandsRequest model)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var SeatsInRowBlockStands = db.SeatsInRowBlockStands.Add(new BusinesEntities.SeatsInRowBlockStands
                {
                    PromotorId = PromotoID,
                    RowsInBlockStandsId = model.RowsInBlockStandsId,
                    SeatNumber = model.SeatNumber,
                    Price = model.Price,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                });

                db.SaveChanges();
                return Json("Record Added Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        [Route("admin/editSeatsInRowBlockStands")]
        public IHttpActionResult EditSeatsInRowBlockStands(UpdateSeatsInRowBlockStandsRequest model)
        {
            var _dc = db.SeatsInRowBlockStands.FirstOrDefault(a => a.Id == model.Id);
            if (_dc != null)
            {
                _dc.RowsInBlockStandsId = model.RowsInBlockStandsId;
                _dc.SeatNumber = model.SeatNumber;
                _dc.Price = model.Price;
            }

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpDelete]
        [Route("admin/deleteSeatsInRowBlockStands")]
        public IHttpActionResult DeleteSeatsInRowBlockStands(int Id)
        {
            var _dc = db.SeatsInRowBlockStands.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("admin/getSeatsInRowBlockStandsById")]
        public IHttpActionResult GetSeatsInRowBlockStandsById(int Id)
        {
            var data = db.SeatsInRowBlockStands.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllSeatsInRowBlockStandsByPromotor")]
        public IHttpActionResult GetAllSeatsInRowBlockStandsByPromotor(int PromotorId)
        {
            var data = db.SeatsInRowBlockStands.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllSeatsInRowBlockStands")]
        public IHttpActionResult GetAllSeatsInRowBlockStands()
        {
            var data = db.SeatsInRowBlockStands.Where(a => a.isActive == true).ToList();
            return Json(data);
        }

        //=============================================================================================//

        [HttpPost]
        [Route("admin/addExclusions")]
        public IHttpActionResult AddExclusions(ExclusionsRequest model)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var Exclusions = db.Exclusions.Add(new BusinesEntities.Exclusions
                {
                    PromotorId = PromotoID,
                    StadiumId = model.StadiumId,
                    RowsInBlockStandsId = model.RowsInBlockStandsId,
                    BlockStandsId = model.BlockStandsId,
                    SeatsInRowBlockStandsId = model.SeatsInRowBlockStandsId,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                });

                db.SaveChanges();
                return Json("Record Added Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        [Route("admin/editExclusions")]
        public IHttpActionResult EditExclusions(UpdateExclusionsRequest model)
        {
            var _dc = db.Exclusions.FirstOrDefault(a => a.Id == model.Id);
            if (_dc != null)
            {
                _dc.StadiumId = model.StadiumId;
                _dc.RowsInBlockStandsId = model.RowsInBlockStandsId;
                _dc.BlockStandsId = model.BlockStandsId;
                _dc.SeatsInRowBlockStandsId = model.SeatsInRowBlockStandsId;
            }

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpDelete]
        [Route("admin/deleteExclusions")]
        public IHttpActionResult DeleteExclusions(int Id)
        {
            var _dc = db.Exclusions.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("admin/getExclusionsById")]
        public IHttpActionResult GetExclusionsById(int Id)
        {
            var data = db.Exclusions.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllExclusionsByPromotor")]
        public IHttpActionResult GetAllExclusionsByPromotor(int PromotorId)
        {
            var data = db.Exclusions.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllExclusions")]
        public IHttpActionResult GetAllExclusions()
        {
            var data = db.Exclusions.Where(a => a.isActive == true).ToList();
            return Json(data);
        }

        //=============================================================================================//

        [HttpPost]
        [Route("admin/addStandardSeatTicket")]
        public IHttpActionResult AddStandardSeatTicket(StandardSeatTicketRequest model)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var StandardSeatTicket = db.StandardSeatTicket.Add(new BusinesEntities.StandardSeatTicket
                {
                    PromotorId = PromotoID,
                    StadiumId = model.StadiumId,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                    RowsId = model.RowsId,
                    SeatId = PromotoID,
                    EventTicketId = model.EventTicketId,
                    BlockStandId = model.BlockStandId,
                });

                db.SaveChanges();
                return Json("Record Added Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        [Route("admin/editStandardSeatTicket")]
        public IHttpActionResult EditStandardSeatTicket(UpdateStandardSeatTicketRequest model)
        {
            var _dc = db.StandardSeatTicket.FirstOrDefault(a => a.Id == model.Id);
            if (_dc != null)
            {
                _dc.StadiumId = model.StadiumId;
            }

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpDelete]
        [Route("admin/deleteStandardSeatTicket")]
        public IHttpActionResult DeleteStandardSeatTicket(int Id)
        {
            var _dc = db.StandardSeatTicket.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("admin/getStandardSeatTicketById")]
        public IHttpActionResult GetStandardSeatTicketById(int Id)
        {
            var data = db.StandardSeatTicket.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllStandardSeatTicketByPromotor")]
        public IHttpActionResult GetAllStandardSeatTicketByPromotor(int PromotorId)
        {
            var data = db.StandardSeatTicket.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllStandardSeatTicket")]
        public IHttpActionResult GetAllStandardSeatTicket()
        {
            var data = db.StandardSeatTicket.Where(a => a.isActive == true).ToList();
            return Json(data);
        }

        //=============================================================================================//

        [HttpPost]
        [Route("admin/addVariableSeatTicket")]
        public IHttpActionResult AddVariableSeatTicket(VariableSeatTicketRequest model)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var VariableSeatTicket = db.VariableSeatTicket.Add(new BusinesEntities.VariableSeatTicket
                {
                    PromotorId = PromotoID,
                    VariationName = model.VariationName,
                    VariationColor = model.VariationColor,
                    VariationPrice = model.VariationPrice,
                    SeasonTicketId = model.SeasonTicketId,
                    AttendeeAgeTitle = model.AttendeeAgeTitle,
                    SeatApplyFor = model.SeatApplyFor,
                    HideFromFrontEnd = model.HideFromFrontEnd,
                    StadiumId = model.StadiumId,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                });

                db.SaveChanges();
                return Json("Record Added Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpPut]
        [Route("admin/editVariableSeatTicket")]
        public IHttpActionResult EditVariableSeatTicket(UpdateVariableSeatTicketRequest model)
        {
            var _dc = db.VariableSeatTicket.FirstOrDefault(a => a.Id == model.Id);
            if (_dc != null)
            {
                _dc.VariationName = model.VariationName;
                _dc.VariationColor = model.VariationColor;
                _dc.VariationPrice = model.VariationPrice;
                _dc.SeasonTicketId = model.SeasonTicketId;
                _dc.AttendeeAgeTitle = model.AttendeeAgeTitle;
                _dc.SeatApplyFor = model.SeatApplyFor;
                _dc.HideFromFrontEnd = model.HideFromFrontEnd;
                _dc.StadiumId = model.StadiumId;
            }

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpDelete]
        [Route("admin/deleteVariableSeatTicket")]
        public IHttpActionResult DeleteVariableSeatTicket(int Id)
        {
            var _dc = db.VariableSeatTicket.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("admin/getVariableSeatTicketById")]
        public IHttpActionResult GetVariableSeatTicketById(int Id)
        {
            var data = db.VariableSeatTicket.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllVariableSeatTicketByPromotor")]
        public IHttpActionResult GetAllVariableSeatTicketByPromotor(int PromotorId)
        {
            var data = db.VariableSeatTicket.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllVariableSeatTicket")]
        public IHttpActionResult GetAllVariableSeatTicket()
        {
            var data = db.VariableSeatTicket.Where(a => a.isActive == true).ToList();
            return Json(data);
        }

        //=============================================================================================//

        [HttpGet]
        [Route("admin/getRowsAndSeatsByStandId")]
        public IHttpActionResult GetRowsAndSeatsByStandId(int StandId)
        {
            List<RowsInBlockStandsModel> lstRowsInBlockStandsModel = new List<RowsInBlockStandsModel>();
            var data = db.RowsInBlockStands.Where(a => a.isActive == true && a.BlockStandsId == StandId).ToList();
            foreach (var item in data)
            {
                var seat = db.SeatsInRowBlockStands.Where(a => a.RowsInBlockStandsId == item.Id).ToList();

                lstRowsInBlockStandsModel.Add(new RowsInBlockStandsModel
                {
                    Id = item.Id,
                    RowName = item.RowName,
                    PromotorId = item.PromotorId,
                    BlockStandsId = item.BlockStandsId,
                    lstSeatsInRowBlockStands = seat
                });
            }
            return Json(lstRowsInBlockStandsModel);
        }

        //=============================================================================================//

        [HttpPost]
        [Route("admin/addEventTicketsFull")]
        public IHttpActionResult AddEventTicketsFull(AddEventTicketRequest model)
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

            try
            {
                var EventTickets = db.EventTickets.Add(new BusinesEntities.EventTickets
                {
                    PromotorId = model.TicketInitials.PromotorId,
                    Date = model.TicketInitials.Date,
                    Time = model.TicketInitials.Time,
                    EventRunTime = model.TicketInitials.EventRunTime,
                    DisplayEventTime = model.TicketInitials.DisplayEventTime,
                    Location = model.TicketInitials.Location,
                    ManagementFeeType = model.TicketInitials.ManagementFeeType,
                    Amount = model.TicketInitials.Amount,
                    Add1EuroBookingFeeUnder10 = model.TicketInitials.Add1EuroBookingFeeUnder10,
                    Copy = model.TicketInitials.Copy,
                    OverrideCapacityScheduleSoldOut = model.TicketInitials.OverrideCapacityScheduleSoldOut,
                    MinimumAge = model.TicketInitials.MinimumAge,
                    ProductURL = model.TicketInitials.ProductURL,
                    isItBuyable = model.TicketInitials.isItBuyable,
                    MarkAsSold = model.TicketInitials.MarkAsSold,
                    Venue = model.TicketInitials.Venue,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                });
                db.SaveChanges();

                var TicketDetails = db.TicketDetails.Add(new BusinesEntities.TicketDetails
                {
                    PromotorId = PromotoID,
                    EventId = model.TicketDetails.EventId,
                    TicketType = model.TicketDetails.TicketType,
                    TicketPrice = model.TicketDetails.TicketPrice,
                    BookingFee = model.TicketDetails.BookingFee,
                    AvailableTickets = model.TicketDetails.AvailableTickets,
                    SeasonTicketId = model.TicketDetails.SeasonTicketId,
                    AttendeeAge = model.TicketDetails.AttendeeAge,
                    HideFromFrontend = model.TicketDetails.HideFromFrontend,
                    ExcludeFromOverallCapacity = model.TicketDetails.ExcludeFromOverallCapacity,
                    MaximumTickets = model.TicketDetails.MaximumTickets,
                    MinimumTickets = model.TicketDetails.MinimumTickets,
                    UnitCost = model.TicketDetails.UnitCost,
                    RequiredTicketHolderDetails = model.TicketDetails.RequiredTicketHolderDetails,
                    TicketDescription = model.TicketDetails.TicketDescription,
                    DocumentURL = model.TicketDetails.DocumentURL,
                    AcknowledgementURL = model.TicketDetails.AcknowledgementURL,
                    MetaDataURL = model.TicketDetails.MetaDataURL,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                    InitialTicketID = EventTickets.Id
                });
                db.SaveChanges();

                var StandardSeatTicket = db.StandardSeatTicket.Add(new BusinesEntities.StandardSeatTicket
                {
                    PromotorId = PromotoID,
                    StadiumId = model.StandardSeatTicketRequest.StadiumId,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                    RowsId = model.StandardSeatTicketRequest.RowsId,
                    SeatId = PromotoID,
                    EventTicketId = EventTickets.Id,
                    BlockStandId = model.StandardSeatTicketRequest.BlockStandId,
                });
                db.SaveChanges();

                var TicketPasswordProtection = db.TicketPasswordProtection.Add(new BusinesEntities.TicketPasswordProtection
                {
                    PromotorId = PromotoID,
                    EventTicketsId = EventTickets.Id,
                    Password = model.TicketPasswordProtectionRequest.Password,
                    isEnablePasswordProtection = model.TicketPasswordProtectionRequest.isEnablePasswordProtection,
                    AutoGeneratedLink = model.TicketPasswordProtectionRequest.AutoGeneratedLink,
                    Visibility = model.TicketPasswordProtectionRequest.Visibility,
                    Slug = model.TicketPasswordProtectionRequest.Slug,
                    URL = model.TicketPasswordProtectionRequest.URL,
                    isActive = true,
                    CreatedDate = DateTime.Now,
                });

                db.SaveChanges();
                return Json("Record Added Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }

        [HttpPost]
        [Route("admin/updateEventTicketsFinal")]
        public IHttpActionResult UpdateEventTicketsFinal(UpdateEventTicketRequest model)
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

            try
            {
                var eventTickets = db.EventTickets.FirstOrDefault(a => a.Id == model.TicketInitials.Id);
                if (eventTickets != null)
                {
                    eventTickets.PromotorId = PromotoID;
                    eventTickets.Date = model.TicketInitials.Date;
                    eventTickets.Time = model.TicketInitials.Time;
                    eventTickets.EventRunTime = model.TicketInitials.EventRunTime;
                    eventTickets.DisplayEventTime = model.TicketInitials.DisplayEventTime;
                    eventTickets.Location = model.TicketInitials.Location;
                    eventTickets.ManagementFeeType = model.TicketInitials.ManagementFeeType;
                    eventTickets.Amount = model.TicketInitials.Amount;
                    eventTickets.Add1EuroBookingFeeUnder10 = model.TicketInitials.Add1EuroBookingFeeUnder10;
                    eventTickets.Copy = model.TicketInitials.Copy;
                    eventTickets.OverrideCapacityScheduleSoldOut = model.TicketInitials.OverrideCapacityScheduleSoldOut;
                    eventTickets.MinimumAge = model.TicketInitials.MinimumAge;
                    eventTickets.ProductURL = model.TicketInitials.ProductURL;
                    eventTickets.isItBuyable = model.TicketInitials.isItBuyable;
                    eventTickets.MarkAsSold = model.TicketInitials.MarkAsSold;
                    eventTickets.Venue = model.TicketInitials.Venue;
                };
                db.SaveChanges();

                var ticketDetails = db.TicketDetails.FirstOrDefault(a => a.Id == model.TicketDetails.Id);
                if (ticketDetails != null)
                {
                    ticketDetails.PromotorId = PromotoID;
                    ticketDetails.EventId = model.TicketDetails.EventId;
                    ticketDetails.TicketType = model.TicketDetails.TicketType;
                    ticketDetails.TicketPrice = model.TicketDetails.TicketPrice;
                    ticketDetails.BookingFee = model.TicketDetails.BookingFee;
                    ticketDetails.AvailableTickets = model.TicketDetails.AvailableTickets;
                    ticketDetails.SeasonTicketId = model.TicketDetails.SeasonTicketId;
                    ticketDetails.AttendeeAge = model.TicketDetails.AttendeeAge;
                    ticketDetails.HideFromFrontend = model.TicketDetails.HideFromFrontend;
                    ticketDetails.ExcludeFromOverallCapacity = model.TicketDetails.ExcludeFromOverallCapacity;
                    ticketDetails.MaximumTickets = model.TicketDetails.MaximumTickets;
                    ticketDetails.MinimumTickets = model.TicketDetails.MinimumTickets;
                    ticketDetails.UnitCost = model.TicketDetails.UnitCost;
                    ticketDetails.RequiredTicketHolderDetails = model.TicketDetails.RequiredTicketHolderDetails;
                    ticketDetails.TicketDescription = model.TicketDetails.TicketDescription;
                    ticketDetails.DocumentURL = model.TicketDetails.DocumentURL;
                    ticketDetails.AcknowledgementURL = model.TicketDetails.AcknowledgementURL;
                    ticketDetails.MetaDataURL = model.TicketDetails.MetaDataURL;
                };
                db.SaveChanges();

                var standardSeatTicket = db.StandardSeatTicket.FirstOrDefault(a => a.Id == model.StandardSeatTicketRequest.Id);
                if (standardSeatTicket != null)
                {
                    standardSeatTicket.PromotorId = PromotoID;
                    standardSeatTicket.StadiumId = model.StandardSeatTicketRequest.StadiumId;
                    standardSeatTicket.RowsId = model.StandardSeatTicketRequest.RowsId;
                    standardSeatTicket.SeatId = model.StandardSeatTicketRequest.SeatId;
                    standardSeatTicket.BlockStandId = model.StandardSeatTicketRequest.BlockStandId;
                };
                db.SaveChanges();

                var ticketPasswordProtection = db.TicketPasswordProtection.FirstOrDefault(a => a.Id == model.TicketPasswordProtectionRequest.Id);
                if (ticketPasswordProtection != null)
                {
                    ticketPasswordProtection.PromotorId = PromotoID;
                    ticketPasswordProtection.Password = model.TicketPasswordProtectionRequest.Password;
                    ticketPasswordProtection.isEnablePasswordProtection = model.TicketPasswordProtectionRequest.isEnablePasswordProtection;
                    ticketPasswordProtection.AutoGeneratedLink = model.TicketPasswordProtectionRequest.AutoGeneratedLink;
                    ticketPasswordProtection.Visibility = model.TicketPasswordProtectionRequest.Visibility;
                    ticketPasswordProtection.Slug = model.TicketPasswordProtectionRequest.Slug;
                    ticketPasswordProtection.URL = model.TicketPasswordProtectionRequest.URL;
                };

                db.SaveChanges();
                return Json("Record Updated Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }
    }
}

public class RowsInBlockStandsModel
{
    public int Id { get; set; }
    public int PromotorId { get; set; }
    public int BlockStandsId { get; set; }
    public string RowName { get; set; }
    public List<SeatsInRowBlockStands> lstSeatsInRowBlockStands { get; set; }
}

public class SeatsInRowBlockStandsModel
{
    public int Id { get; set; }
    public int PromotorId { get; set; }
    public int RowsInBlockStandsId { get; set; }
    public string SeatNumber { get; set; }
    public decimal Price { get; set; }
}
