using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Azure;
using Microsoft.Extensions.Logging;

namespace Carb_Counter.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger _logger;

        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                            ILogger<EmailSender> logger)
        {
            Options = optionsAccessor.Value;
            _logger = logger;
        }

        public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(Options.EmailServerKey))
            {
                throw new Exception("Null EmailServerKey");
            }

            if (string.IsNullOrEmpty(Options.EmailAddress))
            {
                throw new Exception("Null EmailServerAddress");
            }

            await Execute(Options.EmailServerKey, Options.EmailAddress, toEmail, subject, message);
        }

        public async Task Execute(string apiKey, string fromEmail, string toEmail, string subject, string message)
        {
            using (MailMessage emailMessage = new MailMessage())
            {
                emailMessage.From = new MailAddress(fromEmail, "Carb Counter");
                emailMessage.To.Add(new MailAddress(toEmail));
                emailMessage.Subject = subject;
                emailMessage.Body = message;
                emailMessage.IsBodyHtml = true;

                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(fromEmail, apiKey);

                    try
                    {
                        await client.SendMailAsync(emailMessage);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                    }
                }
            }            
        }
    }    
}
