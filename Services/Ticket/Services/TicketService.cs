using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Ticket.Interfaces;

namespace HelpDesk.Services.Ticket.Services
{
    public class TicketService(
        ApplicationDbContext _context,
        ITicketAdjuntoService ticketAdjuntoService
        ) : ITicketService
    {
        private readonly ITicketAdjuntoService _ticketAdjuntoService = ticketAdjuntoService;
        public async Task<bool> GuardarTicket(TicketViewModel ticketViewModel)
        {
            try
            {
                if (ticketViewModel.IdTicket > 0)
                {
                    var ticket = _context.Doc_Ticket.FirstOrDefault(t => t.IdTicket == ticketViewModel.IdTicket);
                    if (ticket != null)
                    {
                        ticket.Asunto = ticketViewModel.Asunto;
                        ticket.Cuerpo = ticketViewModel.Cuerpo;
                        ticket.CorreoElectronico = ticketViewModel.CorreoElectronico;
                        ticket.Telefono = ticketViewModel.Telefono;
                        ticket.IdTipoSoporte = ticketViewModel.IdTipoSoporte;
                        ticket.IdModuloClasificacion = ticketViewModel.IdModuloClasificacion;
                        _context.SaveChanges();
                    }
                }
                else
                {
                    var nuevoTicket = new Doc_Ticket
                    {
                        IdUsuario = ticketViewModel.IdUsuario,
                        Asunto = ticketViewModel.Asunto,
                        Cuerpo = ticketViewModel.Cuerpo,
                        CorreoElectronico = ticketViewModel.CorreoElectronico,
                        Telefono = ticketViewModel.Telefono,
                        IdTipoSoporte = ticketViewModel.IdTipoSoporte,
                        IdModuloClasificacion = ticketViewModel.IdModuloClasificacion
                    };
                    _context.Doc_Ticket.Add(nuevoTicket);
                    _context.SaveChanges();
                    ticketViewModel.IdTicket = nuevoTicket.IdTicket;
                }

                //sea cual sea el caso, se guarda el archivo si trae
                if (ticketViewModel.ArchivoAdjunto != null)
                {
                   
                    var result = await _ticketAdjuntoService.GuardarAdjunto(ticketViewModel);
                }

                return true;

            }
            catch
            {

                return false;
            }
        }
    }
}
