using HelpDesk.Authorization;
using HelpDesk.Models.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HelpDesk.Controllers
{
    public class OpcionesController : Controller
    {
        [VerificaPermiso(roles: ["Administrador"])]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubirFondo(IFormFile fondo)
        {
            var respuesta = new RespuestaGenerica<string>();
            if (fondo == null || fondo.Length == 0 || !fondo.ContentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase))
            {
                respuesta.Exito = false;
                respuesta.Mensaje = "La imagen debe ser en formato jpg.";
                TempData["Notification"] = JsonSerializer.Serialize(respuesta);
                return RedirectToAction("Index");
            }

            var rutaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "fondo.jpg");

            try
            {
                using (var stream = new FileStream(rutaDestino, FileMode.Create))
                {
                    await fondo.CopyToAsync(stream);
                }

                respuesta.Exito = true;
                respuesta.Mensaje = "El fondo se ha actualizado con éxito.";
                TempData["Notification"] = JsonSerializer.Serialize(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Exito = false;
                respuesta.Mensaje = ex.Message;
                TempData["Notification"] = JsonSerializer.Serialize(respuesta);
            }

            return RedirectToAction("Index");
        }
    }
}
