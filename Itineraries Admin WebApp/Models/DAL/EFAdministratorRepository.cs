using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.DAL
{
    public class EFAdministratorRepository : IAdministratorRepository
    {
        private ApplicationDbContext context;
        private readonly ICryptoManager _crypto;
        public EFAdministratorRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Administrator> GetAdministrators => context.Administrators;

        public void CreateAccount(Administrator administrator)
        {
            context.Administrators.Add(administrator);
            context.SaveChanges();
        }

        public void Disable(int administratorId)
        {
            var administrator = new Administrator { Id = administratorId, Active = false };
            context.Administrators.Attach(administrator);
            context.Entry(administrator).Property(a => a.Active).IsModified = true;
        }

        public Administrator ValidateCredentials(string email, string password)
        {
            return context.Administrators.Where(a => a.Email == email && _crypto.VerifyHashedString(password, a.Password)).FirstOrDefault();
        }
    }
}
