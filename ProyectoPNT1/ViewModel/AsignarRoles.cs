using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProyectoPNT1.ViewModel
{
    public class AsignarRoles
    {
        [Display(Name = "Usuario")]
        public int UsuarioId { get; set; }

        [Display(Name = "Nombre Usuario")]
        public string NombreUsuario { get; set; }

        [Display(Name = "Rol")]
        public string Rol { get; set; }

        public List<SelectListItem> Usuarios { get; set; }
        public List<SelectListItem> Roles { get; set; }
    }
}
