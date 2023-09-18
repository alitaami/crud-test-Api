﻿using System.Net;
using System.Net.Mail;

namespace Common.Utilities
{
    public class SendMail
    {
        public static async Task SendAsync(string to, string subject, string body)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("prozheali@gmail.com", "سامانه املاک");
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (SmtpClient smtpServer = new SmtpClient("smtp.gmail.com"))
                {
                    smtpServer.UseDefaultCredentials = false;
                    smtpServer.Port = 587;
                    smtpServer.Credentials = new NetworkCredential("prozheali@gmail.com", "ayszpqnyevqvbujn");
                    smtpServer.EnableSsl = true;

                    try
                    {
                        await smtpServer.SendMailAsync(mail);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
