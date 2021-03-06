﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesApi.Models
{
    public interface ITouristAttractionRepository
    {
        IQueryable<TouristAttraction> GetTouristAttractions { get; }
    }
}
