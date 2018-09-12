using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.DAL
{
    public class EFTouristAttractionSuggestionRepository : ITouristAttractionSuggestionRepository
    {
        private ApplicationDbContext context;
        private ITouristAttractionConnectionRepository _touristAttractionConnectionRepository;
        public EFTouristAttractionSuggestionRepository(ApplicationDbContext ctx,
            ITouristAttractionConnectionRepository connectionRepository)
        {
            context = ctx;
            _touristAttractionConnectionRepository = connectionRepository;
        }
        public IQueryable<TouristAttractionSuggestion> GetSuggestions => context.TouristAttractionSuggestions;

        public void Approve(TouristAttractionSuggestion suggestion, int approverId)
        {
            
            suggestion.Approved = true;
            suggestion.AnsweredBy = approverId;
            suggestion.AnsweredDate = DateTime.UtcNow;
            context.TouristAttractionSuggestions.Attach(suggestion);
            context.Entry(suggestion).Property(s => s.Rating).IsModified = true;
            context.Entry(suggestion).Property(s => s.WebsiteUrl).IsModified = true;
            context.Entry(suggestion).Property(s => s.AnsweredBy).IsModified = true;
            context.Entry(suggestion).Property(s => s.Approved).IsModified = true;
            context.Entry(suggestion).Property(s => s.PhoneNumber).IsModified = true;

            context.Entry(suggestion).Property(s => s.AnsweredDate).IsModified = true;

            //Creation of new touristic attraction
            var newAttraction = context.TouristAttractionSuggestions
                .Where(s => s.Id == suggestion.Id)
                .Select(s => new TouristAttraction
                {
                    CreatedBy = approverId,
                    Active = true,
                    Address = s.Address,
                    CategoryId = s.CategoryId,
                    CityId = s.CityId,
                    Geoposition = new Geoposition { Latitude = s.Geoposition.Latitude, Longitude = s.Geoposition.Longitude },
                    GooglePlaceId = s.GooglePlaceId,
                    Name = s.Name,
                    PhoneNumber = suggestion.PhoneNumber,
                    Rating = suggestion.Rating,
                    WebsiteUrl = suggestion.WebsiteUrl
                }).First();
            context.TouristAttractions.Add(newAttraction);
            context.SaveChanges();
            //After save it will be needed to recreate the matrix
            _touristAttractionConnectionRepository.RecalculateConnections(newAttraction.Id);
        }

        public bool IsExistingAttraction(int id)
        {
            return context.TouristAttractions.Join(context.TouristAttractionSuggestions.Where(tas => tas.Id == id),
                ta => ta.GooglePlaceId,
                tas => tas.GooglePlaceId,
                (ta, tas) => ta).Any();
        }

        public void Reject(int suggestionId, int rejectorId)
        {
            var suggestion = new TouristAttractionSuggestion { Id = suggestionId, Approved = false, AnsweredBy = rejectorId, AnsweredDate =DateTime.UtcNow};
            context.TouristAttractionSuggestions.Attach(suggestion);
            context.Entry(suggestion).Property(s => s.AnsweredBy).IsModified = true;
            context.Entry(suggestion).Property(s => s.Approved).IsModified = true;

            context.Entry(suggestion).Property(s => s.AnsweredDate).IsModified = true;
            context.SaveChanges();
            
        }
    }
}
