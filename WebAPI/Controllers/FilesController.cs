using BusinesEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using WebAPI.Migrations;

namespace WebAPI.Controllers
{
    public class FilesController : ApiController
    {
        // GET: Files

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/SaveFile")]
        public IHttpActionResult SaveFile()
        {
            string ImageUrl = string.Empty;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                foreach (string fileName in httpRequest.Files.Keys)
                {
                    var file = httpRequest.Files[fileName];
                    //Use Namespace called :  System.IO  
                    string FileName = Path.GetFileNameWithoutExtension(file.FileName);
                    //To Get File Extension  
                    string FileExtension = Path.GetExtension(file.FileName);
                    //Add Current Date To Attached File Name  
                    FileName = $"{DateTime.Now.ToString("yyyyMMdd-HHmmss")}-{FileName.Trim()}{FileExtension}";
                    var fileServerPath = HttpContext.Current.Server.MapPath($"~/Files/img/{FileName}");
                    file.SaveAs(fileServerPath);
                    ImageUrl = $"{GetBaseUrl()}Files/img/{FileName}";
                }

                return Json(ImageUrl);
            }
            return Json("Image Upload failed!");
        }

        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("api/DeleteFile")]
        public IHttpActionResult DeleteFile(string fileName)
        {
            string fileServerPath = HttpContext.Current.Server.MapPath($@"~/Files/img/{fileName}");
            if (System.IO.File.Exists(fileServerPath))
            {
                try
                {
                    System.IO.File.Delete(fileServerPath);
                    return Json("File Deleted Successfully!");
                }
                catch (Exception ex)
                {
                    return Json($"Error: {ex.Message}");
                }
            }
            return Json("File Not Found!");

        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/GetFile")]
        public IHttpActionResult GetFile(string fileName)
        {
            string fileServerPath = HttpContext.Current.Server.MapPath($@"~/Files/img/{fileName}");
            if (System.IO.File.Exists(fileServerPath))
            {
                string fileURL = $"{GetBaseUrl()}Files/img/{fileName}";
                return Json(fileURL);
            }
            return Json("File Not Found!");
        }

        public string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/")
                appUrl = "/" + appUrl;

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }
    }
}