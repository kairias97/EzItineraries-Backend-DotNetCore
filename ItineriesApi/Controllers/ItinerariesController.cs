using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItinerariesApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItinerariesApi.Controllers
{
    [Route("api/")]
    [ApiController]
    [ResponseCache(Duration = 0)]
    public class ItinerariesController : ControllerBase
    {
        private IItineraryGenerator _itineraryGenerator;

        public ItinerariesController(IItineraryGenerator generator)
        {
            _itineraryGenerator = generator;
        }
        [Route("itineraries")]
        [HttpPost]
        public ActionResult CalculateProposal([FromBody]ItineraryRequest request)
        {
            var result = _itineraryGenerator.GenerateProposal(request);
            return Ok(result);
        }
    }
}