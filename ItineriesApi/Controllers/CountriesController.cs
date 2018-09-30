using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItinerariesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItinerariesApi.Controllers
{
    [Route("api/countries")]
    [ApiController]
    [ResponseCache(Location =ResponseCacheLocation.None, NoStore = true)]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        public CountriesController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }
        // GET api/countries
        [Route("")]
        [HttpGet]
        public ActionResult Get([FromQuery] bool onlyWithAttractions = false)
        {
            var countries = _countryRepository.GetCountries
                .Where(country => !onlyWithAttractions || country.Cities.Any(city => city.TouristAttractions.Any(ta => ta.Active)))
                .OrderBy(co => co.Name)
                .ToList();
            return Ok(countries);
        }
        
    }
}
