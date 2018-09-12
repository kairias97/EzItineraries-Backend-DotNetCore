using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItinerariesAdminWebApp.Infrastructure;
using ItinerariesAdminWebApp.Models;
using ItinerariesAdminWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ItinerariesAdminWebApp.Controllers
{
    [Authorize]
    public class TouristAttractionController : Controller
    {
        public int PageSize = 10;
        private readonly ITouristAttractionRepository _touristAtracctionRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly ICityRepository _cityRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfiguration _configuration;

        public TouristAttractionController(ITouristAttractionRepository attractionRepo,
            ICountryRepository countryRepository,
            ICategoryRepository categoryRepository,
            ICityRepository cityRepository,
            IConfiguration config)
        {
            _touristAtracctionRepository = attractionRepo;
            _countryRepository = countryRepository;
            _categoryRepository = categoryRepository;
            _configuration = config;
            _cityRepository = cityRepository;
        }
        public IActionResult Search(string filter, int? category, bool? status, int pageNumber = 1)
        {
            Func<TouristAttraction, bool> filterFunction = ta => (category == null || ta.CategoryId == category)
                && (status == null || ta.Active == status) && (String.IsNullOrEmpty(filter) || ta.Name.CaseInsensitiveContains(filter) || ta.City.Name.CaseInsensitiveContains(filter) || ta.City.Country.Name.CaseInsensitiveContains(filter));
            AttractionAdminViewModel vm = new AttractionAdminViewModel
            {
                Categories = _categoryRepository.GetCategories,
                CurrentFilter = filter,
                CurrentStatus = status,
                SelectedCategory = category,
                Attractions = _touristAtracctionRepository.GetAttractions
                    .Include(a => a.Category)
                    .Include(a => a.City)
                    .ThenInclude(city => city.Country)
                    .Where(filterFunction)
                    .OrderBy(ta => ta.Name),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = pageNumber,
                    ItemsPerPage = PageSize,
                    TotalItems = _touristAtracctionRepository.GetAttractions.Count(filterFunction)
                }
            };
            return View(vm);
        }
        public IActionResult New()
        {
            NewAttractionViewModel vm = new NewAttractionViewModel
            {
                Countries = _countryRepository.GetCountries.OrderBy(c=>c.Name),
                Attraction = new TouristAttraction
                {
                    Geoposition = new Geoposition()
                },
                Categories = _categoryRepository.GetCategories.OrderBy(c=>c.Name)
            };
            ViewBag.Key = Convert.ToString(_configuration["ItinerariesAdminWebApp:PlacesApiKey"]);
            ViewBag.Added = TempData["added"];
            return View(vm);

        }
        [HttpPost]
        public IActionResult Add(TouristAttraction attraction)
        {
            attraction.CreatedBy = Convert.ToInt32(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value);
            _touristAtracctionRepository.SaveChanges(attraction);
            TempData["added"] = true;
            return RedirectToAction(nameof(New));
        }
        public IActionResult Edit(int id)
        {
            TouristAttraction attraction = _touristAtracctionRepository
                .GetAttractions
                .Include(a => a.Category)
                .Include(a => a.City)
                .ThenInclude(city => city.Country)
                .Where(a => a.Id == id).FirstOrDefault();
            if (attraction == null)
            {
                return NotFound();
            }
            ViewBag.Updated = TempData["updated"];
            return View(attraction);

        }
        [HttpPost]
        public IActionResult Update(TouristAttraction attraction)
        {
            _touristAtracctionRepository.SaveChanges(attraction);
            TempData["updated"] = true;
            return RedirectToAction(nameof(Edit), new { id = attraction.Id});
        }
        public IActionResult ChangeStatus(int id)
        {
            TouristAttraction attraction = _touristAtracctionRepository
                .GetAttractions
                .Include(a => a.Category)
                .Include(a => a.City)
                .ThenInclude(city => city.Country)
                .Where(a => a.Id == id).FirstOrDefault();
            if (attraction == null)
            {
                return NotFound();
            }
            ViewBag.Enabled = TempData["enabled"];
            return View(attraction);

        }
        [HttpPost]
        public IActionResult Disable(int id)
        {
            _touristAtracctionRepository.Disable(id);
            TempData["enabled"] = false;
            return RedirectToAction(nameof(ChangeStatus), new { id = id});
        }
        [HttpPost]
        public IActionResult Enable(int id)
        {
            _touristAtracctionRepository.Enable(id);
            TempData["enabled"] = true;
            return RedirectToAction(nameof(ChangeStatus), new { id = id });

        }
        public JsonResult GetCitiesByCountry(string countryId)
        {
            return Json(_cityRepository.GetCities
                .OrderBy(c => c.Name)
                .Where(c=> c.CountryId == countryId)
                .Select(c=> new { id = c.Id, name = c.Name, countryId = c.CountryId}));
        }

    }
}