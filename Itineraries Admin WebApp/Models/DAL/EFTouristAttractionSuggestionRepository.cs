using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.DAL
{
    public class EFTouristAttractionSuggestionRepository : ITouristAttractionSuggestionRepository
    {
        private ApplicationDbContext context;

        public EFTouristAttractionSuggestionRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<TouristAttractionSuggestion> GetSuggestions => context.TouristAttractionSuggestions;

        public void Approve(TouristAttractionSuggestion suggestion, int approverId)
        {
            
            suggestion.Approved = true;
            suggestion.AnsweredBy = approverId;
            context.TouristAttractionSuggestions.Attach(suggestion);
            context.Entry(suggestion).Property(s => s.Rating).IsModified = true;
            context.Entry(suggestion).Property(s => s.WebsiteUrl).IsModified = true;
            context.Entry(suggestion).Property(s => s.AnsweredBy).IsModified = true;
            context.Entry(suggestion).Property(s => s.Approved).IsModified = true;
            context.Entry(suggestion).Property(s => s.PhoneNumber).IsModified = true;

            //Creation of new touristic attraction
            var newAttraction = new TouristAttraction
            {
                CreatedBy = approverId,
                Active = true,
                Address = suggestion.Address,
                CategoryId = suggestion.CategoryId,
                CityId = suggestion.CityId,
                Geoposition = new Geoposition { Latitude = suggestion.Geoposition.Latitude, Longitude = suggestion.Geoposition.Longitude },
                GooglePlaceId = suggestion.GooglePlaceId,
                Name = suggestion.Name,
                PhoneNumber = suggestion.PhoneNumber,
                Rating = suggestion.Rating,
                WebsiteUrl = suggestion.WebsiteUrl
            };
            context.TouristAttractions.Add(newAttraction);
            context.SaveChanges();
            //After save it will be needed to recreate the matrix
        }

        public void Reject(int suggestionId, int rejectorId)
        {
            var suggestion = new TouristAttractionSuggestion { Id = suggestionId, Approved = false, AnsweredBy = rejectorId };
            context.TouristAttractionSuggestions.Attach(suggestion);
            context.SaveChanges();
            
        }
    }
}
