using System.ComponentModel.DataAnnotations;

namespace ProyectoPNT1.ViewModel
{
    public class RegistroTecnico
    {
        [Required(ErrorMessage = "El campo Nombre es requerido.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo Apellido es requerido.")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El campo Email es requerido.")]
        [EmailAddress(ErrorMessage = "El campo Email no es una dirección de correo electrónico válida.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo Contraseña es requerido.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo Confirmar Contraseña es requerido.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El campo DNI es requerido.")]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "El campo DNI debe tener 7 u 8 dígitos.")]
        public int Dni { get; set; }

        [Required(ErrorMessage = "El campo Dirección es requerido.")]
        public string Direccion { get; set; }
    }
}
