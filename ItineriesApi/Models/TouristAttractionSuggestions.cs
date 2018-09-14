using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ItinerariesApi.Models
{
    public partial class TouristAttractionSuggestion
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string GooglePlaceId { get; set; }
        public string PhoneNumber { get; set; }
        public string WebsiteUrl { get; set; }
        public decimal? Rating { get; set; }
        public bool? Approved { get; set; }
        public int? AnsweredBy { get; set; }
        public int CityId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? AnsweredDate { get; set; }
        public virtual Geoposition Geoposition { get; set; }
        public virtual Administrator AnsweredByNavigation { get; set; }
        public virtual Category Category { get; set; }
        public virtual City City { get; set; }
    }
}
