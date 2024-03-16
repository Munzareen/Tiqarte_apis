using BusinesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using WebAPI.Models;
using WebGrease.Css.Ast;

namespace WebAPI.Controllers
{
    public class PagesController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("admin/createpage")]
        public IHttpActionResult CreatePage(PagesRequest dto)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var scheduledReport = db.Pages.Add(new Pages
                {
                    PromotorId = PromotoId,
                    PageName = dto.PageName,
                    Title = dto.Title,
                    Description = dto.Description,
                    ImageURL = dto.ImageURL,
                    isActive = true,
                    CreationTime = DateTime.Now,
                });
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpPut]
        [Route("admin/updatepage")]
        public IHttpActionResult UpdatePage(UpdatePagesRequest dto)
        {
            try
            {
                var scheduledReport = db.Pages.FirstOrDefault(a => a.Id == dto.Id);
                if (scheduledReport != null)
                {
                    scheduledReport.Title = dto.Title;
                    scheduledReport.PageName = dto.PageName;
                    scheduledReport.Description = dto.Description;
                    scheduledReport.ImageURL = dto.ImageURL;
                };
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpDelete]
        [Route("admin/deletepage")]
        public IHttpActionResult DeletePage(int id)
        {
            try
            {
                var scheduledReport = db.Pages.FirstOrDefault(a => a.Id == id);
                if (scheduledReport != null)
                {
                    scheduledReport.isActive = false;
                };
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getpagebyid")]
        public IHttpActionResult GetPageById(int id)
        {
            try
            {
                var scheduledReport = db.Pages.FirstOrDefault(a => a.Id == id);
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getpagebypromotor")]
        public IHttpActionResult GetPageByPromotor()
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);


                var scheduledReport = db.Pages.Where(a => a.PromotorId == PromotoId);
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }




        [HttpPost]
        [Route("admin/createHomePageHeader")]
        public IHttpActionResult CreateHomePageHeader(HomePageHeadersRequest dto)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var scheduledReport = db.HomePageHeaders.Add(new HomePageHeader
                {
                    PromotorId = PromotoId,
                    HeaderURL = dto.HeaderURL,
                    BannerURL = dto.BannerURL,
                    BackgroundURL = dto.BackgroundURL,
                    Title = dto.Title,
                    Content = dto.Content,
                    FromPrice = dto.FromPrice,
                    Link = dto.Link,
                    HREF = dto.HREF,
                    isActive = true,
                    CreationTime = DateTime.Now,
                });
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpPut]
        [Route("admin/updateHomePageHeader")]
        public IHttpActionResult UpdateHomePageHeader(UpdateHomePageHeadersRequest dto)
        {
            try
            {
                var scheduledReport = db.HomePageHeaders.FirstOrDefault(a => a.Id == dto.Id);
                if (scheduledReport != null)
                {
                    scheduledReport.HeaderURL = dto.HeaderURL;
                    scheduledReport.BannerURL = dto.BannerURL;
                    scheduledReport.BackgroundURL = dto.BackgroundURL;
                    scheduledReport.Title = dto.Title;
                    scheduledReport.Content = dto.Content;
                    scheduledReport.FromPrice = dto.FromPrice;
                    scheduledReport.Link = dto.Link;
                    scheduledReport.HREF = dto.HREF;
                };
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpDelete]
        [Route("admin/deleteHomePageHeader")]
        public IHttpActionResult DeleteHomePageHeader(int id)
        {
            try
            {
                var scheduledReport = db.HomePageHeaders.FirstOrDefault(a => a.Id == id);
                if (scheduledReport != null)
                {
                    scheduledReport.isActive = false;
                };
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getHomePageHeaderbyid")]
        public IHttpActionResult GetHomePageHeaderById(int id)
        {
            try
            {
                var scheduledReport = db.HomePageHeaders.FirstOrDefault(a => a.Id == id);
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getHomePageHeaderbypromotor")]
        public IHttpActionResult GetHomePageHeaderByPromotor()
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);


                var scheduledReport = db.HomePageHeaders.Where(a => a.PromotorId == PromotoId);
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getHomePageHeaderbypromotorId")]
        public IHttpActionResult GetHomePageHeaderByPromotorId(int PromotorId)
        {
            try
            {
                var homePageHeaders = db.HomePageHeaders.Where(a => a.PromotorId == PromotorId);
                return Json(homePageHeaders);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }



        [HttpPost]
        [Route("admin/createHomePageContent")]
        public IHttpActionResult CreateHomePageContent(HomePageContentRequest dto)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var count = db.HomePageContent.Where(a => a.PromotorId == PromotoId).ToArray().Length;

                var scheduledReport = db.HomePageContent.Add(new HomePageContent
                {
                    PromotorId = PromotoId,
                    Title = dto.Title,
                    ImageURL = dto.ImageURL,
                    Content = dto.Content,
                    isActive = true,
                    CreationTime = DateTime.Now,
                    Position = count + 1
                });
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpPut]
        [Route("admin/updateHomePageContent")]
        public IHttpActionResult UpdateHomePageContent(UpdateHomePageContentRequest dto)
        {
            try
            {
                var scheduledReport = db.HomePageContent.FirstOrDefault(a => a.Id == dto.Id);
                if (scheduledReport != null)
                {
                    scheduledReport.Title = dto.Title;
                    scheduledReport.ImageURL = dto.ImageURL;
                    scheduledReport.Content = dto.Content;
                };
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpDelete]
        [Route("admin/deleteHomePageContent")]
        public IHttpActionResult DeleteHomePageContent(int id)
        {
            try
            {
                var scheduledReport = db.HomePageContent.FirstOrDefault(a => a.Id == id);
                if (scheduledReport != null)
                {
                    scheduledReport.isActive = false;
                };
                db.SaveChanges();
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getHomePageContentbyid")]
        public IHttpActionResult GetHomePageContentById(int id)
        {
            try
            {
                var scheduledReport = db.HomePageContent.FirstOrDefault(a => a.Id == id);
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getHomePageContentbypromotor")]
        public IHttpActionResult GetHomePageContentByPromotor()
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var scheduledReport = db.HomePageContent.Where(a => a.PromotorId == PromotoId).OrderByDescending(a => a.CreationTime);
                return Json(scheduledReport);
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

        [HttpPut]
        [Route("admin/updateHomePageContentPosition")]
        public IHttpActionResult UpdateHomePageContentPosition(int HomePageContentId, int Position)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoId = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                var scheduledReport = db.HomePageContent.FirstOrDefault(a => a.Id == HomePageContentId);
                if (scheduledReport != null)
                {
                    scheduledReport.Position = Position;
                    scheduledReport.CreationTime = DateTime.Now;
                };
                db.SaveChanges();

                var r = db.HomePageContent.Where(a => a.PromotorId == PromotoId && a.Position > Position);
                foreach (var item in r)
                {
                    Position = Position + 1;
                    item.Position = Position;
                    db.SaveChanges();
                }
                return Json(scheduledReport);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }

        [HttpGet]
        [Route("admin/getHomePageContentbyPromotorId")]
        public IHttpActionResult GetHomePageContentByPromotorId(int PromotorId)
        {
            try
            {
                var homePageContent = db.HomePageContent.Where(a => a.PromotorId == PromotorId && a.isActive == true).OrderByDescending(a => a.CreationTime);
                return Json(homePageContent);
            }
            catch (Exception ex)
            {
                AddDBLogs("Error: " + ex.Message + " | InnerException: " + ex.InnerException.ToString());
                return Json(new { result = "Error" });
            }
        }
    }
}