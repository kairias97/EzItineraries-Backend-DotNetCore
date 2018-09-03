using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.ViewModels
{
    public class AdministratorAdminViewModel
    {
        public bool SelectedStatus { get; set; } = true;
        public IEnumerable<Administrator> Administrators { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
