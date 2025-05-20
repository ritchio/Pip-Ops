using HelpDesk.Services.Ticket.Interfaces;
using HelpDesk.Services.Usuario.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Controllers
{
    public class TicketsUsuarioController(
        ITicketUsuarioService ticketUsuarioService,
        IUsuarioService usuarioService
        ) : Controller
    {
        private readonly ITicketUsuarioService _ticketUsuarioService = ticketUsuarioService;
        private readonly IUsuarioService _usuarioService = usuarioService;
        public async Task<IActionResult> Index()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var respuesta = _ticketUsuarioService.ObtenerNuevos(usuario.Id);
            return View(respuesta);
        }

    }
}
