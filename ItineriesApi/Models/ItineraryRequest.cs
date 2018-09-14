using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesApi.Models
{
   
    public class ItineraryRequest
    {
        public int CityId { get; set; }
        public double MaxDistance { get; set; }
        public List<CategoryPreference> Preferences { get; set; }
    }
}
