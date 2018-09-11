using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public class Invitation
    {
        [Column(Order = 0)]
        public int Id { get; set; }
        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "El formato del correo es inválido")]
        [Column(Order = 1, TypeName = "varchar(150)")]
        public string Email { get; set; }
        [Column(Order = 2, TypeName = "varchar(250)")]
        public string Token { get; set; }
        [Column(Order = 3)]
        public int SentBy { get; set; }
        [ForeignKey("SentBy")]
        public virtual Administrator Administrator { get; set; }
        [Column(Order = 4)]
        public InvitationStatus Status { get; set; }
        [Column(Order = 5)]
        public DateTime CreatedDate { get; set; }
    }

    public enum InvitationStatus : int
    {
        Registered = 0,
        Sent = 1,
        Accepted = 2,
        Cancelled = 3
    }
}
