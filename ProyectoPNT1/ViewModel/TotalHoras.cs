using ProyectoPNT1.Models;
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

        #region Valor Horas y donde se mostraria el estimado de salarios
        [Display(Name = "Valor de la hora")]
        public double? ValorHora { get; set; }

        [Display(Name = "Estimado de salarios")]
        public double EstimadoSalarios { get; set; }
        #endregion
        public double TotalHorasMostrar { get; set; }

        #region filtro Tecnico
        public List<Tecnico> Tecnicos { get; set;}

        [Display(Name ="Tecnico")]
        public int? TecnicoId { get; set; }
        #endregion
    }
}