using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public interface IMailSender
    {
        Task<bool> Send(string to, string subject, string body);
        Task<bool> Send(List<string> tos, string subject, string body);
    }
}
