using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public interface IInvitationRepository
    {
        void ChangeStatus(int invitationId, InvitationStatus newStatus);
        void RegisterNew(Invitation invitation, int creatorId);
        bool VerifyInvitationEmail(string email);
        bool IsValidInvitation(string token);
        string GetInvitationEmail(string token);
        void AcceptInvitation(string token);
    }
}
