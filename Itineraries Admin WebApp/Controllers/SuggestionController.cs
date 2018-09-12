using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ItinerariesAdminWebApp.Models;
using ItinerariesAdminWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ItinerariesAdminWebApp.Controllers
{
    [Authorize]
    public class SuggestionController : Controller
    {
        int PageSize = 10;
        private readonly ITouristAttractionSuggestionRepository _suggestionRepository;

        public SuggestionController(ITouristAttractionSuggestionRepository suggestionRepository)
        {
            _suggestionRepository = suggestionRepository;
        }
        public IActionResult Search(bool? status = null, int pageNumber = 1)
        {
            ViewBag.ApprovedSuggestion = TempData["approved"];
            TempData["status"] = status;
            Func<TouristAttractionSuggestion, bool> filterFunction = suggestion 
                => suggestion.Approved == status;
            SuggestionAdminViewModel vm = new SuggestionAdminViewModel
            {
                SelectedStatus = status,
                Suggestions = _suggestionRepository.GetSuggestions
                    .Where(filterFunction)
                    .OrderBy(s => s.CreatedDate)
                    .Skip((pageNumber - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = pageNumber,
                    ItemsPerPage = PageSize,
                    TotalItems = _suggestionRepository.GetSuggestions
                        .Count(filterFunction)
                }
            };
            return View(vm);
        }
        public IActionResult Approval(int id)
        {
            TempData.Keep("status");
            var suggestion = _suggestionRepository.GetSuggestions
                .Where(s => s.Id == id && s.Approved == null)
                .Include(s => s.Category)
                .Include(s => s.City)
                .ThenInclude(city => city.Country)
                .FirstOrDefault();
            if (suggestion == null)
            {
                return NotFound();
            }
            return View(suggestion);
        }
        [HttpPost]
        public IActionResult Approve(TouristAttractionSuggestion suggestion)
        {
            if (_suggestionRepository.IsExistingAttraction(suggestion.Id))
            {
                ModelState.AddModelError("existingAttraction", "Ya existe una atracción turística con la información provista en la sugerencia, se recomienda rechazarla");
                return View(nameof(Approval),suggestion);
            }
            int approverId = Convert.ToInt32(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value);
            _suggestionRepository.Approve(suggestion, approverId);
            TempData["approved"] = true;
            return RedirectToAction(nameof(Search), new { status = TempData["status"] });
        }
        [HttpPost]
        public IActionResult Reject(int suggestionId)
        {
            int rejectorId = Convert.ToInt32(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value);
            _suggestionRepository.Reject(suggestionId, rejectorId);
            TempData["approved"] = false;
            return RedirectToAction(nameof(Search), new { status = TempData["status"]});
        }

    }
}