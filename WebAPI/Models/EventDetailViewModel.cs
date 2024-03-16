using BusinesEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace WebAPI.Models
{
    public class EventDetailViewModel
    {
        public int PeopleGoing { get; set; }
        public bool isOrganizerFollow { get; set; }
        public Event Event { get; set; }
        public Organizer Organizer { get; set; }
        public List<CustomerViewModel> Customers { get; set; }
        public List<EventTicketDetails> EventTicketDetails { get; set; }
        public double ReviewRating { get; set; }
    }

    public class AddEventRequest
    {
        public string Title { get; set; }
        public string Discription { get; set; }
        public string CustomSlang { get; set; }
        public int EventTypeId { get; set; }
        public string StandingTitle { get; set; }
        public string SeatingTitle { get; set; }
        public string TicketSoldOutText { get; set; }
        public int CategoryId { get; set; }
        public string Location { get; set; } //Lat & Long
        public string City { get; set; }
        public DateTime EventDate { get; set; }
        public List<EventImageCollection> EventImages { get; set; }
        public List<EventExternalLink> EventExternalLink { get; set; }
        public List<Sponsor> Sponsor { get; set; }
        public PasswordProtection PasswordProtection { get; set; }
    }

    public class UpdateEventRequest
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Discription { get; set; }
        public string CustomSlang { get; set; }
        public int EventTypeId { get; set; }
        public string StandingTitle { get; set; }
        public string SeatingTitle { get; set; }
        public string TicketSoldOutText { get; set; }
        public int CategoryId { get; set; }
        public string Location { get; set; } //Lat & Long
        public string City { get; set; }
        public DateTime EventDate { get; set; }
    }

    public class EventImageCollection
    {
        public string[] ImageURL { get; set; }
        public EventImageType EventImagesType { get; set; }
    }

    public class EventTicketDetails
    {
        public int Id { get; set; }
        public string TicketType { get; set; }
        public decimal TicketPrice { get; set; }
        public int TicketCount { get; set; }
    }

    public partial class HomeDataViewModel
    {
        public string WelcomeMessage { get; set; }
        public string ProfilePictureUrl { get; set; }
        public string UserName { get; set; }
        public List<EventViewModel> FeaturedEvents { get; set; }
        public List<EventViewModel> UpComingEvents { get; set; }
        public ShopProductViewModel[] Shop { get; set; }
        public EventType[] EventType { get; set; }
        public EventCounts EventCounts { get; set; }
        public string InviteFriendsLink { get; set; }
        public int UserId { get; set; }
    }

    public partial class EventViewModel
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string CompnayName { get; set; }
        public string Discription { get; set; }
        public string Location { get; set; }
        public string City { get; set; }
        public DateTime EventDate { get; set; }
        public long EventDateTimeStamp { get; set; }
        public decimal? CreationUserId { get; set; }
        public decimal? EventStatusId { get; set; }
        public decimal? EventTypeId { get; set; }
        public string[] PostEventImages { get; set; }
        public string[] PreEventImages { get; set; }
        public int? CatagoryId { get; set; }
        public double Price { get; set; }
        public int OrganizerID { get; set; }
        public bool IsPublished { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool isFav { get; set; }
        public string StandingTitle { get; set; }
        public string SeatingTitle { get; set; }
        public string TicketSoldOutText { get; set; }
        public double ReviewRating { get; set; }
    }

    public class EventCounts
    {
        public int Cancelled { get; set; }
        public int Going { get; set; }
        public int Completed { get; set; }
    }

    public class ShopProductViewModel
    {
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
        public string[] ProductImages { get; set; }
    }

    public class UpdateEventRequestFinal
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Discription { get; set; }
        public string CustomSlang { get; set; }
        public int EventTypeId { get; set; }
        public string StandingTitle { get; set; }
        public string SeatingTitle { get; set; }
        public string TicketSoldOutText { get; set; }
        public int CategoryId { get; set; }
        public string Location { get; set; } //Lat & Long
        public string City { get; set; }
        public DateTime EventDate { get; set; }
        public List<EventImageCollection> EventImages { get; set; }
        public List<EventExternalLink> EventExternalLink { get; set; }
        public List<Sponsor> Sponsor { get; set; }
        public PasswordProtection PasswordProtection { get; set; }
    }
}


public enum EventImageType
{
    [EnumMember(Value = "Cover")]
    Cover = 1,
    [EnumMember(Value = "Thumbnail")]
    Thumbnail = 2,
    [EnumMember(Value = "Logo")]
    Logo = 3,
    [EnumMember(Value = "SocialMediaImages")]
    SocialMediaImages = 4,
    [EnumMember(Value = "Tiles")]
    Tiles = 5,
    [EnumMember(Value = "Featured")]
    Featured = 6,
}