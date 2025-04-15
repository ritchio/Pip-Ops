using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Controllers
{
    public class AccesoController : Controller
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccesoController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Titulo = "Acceso";
            ViewBag.Error = null;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccesoLoginViewModel modelo)
        {
            ViewBag.Titulo = "Acceso";
            ViewBag.Error = null;
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Error al recibir los datos.";
                return View(modelo);
            }
            modelo.Recordar = true;
                

            var resultado = await _signInManager.PasswordSignInAsync(modelo.Correo, modelo.Contrasena, modelo.Recordar, lockoutOnFailure: false);
            Console.Write(resultado);

            if (resultado.Succeeded)
            {
                return LocalRedirect("/");
            }
            else
            {
                ViewBag.Error = "Usuario o contraseña no válidos.";
                return View(modelo);

            }

        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Acceso");
        }
    }
}
