using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItineriesApi.Models.DAL
{
    public class EFCountryRepository : ICountryRepository
    {
        private ApplicationDbContext context;
        public EFCountryRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IQueryable<Country> GetCountries => context.Countries;
    }
}
