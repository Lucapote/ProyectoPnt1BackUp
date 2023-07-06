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

        return View(viewModel);
    }


    [HttpPost]
    public IActionResult CalcularTotalHoras(TotalHoras viewModel)
    {
        if (ModelState.IsValid)
        {
            DateTime fechaInicio = viewModel.FechaInicio.Date;
            DateTime fechaFin = viewModel.FechaFin.Date.AddDays(1); // Incluir la fecha final en el rango

            var horarios = _context.Horario
                .Where(h => h.FechaTrabajo >= fechaInicio && h.FechaTrabajo < fechaFin)
                .ToList(); // Traer los registros a memoria

            double totalHoras = horarios
                .Sum(h => h.HoraSalida.HasValue && h.HoraEntrada.HasValue ? (h.HoraSalida.Value - h.HoraEntrada.Value).TotalHours : 0);


            viewModel.TotalHorasMostrar = totalHoras;
        }

        return View(viewModel);
    }
}
