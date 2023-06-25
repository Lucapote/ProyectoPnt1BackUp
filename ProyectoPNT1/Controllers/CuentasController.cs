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

        public CuentasController(SignInManager<Persona> signInManager)
        {
            this._signInManager = signInManager;
        }
        public IActionResult IniciarSesion()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IniciarSesion(Login viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var resultado =
                await _signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, lockoutOnFailure: false);
            if(resultado.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, ErrorMsg.ErrorLogin);
                return View(viewModel);
            }
        }
    }
}
