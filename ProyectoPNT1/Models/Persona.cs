using ProyectoPNT1.Recursos;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProyectoPNT1.Models
{
    public class Persona
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.Required)]
        public string Nombre { get; set; }
        [Required(ErrorMessage = ErrorMsg.Required)]
        public string Apellido { get; set; }
        [Required(ErrorMessage = ErrorMsg.Required)]
        [EmailAddress(ErrorMessage = ErrorMsg.Email)]
        [Display(Name = Alias.Email)]
        public string Email { get; set; }
        [Required(ErrorMessage = ErrorMsg.Required)]
        [DataType(DataType.Password)]
        [Display(Name = Alias.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = ErrorMsg.Required)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = ErrorMsg.MissMatch)]
        [Display(Name = Alias.ConfirmPassword)]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = ErrorMsg.Required)]
        [RegularExpression(@"^\d{10}$", ErrorMessage = ErrorMsg.Telefono)]
        public string Telefono { get; set; }
    }
}
