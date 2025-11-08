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
using HelpDesk.Services.Plaza.Interfaces;
using HelpDesk.Services.Area.Interfaces;
using HelpDesk.Models.Utils;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace HelpDesk.Controllers
{
    [Authorize]
    public class CatalogoPlazasController : Controller
    {
        #region [ Atributos ]
        private readonly IPlazaService _plazaService;
        private readonly IAreaService _areaService;
        #endregion [ Atributos ]

        #region [ Constructor ]

        public CatalogoPlazasController(
            IPlazaService plazaService,
            IAreaService areaService)
        {
            _plazaService = plazaService;
            _areaService = areaService;
        }

        #endregion [ Constructor ]
        public async Task<IActionResult> Index()
        {
            var plazasViewModel = await _plazaService.ObtenerPlazasAsync();
            var areasViewModel = await _areaService.ObtenerAreasAsync();

            var viewModel = new CatPlazasAreasViewModel
            {
                Plazas = plazasViewModel,
                Areas = areasViewModel
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null) return NotFound();

            var plaza = await _plazaService.ObtenerPlazaPorIdAsync(id.Value);
            if (plaza == null) return NotFound();

            return View(plaza);
        }

        public async Task<IActionResult> Crear()
        {
            var model = new CatPlazaAreaViewModel();

            var catalogoPlazas = await _plazaService.ObtenerPlazasAsync();

            model.CatalogoPlazas = catalogoPlazas.Where(p => p.Activo == true).ToList();

            var respuesta = new RespuestaGenerica<string>
            {
                Exito = true,
                Datos = this.RenderizarVista("_Crear", model, true)
            };
            return Ok(respuesta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Cat_Plaza plaza)
        {
            if (ModelState.IsValid)
            {
                var respuesta = await _plazaService.CrearPlazaAsync(plaza);

                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }
            
        }
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            var plaza = await _plazaService.ObtenerPlazaPorIdAsync(id.Value);
            if (plaza == null) return NotFound();

            var respuesta = new RespuestaGenerica<string>
            {
                Exito = true,
                Datos = this.RenderizarVista("_Editar", plaza, true)
            };

            return Ok(respuesta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Cat_Plaza plaza)
        {
            if (id != plaza.IdPlaza) return NotFound();

            if (ModelState.IsValid)
            {
                var respuesta = await _plazaService.ActualizarPlazaAsync(plaza);

                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var respuesta = new RespuestaGenerica<string>
                {
                    Exito = false,
                    Mensaje = "Error al actualizar la plaza. Verifique los datos ingresados."
                };
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                return RedirectToAction(nameof(Index));

            }
        }

        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id == null) return NotFound();

            var plaza = await _plazaService.CambiarEstadoPlazaAsync(id.Value);

            var respuesta = new RespuestaGenerica<string> {
                Exito = true,
                Mensaje = "Estatus de la plaza ha cambiado con éxito.",
                Datos = plaza?"Activo":"Inactivo"
            };

            return Ok(respuesta);
        }

    }
    
}
