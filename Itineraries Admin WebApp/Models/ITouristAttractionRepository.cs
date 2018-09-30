using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public interface ITouristAttractionRepository
    {
        void SaveChanges(TouristAttraction attraction);
        IQueryable<TouristAttraction> GetAttractions { get; }
        void Enable(int touristAttractionId);
        void Disable(int touristAttractionId);
        bool VerifyExistence(string googlePlaceId);
    }
}
