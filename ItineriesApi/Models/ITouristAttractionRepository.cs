using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItineriesApi.Models
{
    public interface ITouristAttractionRepository
    {
        IQueryable<TouristAttraction> GetTouristAttractions { get; }
    }
}
