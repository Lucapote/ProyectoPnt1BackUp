using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoPNT1.ViewModel
{
    public class EditarRol
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string RolSeleccionado { get; set; }
        public SelectList Roles { get; set; }
    }
}
