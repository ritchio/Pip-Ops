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

namespace HelpDesk.Controllers
{
    public class CatalogoAreasController : Controller
    {
        #region [ Atributos ]
        private readonly IAreaService _areaService;
        private readonly IPlazaService _plazaService;
        #endregion [ Atributos ]

        public CatalogoAreasController(
            IAreaService areaService,
            IPlazaService plazaService
            )
        {
            _areaService = areaService;
            _plazaService = plazaService;
        }

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
                var success = await _areaService.CrearAreaAsync(Area);
                if (success) return RedirectToAction("Index", "CatalogoPlazas");
            }
            return View(Area);
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
                var success = await _areaService.ActualizarAreaAsync(Area);
                if (success) return RedirectToAction("Index", "CatalogoPlazas");
            }
            return View(Area);
        }

        public async Task<IActionResult> CambiarEstado(int? id)
        {
            if (id == null) return NotFound();

            var area = await _areaService.CambiarEstadoAreaAsync(id.Value);


            var respuesta = new RespuestaGenerica<string>
            {
                Exito = true,
                Mensaje = "Estatus del area cambiado con éxito.",
                Datos = area ? "Activo" : "Inactivo"
            };

            return Ok(respuesta);
        }
    }
}
