using System.ComponentModel.DataAnnotations;

namespace ProyectoPNT1.ViewModel
{
    public class TotalHoras
    {
        public TotalHoras()
        {
        // Obtener el primer y último día de la semana actual
        var primerDiaSemana = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek);
        var ultimoDiaSemana = primerDiaSemana.AddDays(6);

        // Asignar los valores predeterminados
        FechaInicio = primerDiaSemana;
        FechaFin = ultimoDiaSemana;
        }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        [Display(Name = "Fecha de fin")]
        public DateTime FechaFin { get; set; }

        public double TotalHorasMostrar { get; set; }
    }
}