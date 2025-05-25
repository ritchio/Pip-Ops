using HelpDesk.Authorization;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Autorizacion.Interfaces;
using HelpDesk.Services.ModuloClasificacion.Interfaces;
using HelpDesk.Services.Ticket.Interfaces;
using HelpDesk.Services.TipoSoporte.Interfaces;
using HelpDesk.Services.Usuario.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using System.Text.Json;

namespace HelpDesk.Controllers
{
    [Authorize]
    public class TicketsController(
        IModuloClasificacionService moduloClasificacionService,
        IAutorizacionService autorizacionService,
        ITipoSoporteService tipoSoporte,
        ITicketService ticketService,
        IUsuarioService usuarioService
        ) : Controller
    {
        #region [ Atributos ]
        private readonly IModuloClasificacionService _moduloClasificacionService = moduloClasificacionService;
        private readonly IAutorizacionService _autorizacionService = autorizacionService;
        private readonly ITipoSoporteService _tipoSoporteService = tipoSoporte;
        private readonly ITicketService _ticketService = ticketService;
        private readonly IUsuarioService _usuarioService = usuarioService;
        #endregion [ Atributos ]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/Tickets/Detalles/{idTicket}")]
        public async Task<IActionResult> Detalles(int idTicket)
        {
            var ticket = await _ticketService.ObtenerPorId(idTicket);

            //datos relacionados
            var usuario = await _usuarioService.ObtenerUsuarioPorId(ticket.IdUsuario);

            //catalogos
            ticket.TiposSoporte = await _tipoSoporteService.ObtenerTiposSoporteAsync();
            var tipoSoporte = ticket.TiposSoporte.FirstOrDefault();
            ticket.ModulosClasificaciones = await _moduloClasificacionService.ObtenerModulosClasificacionesPorTipoSoporteAsync(tipoSoporte.IdTipoSoporte);

            if (usuario != null)
            {
                ticket.Usuario = usuario.Datos;
            }
            ticket.Editable = false;

            var respuesta = new RespuestaGenerica<string>
            {
                Exito = true,
                Datos = this.RenderizarVista("_Editar", ticket, true)
            };

            return Ok(respuesta);

        }

        [VerificaPermiso(Permisos.Ticket.Crear)]
        public async Task<IActionResult> Crear()
        {
            var modelo = new TicketViewModel();

            modelo.TiposSoporte = await _tipoSoporteService.ObtenerTiposSoporteAsync();
            var tipoSoporte = modelo.TiposSoporte.FirstOrDefault();
            modelo.ModulosClasificaciones = await _moduloClasificacionService.ObtenerModulosClasificacionesPorTipoSoporteAsync(tipoSoporte.IdTipoSoporte);
            

            var respuesta = new RespuestaGenerica<string>
            {
                Exito = true,
                Datos = this.RenderizarVista("_Crear", modelo, true)
            };

            return Ok(respuesta);
        }

        [VerificaPermiso(Permisos.Ticket.Crear)]
        [HttpPost]
        public async Task<IActionResult> Crear(TicketViewModel ticketModel)
        {
            

            if (ModelState.IsValid)
            {
                var usuario = await _usuarioService.ObtenerUsuarioActual(User);
                ticketModel.IdUsuario = usuario.Id;

                var success = await _ticketService.GuardarTicket(ticketModel);
                if (success)
                {
                    var respuesta = new RespuestaGenerica<string>
                    {
                        Exito = true,
                        Mensaje = "Ticket creado exitosamente."
                    };
                    TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                    return RedirectToAction("Index", "Tickets");
                }
                else
                {
                    ModelState.AddModelError("", "Error al crear el ticket.");
                    var respuesta = new RespuestaGenerica<string>
                    {
                        Exito = false,
                        Mensaje = "Error al crear el ticket."
                    };
                    TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                    return RedirectToAction("Index", "Tickets");

                }
            }
            else
            {
                var respuesta = new RespuestaGenerica<string>
                {
                    Exito = false,
                    Mensaje = "Error al crear el ticket."
                };
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);

                return RedirectToAction("Index", "Tickets");
            }
        }

        [HttpGet("/Tickets/Validar/{idTicket}")]
        public async Task<IActionResult> Validar(int idTicket)
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var valida = await _ticketService.Validar(idTicket, usuario.Id);
            if (valida)
            {
                var respuesta = new RespuestaGenerica<string>
                {
                    Exito = true,
                    Mensaje = "Ticket validado exitosamente."
                };

                return Ok(respuesta);

            }
            else
            {
                var respuesta = new RespuestaGenerica<string>
                {
                    Exito = false,
                    Mensaje = "Error al validar el ticket."
                };

                return Ok(respuesta);
               
            }
        }

        public async Task<IActionResult> CantidadesPorEstado()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var cantidades = _ticketService.ObtenerCantidadesPorEstado(usuario);
            var respuesta = new RespuestaGenerica<List<TicketsCantidadesPorEstadoViewModel>>
            {
                Exito = true,
                Datos = cantidades
            };

            return Ok(respuesta); 
        }

        [HttpGet("/Tickets/Asignar/{idTicket}/{idUsuario}")]
        public async Task<IActionResult> Asignar(int idTicket, string idUsuario)
        {
            var ticket = await _ticketService.ObtenerPorId(idTicket);
            if(idUsuario == "auto")
            {
                var usuario = await _usuarioService.ObtenerUsuarioActual(User);
                idUsuario = usuario.Id;
            }

            var result = _ticketService.Asignar(idTicket, idUsuario);
            if (result)
            {
                var respuesta = new RespuestaGenerica<int>
                {
                    Exito = true,
                    Datos =ticket.IdTicket,
                    Mensaje = "Ticket asignado exitosamente.",
                    Llamada = "mostrarDetallesTicket"
                };
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                return RedirectToAction("Abiertos", "TicketsSoporte");
            }
            else
            {
                var respuesta = new RespuestaGenerica<string>
                {
                    Exito = false,
                    Mensaje = "Error al asignar el ticket."
                };
                return BadRequest(respuesta);
            }
        }
    }
}
