using AutoMapper;
using HelpDesk.Constants;
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

                    //último número de acuerdo al usuario, no total RN-30
                    var nuevoNumero = ObtenerMaximoTicketPorUsuario(ticketViewModel.IdUsuario) + 1;

                    var nuevoTicket = new Doc_Ticket
                    {
                        IdUsuario = ticketViewModel.IdUsuario,
                        Asunto = ticketViewModel.Asunto,
                        Cuerpo = ticketViewModel.Cuerpo,
                        CorreoElectronico = ticketViewModel.CorreoElectronico,
                        Telefono = ticketViewModel.Telefono,
                        IdTipoSoporte = ticketViewModel.IdTipoSoporte,
                        IdModuloClasificacion = ticketViewModel.IdModuloClasificacion,
                        IdEstadoTicket = EstadoTicket.Nuevo,
                        Numero = nuevoNumero,
                        Turno = nuevoNumero,// temporal RN-30
                        FechaCreacion = DateTime.Now
                    };
                    _context.Doc_Ticket.Add(nuevoTicket);
                    _context.SaveChanges();
                    ticketViewModel.IdTicket = nuevoTicket.IdTicket;

                    //guardamos log
                    var logTicket = new Log_TicketActividad
                    {
                        Accion = "CREÓ TICKET",
                        NumeroTicket = ticketViewModel.IdTicket,
                        IdUsuario = ticketViewModel.IdUsuario
                    };
                    _context.Log_TicketActividad.Add(logTicket);
                    _context.SaveChanges();

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
                    .ThenInclude(aru => aru.Area)
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
      

        public int ObtenerMaximoTicketPorUsuario(string idUsuario)
        {
            // Si no hay tickets para el usuario devuelve 0
            return _context.Doc_Ticket
                .Where(t => t.IdUsuario == idUsuario)
                .Select(t => (int?)t.Numero)
                .Max() ?? 0;
        }

        public async Task<bool> Validar(int idTicket,string idUsuario)
        {
            try
            {
                var ticket = await _context.Doc_Ticket.FirstOrDefaultAsync(t => t.IdTicket == idTicket);
                if(ticket != null)
                {
                    //ticket
                    ticket.IdEstadoTicket = EstadoTicket.Validado;
                    _context.Update(ticket);
                    _context.SaveChanges();

                    //log
                    var log = new Log_TicketActividad
                    {
                        Accion = AccionLogTicket.Validar,
                        NumeroTicket = idTicket,
                        IdUsuario = idUsuario
                    };
                    _context.Log_TicketActividad.Add(log);
                    _context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
                    

            }catch(Exception ex)
            {
                throw new Exception("Error al validar la atención del ticket", ex);
            }
        }
    }
}
