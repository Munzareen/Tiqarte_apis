using BusinesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class AtricleController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("admin/addArticle")]
        public IHttpActionResult AddArticle(AddArticleRequest model)
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

            var article = new Article
            {
                Title = model.Title,
                Snippets = model.Snippets,
                ArticleText = model.ArticleText,
                ImageUrl = model.ImageUrl,
                ScheduleDate = model.ScheduleDate,
                ScheduleTime = model.ScheduleTime,
                IsPublished = model.IsPublished,
                CreatedAt = DateTime.Now,
                LastUpdated = DateTime.Now,
                PromotorId = PromotoId,
            };

            db.Articles.Add(article);
            db.SaveChanges();
            return Json(article);
        }

        [HttpPut]
        [Route("admin/editArticle")]
        public IHttpActionResult EditArticle(ArticleViewModel model)
        {
            var _article = db.Articles.FirstOrDefault(a => a.ArticleId == model.ArticleId);
            if (_article != null)
            {
                _article.Title = model.Title;
                _article.Snippets = model.Snippets;
                _article.ArticleText = model.ArticleText;
                _article.ImageUrl = model.ImageUrl;
                _article.ScheduleDate = model.ScheduleDate;
                _article.ScheduleTime = model.ScheduleTime;
                _article.LastUpdated = DateTime.Now;
            }

            db.SaveChanges();
            return Json(_article);
        }

        [HttpDelete]
        [Route("admin/deleteArticle")]
        public IHttpActionResult DeleteArticle(int ArticleId)
        {
            var _article = db.Articles.FirstOrDefault(a => a.ArticleId == ArticleId);
            if (_article != null)
            {
                _article.LastUpdated = DateTime.Now;
                _article.isActive = false;
            }

            db.SaveChanges();
            return Json(_article);
        }

        [HttpGet]
        [Route("admin/getAllArticle")]
        public IHttpActionResult GetAllArticle()
        {
            var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
            var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

            var data = db.Articles.Where(a => a.isActive == true && a.PromotorId == PromotoId).ToList().Select(x => new
            {
                id = x.ArticleId,
                x.Title,
                x.ArticleText,
                Image = x.ImageUrl,
                Published = x.IsPublished,
                LastTimeEdited = x.LastUpdated,
            });
            return Json(data);
        }

        //-------------------------------- Customer --------------------------------//

        [HttpGet]
        [Route("admin/getArticleById")]
        public IHttpActionResult GetArticleById(int ArticleId)
        {
            var data = db.Articles.FirstOrDefault(a => a.ArticleId == ArticleId && a.isActive == true);
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllArticleByPromotor")]
        public IHttpActionResult GetAllArticleByPromotorId(int PromotorId)
        {
            var data = db.Articles.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList().Select(x => new
            {
                ArticleId = x.ArticleId,
                Title = x.Title,
                Snippets = x.Snippets,
                ArticleText = x.ArticleText,
                ImageUrl = x.ImageUrl,
                IsPublished = x.IsPublished,
                Scheduled = x.ScheduleDate.ToString("dd/MM/yyyy") + " " + x.ScheduleTime.ToString("hh:mm"),
                LastUpdated = x.LastUpdated,
            });
            return Json(data);
        }

    }
}
