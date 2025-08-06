using HelpDesk.Constants;
using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Correo.Interfaces;
using HelpDesk.Services.Opcion.Interfaces;
using HelpDesk.Services.Usuario.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Shared;
using OtpNet;
using System.Security.Claims;
using System.Text.Json;

namespace HelpDesk.Controllers
{
    public class AccesoController(
        SignInManager<Cat_Usuario> signInManager, 
        UserManager<Cat_Usuario> userManager,
        IUsuarioService usuarioService,
        ICorreoService correoService,
        IConfiguration appSettings,
        IOpcionService opcionesService 
        ) : Controller
    {

        private readonly SignInManager<Cat_Usuario> _signInManager = signInManager;
        private readonly UserManager<Cat_Usuario> _userManager = userManager;
        private readonly IUsuarioService _usuarioService = usuarioService;
        private readonly ICorreoService _correoService = correoService;
        private readonly IConfiguration _appSettings = appSettings;
        private readonly IOpcionService _opcionesService = opcionesService;

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
                //return RedirectToAction("Index", "Home");
                return Redirect("~/Identity/Account/Login");

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

        [HttpGet]
        public async Task<IActionResult> VerificaTOTP(int? scan)
        {

            var is2fa = User.HasClaim("Is2FAAuthenticated", "true");

            //revisamos si ya se verificó el 2FA
            if (!is2fa)
            {
                //verificamos si el usuario tiene activo el TOTP
                var usuario = await _userManager.GetUserAsync(User);
                if (string.IsNullOrEmpty(usuario.ClaveTOTP) || !usuario.TOTPActivo || usuario.TOTPUltimaVerificacion == null)
                {
                    if(scan.HasValue && scan.Value == 1)
                    {
                        return View();
                    }
                    else
                    {
                       
                        return RedirectToAction("RegistroTOTP", "Acceso");
                    }
                        
                }
                else
                {
                    //revisamos si la última verificación se hizo hace menos de el tiempo máximo
                    if(usuario.TOTPUltimaVerificacion == null || usuario.TOTPUltimaVerificacion < DateTime.UtcNow.AddHours(-15))
                    {
                        return View();
                    }
                    else
                    {
                        //no necesita 2faa

                        await SesionCon2FAA(usuario);

                        return RedirectToAction("Index", "Home");

                    }

                }

            }
            else
            {
                return RedirectToAction("Index", "Home");

            }
        }

        [HttpPost]
        public async Task<IActionResult> VerificaTOTP(string codigoTOTP)
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);

            if (string.IsNullOrEmpty(usuario.ClaveTOTP))
            {
                ModelState.AddModelError(string.Empty, "No se encontró clave TOTP para este usuario.");
                return View();
            }
            //Console.WriteLine($"Clave desde la bd: {usuario.ClaveTOTP}");
            // Decodifica el secreto (Base32)
            var secretBytes = Base32Encoding.ToBytes(usuario.ClaveTOTP);

            var totp = new Totp(secretBytes);


            string currentCode = totp.ComputeTotp();
            //Console.WriteLine($"🔑 Código esperado en servidor AHORA: {currentCode}");
            //Console.WriteLine($"🔑 Código esperado enviado: {codigoTOTP}");

            

            // Verifica ventana ± 1
            bool esValido = totp.VerifyTotp(codigoTOTP.Trim().Replace(" ", ""),
                                           out long timeStepMatched,
                                           new VerificationWindow(previous: 1, future: 1));

            if (!esValido)
            {
                var respuesta = new RespuestaGenerica<string>
                {
                    Exito = false,
                    Mensaje = "Código TOTP inválido. Por favor, inténtelo de nuevo."
                };
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                return View();
            }

            // Código válido: actualiza estado o firma cookie de sesión
            usuario.TOTPActivo = true; // si aún no estaba activo
            usuario.TOTPUltimaVerificacion = DateTime.UtcNow;

            await _usuarioService.ActualizarTOTP(usuario);

            var catusuario = await _userManager.FindByIdAsync(usuario.Id);

            await SesionCon2FAA(catusuario);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult RegistroTOTP()
        {
            return View();
        }

        public async Task<IActionResult> ObtenerFondo()
        {
            var fondo = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.Fondo.Archivo);
            if (fondo == null || string.IsNullOrEmpty(fondo.Valor))
            {
                return NotFound();
            }
            var bytes = Convert.FromBase64String(fondo.Valor);
            return File(bytes, "image/jpeg");
        }

        private async Task SesionCon2FAA(Cat_Usuario usuario)
        {
            var config = _appSettings.GetSection("Security:Session");
            int duracionSesion = int.Parse(config["MaxHours"]);

            // Construye un nuevo ClaimsPrincipal
            var principal = await _signInManager.CreateUserPrincipalAsync(usuario);

            var identity = (ClaimsIdentity)principal.Identity;

            // Encuentra y reemplaza el claim si ya existe
            var existingClaim = identity.FindFirst("Is2FAAuthenticated");
            if (existingClaim != null)
            {
                identity.RemoveClaim(existingClaim);
            }

            identity.AddClaim(new Claim("Is2FAAuthenticated", "true"));

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(duracionSesion),
            };

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal, authProperties);
        }



    }
}
