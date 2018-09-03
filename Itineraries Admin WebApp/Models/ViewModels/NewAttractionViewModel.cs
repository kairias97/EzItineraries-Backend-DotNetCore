using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.ViewModels
{
    public class NewAttractionViewModel
    {
        public IEnumerable<Country> Countries { get; set; }
        public TouristAttraction Attraction { get; set; }
    }
}
