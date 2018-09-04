using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public interface ICityRepository
    {
        IQueryable<City> GetCities { get; }
    }
}
