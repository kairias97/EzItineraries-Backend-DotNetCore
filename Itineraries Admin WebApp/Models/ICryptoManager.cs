using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public interface ICryptoManager
    {
        string HashString(string text);
        bool VerifyHashedString(string plainText, string hashedText);
    }
}
