using HelpDesk.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            if (fondo == null || fondo.Length == 0 || !fondo.ContentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase))
            {
                TempData["Error"] = "Por favor, suba un archivo válido en formato JPG.";
                return RedirectToAction("Index");
            }

            var rutaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "fondo.jpg");

            try
            {
                using (var stream = new FileStream(rutaDestino, FileMode.Create))
                {
                    await fondo.CopyToAsync(stream);
                }

                TempData["Exito"] = "El fondo se ha actualizado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Ocurrió un error al subir el archivo: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
