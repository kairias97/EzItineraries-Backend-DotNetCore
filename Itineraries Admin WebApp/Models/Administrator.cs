using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ItinerariesAdminWebApp.Models
{
    public class Administrator
    {
        [Column(Order = 0)]
        public int Id { get; set; }
        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage ="El formato del correo es inválido")]
        [Column(Order = 1, TypeName = "varchar(200)")]
        public string Email { get; set; }
        [Required(ErrorMessage ="La contraseña es requerida")]
        [Column(Order = 2, TypeName = "varchar(250)")]
        public string Password { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [Column(Order = 3, TypeName = "varchar(120)")]
        public string Name { get; set; }
        [Required(ErrorMessage ="El apellido es requerido")]
        [Column(Order = 4, TypeName = "varchar(120)")]
        public string Lastname { get; set; }
        [Column(Order = 5)]
        public bool Active { get; set; } = true;
        [Column(Order = 6)]
        public bool IsGlobal { get; set; } = false;
        [NotMapped]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmationPassword { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }
        public virtual ICollection<TouristAttraction> TouristAttractions { get; set; }
        public virtual ICollection<TouristAttractionSuggestion> TouristAttractionSuggestions { get; set; }

    }
}
