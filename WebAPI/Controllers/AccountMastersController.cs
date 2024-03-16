using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BusinesEntities;
using DAL;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [AllowAnonymous]
    public class AccountMastersController : ApiController
    {
        SqlConnection con = new SqlConnection(DALHelper.ConnectionString);
        SqlTransaction trans;
        ApplicationDbContext db = new ApplicationDbContext();
        [HttpGet]
        [Route("api/AccountMasters/AccountMasterList")]
       
        [Route("api/account/addCustomer")]
        [HttpPost]
        public IHttpActionResult AccountSaving(AccountMaster accountmaster)
        {
            #region MyRegion

            con.Open();

            trans = con.BeginTransaction();

            #endregion

            try
            {
                //write a logic for Account genrate
                    ChildAccount cha = new ChildAccount();
                accountmaster.ChildCode = new AccountDAL().CreateAccount(1, "Assets", accountmaster.Name, 1, 1, 1, con, trans);
                cha = new AccountDAL().GetChildByCode(accountmaster.ChildCode, con, trans);
                if (cha != null)
                {
                    //Contact(accountmaster, con, trans, "AccountConNo_Insert");
                    //if (accountmaster.PhoneNos.Count>0)
                    //{
                    //    List<PhoneNo> PhoneNoObj = new List<PhoneNo>();

                    //    foreach (var item in accountmaster.PhoneNos)
                    //    {
                    //        PhoneNo phoneNo = new PhoneNo();
                    //        phoneNo.AccountMasterId = accountmaster.AccountMasterId;
                    //        phoneNo.PhoneNumber = item.PhoneNumber;
                    //        PhoneNoObj.Add(phoneNo);
                    //    }
                    //    _db.PhoneNos.AddRange(PhoneNoObj);
                    //    _db.SaveChanges();
                    //}
                    
                    new PartyDAL().AddParty(accountmaster);
                    trans.Commit();
                    con.Close();
                    return Ok("Successfull");
                    //return Ok(cha);
                    //MessageBox.Show("Party Save Successfully !", Messages.Header); all check
                    //btnClear_Click(sender, e);
                }
            }
            catch (Exception)
            {
                trans.Rollback();
                con.Close();
                return Ok("Invalid");


            }
            return Ok();
            //return CreatedAtRoute("DefaultApi", new { id= accountmaster.AccountMasterId},accountmaster );
        }

        [Route("api/account/delete")]
        [HttpGet]
        public IHttpActionResult delete(int? id)
        {
            try
            {
                AccountMaster AccountMaster = db.AccountMasters.Find(id);
                if (AccountMaster == null)
                {
                    return NotFound();
                }

                db.AccountMasters.Remove(AccountMaster);
                db.SaveChanges();

                return Ok("Successful");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
