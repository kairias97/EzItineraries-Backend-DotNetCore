using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItineriesApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItineriesApi.Controllers
{
    [Route("api/")]
    [ApiController]
    [ResponseCache(Duration = 60)]
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
                .Where(city => city.CountryId == countryId && ( !onlyWithAttractions || city.TouristAttractions.Any(ta => ta.Active)))
                .Select(c => new { c.Id, c.CountryId, c.Name})
                .ToList();
            return Ok(cities);
        }
        

        
    }
}
