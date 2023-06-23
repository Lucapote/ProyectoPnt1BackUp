using ProyectoPNT1.Recursos;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ProyectoPNT1.Models
{
    public class Horario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = ErrorMsg.Required)]
        [DataType(DataType.Date)]
        [Display(Name = Alias.FechaTrabajo)]
        public DateTime FechaTrabajo { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = Alias.Entrada)]
        public TimeSpan? HoraEntrada { get; set; }

        [DataType(DataType.Time)]
        [Display(Name = Alias.Salida)]
        public TimeSpan? HoraSalida { get; set; }

        // Relación con el técnico
        public int TecnicoId { get; set; }
        public Tecnico Tecnico { get; set; }
    }
}
