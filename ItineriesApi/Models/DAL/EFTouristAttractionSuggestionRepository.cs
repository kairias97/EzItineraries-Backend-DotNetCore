using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesApi.Models.DAL
{
    public class EFTouristAttractionSuggestionRepository : ITouristAttractionSuggestionRepository
    {
        private ApplicationDbContext context;
        public EFTouristAttractionSuggestionRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public bool IsAttractionRegistered(string googlePlaceId)
        {
            return context.TouristAttractions.Any(ta => ta.GooglePlaceId == googlePlaceId);
        }

        public void SaveSuggestion(TouristAttractionSuggestion suggestion)
        {
            if (suggestion.Id == 0)
            {
                context.TouristAttractionSuggestions.Add(suggestion);
            }
            context.SaveChanges();
        }
    }
}
