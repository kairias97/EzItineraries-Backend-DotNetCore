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
    [ResponseCache(Duration = 0)]
    [ApiController]
    public class SuggestionsController : ControllerBase
    {
        private readonly ITouristAttractionSuggestionRepository _touristAttractionSuggestionRepository;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IMailSender _mailer;
        public SuggestionsController(ITouristAttractionSuggestionRepository touristAttractionSuggestionRepository,
            IAdministratorRepository administratorRepository,
            IMailSender mailSender)
        {
            _mailer = mailSender;
            _touristAttractionSuggestionRepository = touristAttractionSuggestionRepository;
            _administratorRepository = administratorRepository;
        }
        [HttpPost]
        [Route("suggestions")]
        public ActionResult Save([FromBody] TouristAttractionSuggestion suggestion)
        {
            if (suggestion.Name == null || 
                suggestion.Address == null || 
                suggestion.Geoposition == null || 
                suggestion.GooglePlaceId == null ||
                suggestion.CategoryId == 0 || 
                suggestion.CityId == 0)
            {
                return Ok(new { isSuccessful = false, message = "Datos incompletos de la atracción turística" });
            }
            if (_touristAttractionSuggestionRepository.IsAttractionRegistered(suggestion.GooglePlaceId)) {
                return Ok(new { isSuccessful = false, message = "Ya existe una atracción turística registrada con la información proporcionada"});
            }
            _touristAttractionSuggestionRepository.SaveSuggestion(suggestion);
            var notificationEmails = _administratorRepository.GetNotificationEmails();
            _mailer.Send(notificationEmails.ToList(), "Nueva sugerencia registrada", $"Se registró una nueva sugerencia de atracción turística para la atracción '{suggestion.Name}'.");
            return Ok(new { isSuccessful = true, message = "Sugerencia registrada exitosamente" });
        }
    }
}