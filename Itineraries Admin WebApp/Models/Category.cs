using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public class Category
    {
        [Column(Order = 0)]
        public int Id { get; set; }
        [Column(Order = 1, TypeName = "varchar(100)")]
        public string Name { get; set; }

    }
}
