using HelpDesk.Services.Ticket.Interfaces;
using HelpDesk.Services.Usuario.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Controllers
{
    [Authorize]

    public class TicketsUsuarioController(
        ITicketUsuarioService ticketUsuarioService,
        IUsuarioService usuarioService
        ) : Controller
    {
        private readonly ITicketUsuarioService _ticketUsuarioService = ticketUsuarioService;
        private readonly IUsuarioService _usuarioService = usuarioService;
        public async Task<IActionResult> Index()
        {
            //aquí no se puede tomar los tickets asignados al agente porque aún no se ha asignado
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var respuesta = _ticketUsuarioService.ObtenerNuevos(usuario);
            return View(respuesta);
        }
        public async Task<IActionResult> Abiertos()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var respuesta = _ticketUsuarioService.ObtenerAbiertos(usuario);
            return View(respuesta);
        }
        public async Task<IActionResult> Pendientes()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var respuesta = _ticketUsuarioService.ObtenerPendientes(usuario);
            return View(respuesta);
        }
        public async Task<IActionResult> Cerrados()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var respuesta = _ticketUsuarioService.ObtenerCerrados(usuario);
            return View(respuesta);
        }
        public async Task<IActionResult> Validados()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var respuesta = _ticketUsuarioService.ObtenerValidados(usuario);
            return View(respuesta);
        }
        public async Task<IActionResult> Respuestas()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var respuesta = _ticketUsuarioService.ObtenerRespuesta(usuario);
            return View(respuesta);
        }

    }
}
