using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        public EmailSettings _emailSettings { get; }
        public ILogger<EmailService> _logger { get; }

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> SendEmail(Application.Models.Email email)
        {
            var client = new SendGridClient(_emailSettings.ApiKey);
            var from = new EmailAddress()
            {
                Email = _emailSettings.FromAddress,
                Name = _emailSettings.FromName,
            };
            var subject = email.Subject;
            var to = new EmailAddress(email.To);
            var emailBody = email.Body;
            var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
            var response = await client.SendEmailAsync(sendGridMessage);

            _logger.LogInformation("Email sent.");

            if (response.StatusCode == HttpStatusCode.Accepted || response.StatusCode == HttpStatusCode.OK)
                return true;

            _logger.LogInformation("Email sending failed.");
            return false;
        }
    }
}
