using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItineriesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ItineriesApi.Controllers
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
                .Select(country => new { country.IsoNumericCode, country.Name, country.Alpha2Code, country.Alpha3Code, test = DateTime.Now}).ToList();
            return Ok(countries);
        }
        
    }
}
