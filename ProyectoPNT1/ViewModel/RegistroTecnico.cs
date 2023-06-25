using ProyectoPNT1.Recursos;
using System.ComponentModel.DataAnnotations;

namespace ProyectoPNT1.ViewModel
{
    public class RegistroTecnico
    {
        [Required(ErrorMessage = ErrorMsg.Required)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = ErrorMsg.Required)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = ErrorMsg.Required)]
        [EmailAddress(ErrorMessage = ErrorMsg.Email)]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMsg.Required)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = ErrorMsg.Required)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = ErrorMsg.MissMatch)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = ErrorMsg.Required)]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = ErrorMsg.Dni)]
        public int Dni { get; set; }

        [Required(ErrorMessage = ErrorMsg.Required)]
        public string Direccion { get; set; }
    }
}
