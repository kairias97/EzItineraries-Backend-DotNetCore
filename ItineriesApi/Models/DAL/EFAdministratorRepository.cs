using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesApi.Models.DAL
{
    public class EFAdministratorRepository : IAdministratorRepository
    {
        private ApplicationDbContext context;
        public EFAdministratorRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IEnumerable<string> GetNotificationEmails()
        {
            return context.Administrators.Where(a => a.Active)
                .Select(a => a.Email);
        }
    }
}
