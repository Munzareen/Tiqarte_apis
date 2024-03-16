using BusinesEntities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class UILayoutController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        [Route("api/AppSetup")]
        [AllowAnonymous]
        public System.Web.Http.IHttpActionResult GetUILayout(int PromotorId)
        {
            var layout = db.UILayout.FirstOrDefault(ui => ui.PromotorId == PromotorId);
            if (layout == null) { return Json(new { result = "No Record Found" }); }

            var onBoarding = db.OnBoarding.Where(a => a.PromotorId == PromotorId && a.isActive == true).ToList();

            UILayoutViewModel model = new UILayoutViewModel
            {
                PromotorId = PromotorId,
                PrimaryColor = layout.PrimaryColor,
                SecondaryColor = layout.SecondaryColor,
                AdditionalColors = layout.AdditionalColors.Split(',').ToArray(),
                LogoURL = layout.LogoURL,
                DarkLogoURL = layout.DarkLogoURL,
                FaviconURL = layout.FaviconURL,
                LogoLink = layout.LogoLink,
                TicketBgForOrderURL = layout.TicketBgForOrderURL,
                FrontImageForSessionTicketURL = layout.FrontImageForSessionTicketURL,
                CheckBgURL = layout.CheckBgURL,
                OrderCompletionImageURL = layout.OrderCompletionImageURL,
                OnBoarding = onBoarding,
                CreatedDate = layout.CreatedDate
            };

            return Json(model);
        }

        [HttpPost]
        [Route("api/AddOnBoarding")]
        [AllowAnonymous]
        public IHttpActionResult AddOnBoarding(OnBoardingViewModel model)
        {
            try
            {
                var onBoarding = db.OnBoarding.Add(new OnBoarding
                {
                    CreatedDate = DateTime.Now,
                    Description = model.Description,
                    Heading = model.Heading,
                    ImageUrl = model.ImageUrl,
                    isActive = true,
                    PromotorId = model.PromotorId
                });

                db.SaveChanges();
                return Json(onBoarding);
            }
            catch (Exception ex)
            {
                return Json(new { result = "Error" });
            }
        }
    }
}


public class OnBoardingViewModel
{
    public int PromotorId { get; set; }
    public string ImageUrl { get; set; }
    public string Heading { get; set; }
    public string Description { get; set; }
}