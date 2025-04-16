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

namespace HelpDesk.Controllers
{
    public class CatalogoAreasController : Controller
    {
        private readonly IAreaService _areaService;

        public CatalogoAreasController(IAreaService AreaService)
        {
            _areaService = AreaService;
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
                if (success) return RedirectToAction(nameof(Index));
            }
            return View(Area);
        }
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            var Area = await _areaService.ObtenerAreaPorIdAsync(id.Value);
            if (Area == null) return NotFound();

            return View(Area);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Cat_Area Area)
        {
            if (id != Area.IdArea) return NotFound();

            if (ModelState.IsValid)
            {
                var success = await _areaService.ActualizarAreaAsync(Area);
                if (success) return RedirectToAction(nameof(Index));
            }
            return View(Area);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();

            var Area = await _areaService.ObtenerAreaPorIdAsync(id.Value);
            if (Area == null) return NotFound();

            return View(Area);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var success = await _areaService.EliminarAreaAsync(id);
            if (success) return RedirectToAction(nameof(Index));

            return NotFound();
        }
    }
}
