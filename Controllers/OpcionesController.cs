using HelpDesk.Authorization;
using HelpDesk.Constants;
using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Opcion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace HelpDesk.Controllers
{
    public class OpcionesController(
        IOpcionService opcionesService
        ) : Controller
    {
        private readonly IOpcionService _opcionesService = opcionesService;

        [VerificaPermiso(roles: ["Administrador"])]
        public async Task<IActionResult> Index()
        {
            try
            {
                var mensaje = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.MensajeDelDia.Mensaje);
                var posicion = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.MensajeDelDia.Posicion);

                var opciones = new OpcionesMensajeDelDiaViewModel
                {
                    MensajeDelDia = mensaje?.Valor,
                    Posicion = posicion?.Valor
                };
                return View(opciones);
            }
            catch(Exception ex)
            {
                TempData["Notificacion"] = JsonSerializer.Serialize(new RespuestaGenerica<string>
                {
                    Exito = false,
                    Mensaje = "Error al cargar las opciones de bienvenida: " + ex.Message
                });
                return View();
            }

            
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
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                return RedirectToAction("Index");
            }

            var rutaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "fondo.jpg");

            try
            {
                using (var stream = new FileStream(rutaDestino, FileMode.Create))
                {
                    await fondo.CopyToAsync(stream);
                }

                //guardamos la ruta en la base 
                var fondoRuta = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.Fondo.Ruta);
                if(fondoRuta != null)
                {
                    fondoRuta.Valor = "/wwwroot/img/fondo.jpg";
                    await _opcionesService.ActualizarAsync(fondoRuta);
                }
                //guardamos la version
                var fondoVersion = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.Fondo.Version);
                if(fondoVersion != null)
                {
                    var version = Int32.Parse(fondoVersion.Valor) + 1;
                    fondoVersion.Valor = version.ToString();

                    await _opcionesService.ActualizarAsync(fondoVersion);
                }
                
                respuesta.Exito = true;
                respuesta.Mensaje = "El fondo se ha actualizado con éxito.";
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
            }
            catch (Exception ex)
            {
                respuesta.Exito = false;
                respuesta.Mensaje = ex.Message;
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ActualizarMensajeDelDia (OpcionesMensajeDelDiaViewModel opciones)
        {
            if(opciones != null)
            {
                var mensajeDelDia = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.MensajeDelDia.Mensaje);
                if (mensajeDelDia == null)
                {
                    return NotFound();
                }
                mensajeDelDia.Valor = opciones.MensajeDelDia;
                await _opcionesService.ActualizarAsync(mensajeDelDia);
                
                var posicionMensaje = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.MensajeDelDia.Posicion);
                if (posicionMensaje == null)
                {
                    return NotFound();
                }
                posicionMensaje.Valor = opciones.Posicion;
                await _opcionesService.ActualizarAsync(posicionMensaje);

                TempData["Notificacion"] = JsonSerializer.Serialize(new RespuestaGenerica<string>
                {
                    Exito = true,
                    Mensaje = "Se ha configurado correctamente el mensaje del día."
                });
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Notificacion"] = JsonSerializer.Serialize(new RespuestaGenerica<string>
                {
                    Exito = false,
                    Mensaje = "No se ha podido actualizar el mensaje del día."
                });
                return RedirectToAction("Index");
            }
            //foreach (var config in opciones)
            //{
            //    await _opcionesService.ActualizarAsync(config);
            //}
            //return RedirectToAction(nameof(Index));
        }
    }
}
