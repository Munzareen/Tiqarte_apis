using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinesEntities
{
    public class Pages
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string PageName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
        public bool isActive { get; set; } = true;
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }

    public class HomePageHeader
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string HeaderURL { get; set; }
        public string BannerURL { get; set; }
        public string BackgroundURL { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string FromPrice { get; set; }
        public string Link { get; set; }
        public string HREF { get; set; }
        public bool isActive { get; set; } = true;
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }

    public class HomePageContent
    {
        [Key]
        public int Id { get; set; }
        public int PromotorId { get; set; }
        public string ImageURL { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Position { get; set; }
        public bool isActive { get; set; } = true;
        public DateTime CreationTime { get; set; } = DateTime.Now;
    }

}
