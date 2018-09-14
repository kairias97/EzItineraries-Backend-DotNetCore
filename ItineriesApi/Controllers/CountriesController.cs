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
    [ResponseCache(Duration = 60)]
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
                .ToList();
                //.Select(country => new { country.IsoNumericCode, country.Name, country.Alpha2Code, country.Alpha3Code}).ToList();
            return Ok(countries);
        }
        
    }
}
