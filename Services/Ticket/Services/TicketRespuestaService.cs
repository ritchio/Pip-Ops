using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Ticket.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Ticket.Services
{
    public class TicketRespuestaService(
        ApplicationDbContext _context,
        ITicketRespuestaAdjuntoService ticketRespuestaAdjuntoService
        ) : ITicketRespuestaService


    {
        private readonly ITicketRespuestaAdjuntoService _ticketRespuestaAdjuntoService = ticketRespuestaAdjuntoService;
        public async Task<bool> Guardar(TicketRespuestaViewModel respuesta)
        {
            try
            {
                var entidadRespuesta = new Doc_TicketRespuesta
                {
                    IdTicket = respuesta.IdTicket,
                    Mensaje = respuesta.Mensaje,
                    IdUsuario = respuesta.IdUsuario
                };

                _context.Doc_TicketRespuesta.Add(entidadRespuesta);
                _context.SaveChanges();

                respuesta.IdTicketRespuesta = entidadRespuesta.IdTicketRespuesta;
                //guardamos los adjuntos
                if (respuesta.ArchivoAdjunto != null && respuesta.ArchivoAdjunto.Length > 0)
                {
                    
                    await _ticketRespuestaAdjuntoService.GuardarAdjunto(respuesta);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar la respuesta", ex);
            }
        }
    }
}
