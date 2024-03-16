using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;
using WebAPI.Services.Emailing.Models;

namespace WebAPI.Services.Emailing
{
    public class SendGridEmailSender
    {
        public async Task<bool> SendMail(EmailInfo emailInfo, string? fromAddress = null, string? fromDisplayName = null)
        {
            try
            {
                bool isSent = false;
                SendGridMessage mailMessage = new SendGridMessage()
                {
                    Subject = emailInfo.Subject
                };

                var matchingImageTags = Regex.Matches(emailInfo.Body, "<img(?<value>.*?)>");
                if (matchingImageTags.Count > 0)
                {
                    var imgCount = 0;
                    List<Attachment> imageAttachments = new List<Attachment>();
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

                        var tempResource = new Attachment()
                        {
                            ContentId = imgCount.ToString(),
                            Content = base64,
                            Disposition = "inline",
                            Type = type,
                            Filename = "Attachment " + imgCount.ToString()
                        };
                        mailMessage.AddAttachment(tempResource);
                    }
                }
                mailMessage.AddContent(MimeType.Html, emailInfo.Body);

                #region From Email                
                EmailAddress from = new EmailAddress(
                    string.IsNullOrEmpty(fromAddress) ? "saudbintariq8@gmail.com" : fromAddress,
                    string.IsNullOrEmpty(fromDisplayName) ? "Tiqarte" : fromDisplayName);
                mailMessage.From = from;
                #endregion

                #region To Emails
                var lstToEmail = !string.IsNullOrEmpty(emailInfo.ToEmails)
                          ? new List<string>(emailInfo.ToEmails.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList())
                          : new List<string>();
                if (lstToEmail.Any())
                {
                    foreach (var toEmail in lstToEmail)
                        mailMessage.AddTo(toEmail);
                }
                #endregion

                #region CC Emails
                var lstCCEmail = !string.IsNullOrEmpty(emailInfo.CCEmails)
                   ? new List<string>(emailInfo.CCEmails.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList())
                   : new List<string>();
                if (lstCCEmail.Any())
                {
                    foreach (var ccEmail in lstCCEmail)
                        mailMessage.AddCc(ccEmail);
                }
                #endregion

                #region BCC Emails
                var lstBCCEmail = !string.IsNullOrEmpty(emailInfo.BCCEmails)
                ? new List<string>(emailInfo.BCCEmails.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList())
                : new List<string>();
                if (lstBCCEmail.Any())
                {
                    foreach (var bccEmail in lstBCCEmail)
                        mailMessage.AddBcc(bccEmail);
                }
                #endregion

                #region Attachments
                if (emailInfo.Attachments.Any())
                {
                    foreach (var att in emailInfo.Attachments)
                    {
                        if (!string.IsNullOrEmpty(att.AttachmentPath))
                        {
                            mailMessage.AddAttachment(new Attachment()
                            {
                                Content = Convert.ToBase64String(File.ReadAllBytes(att.AttachmentPath)),
                                Filename = att.AttachmentName
                            });
                        }
                        else
                        {
                            mailMessage.AddAttachment(new Attachment()
                            {
                                Content = Convert.ToBase64String(att.AttachmentContent),
                                Filename = att.AttachmentName
                            });
                        }
                    }

                }
                #endregion

                #region SendGrid Configuration
                var sendGridClient = new SendGridClient("");
                #endregion

                #region Send Email
                try
                {
                    var response = await sendGridClient.SendEmailAsync(mailMessage);
                    isSent = response.IsSuccessStatusCode;
                }
                catch (Exception exc)
                {
                    isSent = false;
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
