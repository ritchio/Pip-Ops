using HelpDesk.Authorization;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Ticket.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.Controllers
{
    [Authorize]
    [VerificaPermiso(roles: [Roles.Administrador,Roles.AdministradorSupervisor])]
    public class ReportesController(
        ITicketService ticketService
        ) : Controller
    {
        private readonly ITicketService _ticketService = ticketService;
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Registros(DateTime fechaInicio, DateTime fechaFinal)
        {
            var respuesta = new RespuestaGenerica<List<ReporteAgenteViewModel>>();

            var result = await _ticketService.AtendidosPorAgente(fechaInicio, fechaFinal, null);

            respuesta.Exito = true;
            respuesta.Datos = result;

            return Ok(respuesta);
        }

        public async Task<IActionResult> Detalles(DateTime fechaInicio, DateTime fechaFinal)
        {

            var respuesta = new RespuestaGenerica<List<ReporteDetalladoViewModel>>();
            var result = await _ticketService.ReporteDetallado(fechaInicio, fechaFinal);

            if(result != null)
            {
                respuesta.Exito = true;
                respuesta.Datos = result;
            }
            else
            {
                respuesta.Exito = false;
                respuesta.Mensaje = "No se encontraron registros para el rango de fechas especificado.";
            }
                return Ok(respuesta);
        }
    }
}
