using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProyectoPNT1.Models;
using ProyectoPNT1.Recursos;
using ProyectoPNT1.ViewModel;

namespace ProyectoPNT1.Controllers
{
    public class CuentasController : Controller
    {
        private readonly SignInManager<Persona> _signInManager;
        private readonly UserManager<Persona> _userManager;

        public CuentasController(SignInManager<Persona> signInManager, UserManager<Persona> userManager)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
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

            var user = new Persona
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                Nombre = viewModel.Nombre,
                Apellido = viewModel.Apellido
            };

            var result = await _userManager.CreateAsync(user, viewModel.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
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
