using BusinesEntities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using WebAPI.Models;
using static iTextSharp.text.pdf.AcroFields;

namespace WebAPI.Controllers
{
    public class DiscountCodeController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [Route("admin/addDiscountCode")]
        public IHttpActionResult AddDiscountCode(AddDiscountCodeViewModel model)
        {
            try
            {
                var Claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();
                var PromotoID = Convert.ToInt32(Claims?.FirstOrDefault(x => x.Type.Equals("userid", StringComparison.OrdinalIgnoreCase))?.Value);

                foreach (var item in model.EventId)
                {
                    var discountCode = db.DiscountCodes.Add(new DiscountCode
                    {
                        PromotoId = PromotoID,
                        Code = model.Code,
                        Basis = model.Basis,
                        FixedAmount = model.FixedAmount,
                        IncludeBookingFee = model.IncludeBookingFee,
                        EventId = item,
                        ValidFromDate = model.ValidFromDate,
                        ValidFormTime = model.ValidFormTime,
                        ExpiryDate = model.ExpiryDate,
                        ExpiryTime = model.ExpiryTime,
                        UsageLimit = model.UsageLimit,
                        UsageLimitPerCustomer = model.UsageLimitPerCustomer,
                        PostalCodeDiscounts = model.PostalCodeDiscounts,
                        AutoApply = model.AutoApply,
                        isActive = true,
                        CreatedDate = DateTime.Now,
                    });
                }

                db.SaveChanges();
                return Json("Record Added Successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }

        }

        [HttpPut]
        [Route("admin/editDiscountCode")]
        public IHttpActionResult EditDiscountCode(DiscountCodeViewModel model)
        {
            var _dc = db.DiscountCodes.FirstOrDefault(a => a.Id == model.Id);
            if (_dc != null)
            {
                _dc.Code = model.Code;
                _dc.Basis = model.Basis;
                _dc.FixedAmount = model.FixedAmount;
                _dc.IncludeBookingFee = model.IncludeBookingFee;
                _dc.ValidFromDate = model.ValidFromDate;
                _dc.ValidFormTime = model.ValidFormTime;
                _dc.ExpiryDate = model.ExpiryDate;
                _dc.ExpiryTime = model.ExpiryTime;
                _dc.UsageLimit = model.UsageLimit;
                _dc.UsageLimitPerCustomer = model.UsageLimitPerCustomer;
                _dc.PostalCodeDiscounts = model.PostalCodeDiscounts;
                _dc.AutoApply = model.AutoApply;
            }

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpDelete]
        [Route("admin/deleteDiscountCode")]
        public IHttpActionResult DeleteDiscountCode(int Id)
        {
            var _dc = db.DiscountCodes.FirstOrDefault(a => a.Id == Id);
            if (_dc != null)
                _dc.isActive = false;

            db.SaveChanges();
            return Json(_dc);
        }

        [HttpGet]
        [Route("admin/getDiscountCodeById")]
        public IHttpActionResult GetDiscountCodeById(int Id)
        {
            var data = db.DiscountCodes.FirstOrDefault(a => a.Id == Id && a.isActive == true);
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllDiscountCodeByPromotor")]
        public IHttpActionResult GetAllDiscountCodeByPromotor(int PromotorId)
        {
            var data = db.DiscountCodes.Where(a => a.PromotoId == PromotorId && a.isActive == true).ToList();
            return Json(data);
        }

        [HttpGet]
        [Route("admin/getAllDiscountCode")]
        public IHttpActionResult GetAllDiscountCode()
        {
            var data = db.DiscountCodes.Where(a => a.isActive == true).ToList();
            return Json(data);
        }
    }
}
