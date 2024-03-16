using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class PagesRequest
    {
        public string PageName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
    }

    public class UpdatePagesRequest
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageURL { get; set; }
    }

    public class HomePageHeadersRequest
    {
        public string HeaderURL { get; set; }
        public string BannerURL { get; set; }
        public string BackgroundURL { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string FromPrice { get; set; }
        public string Link { get; set; }
        public string HREF { get; set; }
    }

    public class UpdateHomePageHeadersRequest
    {
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
    }

    public class HomePageContentRequest
    {
        public string ImageURL { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class UpdateHomePageContentRequest
    {
        public int Id { get; set; }
        public string ImageURL { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}