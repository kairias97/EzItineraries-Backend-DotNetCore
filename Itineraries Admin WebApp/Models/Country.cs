using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    
    public class Country
    {
        [Key]
        [Column(Order = 0, TypeName = "varchar(3)")]
        public string IsoNumericCode { get; set; }
        [Column(Order = 1, TypeName = "varchar(100)")]
        public string Name { get; set; }
        [Column(Order = 2, TypeName = "varchar(2)")]
        public string Alpha2Code { get; set; }
        [Column(Order = 3, TypeName = "varchar(3)")]
        public string Alpha3Code { get; set; }
        public virtual ICollection<City> Cities { get; set; }

    }
}
