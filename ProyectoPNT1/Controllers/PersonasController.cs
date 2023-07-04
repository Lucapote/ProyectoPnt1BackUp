using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoPNT1.Data;
using ProyectoPNT1.Models;
using System.Data;

namespace ProyectoPNT1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PersonasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Persona> _userManager;

        public PersonasController(ApplicationDbContext context, UserManager<Persona> userManager)
        {
            _context = context;
            this._userManager = userManager;
        }

        // GET: Personas
        public async Task<IActionResult> Index()
        {
              return View(await _context.Persona.ToListAsync());
        }

        // GET: Personas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Persona == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // GET: Personas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Personas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Email,Password,ConfirmPassword,Telefono")] Persona persona)
        {
            if (ModelState.IsValid)
            {
                _context.Add(persona);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(persona);
        }

        // GET: Personas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Persona == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }
            return View(persona);
        }

        // POST: Personas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Email,Password,ConfirmPassword,Telefono")] Persona persona)
        {
            if (id != persona.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var personaExistente = await _context.Persona.FindAsync(id);
                    if (personaExistente == null)
                    {
                        return NotFound();
                    }

                    // Actualizar las propiedades de existingPersona con los valores de persona
                    personaExistente.Nombre = persona.Nombre;
                    personaExistente.Apellido = persona.Apellido;
                    personaExistente.Email = persona.Email;
                    personaExistente.Telefono = persona.Telefono;

                    // Actualizar el UserName del IdentityUser con el nuevo Email
                    var usuarioExistente = await _userManager.FindByEmailAsync(personaExistente.Email);
                    if (usuarioExistente != null)
                    {
                        usuarioExistente.UserName = persona.Email;
                        await _userManager.UpdateAsync(usuarioExistente);
                    }

                    _context.Update(personaExistente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonaExists(persona.Id))
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
            return View(persona);
        }


        // GET: Personas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Persona == null)
            {
                return NotFound();
            }

            var persona = await _context.Persona
                .FirstOrDefaultAsync(m => m.Id == id);
            if (persona == null)
            {
                return NotFound();
            }

            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Persona == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Persona'  is null.");
            }
            var persona = await _context.Persona.FindAsync(id);
            if (persona != null)
            {
                _context.Persona.Remove(persona);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonaExists(int id)
        {
          return _context.Persona.Any(e => e.Id == id);
        }
    }
}
