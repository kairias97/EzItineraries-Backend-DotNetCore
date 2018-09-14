using System;
using System.Collections.Generic;

namespace ItinerariesApi.Models
{
    public partial class TouristAttractionConnection
    {
        public int CityId { get; set; }
        public int OriginId { get; set; }
        public int DestinationId { get; set; }
        public double Distance { get; set; }

        public virtual City City { get; set; }
        public virtual TouristAttraction Destination { get; set; }
        public virtual TouristAttraction Origin { get; set; }
    }
}
