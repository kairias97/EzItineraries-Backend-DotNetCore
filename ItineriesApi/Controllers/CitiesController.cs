using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItinerariesApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ItinerariesApi.Controllers
{
    [Route("api/")]
    [ApiController]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class CitiesController : ControllerBase
    {
        private readonly ICityRepository _cityRepository;
        public CitiesController(ICityRepository cityRepository)
        {
            _cityRepository = cityRepository;
        }
        // GET: api/Cities
        [Route("countries/{countryId}/cities")]
        [HttpGet]
        public ActionResult Get([FromRoute] string countryId, [FromQuery] bool onlyWithAttractions = false)
        {
            var cities = _cityRepository.GetCities
                .Include(c => c.Country)
                .Where(city => city.CountryId == countryId && ( !onlyWithAttractions || city.TouristAttractions.Any(ta => ta.Active)))
                .OrderBy(c => c.Name)
                //.Select(c => new { c.Id, country = new { c.Country.IsoNumericCode, c.Country.Name}, c.Name})
                .ToList();
            return Ok(cities);
        }
        

        
    }
}
