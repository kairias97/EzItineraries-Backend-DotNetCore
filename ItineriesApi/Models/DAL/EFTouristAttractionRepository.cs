using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItineriesApi.Models.DAL
{
    public class EFTouristAttractionRepository : ITouristAttractionRepository
    {
        private ApplicationDbContext context;
        public EFTouristAttractionRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<TouristAttraction> GetTouristAttractions => context.TouristAttractions;
    }
}
