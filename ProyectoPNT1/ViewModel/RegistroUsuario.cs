using ProyectoPNT1.Recursos;
using System.ComponentModel.DataAnnotations;

namespace ProyectoPNT1.ViewModel
{
    public class RegistroUsuario
    {
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
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMsg.Required)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = ErrorMsg.MissMatch)]
        [Display(Name = Alias.ConfirmPassword)]
        public string ConfirmPassword { get; set; }
        [Display(Name = "Registrarme como administrador")]
        public bool EsAdmin { get; set; }
    }
}
