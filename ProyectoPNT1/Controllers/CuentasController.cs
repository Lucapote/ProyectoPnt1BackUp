using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoPNT1.Data;
using ProyectoPNT1.Models;
using ProyectoPNT1.Recursos;
using ProyectoPNT1.ViewModel;

namespace ProyectoPNT1.Controllers
{
    public class CuentasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<Persona> _signInManager;
        private readonly UserManager<Persona> _userManager;
        private readonly RoleManager<Rol> _roleManager;

        public CuentasController(ApplicationDbContext context ,SignInManager<Persona> signInManager, UserManager<Persona> userManager, RoleManager<Rol> roleManager)
        {
            this._context = context;
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }
        public IActionResult IniciarSesion(string returnUrl)
        {
            //usamos el tempdata para pasar el parametro de metodo a metodo
            TempData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login viewModel)
        {
            //convertimos ese termdata en un string
            string returnUrl = TempData["ReturnUrl"] as string;

            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var resultado =
                await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: false);
            if(resultado.Succeeded)
            {
                //comprobamos que el string del tempdata no sea null o empty
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    //en caso de que no sea nulo, lo usamos para la redireccion del url
                    return Redirect(returnUrl);
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, ErrorMsg.ErrorLogin);
                return View(viewModel);
            }
        }

        [Authorize]
        public IActionResult Registrar()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Registrar(RegistroUsuario viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var usuario = new Persona
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                Nombre = viewModel.Nombre,
                Apellido = viewModel.Apellido
            };

            var resultado = await _userManager.CreateAsync(usuario, viewModel.Password);
            if (resultado.Succeeded)
            {
                // Asignar rol "Admin" o "Dispatcher" según la selección del usuario
                var rol = viewModel.EsAdmin ? "Admin" : "Dispatcher";
                await _userManager.AddToRoleAsync(usuario, rol);

                await _signInManager.SignInAsync(usuario, isPersistent: false);
                return RedirectToAction("Edit", "Personas", new { id = usuario.Id });
            }
            else
            {
                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(viewModel);
            }
        }


        [Authorize]
        public IActionResult RegistrarTecnico()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrarTecnico(RegistroTecnico viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var tecnico = new Tecnico
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                Nombre = viewModel.Nombre,
                Apellido = viewModel.Apellido,
                Dni = viewModel.Dni,
                Direccion = viewModel.Direccion
            };

            var result = await _userManager.CreateAsync(tecnico, viewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(tecnico, isPersistent: false);

                // Asignar rol "Tecnico" al usuario
                await _userManager.AddToRoleAsync(tecnico, "Tecnico");

                return RedirectToAction("Edit", "Tecnicos", new { id = tecnico.Id });
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(viewModel);
            }
        }


        public async Task<IActionResult> CerrarSesion()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
