using System;
using System.Collections.Generic;

namespace ItineriesApi.Models
{
    public partial class City
    {
        public City()
        {
            TouristAttractionConnections = new HashSet<TouristAttractionConnection>();
            TouristAttractionSuggestions = new HashSet<TouristAttractionSuggestion>();
            TouristAttractions = new HashSet<TouristAttraction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryId { get; set; }

        public virtual Country Country { get; set; }
        public virtual ICollection<TouristAttractionConnection> TouristAttractionConnections { get; set; }
        public virtual ICollection<TouristAttractionSuggestion> TouristAttractionSuggestions { get; set; }
        public virtual ICollection<TouristAttraction> TouristAttractions { get; set; }
    }
}
