using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public class TouristAttraction
    {
        [Column(Order = 0)]
        public int Id { get; set; }
        [Column(Order = 1)]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        [Column(Order = 2, TypeName = "250")]
        public string Name { get; set; }
        [Column(Order = 3, TypeName = "400")]
        public string Address { get; set; }
        [Column(Order = 4, TypeName = "100")]
        public string GooglePlaceId { get; set; }
        [Column(Order = 5, TypeName = "60")]
        public string PhoneNumber { get; set; }
        [Column(Order = 6, TypeName = "150")]
        public string WebsiteUrl { get; set; }
        [Column(Order = 7, TypeName = "decimal(18,2)")]
        [Range(0, 5)]
        public decimal? Rating { get; set; }
        [Column(Order = 8)]
        public bool? Approved { get; set; }
        [Column(Order = 9)]
        public int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual Administrator Creator { get; set; }
        [Column(Order = 10)]
        public int CityId { get; set; }
        public virtual City City { get; set; }
        [Column(Order = 11)]
        public bool Active { get; set; } = true;
        public Geoposition Geoposition { get; set; }
    }
}
