using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public interface IDistanceCalculator
    {
        double CalculateKmDistance(double originLatitude, double originLongitude, 
            double destinationLatitude, double destinationLongitude);
    }
}
