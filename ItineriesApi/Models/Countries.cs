using System;
using System.Collections.Generic;

namespace ItineriesApi.Models
{
    public partial class Country
    {
        public Country()
        {
            Cities = new HashSet<City>();
        }

        public string IsoNumericCode { get; set; }
        public string Name { get; set; }
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
