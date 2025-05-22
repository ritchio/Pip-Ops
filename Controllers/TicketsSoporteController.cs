using HelpDesk.Authorization;
using HelpDesk.Services.Ticket.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Controllers
{
    [Authorize]
    [VerificaPermiso(roles: [Roles.Soporte, Roles.Administrador, Roles.AdministradorSupervisor])]
    public class TicketsSoporteController(
        ITicketSoporteService ticketSoporteService
        ) : Controller
    {
        private readonly ITicketSoporteService _ticketSoporteService = ticketSoporteService;
        public IActionResult Index()
        {
            var respuesta = _ticketSoporteService.ObtenerNuevos();
            return View(respuesta);
        }
        public IActionResult Abiertos()
        {
            var respuesta = _ticketSoporteService.ObtenerAbiertos();
            return View(respuesta);
        }
        public IActionResult Pendientes()
        {
            var respuesta = _ticketSoporteService.ObtenerPendientes();
            return View(respuesta);
        }
        public IActionResult Cerrados()
        {
            var respuesta = _ticketSoporteService.ObtenerCerrados();
            return View(respuesta);
        }
        public IActionResult Validados()
        {
            var respuesta = _ticketSoporteService.ObtenerValidados();
            return View(respuesta);
        }
        public IActionResult Respuesta()
        {
            var respuesta = _ticketSoporteService.ObtenerRespuesta();
            return View(respuesta);
        }
    }
}
