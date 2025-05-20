using AutoMapper;
using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Ticket.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Ticket.Services
{
    public class TicketService(
        ApplicationDbContext _context,
        IMapper mapper,
        ITicketAdjuntoService ticketAdjuntoService
        ) : ITicketService
    {
        private readonly ITicketAdjuntoService _ticketAdjuntoService = ticketAdjuntoService;
        private readonly IMapper _mapper = mapper;
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
                        //ticket.FechaCreacion = DateTime.Now;
                        _context.SaveChanges();
                    }
                }
                else
                {
                    //guardamos el ticket
                    var nuevoTicket = new Doc_Ticket
                    {
                        IdUsuario = ticketViewModel.IdUsuario,
                        Asunto = ticketViewModel.Asunto,
                        Cuerpo = ticketViewModel.Cuerpo,
                        CorreoElectronico = ticketViewModel.CorreoElectronico,
                        Telefono = ticketViewModel.Telefono,
                        IdTipoSoporte = ticketViewModel.IdTipoSoporte,
                        IdModuloClasificacion = ticketViewModel.IdModuloClasificacion,
                        IdEstadoTicket = 1,
                        FechaCreacion = DateTime.Now
                    };
                    _context.Doc_Ticket.Add(nuevoTicket);
                    _context.SaveChanges();
                    ticketViewModel.IdTicket = nuevoTicket.IdTicket;

                    //guardamos las autorizaciones 
                    var areasAutorizacion = _context.Cat_Autorizacion
                        .Include(ra => ra.RelacionAreasAutorizan)
                        .Where(au => au.IdModuloClasificacion == ticketViewModel.IdModuloClasificacion)
                        .ToList();
                    
                    if (areasAutorizacion.Any())
                    {
                        foreach(var auto in areasAutorizacion)
                        {
                            var ticketAutorizacion = new Doc_TicketAutorizacion
                            {
                                IdTicket = ticketViewModel.IdTicket,
                                IdArea = auto.IdArea,
                                IdEstadoAutorizacion = 1
                            };
                            
                            _context.Add(ticketAutorizacion);
                            _context.SaveChanges();
                        }
                    }
                }

                //sea cual sea el caso, se guarda el archivo si trae
                if (ticketViewModel.ArchivoAdjunto != null)
                {
                   
                    var result = await _ticketAdjuntoService.GuardarAdjunto(ticketViewModel);
                }

                return true;

            }
            catch(Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TicketViewModel> ObtenerPorId(int idTicket)
        {
            try
            {
                var ticket = _context.Doc_Ticket
                    .Include(ad => ad.Adjuntos)
                    .Include(au => au.Autorizacion)
                    .ThenInclude(aru=>aru.Area)
                    .Include(au => au.Autorizacion)
                    .ThenInclude(a => a.EstadoAutorizacion)
                    .Include(ar => ar.Respuesta)
                    .FirstOrDefault(t => t.IdTicket == idTicket);

                if (ticket != null)
                {
                    var ticketViewModel = _mapper.Map<TicketViewModel>(ticket);

                    return await Task.FromResult(ticketViewModel);
                }
                else
                {
                    throw new Exception("Ticket no encontrado");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el ticket", ex);
            }
        }
    }
}
