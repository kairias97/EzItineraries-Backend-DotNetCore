using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ItinerariesApi.Models
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
        [JsonIgnore]
        public virtual ICollection<City> Cities { get; set; }
    }
}
