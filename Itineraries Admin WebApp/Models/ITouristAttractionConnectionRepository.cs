using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public interface ITouristAttractionConnectionRepository
    {
        void RecalculateConnections(int touristAttractionId);
    }
}
