using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace Wholesaler.Models
{
        public static class Email
        {
            public static void Send(string to, string subject, string body, bool isHtml, params Attachment[] attachments)
            {
                var mailMessage = new MailMessage();

                mailMessage.To.Add(to);

                //mailMessage.Bcc.Add(new MailAddress(""));
                mailMessage.IsBodyHtml = isHtml;
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                foreach (var attachment in attachments)
                    mailMessage.Attachments.Add(attachment);
                using (var smtp = new SmtpClient())
                {
                    smtp.Send(mailMessage);
                }
            }

            public static bool IsValidEmail(string email)
            {
                if (email == null) return false;
                return Regex.IsMatch(email, @"^[\w.+-]+@[\w-.]+\.[\w-]+$");
            }

        }
}