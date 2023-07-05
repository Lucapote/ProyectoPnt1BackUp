using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoPNT1.Data;
using ProyectoPNT1.Models;
using ProyectoPNT1.ViewModel;

namespace ProyectoPNT1.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministradorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Persona> _userManager;

        public AdministradorController(ApplicationDbContext context, UserManager<Persona> userManager)
        {
            this._context = context;
            this._userManager = userManager;
        }

        public async Task<IActionResult> ListadoUsuarios()
        {
            var usuariosConRoles = await _context.Users.ToListAsync();
            var viewModel = new List<UsuarioConRol>();

            foreach (var user in usuariosConRoles)
            {
                var roles = await _userManager.GetRolesAsync((Persona)user);
                var userViewModel = new UsuarioConRol
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    RoleName = roles.FirstOrDefault()
                };

                viewModel.Add(userViewModel);
            }

            return View(viewModel);
        }

        public IActionResult EditarRol(int id)
        {
            var usuario = _context.Users.FirstOrDefault(u => u.Id == id);
            var roles = _context.Roles.Select(r => r.Name).ToList();
            var viewModel = new EditarRol
            {
                UsuarioId = usuario.Id,
                NombreUsuario = usuario.UserName,
                Roles = new SelectList(roles)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarRol(EditarRol viewModel)
        {
            var usuario = await _userManager.FindByIdAsync(viewModel.UsuarioId.ToString());

            if (ModelState.IsValid)
            {
                // Eliminar el rol actual del usuario
                var currentRoles = await _userManager.GetRolesAsync(usuario);
                await _userManager.RemoveFromRolesAsync(usuario, currentRoles);

                // Agregar el nuevo rol seleccionado al usuario
                await _userManager.AddToRoleAsync(usuario, viewModel.RolSeleccionado);

                return RedirectToAction("ListadoUsuarios");
            }

            viewModel.Roles = new SelectList(_context.Roles.Select(r => r.Name).ToList());
            return View(viewModel);
        }
    }
}
