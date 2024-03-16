using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinesEntities
{
    public partial class UILayout
    {
        [Key]
        public int LayoutId { get; set; }
        public int PromotorId { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public string AdditionalColors { get; set; }
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
        public bool isActive { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
