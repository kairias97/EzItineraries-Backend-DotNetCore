using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.DAL
{
    public class EFCityRepository : ICityRepository
    {
        private ApplicationDbContext context;
        public EFCityRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<City> GetCities => context.Cities;
    }
}
