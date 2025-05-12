using HelpDesk.Authorization;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Area.Interfaces;
using HelpDesk.Services.ModuloClasificacion.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace HelpDesk.Controllers
{
    [Authorize]
    [VerificaPermiso(roles: new [] {Roles.Administrador, Roles.AdministradorSupervisor})]
    public class CatalogoModulosController(
        IModuloClasificacionService moduloClasificacionService,
        IAreaService areaService
        ) : Controller
    {
        private readonly IModuloClasificacionService _moduloClasificacionService = moduloClasificacionService;
        private readonly IAreaService _areaService = areaService;
        public async Task<IActionResult> Index()
        {
            var model = await _moduloClasificacionService.ObtenerModulosClasificacionesAsync();

            return View(model);
        }

        public async Task<IActionResult> Crear()
        {
            var modelo = new CatModuloClasificacionViewModel();

            modelo.Areas = await _areaService.ObtenerAreasAsync();
            modelo.TiposSoporte = await _moduloClasificacionService.ObtenerTiposSoporteAsync();


            var respuesta = new RespuestaGenerica<string>
            {
                Exito = true,
                Datos = this.RenderizarVista("_Crear", modelo, true)
            };

            //return View("_Crear", modelo);

            return Ok(respuesta);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CatModuloClasificacionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var success = await _moduloClasificacionService.CrearModuloClasificacionAsync(model);
                    if (success)
                    {
                        var respuesta = new RespuestaGenerica<string>
                        {
                            Exito = true,
                            Mensaje = "El modulo se ha creado correctamente",

                        };
                        TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                        return RedirectToAction("Index", "CatalogoModulos");
                    }
                    else
                    {

                        var respuesta = new RespuestaGenerica<string>
                        {
                            Exito = false,
                            Mensaje = "Error al crear el modulo, falló al insertar."
                        };
                        TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);

                        return RedirectToAction("Index", "CatalogoModulos");
                    }
                }
                else
                {
                    var respuesta = new RespuestaGenerica<string>
                    {
                        Exito = false,
                        Mensaje = "Error al crear el modulo, los datos no son correctos."

                    };
                    TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);

                    return RedirectToAction("Index", "CatalogoModulos");
                }
            }
            catch (Exception ex)
            {
                var respuesta = new RespuestaGenerica<string>
                {
                    Exito = false,
                    Mensaje = "Error al crear el modulo, " + ex.Message
                };
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);

                return RedirectToAction("Index", "CatalogoModulos");
            }
        }

        public IActionResult CambiarEstado(int idModulo)
        {
            var respuesta = _moduloClasificacionService.CambiarEstado(idModulo);
            return Ok(respuesta);
        }
        public async Task<IActionResult> Editar(int idModulo)
        {
            var modelo = await _moduloClasificacionService.ObtenerModuloClasificacionPorIdAsync(idModulo);
            modelo.Areas = await _areaService.ObtenerAreasAsync();
            modelo.TiposSoporte = await _moduloClasificacionService.ObtenerTiposSoporteAsync();
            var respuesta = new RespuestaGenerica<string>
            {
                Exito = true,
                Datos = this.RenderizarVista("_Editar", modelo, true)
            };
            return Ok(respuesta);
        }

        [HttpPost]

        public  IActionResult Editar(CatModuloClasificacionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var success =  _moduloClasificacionService.ModificarModuloClasificacion(model);

                    if(success != null)
                    {
                        var respuesta = new RespuestaGenerica<string>
                        {
                            Exito = true,
                            Mensaje = "El modulo se ha actualizado correctamente"
                        };
                        TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                        return RedirectToAction("Index", "CatalogoModulos");
                    }
                    else
                    {
                        var respuesta = new RespuestaGenerica<string>
                        {
                            Exito = false,
                            Mensaje = "Error al actualizar el modulo, falló al insertar."
                        };
                        TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                        return RedirectToAction("Index", "CatalogoModulos");
                    }
                }
                else
                {
                    var respuesta = new RespuestaGenerica<string>
                    {
                        Exito = false,
                        Mensaje = "Error al actualizar el modulo, los datos no son correctos."
                    };
                    TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                    return RedirectToAction("Index", "CatalogoModulos");
                }
            }
            catch (Exception ex)
            {
                var respuesta = new RespuestaGenerica<string>
                {
                    Exito = false,
                    Mensaje = "Error al actualizar el modulo, " + ex.Message
                };
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                return RedirectToAction("Index", "CatalogoModulos");
            }
        }
    }
}
