using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ItinerariesApi.Models
{
    public partial class Category
    {
        public Category()
        {
            TouristAttractionSuggestions = new HashSet<TouristAttractionSuggestion>();
            TouristAttractions = new HashSet<TouristAttraction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        [JsonIgnore]
        public virtual ICollection<TouristAttractionSuggestion> TouristAttractionSuggestions { get; set; }
        [JsonIgnore]
        public virtual ICollection<TouristAttraction> TouristAttractions { get; set; }
    }
}
