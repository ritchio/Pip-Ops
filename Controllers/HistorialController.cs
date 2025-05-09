using HelpDesk.Authorization;
using HelpDesk.Data;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Historial.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Controllers
{
    [VerificaPermiso(roles: ["Administrador"])]
    public class HistorialController(
        IHistorialService historialService
        ) : Controller
    {
        private readonly IHistorialService _historialService = historialService;
        public async Task<IActionResult> Index()
        {
            var accesos = await _historialService.ObtenerAccesosAsync();
            var actividades = await _historialService.ObtenerTicketsActividadesAsync();

            var model = new HistorialViewModel
            {
                Accesos = accesos,
                TicketsActividades = actividades
            };
            return View(model);
        }
    }
}
