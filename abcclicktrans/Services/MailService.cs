﻿using abcclicktrans.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;

namespace abcclicktrans.Services
{
    public class MailService : IEmailSender
    {
        private readonly ILogger _logger;
        public MailService(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var mailClass = new MailClass();
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(mailClass.FromMailId);
                    mail.To.Add(email);
                    mail.Subject = subject;
                    mail.Body = htmlMessage;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.webio.pl", 587)) //"smtp.mail.yahoo.com", 587
                    {
                        smtp.Credentials = new System.Net.NetworkCredential(
                            mailClass.FromMailId, mailClass.FromMailIdPassword
                        );
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mail);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new SmtpException(ex.Message);
            }
        }
    }
}
