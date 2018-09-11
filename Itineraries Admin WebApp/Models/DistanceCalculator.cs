using ItinerariesAdminWebApp.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public class KmDistanceCalculator : IDistanceCalculator
    {
        private const float KmEarthRadius = 6378.0F;
        
        public double CalculateKmDistance(double originLatitude, double originLongitude, double destinationLatitude, double destinationLongitude)
        {
            var deltaLatitude = (destinationLatitude - originLatitude).ToRadians();
            var deltaLongitude = (destinationLongitude - originLongitude).ToRadians();
            var a = Math.Sin(deltaLatitude / 2).ToSquare() +
                Math.Cos(originLatitude.ToRadians()) *
                Math.Cos(destinationLatitude.ToRadians()) *
                Math.Sin(deltaLongitude / 2).ToSquare();

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return KmEarthRadius * Convert.ToSingle(c);
        }
    }
}
