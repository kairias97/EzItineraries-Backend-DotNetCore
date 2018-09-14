
namespace ItinerariesApi.Models
{
    public interface ITouristAttractionSuggestionRepository
    {

        bool IsAttractionRegistered(string googlePlaceId);
        void SaveSuggestion(TouristAttractionSuggestion suggestion);
    }
}
