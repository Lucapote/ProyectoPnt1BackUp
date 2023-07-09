using Microsoft.AspNetCore.Mvc;
using ProyectoPNT1.Data;
using ProyectoPNT1.ViewModel;

public class FuncionalidadesController : Controller
{
    private readonly ApplicationDbContext _context;

    public FuncionalidadesController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult CalcularTotalHoras()
    {
        //Creamos la instancia del ViewModel que se va a mostrar
        var viewModel = new TotalHoras();
        //obtenemos la lista de Tecnicos
        viewModel.Tecnicos = _context.Tecnico.ToList();
        return View(viewModel);
    }


    [HttpPost]
    public IActionResult CalcularTotalHoras(TotalHoras viewModel)
    {
        if (ModelState.IsValid)
        {   
            //nos va a mostrar por defecto las fechas de esta semana
            DateTime fechaInicio = viewModel.FechaInicio.Date;
            DateTime fechaFin = viewModel.FechaFin.Date.AddDays(1);

            //en una variable guardamos los registros de la fecha de trabajo entre las fechas seleccionadas
            var consulta = _context.Horario.Where(h => h.FechaTrabajo >= fechaInicio && h.FechaTrabajo < fechaFin);

            //si seleccionamos un tecncio especifico en el filtro
            if (viewModel.TecnicoId.HasValue && viewModel.TecnicoId.Value > 0)
            {
                //lo filtramos en la consulta
                consulta = consulta.Where(h => h.TecnicoId == viewModel.TecnicoId.Value);
            }

            //se ejecuta la consulta obteniendo los registros de los horarios con los filtros que solicitamos
            var horarios = consulta.ToList();

            //sumamos las horas que tienene valores, sino lo tienen se aplica 0
            double totalHoras = horarios.Sum(h => h.HoraSalida.HasValue && h.HoraEntrada.HasValue ? (h.HoraSalida.Value - h.HoraEntrada.Value).TotalHours : 0);

            //asignamos la suma al view model
            viewModel.TotalHorasMostrar = totalHoras;

            //si en la vista el valor de hora tiene un valor
            if (viewModel.ValorHora.HasValue)
            {
                //calculamos el estimado de salarios
                double estimadoSalarios = totalHoras * viewModel.ValorHora.Value;
                //asignamos el resultado al view model
                viewModel.EstimadoSalarios = estimadoSalarios;
            }
        }

        //obtenemos la lista de tecnicos y la asignamos al view model
        viewModel.Tecnicos = _context.Tecnico.ToList();
        return View(viewModel);
    }


}
