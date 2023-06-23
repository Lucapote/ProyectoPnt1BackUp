using ProyectoPNT1.Recursos;
using System.ComponentModel.DataAnnotations;

namespace ProyectoPNT1.Models
{
    public class Tecnico : Persona
    {
        [Required(ErrorMessage = ErrorMsg.Required)]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = ErrorMsg.Dni)]
        public int Dni { get; set; }

        [Required(ErrorMessage = ErrorMsg.Required)]
        public string Direccion { get; set; }

        public ICollection<Horario> Horarios { get; set; }
    }
}
