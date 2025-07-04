using HelpDesk.Authorization;
using HelpDesk.Constants;
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
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using static HelpDesk.Authorization.Permisos;

namespace HelpDesk.Controllers
{
    [Authorize]
    public class TicketsController(
        IModuloClasificacionService moduloClasificacionService,
        IAutorizacionService autorizacionService,
        ITipoSoporteService tipoSoporte,
        ITicketService ticketService,
        ITicketAdjuntoService ticketAdjuntoService,
        ITicketRespuestaService ticketRespuestaService,
        ITicketRespuestaAdjuntoService ticketRespuestaAdjuntoService,
        IUsuarioService usuarioService
        ) : Controller
    {
        #region [ Atributos ]
        private readonly IModuloClasificacionService _moduloClasificacionService = moduloClasificacionService;
        private readonly IAutorizacionService _autorizacionService = autorizacionService;
        private readonly ITipoSoporteService _tipoSoporteService = tipoSoporte;
        private readonly ITicketService _ticketService = ticketService;
        private readonly ITicketAdjuntoService _ticketAdjuntoService = ticketAdjuntoService;
        private readonly ITicketRespuestaService _ticketRespuestaService = ticketRespuestaService;
        private readonly ITicketRespuestaAdjuntoService _ticketRespuestaAdjuntoService = ticketRespuestaAdjuntoService;
        private readonly IUsuarioService _usuarioService = usuarioService;
        #endregion [ Atributos ]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "TicketsUsuario");
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
            ticket.ModulosClasificaciones = await _moduloClasificacionService.ObtenerModulosClasificacionesPorTipoSoporteAsync(ticket.IdTipoSoporte);
            var estados = await _ticketService.ObtenerEstadosTicket();
            ticket.EstadosTicket = estados.Where(et => et.IdEstadoTicket > EstadoTicket.Abierto && et.IdEstadoTicket < EstadoTicket.Validado).ToList();



            if (usuario != null)
            {
                ticket.Usuario = usuario.Datos;
            }
            //agente
            var usuarioActual = await _usuarioService.ObtenerUsuarioActual(User);

            if (usuarioActual != null)
            {
                ticket.EsAgente = usuarioActual.Id == ticket.Asignacion?.Where(a => a.Activo = true).FirstOrDefault()?.IdUsuario;
                ticket.EsSolicitante = usuarioActual.Id == ticket.IdUsuario;
            }


            ticket.Editable = (ticket.IdEstadoTicket == EstadoTicket.Rechazado);

            var respuesta = new RespuestaGenerica<string>
            {
                Exito = true,
                Datos = this.RenderizarVista("_Editar", ticket, true)
            };

            return Ok(respuesta);

        }

        public async Task<IActionResult> Crear()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var modelo = new TicketViewModel();

            modelo.CorreoElectronico = usuario.CorreoElectronico;
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

        [HttpPost]
        public async Task<IActionResult> Crear(TicketViewModel ticketModel)
        {

            var usuario = await _usuarioService.ObtenerUsuarioActual(User);

            if (ModelState.IsValid)
            {

                var conto = (usuario.RolNombre == Roles.Usuario) ? "TicketsUsuario" : "TicketsSoporte";
                var success = await _ticketService.GuardarTicket(ticketModel,usuario);
                if (success)
                {
                    var respuesta = new RespuestaGenerica<string>
                    {
                        Exito = true,
                        Mensaje = "Ticket generado con éxito."
                    };
                    TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                    return RedirectToAction("Index", conto);
                }
                else
                {
                    ModelState.AddModelError("", "Error al crear el ticket.");
                    var respuesta = new RespuestaGenerica<string>
                    {
                        Exito = false,
                        Mensaje = "Error al generar el ticket."
                    };
                    TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                    return RedirectToAction("Index", conto);

                }
            }
            else
            {
                var conto = (usuario.RolNombre == Roles.Usuario) ? "TicketsUsuario" : "TicketsSoporte";

                var respuesta = new RespuestaGenerica<string>
                {
                    Exito = false,
                    Mensaje = "Error al generar el ticket."
                };
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);

                return RedirectToAction("Index",conto);
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
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            if (idUsuario == "auto")
            {
                
                idUsuario = usuario.Id;
            }

            var result = await _ticketService.Asignar(idTicket, idUsuario,usuario.Id);
            if (result)
            {
                var respuesta = new RespuestaGenerica<int>
                {
                    Exito = true,
                    Datos = ticket.IdTicket,
                    Mensaje = "Ticket asignado exitosamente.",
                    Llamada = "mostrarDetallesTicket"
                };
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);

                if(usuario.RolNombre == Roles.Usuario || usuario.RolNombre == Roles.Soporte)
                {
                    return RedirectToAction("Abiertos", "TicketsUsuario");
                }
                else
                {
                    return RedirectToAction("Abiertos", "TicketsSoporte");
                }
                
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

        [HttpPost]
        public async Task<IActionResult> GuardarRespuesta(TicketRespuestaViewModel ticketRespuesta)
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);

            ticketRespuesta.IdUsuario = usuario.Id;
            if (usuario.RolNombre == Roles.Usuario)
            {
                ticketRespuesta.DelSolicitante = true;
            }

            var exito = await _ticketRespuestaService.Guardar(ticketRespuesta);

            if (exito)
            {
                //cambiamos estado si está abierto
                var ticket = await _ticketService.ObtenerPorId(ticketRespuesta.IdTicket);
                if (ticket != null && (ticket.IdEstadoTicket == EstadoTicket.Abierto || ticket.IdEstadoTicket == EstadoTicket.Cerrado))
                {
                    await _ticketService.CambiarEstado(ticket.IdTicket, EstadoTicket.Pendiente, usuario.Id);
                }

                var respuesta = new RespuestaGenerica<int>
                {
                    Exito = true,
                    Mensaje = "Respuesta guardada exitosamente.",
                    Llamada = "mostrarDetallesTicket",
                    Datos = ticketRespuesta.IdTicket
                };
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                if (usuario.RolNombre == Roles.Usuario || usuario.RolNombre == Roles.Soporte)
                {
                    return RedirectToAction("Respuestas", "TicketsUsuario");
                }
                else
                {
                    return RedirectToAction("Pendientes", "TicketsSoporte");
                }

            }
            else
            {
                var respuesta = new RespuestaGenerica<int>
                {
                    Exito = false,
                    Mensaje = "Error al guardar la respuesta.",
                    Llamada = "mostrarDetallesTicket",
                    Datos = ticketRespuesta.IdTicket
                };
                TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);
                return RedirectToAction("Index", "TicketsSoporte");
            }

        }

        public async Task<IActionResult> Adjunto(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                return NotFound();

            var adjunto = await _ticketAdjuntoService.ObtenerPorHash(hash);

            if (adjunto == null || adjunto.Archivo == null)
                return NotFound();

            // Devolver el archivo para descarga
            return File(adjunto.Archivo, adjunto.TipoMime ?? "application/octet-stream", adjunto.Nombre ?? "archivo");
        }
        public async Task<IActionResult> AdjuntoRespuesta(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                return NotFound();

            var adjunto = await _ticketRespuestaAdjuntoService.ObtenerPorHash(hash);

            if (adjunto == null || adjunto.Archivo == null)
                return NotFound();

            // Devolver el archivo para descarga
            return File(adjunto.Archivo, adjunto.TipoMime ?? "application/octet-stream", adjunto.Nombre ?? "archivo");
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int idTicket, int idEstadoTicket)
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(User);
            var exito = await _ticketService.CambiarEstado(idTicket, idEstadoTicket, usuario.Id);
            var accion = idEstadoTicket switch {
                EstadoTicket.Abierto => "Abiertos",
                EstadoTicket.Pendiente => "Pendientes",
                EstadoTicket.Cerrado => "Cerrados",
                _ => "Index"
            };

            var respuesta = new RespuestaGenerica<string>
            {
                Exito = true,
                Mensaje = "Cambio de estatus realizado con éxito."
            };

            TempData["Notificacion"] = JsonSerializer.Serialize(respuesta);

            return RedirectToAction(accion, "TicketsSoporte");
        }
    }
}
