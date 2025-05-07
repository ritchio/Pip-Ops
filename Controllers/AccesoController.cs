using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Correo.Interfaces;
using HelpDesk.Services.Usuario.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using System.Text.Json;

namespace HelpDesk.Controllers
{
    public class AccesoController(
        SignInManager<Cat_Usuario> signInManager, 
        UserManager<Cat_Usuario> userManager,
        IUsuarioService usuarioService,
        ICorreoService correoService
        ) : Controller
    {

        private readonly SignInManager<Cat_Usuario> _signInManager = signInManager;
        private readonly UserManager<Cat_Usuario> _userManager = userManager;
        private readonly IUsuarioService _usuarioService = usuarioService;
        private readonly ICorreoService _correoService = correoService;

        public IActionResult ReiniciarContrasenia()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReiniciarContrasenia(string correo)
        {
            var respuesta = new RespuestaGenerica<string>();
            if (string.IsNullOrEmpty(correo))
            {

                respuesta.Exito = false;
                respuesta.Mensaje = "Debe capturar el correo electrónico registrado.";
            }
            else
            {
                var normalizedCorreo = correo.ToUpper();
                var usuario = await _userManager.Users
                    .Where(u => u.NormalizedEmail == normalizedCorreo.Trim() && u.Activo)
                    .FirstOrDefaultAsync();

                if (usuario != null)
                {

                    usuario.CambiarContrasenia = true;
                    await _userManager.UpdateAsync(usuario);

                    //token para poder resetear sin la contraseña actual
                    var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
                    var nuevaContrasenia = _usuarioService.GenerarContraseniaAleatoria(8);
                    var resultado = await _userManager.ResetPasswordAsync(usuario, token, nuevaContrasenia);
                    if(resultado.Succeeded){

                        // Enviar correo con la nueva contraseña
                        var destinatario = correo;
                        var asunto = "Restablecimiento de contraseña";
                        var cuerpo = $"Hola {usuario.NombreCompleto},<br/>" +
                                      $"Se ha solicitado un restablecimiento de contraseña. " +
                                      $"Su nueva contraseña es: <strong>{nuevaContrasenia}</strong><br/>" + 
                                      $"Por favor, cambie su contraseña después de iniciar sesión.";

                        await _correoService.EnviarCorreoAsync(destinatario, asunto, cuerpo, true);
                        respuesta.Exito = true;
                        respuesta.Mensaje = "Se ha enviado un correo para restablecer la contraseña.";
                    }
                    else
                    {
                        respuesta.Exito = false;
                        respuesta.Mensaje = "No se pudo actualizar la contraseña.";
                    }
                    
                }
                else
                {
                    respuesta.Exito = false;
                    respuesta.Mensaje = "Usuario no encontrado.";
                }
            }

            TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
            // Redirigimos al login
            if (respuesta.Exito)
            {
                return Redirect("/Login");
            }
            else
            {
                return View();
            }
            
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
