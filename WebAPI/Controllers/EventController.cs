using BusinesEntities;
using CsQuery.Engine.PseudoClassSelectors;
using CsQuery.ExtensionMethods.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Lifetime;
using System.Security.Claims;
using System.Security.Policy;
using System.Web;
using System.Web.Http;
using WebAPI.Models;
using WebAPI.Services;
using static iTextSharp.text.pdf.AcroFields;
using static System.Net.Mime.MediaTypeNames;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace WebAPI.Controllers
{
    public class EventController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("api/setFav")]
        public IHttpActionResult SetFav(int eventID, bool fav, int customerID)
        {
            try
            {
                if (!fav)
                {
                    var favourite = db.Favorites.Where(ui => ui.CustomerID == customerID && ui.EventID == eventID).FirstOrDefault();
                    if (favourite == null) { return Json("No record found"); }
                    db.Favorites.Remove(favourite);
                    db.SaveChanges();
                    return Json("Favourite Removed");
                }
                else
                {
                    var evnt = db.Event.Where(d => d.EventId == eventID && d.isActive == true).FirstOrDefault();
                    if (evnt == null)
                        return Json("Event Not found");

                    var isAdded = db.Favorites.FirstOrDefault(a => a.EventID == eventID && a.CustomerID == customerID);
                    if (isAdded != null)
                        return Json("Event Already Added");

                    db.Favorites.Add(new BusinesEntities.Favourites { CustomerID = customerID, EventID = eventID });
                    db.SaveChanges();
                    return Json("Favourite Added");
                }
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getfavList")]
        public IHttpActionResult GetfavList(int customerID)
        {
            try
            {
                List<Event> evr = new List<Event>();
                var favourite = db.Favorites.Where(ui => ui.CustomerID == customerID).ToList();
                if (favourite.Count == 0) { return Json(evr); }

                evr = (from e in db.Event
                       join f in db.Favorites on e.EventId equals f.EventID
                       where e.isActive == true
                       select e).Distinct().ToList(); ;
                //var events = db.Event.Where(e => favourite.Any(f => e.EventId == f.EventID));
                foreach (var ev in evr)
                {
                    ev.PostEventImages = db.EventImages.Where(d => d.eventid == ev.EventId && d.Type == 1).Select(i => i.url).ToArray();
                    ev.PreEventImages = db.EventImages.Where(d => d.eventid == ev.EventId && d.Type == 0).Select(i => i.url).ToArray();
                    ev.isFav = true;
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
        [Route("api/getEvents")]
        public HttpResponseMessage GetEvent(string SearchText)
        {
            try
            {
                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var CustomerID = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var lst = db.Event.Where(a => a.isActive == true).AsEnumerable();

                if (!string.IsNullOrEmpty(SearchText))
                    lst = lst.Where(a => a.Name.Contains(SearchText) || a.Discription.Contains(SearchText) || a.Location.Contains(SearchText)).AsEnumerable();

                foreach (Event ve in lst)
                {
                    ve.PreEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 0).Select(i => i.url).ToArray();
                    ve.PostEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
                    var Fav = db.Favorites.FirstOrDefault(e => e.CustomerID == CustomerID && e.EventID == ve.EventId);
                    ve.isFav = Fav == null ? false : true;
                }

                if (lst != null)
                    return this.Request.CreateResponse(HttpStatusCode.OK, lst);
                else
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, lst);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
            }
        }

        [HttpGet]
        [Route("api/getEventDetail/")]
        public IHttpActionResult GetEventDetail(int eventID)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var CustomerID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var _Event = db.Event.FirstOrDefault(ui => ui.EventId == eventID && ui.isActive == true);
                if (_Event == null)
                    return Json("No record found");

                _Event.PreEventImages = db.EventImages.Where(e => e.eventid == _Event.EventId && e.Type == 1).Select(i => i.url).ToArray();
                _Event.PostEventImages = db.EventImages.Where(e => e.eventid == _Event.EventId && e.Type == 1).Select(i => i.url).ToArray();
                var Fav = db.Favorites.FirstOrDefault(e => e.CustomerID == CustomerID && e.EventID == _Event.EventId);
                _Event.isFav = Fav == null ? false : true;

                var _Organizer = db.Organizers.FirstOrDefault(a => a.Id == _Event.OrganizerID);
                var _Customer = db.TicketBookings.Where(a => a.EventId == _Event.EventId).ToList();
                var _Tickets = db.TicketDetails.Where(a => a.EventId == eventID).ToList();

                List<CustomerViewModel> _CustomerViewModel = new List<CustomerViewModel>();
                foreach (var item in _Customer)
                {
                    var usr = db.Users.FirstOrDefault(a => a.UserId == item.UserId);
                    if (usr != null)
                    {
                        _CustomerViewModel.Add(new CustomerViewModel
                        {
                            UserId = item.UserId,
                            ImageURL = usr == null ? "" : usr.ImageUrl ?? "",
                            Name = usr.FirstName + " " + usr.LastName,
                        });
                    }
                }

                List<EventTicketDetails> _EventTicketDetails = new List<EventTicketDetails>();
                foreach (var item in _Tickets)
                {
                    _EventTicketDetails.Add(new EventTicketDetails
                    {
                        Id = item.Id,
                        TicketType = item.TicketType,
                        TicketPrice = item.TicketPrice,
                    });
                }

                double totalRating = 0;
                var _totalRating = db.EventReviews.Where(a => a.EventId == _Event.EventId).AsQueryable();
                if (_totalRating.Any())
                {
                    totalRating = _totalRating.Sum(item => item.Ratings);
                    int reviews = db.EventReviews.Count(a => a.EventId == _Event.EventId);
                    totalRating = Math.Round(totalRating / reviews, 1);
                }


                EventDetailViewModel eventDetailViewModel = new EventDetailViewModel();
                eventDetailViewModel.isOrganizerFollow = db.CustomerOrganizerFollow.FirstOrDefault(a => a.UserId == CustomerID && a.OrganizerId == _Event.OrganizerID) == null ? false : true;
                eventDetailViewModel.PeopleGoing = _Customer.Select(a => a.TicketCount).Sum();
                eventDetailViewModel.Event = _Event;
                eventDetailViewModel.Organizer = _Organizer;
                eventDetailViewModel.Organizer.Collections = db.OrganizerCollections.Where(a => a.OrganizerId == _Event.OrganizerID).Select(i => i.ImageURL).ToArray();
                eventDetailViewModel.Customers = _CustomerViewModel;
                eventDetailViewModel.EventTicketDetails = _EventTicketDetails;
                eventDetailViewModel.ReviewRating = totalRating;

                return Json(eventDetailViewModel);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getHomeData/")]
        public IHttpActionResult GetHomeData(int? CategoryId, string City, LocationSearch LocationSearch)
        {
            try
            {
                List<EventViewModel> events = new List<EventViewModel>();
                var HomeData = new HomeDataViewModel();
                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var CustomerID = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
                var user = db.Users.FirstOrDefault(a => a.UserId == CustomerID);
                if (user == null) { return Json(HomeData); }

                var upcomingEvents = db.Event.Where(a => a.isActive == true).OrderByDescending(a => a.EventDate).Select(a => new EventViewModel
                {
                    EventId = a.EventId,
                    Name = a.Name,
                    CompnayName = a.CompnayName,
                    Discription = a.Discription,
                    Location = a.Location,
                    City = a.City,
                    EventDate = a.EventDate,
                    CreationUserId = a.CreationUserId,
                    EventStatusId = a.EventStatusId,
                    EventTypeId = a.EventTypeId,
                    CatagoryId = a.CatagoryId,
                    Price = a.Price,
                    OrganizerID = a.OrganizerID,
                    IsPublished = a.IsPublished,
                    LastUpdated = a.LastUpdated,
                    StandingTitle = a.StandingTitle,
                    SeatingTitle = a.SeatingTitle,
                    TicketSoldOutText = a.TicketSoldOutText,
                }).AsEnumerable();

                if (CategoryId != null)
                    upcomingEvents = upcomingEvents.Where(a => a.CatagoryId == CategoryId).AsEnumerable();

                if (!string.IsNullOrWhiteSpace(City))
                    upcomingEvents = upcomingEvents.Where(a => a.City == City).AsEnumerable();

                if (LocationSearch != null)
                {
                    upcomingEvents = upcomingEvents.ToList();
                    foreach (var item in upcomingEvents)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Location))
                        {
                            double lat1 = Convert.ToDouble(item.Location.Split(',').First());
                            double lon1 = Convert.ToDouble(item.Location.Split(',').Last());
                            double _distance = CalculateDistance(lat1, lon1, LocationSearch.Lat, LocationSearch.Long);
                            if (_distance <= LocationSearch.Disctance)
                                events.Add(item);
                        }
                    }

                    upcomingEvents = events;
                }

                var prods = db.ShopProduct.Where(a => a.isActive == true && a.PromotorId == user.PromotorId).OrderByDescending(d => d.CreatedDate).Select(a => new ShopProductViewModel
                {
                    Id = a.Id,
                    Sku = a.Sku,
                    ProductName = a.ProductName,
                    DeliveryDetails = a.DeliveryDetails,
                    Price = a.Price,
                    CatagoryId = a.CatagoryId,
                    ProductFor = a.ProductFor,
                    isActive = a.isActive,
                    PromotorId = a.PromotorId,
                    CreatedDate = a.CreatedDate,
                }).Take(10).ToArray();

                HomeData.Shop = prods;
                HomeData.WelcomeMessage = DateTime.Now.Hour < 12 ? "Good Morning" : DateTime.Now.Hour < 17 ? "Good Afternoon" : "Good Evening";
                HomeData.FeaturedEvents = upcomingEvents.Where(a => a.EventTypeId == 1).Take(10).ToList();
                HomeData.UpComingEvents = upcomingEvents.Where(a => a.EventDate >= DateTime.Now).Take(10).ToList();
                HomeData.UserId = Convert.ToInt32(CustomerID);

                foreach (var item in HomeData.Shop)
                    item.ProductImages = db.ShopProductImages.Where(a => a.ProductId == item.Id).Select(a => a.ImageURL).ToArray();

                foreach (EventViewModel ve in HomeData.FeaturedEvents)
                {
                    DateTimeOffset dto = new DateTimeOffset(ve.EventDate);

                    ve.PreEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
                    ve.PostEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
                    ve.City = ve.City == null ? "" : ve.City;
                    ve.EventDateTimeStamp = dto.ToUnixTimeSeconds();

                    var Fav = db.Favorites.FirstOrDefault(e => e.CustomerID == HomeData.UserId && e.EventID == ve.EventId);
                    ve.isFav = Fav == null ? false : true;
                }

                foreach (EventViewModel ve in HomeData.UpComingEvents)
                {
                    DateTimeOffset dto = new DateTimeOffset(ve.EventDate);

                    ve.PreEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
                    ve.PostEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
                    ve.City = ve.City == null ? "" : ve.City;
                    ve.EventDateTimeStamp = dto.ToUnixTimeSeconds();

                    var Fav = db.Favorites.FirstOrDefault(e => e.CustomerID == HomeData.UserId && e.EventID == ve.EventId);
                    ve.isFav = Fav == null ? false : true;
                }

                int Cancelled = 0, Going = 0, Completed = 0;
                var ticketBookings = db.TicketBookings.Where(ui => ui.UserId == HomeData.UserId).ToList();

                foreach (var item in ticketBookings)
                {
                    var _event = db.Event.FirstOrDefault(a => a.EventId == item.EventId && a.isActive == true);
                    if (_event != null)
                    {
                        if (item.isCancelled == true)
                            Cancelled++;
                        else if (_event.EventDate.Date >= DateTime.Now.Date)
                            Going++;
                        else if (_event.EventDate.Date < DateTime.Now.Date)
                            Completed++;
                    }
                }

                HomeData.EventCounts = new EventCounts { Cancelled = Cancelled, Completed = Completed, Going = Going };
                HomeData.InviteFriendsLink = db.Links.FirstOrDefault(a => a.PromotorId == 1).InviteFriendsLink;

                return Json(HomeData);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getHomeDataV2")]
        public IHttpActionResult GetHomeDataV2(string SearchText, int? CategoryId, string City, LocationSearchV2 LocationSearch, int? sPrice = 0, int? ePrice = 100)
        {
            try
            {
                List<int> ticketeventids = new List<int>();
                List<EventViewModel> events = new List<EventViewModel>();
                var HomeData = new HomeDataViewModel();
                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var CustomerID = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
                var user = db.Users.FirstOrDefault(a => a.UserId == CustomerID);
                if (user == null) { return Json(HomeData); }

                var upcomingEvents = db.Event.Where(a => a.isActive == true).OrderByDescending(a => a.EventDate).Select(a => new EventViewModel
                {
                    EventId = a.EventId,
                    Name = a.Name,
                    CompnayName = a.CompnayName,
                    Discription = a.Discription,
                    Location = a.Location,
                    City = a.City,
                    EventDate = a.EventDate,
                    CreationUserId = a.CreationUserId,
                    EventStatusId = a.EventStatusId,
                    EventTypeId = a.EventTypeId,
                    CatagoryId = a.CatagoryId,
                    Price = a.Price,
                    OrganizerID = a.OrganizerID,
                    IsPublished = a.IsPublished,
                    LastUpdated = a.LastUpdated,
                    StandingTitle = a.StandingTitle,
                    SeatingTitle = a.SeatingTitle,
                    TicketSoldOutText = a.TicketSoldOutText,
                }).AsEnumerable();

                if (!string.IsNullOrEmpty(SearchText))
                    upcomingEvents = upcomingEvents.Where(a => a.Name.ToLower().Contains(SearchText.ToLower()) || a.Discription.ToLower().Contains(SearchText.ToLower())).AsEnumerable();

                if (CategoryId != null)
                    upcomingEvents = upcomingEvents.Where(a => a.CatagoryId == CategoryId).AsEnumerable();

                if (!string.IsNullOrWhiteSpace(City))
                    upcomingEvents = upcomingEvents.Where(a => a.City.ToLower().Contains(City.ToLower())).AsEnumerable();

                if (LocationSearch != null)
                {
                    upcomingEvents = upcomingEvents.ToList();
                    foreach (var item in upcomingEvents)
                    {
                        if (!string.IsNullOrWhiteSpace(item.Location))
                        {
                            double lat1 = Convert.ToDouble(item.Location.Split(',').First());
                            double lon1 = Convert.ToDouble(item.Location.Split(',').Last());
                            double _distance = CalculateDistance(lat1, lon1, LocationSearch.Lat, LocationSearch.Long);
                            if (_distance <= LocationSearch.Distance)
                                events.Add(item);
                        }
                    }

                    upcomingEvents = events;
                }

                foreach (var item in upcomingEvents)
                {
                    double totalRating = 0;
                    var _totalRating = db.EventReviews.Where(a => a.EventId == item.EventId).AsQueryable();
                    if (_totalRating.Any())
                    {
                        totalRating = _totalRating.Sum(item => item.Ratings);
                    }

                    int reviews = db.EventReviews.Count(a => a.EventId == item.EventId);
                    item.ReviewRating = Math.Round(totalRating / reviews, 1);

                    var _Tickets = db.TicketDetails.Where(a => a.EventId == item.EventId).AsQueryable();
                    if (_Tickets.Any())
                    {
                        _Tickets = _Tickets.Where(a => a.TicketPrice >= sPrice && a.TicketPrice <= ePrice).AsQueryable();
                        ticketeventids.AddRange(_Tickets.Select(a => a.EventId).Distinct());
                    }
                }

                upcomingEvents = upcomingEvents.Where(a => ticketeventids.Contains(a.EventId)).AsQueryable();

                var prods = db.ShopProduct.Where(a => a.isActive == true && a.PromotorId == user.PromotorId).OrderByDescending(d => d.CreatedDate).Select(a => new ShopProductViewModel
                {
                    Id = a.Id,
                    Sku = a.Sku,
                    ProductName = a.ProductName,
                    DeliveryDetails = a.DeliveryDetails,
                    Price = a.Price,
                    CatagoryId = a.CatagoryId,
                    ProductFor = a.ProductFor,
                    isActive = a.isActive,
                    PromotorId = a.PromotorId,
                    CreatedDate = a.CreatedDate,
                }).Take(10).ToArray();

                HomeData.Shop = prods;
                HomeData.WelcomeMessage = DateTime.Now.Hour < 12 ? "Good Morning" : DateTime.Now.Hour < 17 ? "Good Afternoon" : "Good Evening";
                HomeData.FeaturedEvents = upcomingEvents.Where(a => a.EventTypeId == 1).Take(10).ToList();
                HomeData.UpComingEvents = upcomingEvents.Where(a => a.EventDate >= DateTime.Now).Take(10).ToList();
                HomeData.UserId = Convert.ToInt32(CustomerID);

                foreach (var item in HomeData.Shop)
                    item.ProductImages = db.ShopProductImages.Where(a => a.ProductId == item.Id).Select(a => a.ImageURL).ToArray();

                foreach (EventViewModel ve in HomeData.FeaturedEvents)
                {
                    DateTimeOffset dto = new DateTimeOffset(ve.EventDate);

                    ve.PreEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
                    ve.PostEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
                    ve.City = ve.City == null ? "" : ve.City;
                    ve.EventDateTimeStamp = dto.ToUnixTimeSeconds();

                    var Fav = db.Favorites.FirstOrDefault(e => e.CustomerID == HomeData.UserId && e.EventID == ve.EventId);
                    ve.isFav = Fav == null ? false : true;
                }

                foreach (EventViewModel ve in HomeData.UpComingEvents)
                {
                    DateTimeOffset dto = new DateTimeOffset(ve.EventDate);

                    ve.PreEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
                    ve.PostEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
                    ve.City = ve.City == null ? "" : ve.City;
                    ve.EventDateTimeStamp = dto.ToUnixTimeSeconds();

                    var Fav = db.Favorites.FirstOrDefault(e => e.CustomerID == HomeData.UserId && e.EventID == ve.EventId);
                    ve.isFav = Fav == null ? false : true;
                }

                int Cancelled = 0, Going = 0, Completed = 0;
                var ticketBookings = db.TicketBookings.Where(ui => ui.UserId == HomeData.UserId).ToList();

                foreach (var item in ticketBookings)
                {
                    var _event = db.Event.FirstOrDefault(a => a.EventId == item.EventId && a.isActive == true);
                    if (_event != null)
                    {
                        if (item.isCancelled == true)
                            Cancelled++;
                        else if (_event.EventDate.Date >= DateTime.Now.Date)
                            Going++;
                        else if (_event.EventDate.Date < DateTime.Now.Date)
                            Completed++;
                    }
                }

                HomeData.EventCounts = new EventCounts { Cancelled = Cancelled, Completed = Completed, Going = Going };
                HomeData.InviteFriendsLink = db.Links.FirstOrDefault(a => a.PromotorId == 1).InviteFriendsLink;

                return Json(HomeData);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getHomeDataV3")]
        public IHttpActionResult GetHomeDataV3(string searchText, int? categoryId, string city, LocationSearchV2 locationSearch, int? sPrice = 0, int? ePrice = 1000)
        {
            try
            {
                var homeData = new HomeDataViewModel();

                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var customerId = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var user = db.Users.FirstOrDefault(a => a.UserId == customerId);
                if (user == null)
                {
                    return Json(homeData);
                }

                var upcomingEventsQuery = db.Event
                    .Where(a => a.isActive && (string.IsNullOrEmpty(searchText) || a.Name.ToLower().Contains(searchText.ToLower()) || a.Discription.ToLower().Contains(searchText.ToLower())))
                    .OrderByDescending(a => a.EventDate).Select(a => new EventViewModel
                    {
                        EventId = a.EventId,
                        Name = a.Name,
                        CompnayName = a.CompnayName,
                        Discription = a.Discription,
                        Location = a.Location,
                        City = a.City,
                        EventDate = a.EventDate,
                        CreationUserId = a.CreationUserId,
                        EventStatusId = a.EventStatusId,
                        EventTypeId = a.EventTypeId,
                        CatagoryId = a.CatagoryId,
                        Price = a.Price,
                        OrganizerID = a.OrganizerID,
                        IsPublished = a.IsPublished,
                        LastUpdated = a.LastUpdated,
                        StandingTitle = a.StandingTitle,
                        SeatingTitle = a.SeatingTitle,
                        TicketSoldOutText = a.TicketSoldOutText,
                    }).AsQueryable();

                if (categoryId != null)
                {
                    upcomingEventsQuery = upcomingEventsQuery.Where(a => a.CatagoryId == categoryId).AsQueryable();
                }

                if (!string.IsNullOrWhiteSpace(city))
                {
                    upcomingEventsQuery = upcomingEventsQuery.Where(a => a.City.ToLower().Contains(city.ToLower())).AsQueryable();
                }

                if (locationSearch != null)
                {
                    upcomingEventsQuery = upcomingEventsQuery.Where(a =>
                        !string.IsNullOrWhiteSpace(a.Location) &&
                        CalculateDistance(Convert.ToDouble(a.Location.Split(',').First()), Convert.ToDouble(a.Location.Split(',').Last()), locationSearch.Lat, locationSearch.Long) <= locationSearch.Distance);
                }

                var upcomingEvents = upcomingEventsQuery
                    .ToList();

                var upcomingEventIds = upcomingEvents.Select(a => a.EventId).ToList();

                var ticketEventIds = db.TicketDetails
                    .Where(a => upcomingEventIds.Contains(a.EventId) && a.TicketPrice >= sPrice && a.TicketPrice <= ePrice)
                    .Select(a => a.EventId)
                    .Distinct()
                    .ToList();

                upcomingEvents = upcomingEvents
                    .Where(a => ticketEventIds.Contains(a.EventId))
                    .ToList();

                var shopProducts = db.ShopProduct
                    .Where(a => a.isActive && a.PromotorId == user.PromotorId && a.Price >= sPrice && a.Price <= ePrice)
                    .OrderByDescending(d => d.CreatedDate)
                    .Take(10)
                    .Select(a => new ShopProductViewModel
                    {
                        Id = a.Id,
                        Sku = a.Sku,
                        ProductName = a.ProductName,
                        DeliveryDetails = a.DeliveryDetails,
                        Price = a.Price,
                        CatagoryId = a.CatagoryId,
                        ProductFor = a.ProductFor,
                        isActive = a.isActive,
                        PromotorId = a.PromotorId,
                        CreatedDate = a.CreatedDate,
                    })
                    .ToArray();

                homeData.Shop = shopProducts;
                homeData.WelcomeMessage = DateTime.Now.Hour < 12 ? "Good Morning" : DateTime.Now.Hour < 17 ? "Good Afternoon" : "Good Evening";
                homeData.FeaturedEvents = upcomingEvents.Where(a => a.EventTypeId == 1).Take(10).ToList();
                homeData.UpComingEvents = upcomingEvents.Where(a => a.EventDate >= DateTime.Now).Take(10).ToList();
                homeData.UserId = Convert.ToInt32(customerId);

                foreach (var item in homeData.Shop)
                {
                    item.ProductImages = db.ShopProductImages.Where(a => a.ProductId == item.Id).Select(a => a.ImageURL).ToArray();
                }

                foreach (var ve in homeData.FeaturedEvents.Concat(homeData.UpComingEvents))
                {
                    DateTimeOffset dto = new DateTimeOffset(ve.EventDate);

                    ve.PreEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
                    ve.PostEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
                    ve.City = ve.City == null ? "" : ve.City;
                    ve.EventDateTimeStamp = dto.ToUnixTimeSeconds();

                    var fav = db.Favorites.FirstOrDefault(e => e.CustomerID == homeData.UserId && e.EventID == ve.EventId);
                    ve.isFav = fav != null;
                }

                var eventCounts = new EventCounts
                {
                    Cancelled = 0,
                    Going = 0,
                    Completed = 0
                };

                var ticketBookings = db.TicketBookings.Where(ui => ui.UserId == homeData.UserId).ToList();

                foreach (var item in ticketBookings)
                {
                    var _event = db.Event.FirstOrDefault(a => a.EventId == item.EventId && a.isActive);
                    if (_event != null)
                    {
                        if (item.isCancelled == true)
                        {
                            eventCounts.Cancelled++;
                        }
                        else if (_event.EventDate.Date >= DateTime.Now.Date)
                        {
                            eventCounts.Going++;
                        }
                        else if (_event.EventDate.Date < DateTime.Now.Date)
                        {
                            eventCounts.Completed++;
                        }
                    }
                }

                homeData.EventCounts = eventCounts;
                homeData.InviteFriendsLink = db.Links.FirstOrDefault(a => a.PromotorId == 1)?.InviteFriendsLink;

                return Json(homeData);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException?.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getOrganizerDetail")]
        public IHttpActionResult GetOrganizerDetail(int organizerID)
        {
            try
            {
                OrganizerViewModel organizerViewModel = new OrganizerViewModel();
                var organizer = db.Organizers.Where(o => o.Id == organizerID).FirstOrDefault();
                if (organizer == null)
                    return Json("No record found");

                var Events = db.Event.Where(e => e.OrganizerID == organizerID && e.isActive == true).ToList();
                Events.ForEach(e =>
                {
                    e.PreEventImages = db.EventImages.Where(r => r.eventid == e.EventId && r.Type == 0).Select(t => t.url).ToArray();
                    e.PostEventImages = db.EventImages.Where(r => r.eventid == e.EventId && r.Type == 1).Select(t => t.url).ToArray();
                });

                organizerViewModel.Organizer = organizer;
                organizerViewModel.Events = Events;
                organizerViewModel.Collections = db.OrganizerCollections.Where(a => a.OrganizerId == organizerID).ToList();
                return Json(organizer);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getEventByLocation")]
        public IHttpActionResult GetEventByLocation(string location, int distance)
        {
            List<Event> events = new List<Event>();
            double lat2 = Convert.ToDouble(location.Split(',').First());
            double lon2 = Convert.ToDouble(location.Split(',').Last());

            try
            {
                var _Events = db.Event.Where(a => a.isActive == true).ToList();
                foreach (var item in _Events)
                {
                    if (!string.IsNullOrWhiteSpace(item.Location))
                    {
                        double lat1 = Convert.ToDouble(item.Location.Split(',').First());
                        double lon1 = Convert.ToDouble(item.Location.Split(',').Last());
                        double _distance = CalculateDistance(lat1, lon1, lat2, lon2);
                        if (_distance <= distance)
                            events.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }

            //var qry = $@"DECLARE @target_latitude DECIMAL(9, 6) = {lat} -- target latitude
            //DECLARE @target_longitude DECIMAL(9, 6) = {lon} -- target longitude

            //SELECT *
            //FROM events
            //WHERE ISNUMERIC(LEFT(location, CHARINDEX(',', location) - 1)) = 1 -- check if the latitude is numeric
            //  AND ISNUMERIC(RIGHT(location, LEN(location) - CHARINDEX(',', location))) = 1 -- check if the longitude is numeric
            //  AND GEOGRAPHY::Point(CAST(LEFT(location, CHARINDEX(',', location) - 1) AS FLOAT), CAST(RIGHT(location, LEN(location) - CHARINDEX(',', location)) AS FLOAT), 4326).STDistance(GEOGRAPHY::Point(@target_latitude, @target_longitude, 4326)) <= 10000 -- check if the distance between the event location and target location is less than or equal to 1 km
            //";
            //var events = db.Event.SqlQuery(qry).ToList<Event>();
            //if (events == null)
            //    return Json("No record found");
            return Json(events);
        }

        [HttpGet]
        [Route("api/getCategory")]
        public IHttpActionResult GetCategory()
        {
            var catagories = db.EventCatagories;
            if (catagories == null)
                return Json("No record found");
            return Json(catagories);
        }

        [HttpGet]
        [Route("api/getRelatedEvents")]
        public IHttpActionResult GetRelatedEvents(int eventID)
        {
            var relatedEvents = db.Event.Where(c => c.CatagoryId == db.Event.Where(e => e.EventId == eventID && e.isActive == true).FirstOrDefault().CatagoryId);
            if (relatedEvents == null)
                return Json("No record found");

            foreach (Event ve in relatedEvents)
            {
                ve.PreEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 0).Select(i => i.url).ToArray();
                ve.PostEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
            }

            return Json(relatedEvents);
        }

        [HttpGet]
        [Route("api/getEventsByType")]
        public IHttpActionResult GetEventsByType(int? eventTypeId)
        {
            if (eventTypeId == null)
            {
                var relatedEvents = db.Event.Where(a => a.isActive == true).ToList();
                if (relatedEvents == null)
                    return Json("No record found");

                foreach (Event ve in relatedEvents)
                {
                    ve.PreEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 0).Select(i => i.url).ToArray();
                    ve.PostEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();

                    double totalRating = 0;
                    var _totalRating = db.EventReviews.Where(a => a.EventId == ve.EventId).AsQueryable();
                    if (_totalRating.Any())
                    {
                        totalRating = _totalRating.Sum(item => item.Ratings);
                    }

                    int reviews = db.EventReviews.Count(a => a.EventId == ve.EventId);
                    ve.ReviewRating = Math.Round(totalRating / reviews, 1);
                }
                return Json(relatedEvents);
            }
            else
            {
                var relatedEvents = db.Event.Where(c => c.EventTypeId == eventTypeId && c.isActive == true).ToList();
                if (relatedEvents == null)
                    return Json("No record found");

                foreach (Event ve in relatedEvents)
                {
                    ve.PreEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 0).Select(i => i.url).ToArray();
                    ve.PostEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();

                    double totalRating = 0;
                    var _totalRating = db.EventReviews.Where(a => a.EventId == ve.EventId).AsQueryable();
                    if (_totalRating.Any())
                    {
                        totalRating = _totalRating.Sum(item => item.Ratings);
                    }

                    int reviews = db.EventReviews.Count(a => a.EventId == ve.EventId);
                    ve.ReviewRating = Math.Round(totalRating / reviews, 1);
                }
                return Json(relatedEvents);
            }
        }

        [HttpGet]
        [Route("api/getEventSearch")]
        public IHttpActionResult GetEventSearch(string searchText)
        {
            var relatedEvents = db.Event.Where(c => c.isActive == true && (c.Name.Contains(searchText) || c.Discription.Contains(searchText) || c.City.Contains(searchText))).ToList();
            if (relatedEvents == null)
                return Json("No record found");
            foreach (Event ve in relatedEvents)
            {
                ve.PreEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 0).Select(i => i.url).ToArray();
                ve.PostEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
            }
            return Json(relatedEvents);
        }

        [HttpGet]
        [Route("api/getAllEvents")]
        public IHttpActionResult GetAllEvents()
        {
            var data = db.Event.Where(a => a.isActive == true).ToList().Select(x => new
            {
                Eventid = x.EventId,
                Title = x.Name,
                Published = x.IsPublished,
                EventDate = x.EventDate,
                LastTimeEdited = x.LastUpdated,
                ImageUrl = db.EventImages.Where(a => a.Type == 1),
            });

            return Json(data);
        }

        [HttpPost]
        [Route("api/setOrganizerFollow")]
        public IHttpActionResult SetOrganizerFollow(int organizerID, int customerID, bool follow)
        {
            try
            {
                var org = db.Organizers.Where(d => d.Id == organizerID).FirstOrDefault();
                if (org == null)
                    return Json("Organizer Not found");

                if (!follow)
                {
                    var orgFollow = db.CustomerOrganizerFollow.Where(ui => ui.UserId == customerID && ui.OrganizerId == organizerID).FirstOrDefault();
                    if (orgFollow == null) { return Json("No Record Found"); }
                    db.CustomerOrganizerFollow.Remove(orgFollow);
                    db.SaveChanges();

                    org.Followers = org.Followers - 1;
                    db.SaveChanges();
                    return Json("Follow Organizer Removed");
                }
                else
                {
                    var isAdded = db.CustomerOrganizerFollow.FirstOrDefault(a => a.OrganizerId == organizerID && a.UserId == customerID);
                    if (isAdded != null)
                        return Json("Organizer Already Added");

                    db.CustomerOrganizerFollow.Add(new CustomerOrganizerFollow { UserId = customerID, OrganizerId = organizerID });
                    db.SaveChanges();

                    org.Followers = org.Followers + 1;
                    db.SaveChanges();
                    return Json("Follow Organizer Added");
                }
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("api/getOrganizerDetails")]
        public IHttpActionResult GetOrganizerDetails(int organizerID)
        {
            OrganizerViewModel organizerViewModel = new OrganizerViewModel();

            var org = db.Organizers.Where(d => d.Id == organizerID).FirstOrDefault();
            if (org == null)
                return Json("Organizer Not found");

            var coll = db.OrganizerCollections.Where(a => a.OrganizerId == organizerID).ToList();
            var evts = db.Event.Where(a => a.OrganizerID == organizerID && a.isActive == true).ToList();
            var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var CustomerID = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

            foreach (var item in evts)
            {
                var Fav = db.Favorites.FirstOrDefault(e => e.CustomerID == CustomerID && e.EventID == item.EventId);
                item.isFav = Fav == null ? false : true;

                item.PreEventImages = db.EventImages.Where(e => e.eventid == item.EventId && e.Type == 0).Select(i => i.url).ToArray();
                item.PostEventImages = db.EventImages.Where(e => e.eventid == item.EventId && e.Type == 1).Select(i => i.url).ToArray();
            }

            var custFollow = db.CustomerOrganizerFollow.FirstOrDefault(a => a.OrganizerId == organizerID && a.UserId == CustomerID);
            organizerViewModel.isFollow = custFollow == null ? false : true;

            organizerViewModel.Organizer = org;
            organizerViewModel.Collections = coll;
            organizerViewModel.Events = evts;

            return Json(organizerViewModel);
        }

        [HttpGet]
        [Route("api/getEventShareableLink")]
        public IHttpActionResult GetEventShareableLink(int eventID)
        {
            string encryptedNumber = Encrypt(eventID);
            return Json("https://tiqarte.azurewebsites.net/api/shareEventDetails?eventID=" + encryptedNumber);
        }

        [HttpGet]
        [Route("api/shareEventDetails")]
        [AllowAnonymous]
        public IHttpActionResult ShareEventDetails(string eventID)
        {
            try
            {
                var decryptedNumber = Convert.ToInt32(Decrypt(eventID));

                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var CustomerID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var _Event = db.Event.FirstOrDefault(ui => ui.EventId == decryptedNumber && ui.isActive == true);
                if (_Event == null)
                    return Json("No record found");

                _Event.PreEventImages = db.EventImages.Where(e => e.eventid == _Event.EventId && e.Type == 0).Select(i => i.url).ToArray();
                _Event.PostEventImages = db.EventImages.Where(e => e.eventid == _Event.EventId && e.Type == 1).Select(i => i.url).ToArray();
                var Fav = db.Favorites.FirstOrDefault(e => e.CustomerID == CustomerID && e.EventID == _Event.EventId);
                _Event.isFav = Fav == null ? false : true;

                var _Organizer = db.Organizers.FirstOrDefault(a => a.Id == _Event.OrganizerID);
                var _Customer = db.TicketBookings.Where(a => a.EventId == _Event.EventId).ToList();
                var _Tickets = db.TicketDetails.Where(a => a.EventId == decryptedNumber).ToList();

                List<CustomerViewModel> _CustomerViewModel = new List<CustomerViewModel>();
                foreach (var item in _Customer)
                {
                    var usr = db.Users.FirstOrDefault(a => a.UserId == item.UserId);
                    _CustomerViewModel.Add(new CustomerViewModel
                    {
                        UserId = item.UserId,
                        ImageURL = usr.ImageUrl,
                        Name = usr.FirstName + " " + usr.LastName,
                    });
                }

                List<EventTicketDetails> _EventTicketDetails = new List<EventTicketDetails>();
                foreach (var item in _Tickets)
                {
                    _EventTicketDetails.Add(new EventTicketDetails
                    {
                        Id = item.Id,
                        TicketType = item.TicketType,
                        TicketPrice = item.TicketPrice,
                    });
                }

                EventDetailViewModel eventDetailViewModel = new EventDetailViewModel();
                eventDetailViewModel.isOrganizerFollow = db.CustomerOrganizerFollow.FirstOrDefault(a => a.UserId == CustomerID && a.OrganizerId == _Event.OrganizerID) == null ? false : true;
                eventDetailViewModel.PeopleGoing = _Customer.Select(a => a.TicketCount).Sum();
                eventDetailViewModel.Event = _Event;
                eventDetailViewModel.Organizer = _Organizer;
                eventDetailViewModel.Organizer.Collections = db.OrganizerCollections.Where(a => a.OrganizerId == _Event.OrganizerID).Select(i => i.ImageURL).ToArray();
                eventDetailViewModel.Customers = _CustomerViewModel;
                eventDetailViewModel.EventTicketDetails = _EventTicketDetails;

                return Json(eventDetailViewModel);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpPost]
        [Route("api/eventReview")]
        public IHttpActionResult EventReview(int eventID, string review, double rating)
        {
            try
            {
                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var CustomerID = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var ratings = db.EventReviews.Add(new BusinesEntities.EventReview
                {
                    EventId = eventID,
                    Review = review,
                    Ratings = rating,
                    CreateDate = DateTime.Now,
                    UserId = CustomerID
                });

                var Evvent = db.Event.FirstOrDefault(a => a.EventId == eventID && a.isActive == true);
                if (Evvent != null)
                    Evvent.isReviewed = true;

                db.SaveChanges();

                return Json(ratings);
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

        public static double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            double EarthRadiusKm = 6371;
            double lat1Rad = DegreeToRadian(latitude1);
            double lon1Rad = DegreeToRadian(longitude1);
            double lat2Rad = DegreeToRadian(latitude2);
            double lon2Rad = DegreeToRadian(longitude2);

            double deltaLat = lat2Rad - lat1Rad;
            double deltaLon = lon2Rad - lon1Rad;

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = EarthRadiusKm * c;
            return distance;
        }

        private static double DegreeToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }

        public static string Encrypt(int number)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(Convert.ToString(number));
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Decrypt(string encryptedNumber)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(Convert.ToString(encryptedNumber));
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        //---------------------------------------------------------------------------------------------------------//
        //-----------------------------------------------Promotor Portal-------------------------------------------//
        //---------------------------------------------------------------------------------------------------------//

        [HttpPost]
        [Route("admin/addEvent")]
        public IHttpActionResult AddEvent(AddEventRequest eventRequest)
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var OrganizerId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

            var _event = db.Event.Add(new Event
            {
                Name = eventRequest.Title,
                CustomSlang = eventRequest.CustomSlang,
                CatagoryId = eventRequest.CategoryId,
                City = eventRequest.City,
                EventStatusId = 1,
                Location = eventRequest.Location,
                Discription = eventRequest.CustomSlang,
                EventTypeId = eventRequest.EventTypeId,
                StandingTitle = eventRequest.StandingTitle,
                SeatingTitle = eventRequest.SeatingTitle,
                TicketSoldOutText = eventRequest.TicketSoldOutText,
                EventDate = eventRequest.EventDate,
                CreationTime = DateTime.Now,
                OrganizerID = OrganizerId,
                CreationUserId = OrganizerId,
                LastUpdated = DateTime.Now,
            });
            db.SaveChanges();

            foreach (var item in eventRequest.EventImages)
            {
                for (int i = 0; i < item.ImageURL.Length; i++)
                {
                    db.EventImages.Add(new EventImages
                    {
                        eventid = _event.EventId,
                        ImageName = item.EventImagesType.ToString(),
                        Type = item.EventImagesType.GetValue(),
                        url = item.ImageURL[i]
                    });
                }
            }
            db.SaveChanges();

            foreach (var item in eventRequest.EventExternalLink)
            {
                db.EventExternalLinks.Add(new EventExternalLink
                {
                    EventId = _event.EventId,
                    Text = item.Text,
                    URL = item.URL,
                    ImageURL = item.ImageURL,
                    Discription = item.Discription,
                    AdvertImageURL1 = item.AdvertImageURL1,
                    AdvertImageURL2 = item.AdvertImageURL2,
                });
            }
            db.SaveChanges();

            foreach (var item in eventRequest.Sponsor)
            {
                db.Sponsors.Add(new Sponsor
                {
                    EventId = _event.EventId,
                    Name = item.Name,
                    ImageURL = item.ImageURL,
                });
            }
            db.SaveChanges();


            db.PasswordProtection.Add(new BusinesEntities.PasswordProtection
            {
                AutoGeneratedLink = eventRequest.PasswordProtection.AutoGeneratedLink,
                EventId = _event.EventId,
                Password = eventRequest.PasswordProtection.Password,
                ScheduleDate = eventRequest.PasswordProtection.ScheduleDate,
                ScheduleTime = eventRequest.PasswordProtection.ScheduleTime,
            });
            db.SaveChanges();

            //var PasswordProtection = db.PasswordProtection.FirstOrDefault(a => a.EventId == _event.EventId);
            //PasswordProtection.EventId = _event.EventId;
            //PasswordProtection.Password = eventRequest.PasswordProtection.Password;
            //PasswordProtection.ScheduleDate = eventRequest.PasswordProtection.ScheduleDate;
            //PasswordProtection.ScheduleTime = eventRequest.PasswordProtection.ScheduleTime;
            //PasswordProtection.AutoGeneratedLink = eventRequest.PasswordProtection.AutoGeneratedLink;

            return Json(new
            {
                Message = "Event Created",
                isSuccess = true
            });
        }

        [HttpPost]
        [Route("admin/updateEvent")]
        public IHttpActionResult UpdateEvent(UpdateEventRequest eventRequest)
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var OrganizerId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
            var _event = db.Event.FirstOrDefault(a => a.EventId == eventRequest.EventId && a.isActive == true);

            if (_event != null)
            {
                _event.Name = eventRequest.Title;
                _event.CustomSlang = eventRequest.CustomSlang;
                _event.Discription = eventRequest.CustomSlang;
                _event.EventTypeId = eventRequest.EventTypeId;
                _event.StandingTitle = eventRequest.StandingTitle;
                _event.SeatingTitle = eventRequest.SeatingTitle;
                _event.TicketSoldOutText = eventRequest.TicketSoldOutText;
                _event.EventDate = eventRequest.EventDate;
                _event.LastUpdated = DateTime.Now;
                _event.CatagoryId = eventRequest.CategoryId;
                _event.City = eventRequest.City;
                _event.Location = eventRequest.Location;

                db.SaveChanges();
            }

            return Json("Event Updated");
        }

        [HttpPost]
        [Route("admin/updateEventImages")]
        public IHttpActionResult UpdateEventImages(EventImages eventRequest)
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var OrganizerId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
            var _event = db.EventImages.FirstOrDefault(a => a.eventid == eventRequest.eventid && a.id == eventRequest.id);

            _event.url = eventRequest.url;
            _event.ImageName = eventRequest.ImageName;

            db.SaveChanges();

            return Json("Event Updated");
        }

        [HttpPost]
        [Route("admin/updateEventExternalLinks")]
        public IHttpActionResult UpdateEventExternalLinks(EventExternalLink eventRequest)
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var OrganizerId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
            var _event = db.EventExternalLinks.FirstOrDefault(a => a.EventId == eventRequest.EventId && a.Id == eventRequest.Id);

            _event.Text = eventRequest.Text;
            _event.URL = eventRequest.URL;
            _event.ImageURL = eventRequest.ImageURL;
            _event.Discription = eventRequest.Discription;
            _event.AdvertImageURL1 = eventRequest.AdvertImageURL1;
            _event.AdvertImageURL2 = eventRequest.AdvertImageURL2;

            db.SaveChanges();

            return Json("Event Updated");
        }

        [HttpPost]
        [Route("admin/updateSponsors")]
        public IHttpActionResult UpdateSponsors(Sponsor eventRequest)
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var OrganizerId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
            var _event = db.Sponsors.FirstOrDefault(a => a.EventId == eventRequest.EventId && a.Id == eventRequest.Id);

            _event.Name = eventRequest.Name;
            _event.ImageURL = eventRequest.ImageURL;

            db.SaveChanges();

            return Json("Event Updated");
        }

        [HttpPost]
        [Route("admin/updatePasswordProtection")]
        public IHttpActionResult PasswordProtection(PasswordProtection eventRequest)
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var OrganizerId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
            var _event = db.PasswordProtection.FirstOrDefault(a => a.EventId == eventRequest.EventId && a.Id == eventRequest.Id);

            _event.Password = eventRequest.Password;
            _event.ScheduleDate = eventRequest.ScheduleDate;
            _event.ScheduleTime = eventRequest.ScheduleTime;
            _event.AutoGeneratedLink = eventRequest.AutoGeneratedLink;

            db.SaveChanges();

            return Json("Event Updated");
        }

        [Route("admin/uploadEventImages")]
        [HttpPost]
        public IHttpActionResult UploadEventImages()
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var UserId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
            var fileName = Path.GetFileName(file.FileName);
            try
            {
                var blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=tiqarteblob;AccountKey=n4JlNEkWc5KzFSPd02dkNKLJqbWjbxH8LHQqrk8zBAd4B8RUtnI9XJetMyo4wtbOJddQ++e93Emd+AStYPy3Vw==;EndpointSuffix=core.windows.net";
                BlobService blobService = new BlobService(blobStorageConnectionString);
                MemoryStream memoryStream = new MemoryStream();

                using (Stream fileStream = file.InputStream)
                {
                    fileStream.CopyTo(memoryStream);
                }
                memoryStream.Position = 0;
                var URI = blobService.UploadFileBlobAsync("tiqarteblob", fileName, memoryStream).Result;

                return Json(URI.AbsoluteUri);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [HttpDelete]
        [Route("admin/deleteEvent")]
        public IHttpActionResult deleteEvent(int EventId)
        {
            var _dc = db.Event.FirstOrDefault(a => a.EventId == EventId && a.isActive == true);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("admin/getEventsByPromotor")]
        public IHttpActionResult GetEventsByPromotorId(int PromotorId)
        {
            try
            {
                List<object> lst = new List<object>();
                var _event = db.Event.Where(a => a.OrganizerID == PromotorId && a.isActive == true).Select(x => new
                {
                    EventId = x.EventId,
                    Title = x.Name,
                    ImageUrl = db.EventImages.Where(e => e.eventid == x.EventId && e.Type == 0).Select(i => i.url).FirstOrDefault(),
                    Published = x.IsPublished,
                    EventDate = x.EventDate,
                    LastTimeEdited = x.LastUpdated,
                });
                foreach (var item in _event)
                {
                    var eventDetails = db.Event.FirstOrDefault(a => a.EventId == item.EventId && a.OrganizerID == PromotorId && a.isActive == true);
                    var eventImages = db.EventImages.Where(a => a.eventid == item.EventId).ToList();
                    var eventExternalLinks = db.EventExternalLinks.Where(a => a.EventId == item.EventId).ToList();
                    var sponsors = db.Sponsors.Where(a => a.EventId == item.EventId).ToList();
                    var passwordProtection = db.PasswordProtection.FirstOrDefault(a => a.EventId == item.EventId);

                    var response = new
                    {
                        data = item,
                        eventDetails,
                        eventImages,
                        eventExternalLinks,
                        sponsors,
                        passwordProtection,
                    };

                    lst.Add(response);
                }

                return Json(lst);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("admin/getEventsById")]
        public IHttpActionResult getEventsById(int EventId)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotorId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var eventDetails = db.Event.FirstOrDefault(a => a.EventId == EventId && a.OrganizerID == PromotorId && a.isActive == true);
                var eventImages = db.EventImages.Where(a => a.eventid == EventId).ToList();
                var eventExternalLinks = db.EventExternalLinks.Where(a => a.EventId == EventId).ToList();
                var sponsors = db.Sponsors.Where(a => a.EventId == EventId).ToList();
                var passwordProtection = db.PasswordProtection.FirstOrDefault(a => a.EventId == EventId);

                var data = new
                {
                    eventDetails = eventDetails,
                    eventImages,
                    eventExternalLinks,
                    sponsors,
                    passwordProtection,
                };

                return Json(data);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/getEventsBySearch")]
        public HttpResponseMessage GetEventsBySearch(string searchText, DateTime? eventDate, int? eventCategoryId, int? eventTypeId, string cityName)
        {
            try
            {
                var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var CustomerID = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var lst = db.Event.Where(a => a.isActive == true).AsQueryable();

                if (!string.IsNullOrEmpty(Convert.ToString(searchText)))
                    lst = lst.Where(a => a.Name.ToLower().Contains(searchText.ToLower()) || a.Discription.ToLower().Contains(searchText.ToLower()) || a.City.ToLower().Contains(searchText.ToLower())).AsQueryable();

                if (!string.IsNullOrEmpty(Convert.ToString(cityName)))
                    lst = lst.Where(a => a.City.ToLower().Contains(cityName.ToLower())).AsQueryable();

                if (eventDate != null)
                    lst = lst.Where(a => a.EventDate.Date == eventDate.Value.Date).AsQueryable();

                if (eventCategoryId != null)
                    lst = lst.Where(a => a.CatagoryId == eventCategoryId).AsQueryable();

                if (eventTypeId != null)
                    lst = lst.Where(a => a.EventTypeId == eventTypeId).AsQueryable();

                foreach (Event ve in lst)
                {
                    ve.PreEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 0).Select(i => i.url).ToArray();
                    ve.PostEventImages = db.EventImages.Where(e => e.eventid == ve.EventId && e.Type == 1).Select(i => i.url).ToArray();
                    var Fav = db.Favorites.FirstOrDefault(e => e.CustomerID == CustomerID && e.EventID == ve.EventId);
                    ve.isFav = Fav == null ? false : true;
                }

                if (lst != null)
                    return this.Request.CreateResponse(HttpStatusCode.OK, lst);
                else
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, lst);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Error");
            }
        }

        [HttpPost]
        [Route("admin/updateEventFinal")]
        public IHttpActionResult UpdateEventFinal(UpdateEventRequestFinal eventRequest)
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var OrganizerId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);
            var _event = db.Event.FirstOrDefault(a => a.EventId == eventRequest.EventId && a.isActive == true);

            if (_event != null)
            {
                _event.Name = eventRequest.Title;
                _event.CustomSlang = eventRequest.CustomSlang;
                _event.Discription = eventRequest.CustomSlang;
                _event.EventTypeId = eventRequest.EventTypeId;
                _event.StandingTitle = eventRequest.StandingTitle;
                _event.SeatingTitle = eventRequest.SeatingTitle;
                _event.TicketSoldOutText = eventRequest.TicketSoldOutText;
                _event.EventDate = eventRequest.EventDate;
                _event.LastUpdated = DateTime.Now;
                _event.CatagoryId = eventRequest.CategoryId;
                _event.City = eventRequest.City;
                _event.Location = eventRequest.Location;

                db.SaveChanges();
            }

            foreach (var item in eventRequest.EventImages)
            {
                for (int i = 0; i < item.ImageURL.Length; i++)
                {
                    db.EventImages.Add(new EventImages
                    {
                        eventid = _event.EventId,
                        ImageName = item.EventImagesType.ToString(),
                        Type = item.EventImagesType.GetValue(),
                        url = item.ImageURL[i]
                    });
                }
            }
            db.SaveChanges();

            foreach (var item in eventRequest.EventExternalLink)
            {
                var eventExternalLinks = db.EventExternalLinks.FirstOrDefault(a => a.Id == item.Id && a.EventId == item.EventId);
                if (eventExternalLinks != null)
                {
                    eventExternalLinks.Text = item.Text;
                    eventExternalLinks.URL = item.URL;
                    eventExternalLinks.ImageURL = item.ImageURL;
                    eventExternalLinks.Discription = item.Discription;
                    eventExternalLinks.AdvertImageURL1 = item.AdvertImageURL1;
                    eventExternalLinks.AdvertImageURL2 = item.AdvertImageURL2;
                }
                db.SaveChanges();
            }

            foreach (var item in eventRequest.Sponsor)
            {
                var sponsors = db.Sponsors.FirstOrDefault(a => a.Id == item.Id && a.EventId == item.EventId);
                if (sponsors != null)
                {
                    sponsors.Name = item.Name;
                    sponsors.ImageURL = item.ImageURL;
                }
                db.SaveChanges();
            }

            var passwordProtection = db.PasswordProtection.FirstOrDefault(a => a.Id == eventRequest.PasswordProtection.Id);
            if (passwordProtection != null)
            {
                passwordProtection.AutoGeneratedLink = eventRequest.PasswordProtection.AutoGeneratedLink;
                passwordProtection.EventId = _event.EventId;
                passwordProtection.Password = eventRequest.PasswordProtection.Password;
                passwordProtection.ScheduleDate = eventRequest.PasswordProtection.ScheduleDate;
                passwordProtection.ScheduleTime = eventRequest.PasswordProtection.ScheduleTime;
            }
            db.SaveChanges();

            return Json(new
            {
                Message = "Event Updated",
                isSuccess = true
            });
        }

        [HttpGet]
        [Route("api/getEventTypes")]
        public IHttpActionResult GetEventTypes()
        {
            try
            {
                var eventDetails = db.EventType.ToList();
                return Json(eventDetails);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/getReviewsByUser")]
        public IHttpActionResult GetReviewsByUser(int userId)
        {
            try
            {
                var eventDetails = (from er in db.EventReviews
                                    join e in db.Event on er.EventId equals e.EventId
                                    where er.UserId == userId
                                    select new
                                    {
                                        EventId = er.EventId,
                                        Review = er.Review,
                                        Rating = er.Ratings,
                                        UserId = er.UserId,
                                        EventName = e.Name,
                                        Description = e.Discription,
                                        CreateDate = er.CreateDate
                                    }).ToList();
                return Json(eventDetails);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/getReviewsByEvent")]
        public IHttpActionResult GetReviewsByEvent(int eventId)
        {
            try
            {
                var eventDetails = (from er in db.EventReviews
                                    join e in db.Event on er.EventId equals e.EventId
                                    join u in db.Users on er.UserId equals u.UserId
                                    where er.EventId == eventId
                                    select new
                                    {
                                        EventId = er.EventId,
                                        UserName = u.FirstName + " " + u.LastName,
                                        Review = er.Review,
                                        Rating = er.Ratings,
                                        EventName = e.Name,
                                        Description = e.Discription,
                                        CreateDate = er.CreateDate
                                    }).ToList();
                return Json(eventDetails);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return BadRequest();
            }
        }
    }
}

public class LocationSearch
{
    public double Lat { get; set; }
    public double Long { get; set; }
    public int Disctance { get; set; }
}

public class LocationSearchV2
{
    public double Lat { get; set; }
    public double Long { get; set; }
    public int Distance { get; set; }
}

//[HttpPost]
//[Route("api/addEvent")]
//public IHttpActionResult AddEvent()
//{
//    var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
//    var OrganizerId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

//    var Title = HttpContext.Current.Request.Params["Title"];
//    var CustomSlang = HttpContext.Current.Request.Params["CustomSlang"];
//    var Discription = HttpContext.Current.Request.Params["Discription"];
//    var EventTypeId = Convert.ToInt32(HttpContext.Current.Request.Params["EventTypeId"]);
//    var CategoryId = Convert.ToInt32(HttpContext.Current.Request.Params["CategoryId"]);
//    var Location = HttpContext.Current.Request.Params["Location"];
//    var City = HttpContext.Current.Request.Params["City"];
//    var EventDate = Convert.ToDateTime(HttpContext.Current.Request.Params["EventDate"]);
//    var StandingTitle = HttpContext.Current.Request.Params["StandingTitle"];
//    var TicketSoldOutText = HttpContext.Current.Request.Params["TicketSoldOutText"];
//    var SeatingTitle = HttpContext.Current.Request.Params["SeatingTitle"];

//    var _event = db.Event.Add(new Event
//    {
//        OrganizerID = OrganizerId,
//        CompnayName = OrganizerId.ToString(),

//        Name = Title,
//        CustomSlang = CustomSlang,
//        Discription = Discription,
//        EventTypeId = EventTypeId,
//        CatagoryId = CategoryId,
//        Location = Location,
//        City = City,
//        EventDate = EventDate,
//        StandingTitle = StandingTitle,
//        SeatingTitle = SeatingTitle,
//        TicketSoldOutText = TicketSoldOutText,
//        CreationTime = DateTime.Now,
//        CreationUserId = OrganizerId,
//        EventStatusId = 1,
//        Price = 0,
//        IsPublished = true,
//    });
//    db.SaveChanges();

//    //foreach (var item in eventRequest.EventImages)
//    //{
//    //    for (int i = 0; i < item.ImageURL.Length; i++)
//    //    {
//    //        db.EventImages.Add(new EventImages
//    //        {
//    //            eventid = _event.EventId,
//    //            ImageName = item.ImageName,
//    //            Type = Helper.GetImageTypeId(item.ImageName),
//    //            url = item.ImageURL[i]
//    //        });
//    //    }
//    //}

//    //foreach (var item in eventRequest.EventExternalLink)
//    //{
//    //    db.EventExternalLinks.Add(new EventExternalLink
//    //    {
//    //        EventId = _event.EventId,
//    //        Text = item.Text,
//    //        URL = item.URL,
//    //        ImageURL = item.ImageURL,
//    //        Discription = item.Discription,
//    //        AdvertImageURL1 = item.AdvertImageURL1,
//    //        AdvertImageURL2 = item.AdvertImageURL2,
//    //    });
//    //}

//    //foreach (var item in eventRequest.Sponsor)
//    //{
//    //    db.Sponsors.Add(new Sponsor
//    //    {
//    //        EventId = _event.EventId,
//    //        Name = item.Name,
//    //        ImageURL = item.ImageURL,
//    //    });
//    //}

//    //db.PasswordProtection.Add(new PasswordProtection
//    //{
//    //    EventId = _event.EventId,
//    //    Password = eventRequest.PasswordProtection.Password,
//    //    ScheduleDate = eventRequest.PasswordProtection.ScheduleDate,
//    //    ScheduleTime = eventRequest.PasswordProtection.ScheduleTime,
//    //    AutoGeneratedLink = eventRequest.PasswordProtection.AutoGeneratedLink,
//    //});

//    //db.SaveChanges();



//    return Json("event created");
//}