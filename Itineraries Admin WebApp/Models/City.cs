using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public class City
    {
        [Column(Order = 0)]
        public int Id { get; set; }
        [Column(Order = 1, TypeName = "varchar(200)")]
        public string Name { get; set; }
        [Column(Order = 2, TypeName = "varchar(3)")]
        public string CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
    }
}
