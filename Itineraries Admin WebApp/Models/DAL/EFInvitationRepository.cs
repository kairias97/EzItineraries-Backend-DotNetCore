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

        public void AcceptInvitation(string token)
        {
            var invitationId = context.Invitations.Where(i => i.Token == token).Select(i => i.Id).First();
            var invitationAccepted = new Invitation { Id = invitationId, Status = InvitationStatus.Accepted };
            context.Invitations.Attach(invitationAccepted);
            context.Entry(invitationAccepted).Property(i => i.Status).IsModified = true;
            context.SaveChanges();
        }

        public void ChangeStatus(int invitationId, InvitationStatus newStatus)
        {
            var invitation = new Invitation { Id = invitationId, Status = newStatus};
            context.Invitations.Attach(invitation);
            context.Entry(invitation).Property(i => i.Status).IsModified = true;
            context.SaveChanges();
            
        }

        public string GetInvitationEmail(string token)
        {
            return context.Invitations.Where(i => i.Token == token).Select(i => i.Email).FirstOrDefault();
        }

        public bool IsValidInvitation(string token)
        {
            return context.Invitations.Any(i => i.Token == token && i.Status == InvitationStatus.Sent);
        }

        public void RegisterNew(Invitation invitation, int creatorId)
        {
            invitation.SentBy = creatorId;
            invitation.CreatedDate = DateTime.UtcNow;
            invitation.Status = InvitationStatus.Registered;
            invitation.Token = Guid.NewGuid().ToString();
            context.Invitations.Add(invitation);
            context.SaveChanges();
        }

        public bool VerifyInvitationEmail(string email)
        {
            //If there is no admin with the given email, then let proceed
            return !context.Administrators.Any(a => a.Email == email);
        }
    }
}
