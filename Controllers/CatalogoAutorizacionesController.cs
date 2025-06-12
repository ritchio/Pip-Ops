using HelpDesk.Authorization;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Area.Interfaces;
using HelpDesk.Services.Autorizacion.Interfaces;
using HelpDesk.Services.ModuloClasificacion.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HelpDesk.Controllers
{
    [Authorize]
    [VerificaPermiso(roles: new[] { Roles.Administrador,Roles.AdministradorSupervisor})]
    public class CatalogoAutorizacionesController(
        IAutorizacionService autorizacionService,
        IAreaService areaService,
        IModuloClasificacionService moduloClasificacionService
        ) : Controller
    {
        private readonly IAutorizacionService _autorizacionService = autorizacionService;
        private readonly IAreaService _areaService = areaService;
        private readonly IModuloClasificacionService _moduloClasificacionService = moduloClasificacionService;
        public async Task<IActionResult> Index()
        {
            var autorizaciones = await _autorizacionService.ObtenerAutorizacionesAsync();
            return View(autorizaciones);
        }

        public async Task<IActionResult> Crear()
        {
            var modelo = new CatAutorizacionViewModel();
            modelo.CatalogoAreas = await _areaService.ObtenerAreasAsync();
            modelo.CatalogoModulosClasificaciones = await _moduloClasificacionService.ObtenerModulosClasificacionesAsync();

            var respuesta = new RespuestaGenerica<string>
            {
                Exito = true,
                Datos = this.RenderizarVista("_Crear",modelo,true)
            };

            return Ok(respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CatAutorizacionViewModel modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var respuesta = await _autorizacionService.CrearAutorizacionAsync(modelo);

                    TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);

                    return RedirectToAction("Index");
                    
                }
                else
                {
                    var respuesta = new RespuestaGenerica<string>
                    {
                        Exito = false,
                        Mensaje = "Los datos no son correctos."
                    };

                    TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                var respuesta = new RespuestaGenerica<string>
                {
                    Exito = false,
                    Mensaje = "Error al guardar la autorizacion. " + ex.Message
                };

                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                return RedirectToAction("Index");
            }
            
        }

        public async Task<IActionResult> Editar(int idAutorizacion)
        {
            var modelo = await _autorizacionService.ObtenerAutorizacionPorIdAsync(idAutorizacion);
            if (modelo == null)
            {
                return NotFound();
            }
            modelo.CatalogoAreas = await _areaService.ObtenerAreasAsync();
            modelo.CatalogoModulosClasificaciones = await _moduloClasificacionService.ObtenerModulosClasificacionesAsync();
            var respuesta = new RespuestaGenerica<string>
            {
                Exito = true,
                Datos = this.RenderizarVista("_Editar", modelo, true)
            };
            return Ok(respuesta);
        }
        [HttpPost]
        public async Task<IActionResult> Editar(CatAutorizacionViewModel modelo)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var respuesta = await _autorizacionService.ActualizarAutorizacionAsync(modelo);

                    TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                    return RedirectToAction("Index");
                    
                }
                else
                {
                    var respuesta = new RespuestaGenerica<string>
                    {
                        Exito = false,
                        Mensaje = "Los datos no son correctos."
                    };
                    TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                var respuesta = new RespuestaGenerica<string>
                {
                    Exito = false,
                    Mensaje = "Error al guardar la autorizacion. " + ex.Message
                };
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                return RedirectToAction("Index");
            }
        }
        
        public async Task<IActionResult> ObtenerPorModulo(int idModulo)
        {
            var respuesta = new RespuestaGenerica<CatAutorizacionViewModel>();
            var autorizacion = await _autorizacionService.ObtenerAutorizacionPorModuloAsync(idModulo);
            
            if(autorizacion != null)
            {
                respuesta.Exito = true;
                respuesta.Datos = autorizacion;
            }
            else
            {
                respuesta.Exito = false;
                respuesta.Mensaje = "No se encontró la autorización relacionada al módulo";
            }

            return Ok(respuesta);
        }
    }
}
