using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoPNT1.ViewModel
{
    public class EditarRol
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string SelectedRole { get; set; }
        public SelectList Roles { get; set; }
    }
}
