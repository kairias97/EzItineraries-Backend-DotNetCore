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
        [Column(Order = 2, TypeName = "varchar(250)")]
        public string Name { get; set; }
        [Column(Order = 3, TypeName = "varchar(400)")]
        public string Address { get; set; }
        [Column(Order = 4, TypeName = "varchar(100)")]
        public string GooglePlaceId { get; set; }
        [Column(Order = 5, TypeName = "varchar(60)")]
        public string PhoneNumber { get; set; }
        [Column(Order = 6, TypeName = "varchar(1000)")]
        public string WebsiteUrl { get; set; }
        [Column(Order = 7, TypeName = "decimal(18,2)")]
        [Range(0, 5)]
        public decimal? Rating { get; set; }
        [Column(Order = 8)]
        public int CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual Administrator Creator { get; set; }
        [Column(Order = 9)]
        public int CityId { get; set; }
        public virtual City City { get; set; }
        [Column(Order = 10)]
        public bool Active { get; set; } = true;
        public virtual Geoposition Geoposition { get; set; }
        public virtual ICollection<TouristAttractionConnection> OriginPositionDistances { get; set; }
        public virtual ICollection<TouristAttractionConnection> DestinationPositionDistances { get; set; }
    }
}
