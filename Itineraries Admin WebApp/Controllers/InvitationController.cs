using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using ItinerariesAdminWebApp.Models;
using ItinerariesAdminWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ItinerariesAdminWebApp.Controllers
{
    [Authorize(Roles = "GlobalAdmin")]
    public class InvitationController : Controller
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IAdministratorRepository _administratorRepository;
        private readonly IMailSender _mailSender;
        private readonly IConfiguration _configuration;
        public InvitationController(IInvitationRepository invitationRepository, 
            IMailSender mailSender,
            IAdministratorRepository adminRepository,
            IConfiguration config)
        {
            _invitationRepository = invitationRepository;
            _mailSender = mailSender;
            _configuration = config;
            _administratorRepository = adminRepository;
        }
        public IActionResult New()
        {
            ViewBag.InvitationSent = TempData["invitationSent"];
            return View(new Invitation());
        }
        [HttpPost]
        public async Task<IActionResult> New(Invitation invitation)
        {
            if (!_invitationRepository.VerifyInvitationEmail(invitation.Email))
            {
                ModelState.AddModelError("emailInUse", "El correo especificado ya se encuentra en uso");
            }
            if (!ModelState.IsValid)
            {
                return View(invitation);
            }
            
            int creatorId = Convert.ToInt32(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value);
            _invitationRepository.RegisterNew(invitation, creatorId);
            //Then proceed to send the email
            string url = $"{_configuration["ItinerariesAdminWebApp:invitationUrl"]}?token={HttpUtility.UrlEncode(invitation.Token)}";
            string msg = $"Ha sido invitado a convertirse en un administrador de atracciones turísticas para la app móvil [NombreApp]." +
                $"Ingrese al siguiente <a href='{url}'>enlace</a> para crear su cuenta de usuario";
            bool success = await _mailSender.Send(invitation.Email, "Invitación para ser Administrador de atracciones turísticas", "", msg);
            if (success)
            {
                _invitationRepository.ChangeStatus(invitation.Id, InvitationStatus.Sent);
                TempData["invitationSent"] = true;
            } else
            {
                _invitationRepository.ChangeStatus(invitation.Id, InvitationStatus.Cancelled);

                TempData["invitationSent"] = false;
            }
            return RedirectToAction(nameof(New));
        }
        [AllowAnonymous]
        public IActionResult Confirmation(string token)
        {
            ViewBag.ValidInvitation = _invitationRepository.IsValidInvitation(token);
            var viewModel = new InvitationConfirmationViewModel
            {
                Token = token,
                Administrator = new Administrator
                {
                    Email = _invitationRepository.GetInvitationEmail(token)
                }
            };
            return View(viewModel);
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Accept(string Token, Administrator Administrator)
        {
            if(!_invitationRepository.IsValidInvitation(Token))
            {
                ModelState.AddModelError("invalidInvitation", "Invitación inválida, no es posible crear una nueva cuenta");
            }
            if(!_invitationRepository.VerifyInvitationEmail(Administrator.Email))
            {
                ModelState.AddModelError("existingEmail", "El correo que desea utilizar ya existe, no es posible crear una nueva cuenta");
            }
            if (!ModelState.IsValid)
            {
                return View("Confirmation",new InvitationConfirmationViewModel { Token = Token, Administrator = Administrator });
            }
            _administratorRepository.CreateAccount(Administrator);
            _invitationRepository.AcceptInvitation(Token);
            TempData["accountCreated"] = true;
            return RedirectToAction("Login", "Account");
        }


    }
}