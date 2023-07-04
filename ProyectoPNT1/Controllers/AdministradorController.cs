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
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            var roles = _context.Roles.Select(r => r.Name).ToList();
            var viewModel = new EditarRol
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = new SelectList(roles)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarRol(EditarRol viewModel)
        {
            var user = await _userManager.FindByIdAsync(viewModel.UserId.ToString());

            if (ModelState.IsValid)
            {
                // Eliminar el rol actual del usuario
                var currentRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

                // Agregar el nuevo rol seleccionado al usuario
                await _userManager.AddToRoleAsync(user, viewModel.SelectedRole);

                return RedirectToAction("ListadoUsuarios");
            }

            viewModel.Roles = new SelectList(_context.Roles.Select(r => r.Name).ToList());
            return View(viewModel);
        }
    }
}
