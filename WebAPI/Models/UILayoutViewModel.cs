using BusinesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class UILayoutViewModel
    {
        public decimal? PromotorId { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public string[] AdditionalColors { get; set; }
        public string LogoURL { get; set; }
        public string DarkLogoURL { get; set; }
        public string FaviconURL { get; set; }
        public string LogoLink { get; set; }
        public string TicketBgForOrderURL { get; set; }
        public string FrontImageForSessionTicketURL { get; set; }
        public string CheckBgURL { get; set; }
        public string OrderCompletionImageURL { get; set; }
        public DateTime? CreatedDate { get; set; }
        public List<OnBoarding> OnBoarding { get; set; }
    }

    public class UILayoutRequestModel
    {
        public int PromotorId { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public string[] AdditionalColors { get; set; }
        public string LogoURL { get; set; }
        public string DarkLogoURL { get; set; }
        public string FaviconURL { get; set; }
        public string LogoLink { get; set; }
        public string TicketBgForOrderURL { get; set; }
        public string FrontImageForSessionTicketURL { get; set; }
        public string CheckBgURL { get; set; }
        public string OrderCompletionImageURL { get; set; }
        public string Title { get; set; }
        public string StickOnNumber { get; set; }
        public string Description { get; set; }
    }
}