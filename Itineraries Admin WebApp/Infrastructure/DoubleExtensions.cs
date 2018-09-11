using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Infrastructure
{
    public static class DoubleExtensions
    {
        public static double ToRadians(this double value)
        {
            return Convert.ToSingle(Math.PI / 180) * value;
        }
        public static double ToSquare(this double value)
        {
            return Math.Pow(value, 2);
        }
    }
}
