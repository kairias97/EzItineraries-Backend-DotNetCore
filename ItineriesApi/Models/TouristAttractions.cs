﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ItinerariesApi.Models
{
    public partial class TouristAttraction
    {
        public TouristAttraction()
        {
            TouristAttractionConnectionsDestination = new HashSet<TouristAttractionConnection>();
            TouristAttractionConnectionsOrigin = new HashSet<TouristAttractionConnection>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string GooglePlaceId { get; set; }
        public string PhoneNumber { get; set; }
        public string WebsiteUrl { get; set; }
        public decimal? Rating { get; set; }
        [JsonIgnore]
        public int CreatedBy { get; set; }
        public int CityId { get; set; }
        public bool Active { get; set; }
        public virtual Geoposition Geoposition { get; set; }
        public virtual Category Category { get; set; }
        public virtual City City { get; set; }
        [JsonIgnore]
        public virtual Administrator CreatedByNavigation { get; set; }
        [JsonIgnore]
        public virtual ICollection<TouristAttractionConnection> TouristAttractionConnectionsDestination { get; set; }
        [JsonIgnore]
        public virtual ICollection<TouristAttractionConnection> TouristAttractionConnectionsOrigin { get; set; }
    }
}
