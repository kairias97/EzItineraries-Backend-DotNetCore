using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesApi.Models
{
    public class TouristVisit
    {
        public int Order { get; set; }
        public TouristAttraction TouristAttraction { get; set; }
    }
}
