using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using BusinesEntities;
using Microsoft.AspNet.Identity;

namespace WebAPI.Services.UserLog
{
    public static class UserActions
    {
       
       public static UserLogStatus UserPerformOpration(DateTime CreatedOn,string CreatedBy, string UserId, string TableName, string PerformOpration, int TableEntryId)
       {
            UserLogStatus userLog = new UserLogStatus();
            userLog.CreatedOn = CreatedOn;
            userLog.CreatedBy = CreatedBy;
            userLog.UserId = UserId;
            userLog.TableName = TableName;
            userLog.PerformOpration = PerformOpration;
            userLog.TableEntryId = TableEntryId;
           return userLog;
        }
    }
}