using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoPNT1.Data;
using ProyectoPNT1.Models;
using System.Data;

namespace ProyectoPNT1.Controllers
{
    public class TecnicosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Persona> _userManager;

        public TecnicosController(ApplicationDbContext context, UserManager<Persona> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Tecnicos
        public async Task<IActionResult> Index()
        {
              return View(await _context.Tecnico.ToListAsync());
        }

        // GET: Tecnicos/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tecnico == null)
            {
                return NotFound();
            }

            var tecnico = await _context.Tecnico
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tecnico == null)
            {
                return NotFound();
            }

            return View(tecnico);
        }

        // GET: Tecnicos/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tecnicos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Dni,Direccion,Id,Nombre,Apellido,Email,Password,ConfirmPassword,Telefono")] Tecnico tecnico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tecnico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tecnico);
        }

        // GET: Tecnicos/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tecnico == null)
            {
                return NotFound();
            }

            var tecnico = await _context.Tecnico.FindAsync(id);
            if (tecnico == null)
            {
                return NotFound();
            }
            return View(tecnico);
        }

        // POST: Tecnicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Dni,Direccion,Id,Nombre,Apellido,Email,Password,ConfirmPassword,Telefono")] Tecnico tecnico)
        {
            if (id != tecnico.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var tecnicoExistente = await _context.Tecnico.FindAsync(id);
                    if (tecnicoExistente == null)
                    {
                        return NotFound();
                    }

                    // Volver a cargar los datos de la entidad desde la base de datos
                    _context.Entry(tecnicoExistente).Reload();

                    // Actualizar las propiedades de existingTecnico con los valores de tecnico
                    tecnicoExistente.Dni = tecnico.Dni;
                    tecnicoExistente.Direccion = tecnico.Direccion;
                    tecnicoExistente.Nombre = tecnico.Nombre;
                    tecnicoExistente.Apellido = tecnico.Apellido;
                    tecnicoExistente.Email = tecnico.Email;
                    tecnicoExistente.UserName = tecnico.Email;
                    tecnicoExistente.Telefono = tecnico.Telefono;

                    // Actualizar el UserName del IdentityUser con el nuevo Email
                    var usuarioExistente = await _userManager.FindByEmailAsync(tecnicoExistente.Email);
                    if (usuarioExistente != null)
                    {
                        usuarioExistente.UserName = tecnico.Email;
                        await _userManager.UpdateAsync(usuarioExistente);
                    }

                    _context.Update(tecnicoExistente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TecnicoExists(tecnico.Id))
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
            return View(tecnico);
        }


        // GET: Tecnicos/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)    
        {
            if (id == null || _context.Tecnico == null)
            {
                return NotFound();
            }

            var tecnico = await _context.Tecnico
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tecnico == null)
            {
                return NotFound();
            }

            return View(tecnico);
        }

        // POST: Tecnicos/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tecnico == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tecnico'  is null.");
            }
            var tecnico = await _context.Tecnico.FindAsync(id);
            if (tecnico != null)
            {
                _context.Tecnico.Remove(tecnico);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TecnicoExists(int id)
        {
          return _context.Tecnico.Any(e => e.Id == id);
        }
    }
}
