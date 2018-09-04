using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public class BCryptEncrypter : ICryptoManager
    {
        public string HashString(string text)
        {
            return BCrypt.Net.BCrypt.HashPassword(text);
        }

        public bool VerifyHashedString(string plainText, string hashedText)
        {
            return BCrypt.Net.BCrypt.Verify(plainText, hashedText);
        }
    }
}
