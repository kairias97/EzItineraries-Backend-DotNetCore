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
        void ChangeStatus(int touristAttractionId, bool active);
    }
}
