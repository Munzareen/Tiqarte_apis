using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinesEntities
{
    public partial class Event
    {
        public int EventId { get; set; }
        public string Name { get; set; } //Title
        public string CustomSlang { get; set; }
        public string Discription { get; set; }

        public decimal? EventTypeId { get; set; }
        public int? CatagoryId { get; set; }

        public string Location { get; set; } //Lat & Long
        public string City { get; set; }

        public decimal? EventStatusId { get; set; }

        public string[] PostEventImages { get; set; }
        public string[] PreEventImages { get; set; }

        public double Price { get; set; }
        public int OrganizerID { get; set; } //Promotor Id

        public DateTime EventDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime CreationTime { get; set; }
        public decimal? CreationUserId { get; set; }

        public bool isFav { get; set; }
        public bool isReviewed { get; set; }

        public string StandingTitle { get; set; }
        public string SeatingTitle { get; set; }
        public string TicketSoldOutText { get; set; }

        public string CompnayName { get; set; }
        public bool IsPublished { get; set; }

        public bool isActive { get; set; } = true;

        [NotMapped]
        public double ReviewRating { get; set; }
    }

    public partial class EventImages
    {
        [Key]
        public int id { get; set; }
        public string url { get; set; }
        public int eventid { get; set; }
        public int Type { get; set; } //0 for Previous 1 for current and 2 for others
        public string ImageName { get; set; }
    }

    public partial class EventReview
    {
        [Key]
        public int Id { get; set; }
        public int EventId { get; set; }
        public string Review { get; set; }
        public double Ratings { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }

    }
}

//0 PreEvent: Multi
//1 PostEvent: Multi

//2 Cover: Multi
//3 Thumbnail: Single
//4 Logo: Single
//5 Social Media Images: Multi
//6 Tiles: Multi
//7 Featured: Multi