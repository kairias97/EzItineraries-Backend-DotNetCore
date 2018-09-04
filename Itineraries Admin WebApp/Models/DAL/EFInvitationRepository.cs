using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models.DAL
{
    public class EFInvitationRepository : IInvitationRepository
    {
        private ApplicationDbContext context;
        public EFInvitationRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public void ChangeStatus(int invitationId, InvitationStatus newStatus)
        {
            var invitation = new Invitation { Id = invitationId, Status = newStatus};
            context.Invitations.Attach(invitation);
            context.Entry(invitation).Property(i => i.Status).IsModified = true;
            context.SaveChanges();
            
        }

        public void RegisterNew(Invitation invitation)
        {
            invitation.Token = Guid.NewGuid().ToString();
            context.Invitations.Add(invitation);
            context.SaveChanges();
        }
    }
}
