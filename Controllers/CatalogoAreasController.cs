using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Area.Interfaces;
using HelpDesk.Models.Utils;
using HelpDesk.Services.Plaza.Interfaces;
using System.Numerics;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace HelpDesk.Controllers
{
    [Authorize]
    public class CatalogoAreasController(
        IAreaService areaService,
        IPlazaService plazaService
            ) : Controller
    {
        #region [ Atributos ]
        private readonly IAreaService _areaService = areaService;
        private readonly IPlazaService _plazaService = plazaService;

        #endregion [ Atributos ]

        #region [ Consulta ]
        public async Task<IActionResult> Index()
        {
            var AreasViewModel = await _areaService.ObtenerAreasAsync();
            return View(AreasViewModel);
        }

        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null) return NotFound();

            var Area = await _areaService.ObtenerAreaPorIdAsync(id.Value);
            if (Area == null) return NotFound();

            return View(Area);
        }

        [HttpGet]
        [Route("CatalogoAreas/ObtenerPorPlaza/{idPlaza:int}")]
        public async Task<IActionResult> ObtenerPorPlaza(int idPlaza)
        {
            var respuesta =  new RespuestaGenerica<List<CatAreaViewModel>>();

            try
            {
                var areas = await _areaService.ObtenerAreasPorPlazaAsync(idPlaza);
                respuesta.Exito = true;
                respuesta.Datos = areas.Where(a => a.Activo == true).ToList();
            }
            catch (Exception ex)
            {
                respuesta.Exito = false;
                respuesta.Mensaje = "Error al obtener las áreas por plaza: " + ex.Message;
            }

            return Ok(respuesta);
        }
        #endregion [ Consulta ]

        #region [ Modificaciones ]
        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Cat_Area Area)
        {
            if (ModelState.IsValid)
            {
                var respuesta = await _areaService.CrearAreaAsync(Area);
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
            }
            else
            {
                var respuesta = new RespuestaGenerica<string>
                {
                    Exito = false,
                    Mensaje = "Error al crear el área. Verifique los datos ingresados."
                };
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
            }
            
            return RedirectToAction("Index", "CatalogoPlazas");
        }
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            var area = await _areaService.ObtenerAreaPorIdAsync(id.Value);
            if (area == null) return NotFound();

            var plazas = await _plazaService.ObtenerPlazasAsync();

            var model = new CatPlazaAreaViewModel
            {
                Plaza = new CatPlazaViewModel(),
                Area = area,
                CatalogoPlazas = plazas.Where(p => p.Activo == true).ToList()
            };

            var respuesta = new RespuestaGenerica<string>
            {
                Datos = this.RenderizarVista("_Editar", model, true),
                Exito = true
            };

            return Ok(respuesta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Cat_Area Area)
        {
            if (id != Area.IdArea) return NotFound();

            if (ModelState.IsValid)
            {
                var respuesta = await _areaService.ActualizarAreaAsync(Area);
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                return RedirectToAction("Index", "CatalogoPlazas");

            }
            else
            {
                var respuesta = new RespuestaGenerica<string>
                {
                    Exito = false,
                    Mensaje = "Error al actualizar el área. Verifique los datos ingresados."
                };
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                return RedirectToAction("Index", "CatalogoPlazas");
            }

        }

        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id == null) return NotFound();

            var area = await _areaService.CambiarEstadoAreaAsync(id.Value);


            var respuesta = new RespuestaGenerica<string>
            {
                Exito = true,
                Mensaje = "Estatus del area ha cambiado con éxito.",
                Datos = area ? "Activo" : "Inactivo"
            };

            return Ok(respuesta);
        }

        #endregion [ Modificaciones ]

    }
}
