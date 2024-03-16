using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class ArticleViewModel
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Snippets { get; set; }
        public string ArticleText { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime ScheduleTime { get; set; }
    }

    public class AddArticleRequest
    {
        public bool IsPublished { get; set; }
        public string Title { get; set; }
        public string Snippets { get; set; }
        public string ArticleText { get; set; }
        public string ImageUrl { get; set; }
        public DateTime ScheduleDate { get; set; }
        public DateTime ScheduleTime { get; set; }
    }
}