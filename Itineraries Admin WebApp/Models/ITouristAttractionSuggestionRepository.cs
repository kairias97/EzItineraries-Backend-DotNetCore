using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public interface ITouristAttractionSuggestionRepository
    {
        IQueryable<TouristAttractionSuggestion> GetSuggestions { get; }
        bool IsExistingAttraction(int id);
        void Approve(TouristAttractionSuggestion suggestion, int approverId);
        void Reject(int suggestionId, int rejectorId);
    }
}
