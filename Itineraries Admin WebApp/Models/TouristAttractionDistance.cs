using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public class TouristAttractionConnection
    {
        public int CityId { get; set; }
        public int OriginId { get; set; }
        public int DestinationId { get; set; }
        public double Distance { get; set; }
        public virtual City City { get; set; }
        public virtual TouristAttraction Origin {get; set;}
        public virtual TouristAttraction Destination { get; set; }

    }
}
