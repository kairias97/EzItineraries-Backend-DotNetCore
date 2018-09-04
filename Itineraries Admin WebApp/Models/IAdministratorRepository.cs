using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public interface IAdministratorRepository
    {
        IQueryable<Administrator> GetAdministrators { get; }
        void CreateAccount(Administrator administrator);
        void Disable(int administratorId);
        Administrator ValidateCredentials(string email, string password);
    }
}
