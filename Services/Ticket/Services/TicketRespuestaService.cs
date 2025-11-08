using HelpDesk.Constants;
using HelpDesk.Data;
using HelpDesk.Hubs;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Correo.Interfaces;
using HelpDesk.Services.Ticket.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using static HelpDesk.Authorization.Permisos;

namespace HelpDesk.Services.Ticket.Services
{
    public class TicketRespuestaService(
        ApplicationDbContext _context,
        ITicketRespuestaAdjuntoService ticketRespuestaAdjuntoService,
        IHubContext<NotificacionHub> hubContext,
        ICorreoService correoService
        ) : ITicketRespuestaService


    {
        private readonly ITicketRespuestaAdjuntoService _ticketRespuestaAdjuntoService = ticketRespuestaAdjuntoService;
        private readonly ICorreoService _correoService = correoService;
        private readonly IHubContext<NotificacionHub> _hubContext = hubContext;
        public async Task<bool> Guardar(TicketRespuestaViewModel respuesta)
        {
            try
            {
                var entidadRespuesta = new Doc_TicketRespuesta
                {
                    IdTicket = respuesta.IdTicket,
                    Mensaje = respuesta.Mensaje,
                    IdUsuario = respuesta.IdUsuario,
                    DelSolicitante = respuesta.DelSolicitante
                };

                _context.Doc_TicketRespuesta.Add(entidadRespuesta);
                _context.SaveChanges();

                respuesta.IdTicketRespuesta = entidadRespuesta.IdTicketRespuesta;
                //guardamos los adjuntos
                if (respuesta.ArchivoAdjunto != null && respuesta.ArchivoAdjunto.Length > 0)
                {
                    
                    await _ticketRespuestaAdjuntoService.GuardarAdjunto(respuesta);
                }


                var ticket = await _context.Doc_Ticket
                    .Include(t => t.Usuario)
                    .Include(a => a.Asignacion)
                    .ThenInclude(a => a.Usuario)
                    .FirstOrDefaultAsync(t => t.IdTicket == respuesta.IdTicket);

                //mandamos correo
                if (respuesta.DelSolicitante && ticket != null)
                {
                    var asunto = "Centro de soporte";
                    var cuerpo = $"Creado por: {ticket.Usuario.PrimerNombre} {ticket.Usuario.ApellidoPaterno}<br><br>"
                        + $"El ticket #{respuesta.IdTicket} ha recibido una respuesta."
                        + $"<br><br>Mensaje: {respuesta.Mensaje}."
                        + "<br><br>No responder a este correo."
                        + $"<br>{DateTime.Now}";

                    var agente = ticket.Asignacion.FirstOrDefault(u => u.Activo = true).Usuario;

                    var destinatario = agente.Email;

                    var correo = await _correoService.EnviarCorreoAsync(destinatario, asunto, cuerpo);

                    await _hubContext.Clients.User(agente.Id).SendAsync(
                          "RecibirNotificacion",
                          "Centro de Soporte",
                          $"El ticket #{respuesta.IdTicket} ha recibido una respuesta: {respuesta.Mensaje}"
                      );


                }
                else
                {
                    var asunto = "Centro de soporte";
                    var cuerpo = $"Hola {ticket.Usuario.PrimerNombre} {ticket.Usuario.ApellidoPaterno}<br><br>"
                        + $"El ticket #{respuesta.IdTicket} ha recibido una respuesta."
                        + $"<br><br>Mensaje: {respuesta.Mensaje}."
                        + "<br><br>No responder a este correo."
                        + $"<br>{DateTime.Now}";
                    var destinatario = ticket.Usuario.Email;

                    var correo = await _correoService.EnviarCorreoAsync(destinatario, asunto, cuerpo);

                    await _hubContext.Clients.User(ticket.Usuario.Id).SendAsync(
                          "RecibirNotificacion",
                          "Centro de Soporte",
                          $"El ticket #{respuesta.IdTicket} ha recibido una respuesta: {respuesta.Mensaje}"
                      );
                }

                //log 
                var log = new Log_TicketActividad
                {
                    Accion =  AccionLogTicket.Responder,
                    NumeroTicket = respuesta.IdTicket,
                    IdUsuario = respuesta.IdUsuario
                };
                _context.Log_TicketActividad.Add(log);

                _context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar la respuesta", ex);
            }
        }
    }
}
