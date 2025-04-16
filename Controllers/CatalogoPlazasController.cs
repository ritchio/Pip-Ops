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

namespace HelpDesk.Controllers
{
    public class CatalogoPlazasController : Controller
    {
        private readonly IPlazaService _plazaService;

        public CatalogoPlazasController(IPlazaService plazaService)
        {
            _plazaService = plazaService;
        }

        public async Task<IActionResult> Index()
        {
            var plazasViewModel = await _plazaService.ObtenerPlazasAsync();
            return View(plazasViewModel);
        }

        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null) return NotFound();

            var plaza = await _plazaService.ObtenerPlazaPorIdAsync(id.Value);
            if (plaza == null) return NotFound();

            return View(plaza);
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Cat_Plaza plaza)
        {
            if (ModelState.IsValid)
            {
                var success = await _plazaService.CrearPlazaAsync(plaza);
                if (success) return RedirectToAction(nameof(Index));
            }
            return View(plaza);
        }
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null) return NotFound();

            var plaza = await _plazaService.ObtenerPlazaPorIdAsync(id.Value);
            if (plaza == null) return NotFound();

            return View(plaza);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, Cat_Plaza plaza)
        {
            if (id != plaza.IdPlaza) return NotFound();

            if (ModelState.IsValid)
            {
                var success = await _plazaService.ActualizarPlazaAsync(plaza);
                if (success) return RedirectToAction(nameof(Index));
            }
            return View(plaza);
        }

        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null) return NotFound();

            var plaza = await _plazaService.ObtenerPlazaPorIdAsync(id.Value);
            if (plaza == null) return NotFound();

            return View(plaza);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            var success = await _plazaService.EliminarPlazaAsync(id);
            if (success) return RedirectToAction(nameof(Index));

            return NotFound();
        }
    }
}
