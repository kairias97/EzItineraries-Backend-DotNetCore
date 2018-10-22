
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.DAL
{
    public class EFTouristAttractionRepository : ITouristAttractionRepository
    {
        private ApplicationDbContext context;
        private ITouristAttractionConnectionRepository _touristAttractionConnectionRepository;
        public EFTouristAttractionRepository(ApplicationDbContext ctx,
            ITouristAttractionConnectionRepository connectionRepository)
        {
            context = ctx;
            _touristAttractionConnectionRepository = connectionRepository;
        }

        public IQueryable<TouristAttraction> GetAttractions => context.TouristAttractions;


        public void Disable(int touristAttractionId)
        {
            var attraction = new TouristAttraction { Id = touristAttractionId, Active = false };
            context.TouristAttractions.Attach(attraction);
            context.Entry(attraction).Property(a => a.Active).IsModified = true;
            context.SaveChanges();
            //After update it will be needed to recreate the matrix

            _touristAttractionConnectionRepository.RecalculateConnections(attraction.Id);
        }

        public void Enable(int touristAttractionId)
        {
            var attraction = new TouristAttraction { Id = touristAttractionId, Active = true };
            context.TouristAttractions.Attach(attraction);
            context.Entry(attraction).Property(a => a.Active).IsModified = true;
            context.SaveChanges();
            //After update it will be needed to recreate the matrix

            _touristAttractionConnectionRepository.RecalculateConnections(attraction.Id);
        }

        public void SaveChanges(TouristAttraction attraction)
        {
            bool isNew = attraction.Id == 0;
            if (attraction.Id == 0)
            {
                context.TouristAttractions.Add(attraction);
                //After creation it will be needed to recreate the matrix
            } else
            {
                context.TouristAttractions.Attach(attraction);
                //context.Entry(attraction).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.Entry(attraction).Property(ta => ta.WebsiteUrl).IsModified = true;
                context.Entry(attraction).Property(ta => ta.Rating).IsModified = true;
                context.Entry(attraction).Property(ta => ta.PhoneNumber).IsModified = true;
            }
            context.SaveChanges();
            if (isNew)
            {
                _touristAttractionConnectionRepository.RecalculateConnections(attraction.Id);
            }
            
        }

        public bool VerifyExistence(string googlePlaceId)
        {
            return context.TouristAttractions.Any(ta => ta.GooglePlaceId == googlePlaceId);
        }
    }
}
