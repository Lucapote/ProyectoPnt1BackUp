using ProyectoPNT1.Recursos;
using System.ComponentModel.DataAnnotations;

namespace ProyectoPNT1.ViewModel
{
    public class Login
    {
        [Required(ErrorMessage = ErrorMsg.Required)]
        [EmailAddress(ErrorMessage = ErrorMsg.Email)]
        [Display(Name = Alias.Email)]
        public string Email { get; set; }

        [Required(ErrorMessage = ErrorMsg.Required)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = Alias.RememberMe)]
        public bool RememberMe { get; set; }
    }
}
