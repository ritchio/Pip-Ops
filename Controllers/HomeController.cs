using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HelpDesk.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using HelpDesk.Services.Usuario.Interfaces;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;

namespace HelpDesk.Controllers;
[Authorize]
public class HomeController(
    ILogger<HomeController> logger,
    IUsuarioService usuarioService
    ) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly IUsuarioService _usuarioService = usuarioService;

    public async Task<IActionResult> Index()
    {
        ViewBag.titulo = "HelpDesk - Panel";

        //checamos si el usuario tiene que cambiar la contraseña
        var usuario = await _usuarioService.ObtenerUsuarioActual(User);
        if (usuario.CambiarContrasenia)
        {
            return RedirectToAction("CambiarContrasenia");
        }
        else
        {
            return View();
        }
    }

   

    public IActionResult CambiarContrasenia() {

        ViewBag.titulo = "HelpDesk - Cambiar Contraseña";
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CambiarContrasenia(CambiarContraseniaViewModel model)
    {
        try
        {

            var respuesta = await _usuarioService.CambiarContrasenia(User, model.ContraseniaNueva);
            if (respuesta.Exito)
            {
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                await HttpContext.SignOutAsync();
                return Redirect("/Login");
            }
            else
            {
                ModelState.AddModelError("", respuesta.Mensaje);
                return View(model);
            }

        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Ocurrió un error al cambiar la contraseña");
            return View(model);
        }
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}




