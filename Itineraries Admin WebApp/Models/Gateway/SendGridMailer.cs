using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.Gateway
{
    public class SendGridMailer : IMailSender
    {
        private readonly string _apiKey;
        private readonly EmailAddress _fromAddress;
        public SendGridMailer(IConfiguration config)
        {
            _apiKey = config["ItinerariesAdminWebApp:SendGridApiKey"];
            string fromEmail = config["LexincorpAdmin:SendGridSenderEmail"];
            string fromName = config["LexincorpAdmin:SendGridSenderName"];
            _fromAddress = new EmailAddress(fromEmail, fromName);
        }
        public async Task<bool> Send(string to, string subject, string body, string htmlBody = "")
        {
            var client = new SendGridClient(_apiKey);
            var toEmailAddress = new EmailAddress(to);
            var msg = MailHelper.CreateSingleEmail(_fromAddress, toEmailAddress, subject, body, htmlBody);
            var response = await client.SendEmailAsync(msg);
            return response.StatusCode == System.Net.HttpStatusCode.Accepted;
        }

        public async Task<bool> Send(List<string> toList, string subject, string body, string htmlBody = "")
        {
            var client = new SendGridClient(_apiKey);
            var tos = toList.Select(email => new EmailAddress(email)).ToList();
            var msg = MailHelper.CreateSingleEmailToMultipleRecipients(_fromAddress, tos, subject, body, htmlBody);
            var response = await client.SendEmailAsync(msg);
            return response.StatusCode == System.Net.HttpStatusCode.Accepted;
        }
    }
}
