using System;
using System.Collections.Generic;

namespace ItineriesApi.Models
{
    public partial class Administrator
    {
        public Administrator()
        {
            Invitations = new HashSet<Invitation>();
            TouristAttractionSuggestions = new HashSet<TouristAttractionSuggestion>();
            TouristAttractions = new HashSet<TouristAttraction>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public bool Active { get; set; }
        public bool IsGlobal { get; set; }

        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<TouristAttractionSuggestion> TouristAttractionSuggestions { get; set; }
        public virtual ICollection<TouristAttraction> TouristAttractions { get; set; }
    }
}
