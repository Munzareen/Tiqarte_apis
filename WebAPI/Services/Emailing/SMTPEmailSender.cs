using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using WebAPI.Services.Emailing.Models;
using CsQuery.Utility;
using System.Net.PeerToPeer;
using System.Web.Http;

namespace WebAPI.Services.Emailing
{
    public static class SMTPEmailSender
    {
        public static bool SendMail(EmailInfo emailInfo, string? fromAddress = null, string? fromDisplayName = null)
        {
            try
            {
                bool isSent = false;
                MailMessage mailMessage = new MailMessage()
                {
                    Subject = emailInfo.Subject,
                    IsBodyHtml = emailInfo.IsBodyHtml
                };

                var matchingImageTags = Regex.Matches(emailInfo.Body, "<img(?<value>.*?)>");
                if (matchingImageTags.Count > 0)
                {
                    var imgCount = 0;
                    List<LinkedResource> resourceCollection = new List<LinkedResource>();
                    foreach (Match m in matchingImageTags)
                    {
                        imgCount++;
                        var imgContent = m.Groups["value"].Value;
                        string type = Regex.Match(imgContent, ":(?<type>.*?);base64,").Groups["type"].Value;
                        string base64 = Regex.Match(imgContent, "base64,(?<base64>.*?)\"").Groups["base64"].Value;
                        if (String.IsNullOrEmpty(type) || String.IsNullOrEmpty(base64))
                        {
                            //ignore replacement when match normal <img> tag
                            continue;
                        }
                        var replacement = " src=\"cid:" + imgCount + "\"";
                        emailInfo.Body = emailInfo.Body.Replace(imgContent, replacement);

                        byte[] imageBytes = Convert.FromBase64String(base64);
                        MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

                        var tempResource = new LinkedResource(ms, new ContentType(type))
                        {
                            ContentId = imgCount.ToString()
                        };
                        resourceCollection.Add(tempResource);
                    }

                    AlternateView alternateView = AlternateView.CreateAlternateViewFromString(emailInfo.Body, null, MediaTypeNames.Text.Html);
                    foreach (var item in resourceCollection)
                    {
                        alternateView.LinkedResources.Add(item);
                    }
                    mailMessage.AlternateViews.Add(alternateView);
                }
                mailMessage.Body = emailInfo.Body;

                #region From Email                
                mailMessage.From = new MailAddress(
                    string.IsNullOrEmpty(fromAddress) ? "saudbintariq2@gmail.com" : fromAddress,
                    string.IsNullOrEmpty(fromDisplayName) ? "Tiqarte" : fromDisplayName);
                #endregion

                #region To Emails
                var lstToEmail = !string.IsNullOrEmpty(emailInfo.ToEmails)
                          ? new List<string>(emailInfo.ToEmails.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList())
                          : new List<string>();
                foreach (var toEmail in lstToEmail)
                    mailMessage.To.Add(new MailAddress(toEmail));
                #endregion

                #region CC Emails
                var lstCCEmail = !string.IsNullOrEmpty(emailInfo.CCEmails)
                   ? new List<string>(emailInfo.CCEmails.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList())
                   : new List<string>();
                foreach (var ccEmail in lstCCEmail)
                    mailMessage.CC.Add(new MailAddress(ccEmail));
                #endregion

                #region BCC Emails
                var lstBCCEmail = !string.IsNullOrEmpty(emailInfo.BCCEmails)
                ? new List<string>(emailInfo.BCCEmails.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList())
                : new List<string>();
                foreach (var bccEmail in lstBCCEmail)
                    mailMessage.Bcc.Add(new MailAddress(bccEmail));
                #endregion

                #region Attachments
                if (emailInfo.Attachments != null && emailInfo.Attachments.Any())
                {
                    foreach (var att in emailInfo.Attachments)
                    {
                        if (!string.IsNullOrEmpty(att.AttachmentPath))
                        {
                            mailMessage.Attachments.Add(new System.Net.Mail.Attachment(att.AttachmentPath));
                        }
                        else
                        {
                            mailMessage.Attachments.Add(new System.Net.Mail.Attachment(new MemoryStream(att.AttachmentContent), att.AttachmentName));
                        }
                    }
                }
                #endregion

                #region SMTP Configuration
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("saudbintariq2@gmail.com", "pucrwfdzrmhfqjms"),
                    EnableSsl = true
                };
                #endregion

                #region Send Email
                try
                {
                    Task.Run(() => smtpClient.Send(mailMessage));
                    isSent = true;
                }
                catch (Exception exc)
                {
                    isSent = false;
                }
                finally
                {
                    if (mailMessage.Attachments.Any())
                        mailMessage.Attachments.Dispose();
                }
                #endregion

                return isSent;
            }
            catch (Exception exc)
            {
                return false;
            }
        }
    }
}
