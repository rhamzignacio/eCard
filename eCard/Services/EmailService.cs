using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eCard.Models;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace eCard.Services
{
    public class EmailService
    {
        private static EmailModel GetEmail()
        {
            try
            {
                using (var db = new eCardEntities())
                {
                    var query = from e in db.EmailSender
                                select new EmailModel
                                {
                                    Email = e.Email,
                                    Password = e.Password
                                };

                    return query.FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }

        public static void SendEmail(string _subject, string _text, string _sendTo)
        {
            try
            {
                var email = GetEmail();

                using (MailMessage mail = new MailMessage(email.Email, _sendTo))
                {
                    mail.Subject = _subject;

                    _text += "\n\n\n\n"
                        + " ***** This is a system-generated message. Please do not reply to this email *****";

                    mail.Body = _text;

                    mail.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = "smtp.gmail.com";

                    smtp.EnableSsl = true;

                    NetworkCredential NetworkCred = new NetworkCredential(email.Email, email.Password);

                    smtp.UseDefaultCredentials = true;

                    smtp.Credentials = NetworkCred;

                    smtp.Port = 587;

                    smtp.Send(mail);
                    
                }

            }
            catch(Exception error)
            {

            }
        }
    }
}