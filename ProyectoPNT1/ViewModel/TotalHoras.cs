using ProyectoPNT1.Models;
using ProyectoPNT1.Recursos;
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

        [Required(ErrorMessage = ErrorMsg.Required)]
        [Display(Name = Nombres.FechaInicio)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = ErrorMsg.Required)]
        [Display(Name = Nombres.FechaFin)]
        public DateTime FechaFin { get; set; }

        #region Valor Horas y donde se mostraria el estimado de salarios
        [Display(Name = Nombres.ValorHora)]
        public double? ValorHora { get; set; }

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