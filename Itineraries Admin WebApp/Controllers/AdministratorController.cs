using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ItinerariesAdminWebApp.Models;
using ItinerariesAdminWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItinerariesAdminWebApp.Controllers
{
    [Authorize(Roles = "GlobalAdmin")]
    public class AdministratorController : Controller
    {
        int PageSize = 10;
        private readonly IAdministratorRepository _administratorRepository;
        
        public AdministratorController(IAdministratorRepository administratorRepository)
        {
            _administratorRepository = administratorRepository;
        }
        public IActionResult Search(bool status = true, int pageNumber = 1)
        {
            ViewBag.DisabledAdmin = TempData["disabled"];
            TempData["status"] = status;
            Func<Administrator, bool> filterFunction = admin => admin.Active == status && admin.IsGlobal == false;
            AdministratorAdminViewModel vm = new AdministratorAdminViewModel
            {
                SelectedStatus = status,
                Administrators = _administratorRepository.GetAdministrators
                    .Where(filterFunction)
                    .OrderBy(a => a.Email)
                    .Skip((pageNumber - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = pageNumber,
                    ItemsPerPage = PageSize,
                    TotalItems = _administratorRepository.GetAdministrators
                        .Count(filterFunction)
                }
            };
            return View(vm);
        }
        [HttpPost]
        public IActionResult Disable(int adminId)
        {
            _administratorRepository.Disable(adminId);
            TempData["disabled"] = true;
            return RedirectToAction(nameof(Search), new { status = TempData["status"]});
        }

    }
}