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
    }
}
