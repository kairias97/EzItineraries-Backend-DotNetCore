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
    [ResponseCache(Duration = 60)]
    public class TouristAttractionsController : ControllerBase
    {
        private readonly ITouristAttractionRepository _touristAttractionsRepository;
        public TouristAttractionsController(ITouristAttractionRepository touristAttractionsRepository)
        {
            _touristAttractionsRepository = touristAttractionsRepository;
        }
        // GET: api/cities/cityId/touristAttractions
        [Route("cities/{cityId:int}/touristAttractions")]
        [HttpGet]
        public ActionResult Get([FromRoute] int cityId)
        {
            var attractions = _touristAttractionsRepository
                .GetTouristAttractions
                .Include(ta => ta.Category)
                .Include(ta => ta.City)
                .ThenInclude(ta => ta.Country)
                .Where(ta => ta.CityId == cityId && ta.Active)
                //.Select(ta => new {
                //    ta.Id,
                //    ta.Name,
                //    ta.Address,
                //    ta.GooglePlaceId,
                //    ta.PhoneNumber,
                //    ta.WebsiteUrl,
                //    ta.Rating,
                //    ta.Active,
                //    category = new { ta.Category.Id, ta.Category.Name},
                //    ta.CityId,
                //    ta.Geoposition
                //})
                .ToList();
            return Ok(attractions);
        }
        
    }
}
