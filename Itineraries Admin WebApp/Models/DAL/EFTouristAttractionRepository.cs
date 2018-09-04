
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.DAL
{
    public class EFTouristAttractionRepository : ITouristAttractionRepository
    {
        private ApplicationDbContext context;
        public EFTouristAttractionRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<TouristAttraction> GetAttractions => context.TouristAttractions;

        public void ChangeStatus(int touristAttractionId, bool active)
        {
            var attraction = new TouristAttraction { Id = touristAttractionId, Active = active };
            context.TouristAttractions.Attach(attraction);
            context.Entry(attraction).Property(a => a.Active).IsModified = true;
            context.SaveChanges();
            //After update it will be needed to recreate the matrix
        }

        public void SaveChanges(TouristAttraction attraction)
        {
            if (attraction.Id == 0)
            {
                context.TouristAttractions.Add(attraction);
                //After creation it will be needed to recreate the matrix
            } else
            {
                context.Entry(attraction).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            context.SaveChanges();
        }
    }
}
