using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesApi.Models
{
    public class ItineraryProposal
    {
        public double Score { get; set; }
        public double CalculatedKmDistance { get; set; }
        public IEnumerable<TouristVisit> TouristVisits { get; set; }
    }
}
