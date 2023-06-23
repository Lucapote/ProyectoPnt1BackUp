using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoPNT1.Data;
using ProyectoPNT1.Models;

namespace ProyectoPNT1.Controllers
{
    public class HorariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HorariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, TimeSpan? horaDesde, TimeSpan? horaHasta)
        {
            var horarios = _context.Horario.AsQueryable();
            ViewData["FechaSortParm"] = String.IsNullOrEmpty(sortOrder) ? "fecha_desc" : "";
            ViewData["TecnicoSortParm"] = sortOrder == "tecnico" ? "tecnico_desc" : "tecnico";

            if (horaDesde.HasValue)
            {
                horarios = horarios.Where(h => h.HoraEntrada >= horaDesde.Value);
                ViewData["HoraDesde"] = horaDesde.Value;
            }

            if (horaHasta.HasValue)
            {
                horarios = horarios.Where(h => h.HoraSalida <= horaHasta.Value);
                ViewData["HoraHasta"] = horaHasta.Value;
            }

            horarios = horarios.Include(h => h.Tecnico);

            IOrderedQueryable<Horario> horariosOrdenados;
            switch (sortOrder)
            {
                case "fecha_desc":
                    horariosOrdenados = horarios.OrderByDescending(h => h.FechaTrabajo);
                    break;
                case "tecnico":
                    horariosOrdenados = horarios.OrderBy(h => h.Tecnico.Apellido);
                    break;
                case "tecnico_desc":
                    horariosOrdenados = horarios.OrderByDescending(h => h.Tecnico.Apellido);
                    break;
                default:
                    horariosOrdenados = horarios.OrderBy(h => h.FechaTrabajo);
                    break;
            }

            ViewData["HoraDesde"] = horaDesde;
            ViewData["HoraHasta"] = horaHasta;

            return View(await horariosOrdenados.ToListAsync());
        }

        // GET: Horarios con parámetro tecnicoId
        public async Task<IActionResult> IndexByTecnico(int tecnicoId, string sortOrder)
        {
            var tecnico = await _context.Tecnico.FindAsync(tecnicoId);
            if (tecnico == null)
            {
                return NotFound();
            }

            ViewData["TecnicoId"] = tecnicoId;
            ViewData["TecnicoNombre"] = tecnico.Nombre; // Agregar el nombre del técnico a ViewData

            // Obtener los horarios filtrados por técnico, incluyendo la entidad relacionada Tecnico
            var horarios = _context.Horario.Include(h => h.Tecnico)
                .Where(h => h.TecnicoId == tecnicoId);

            // Establecer las variables de ordenamiento por defecto
            ViewData["FechaSortParm"] = String.IsNullOrEmpty(sortOrder) ? "fecha_desc" : "";
            ViewData["HoraSortParm"] = sortOrder == "hora" ? "hora_desc" : "hora";

            // Ordenar los horarios según el criterio seleccionado
            IOrderedQueryable<Horario> horariosOrdenados;
            switch (sortOrder)
            {
                case "fecha_desc":
                    horariosOrdenados = horarios.OrderByDescending(h => h.FechaTrabajo);
                    break;
                case "hora":
                    horariosOrdenados = horarios.OrderBy(h => h.HoraEntrada);
                    break;
                case "hora_desc":
                    horariosOrdenados = horarios.OrderByDescending(h => h.HoraEntrada);
                    break;
                default:
                    horariosOrdenados = horarios.OrderBy(h => h.FechaTrabajo);
                    break;
            }

            return View(await horariosOrdenados.ToListAsync());
        }




        // GET: Horarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Horario == null)
            {
                return NotFound();
            }

            var horario = await _context.Horario
                .Include(h => h.Tecnico)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horario == null)
            {
                return NotFound();
            }

            return View(horario);
        }

        // GET: Horarios/Create
        public IActionResult Create()
        {
            ViewData["TecnicoId"] = new SelectList(_context.Tecnico, "Id", "Apellido");
            return View();
        }

        // POST: Horarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FechaTrabajo,HoraEntrada,HoraSalida,TecnicoId")] Horario horario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(horario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TecnicoId"] = new SelectList(_context.Tecnico, "Id", "Apellido", horario.TecnicoId);
            return View(horario);
        }

        // GET: Horarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Horario == null)
            {
                return NotFound();
            }

            var horario = await _context.Horario.FindAsync(id);
            if (horario == null)
            {
                return NotFound();
            }
            ViewData["TecnicoId"] = new SelectList(_context.Tecnico, "Id", "Apellido", horario.TecnicoId);
            return View(horario);
        }

        // POST: Horarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FechaTrabajo,HoraEntrada,HoraSalida,TecnicoId")] Horario horario)
        {
            if (id != horario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(horario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorarioExists(horario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TecnicoId"] = new SelectList(_context.Tecnico, "Id", "Apellido", horario.TecnicoId);
            return View(horario);
        }

        // GET: Horarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Horario == null)
            {
                return NotFound();
            }

            var horario = await _context.Horario
                .Include(h => h.Tecnico)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horario == null)
            {
                return NotFound();
            }

            return View(horario);
        }

        // POST: Horarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Horario == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Horario'  is null.");
            }
            var horario = await _context.Horario.FindAsync(id);
            if (horario != null)
            {
                _context.Horario.Remove(horario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorarioExists(int id)
        {
          return _context.Horario.Any(e => e.Id == id);
        }
    }
}
