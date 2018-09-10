using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public class TouristAttractionSuggestion
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
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        public string PhoneNumber { get; set; }
        [Column(Order = 6, TypeName = "varchar(150)")]
        public string WebsiteUrl { get; set; }
        [Column(Order = 7, TypeName = "decimal(18,2)")]
        [Range(0, 5)]
        public decimal? Rating { get; set; }
        [Column(Order = 8)]
        public bool? Approved { get; set; }
        [Column(Order = 9)]
        public int? AnsweredBy { get; set; }
        [ForeignKey("AnsweredBy")]
        public virtual Administrator Approver {get; set; }
        [Column(Order = 10)]
        public int CityId { get; set; }
        [Column(Order = 11)]
        public DateTime? CreatedDate { get; set; }
        [Column(Order = 12)]
        public DateTime? AnsweredDate { get; set; }
        public virtual City City { get; set; }
        public virtual Geoposition Geoposition { get; set; }

    }
}
