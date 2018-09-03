using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.ViewModels
{
    public class SuggestionAdminViewModel
    {
        public bool? SelectedStatus { get; set; }
        public IEnumerable<TouristAttractionSuggestion> Suggestions { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
