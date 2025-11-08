using HelpDesk.Authorization;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Ticket.Interfaces;
using HelpDesk.Services.Usuario.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HelpDesk.Controllers
{
    [Authorize]
    [VerificaPermiso(roles: [Roles.Soporte, Roles.Administrador, Roles.AdministradorSupervisor])]
    public class TicketsSoporteController(
        ITicketSoporteService ticketSoporteService,
        IUsuarioService usuarioService
        ) : Controller
    {
        private readonly ITicketSoporteService _ticketSoporteService = ticketSoporteService;
        private readonly IUsuarioService _usuarioService = usuarioService;
        public async Task<IActionResult> Index()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var respuesta = _ticketSoporteService.ObtenerNuevos(usuario);
            return View(respuesta);
        }
        public async Task<IActionResult> Abiertos()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var respuesta = _ticketSoporteService.ObtenerAbiertos(usuario);
            return View(respuesta);
        }
        public async Task<IActionResult> Pendientes()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var respuesta = _ticketSoporteService.ObtenerPendientes(usuario);
            return View(respuesta);
        }
        public async Task<IActionResult> Cerrados()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var respuesta = _ticketSoporteService.ObtenerCerrados(usuario);
            return View(respuesta);
        }
        public async Task<IActionResult> Validados()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var respuesta = _ticketSoporteService.ObtenerValidados(usuario);
            return View(respuesta);
        }
        public async Task<IActionResult> Respuesta()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var respuesta = _ticketSoporteService.ObtenerRespuesta(usuario);
            return View(respuesta);
        }

        [VerificaPermiso(Permisos.Ticket.Asignar)]
        [HttpGet("/TicketsSoporte/AsignarAgente/{idTicket}")]
        public async Task<IActionResult> AsignarAgente(int idTicket)
        {
            var usuarios = await _usuarioService.ObtenerUsuariosPorRol(Roles.Soporte);
            
            ViewData["IdTicket"] = idTicket;
            var respuesta = new RespuestaGenerica<string>
            {
                Exito = true,
                Datos = this.RenderizarVista("_AsignarAgente", usuarios, true)
            };

            
            return Ok(respuesta);
            //return View("_AsignarAgente", usuarios);
        }
    }
}
