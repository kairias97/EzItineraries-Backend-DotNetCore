using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesApi.Models
{
    public interface IMailSender
    {
        Task<bool> Send(string to, string subject, string body, string htmlBody = "");
        Task<bool> Send(List<string> tos, string subject, string body, string htmlBody = "");
    }
}
