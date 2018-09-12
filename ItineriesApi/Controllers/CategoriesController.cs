using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItineriesApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItineriesApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [ResponseCache(Duration = 60)]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        // GET: api/categories
        [Route("")]
        [HttpGet]
        public ActionResult Get()
        {
            var categories =  _categoryRepository.GetCategories
                .Select(cat => new { cat.Id, cat.Name})
                .ToList();
            return Ok(categories);
        }
        
    }
}
