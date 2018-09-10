using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.ViewModels
{
    public class InvitationConfirmationViewModel
    {
        public string Token { get; set; }
        public Administrator Administrator { get; set; }
    }
}
