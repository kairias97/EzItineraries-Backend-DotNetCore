using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.ViewModels
{
    public class AttractionAdminViewModel
    {
        public string CurrentFilter { get; set; }
        public bool? CurrentStatus { get; set; }
        public int? SelectedCategory { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public PagingInfo PagingInfo { get; set; }
        public IEnumerable<TouristAttraction> Attractions { get; set; }
    }
}
