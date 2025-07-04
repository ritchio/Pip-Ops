using AutoMapper;
using HelpDesk.Authorization;
using HelpDesk.Constants;
using HelpDesk.Data;
using HelpDesk.Hubs;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Correo.Interfaces;
using HelpDesk.Services.Ticket.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore; 
using System.Linq.Expressions;
using System.Security.Claims;

namespace HelpDesk.Services.Ticket.Services
{
    public class TicketService(
        ApplicationDbContext _context,
        IMapper mapper,
        ITicketAdjuntoService ticketAdjuntoService,
        IHubContext<NotificacionHub> hubContext,
        ICorreoService correoService
        ) : ITicketService
    {
        private readonly ITicketAdjuntoService _ticketAdjuntoService = ticketAdjuntoService;
        private readonly ICorreoService _correoService = correoService;
        private readonly IHubContext<NotificacionHub> _hubContext = hubContext;
        private readonly IMapper _mapper = mapper;
        public async Task<bool> GuardarTicket(TicketViewModel ticketViewModel,CatUsuarioViewModel usuario)
        {
            try
            {
                if (ticketViewModel.IdTicket > 0)
                {
                    var ticket = _context.Doc_Ticket.FirstOrDefault(t => t.IdTicket == ticketViewModel.IdTicket);

                    if (ticket != null)
                    {
                        var esSolicitante = (ticket.IdUsuario == usuario.Id);
                       
                        if (esSolicitante)
                        {
                            var limpiarAutorizaciones = false;
                            ticket.Asunto = ticketViewModel.Asunto;
                            ticket.Cuerpo = ticketViewModel.Cuerpo;
                            ticket.CorreoElectronico = ticketViewModel.CorreoElectronico;
                            ticket.Telefono = ticketViewModel.Telefono;

                            var autorizaciones = _context.Doc_TicketAutorizacion.Where(au => au.IdTicket == ticket.IdTicket).ToList();
                            
                            //revisamo si necesita autorización
                            var areasAutorizacion = _context.Cat_Autorizacion
                                .Include(ra => ra.RelacionAreasAutorizan)
                                .Where(au => au.IdModuloClasificacion == ticketViewModel.IdModuloClasificacion)
                                .ToList();

                            if (ticket.IdModuloClasificacion != ticketViewModel.IdModuloClasificacion)
                            {
                               
                                ticket.IdEstadoTicket = (areasAutorizacion.Count > 0)? EstadoTicket.PorAutorizar : EstadoTicket.Nuevo;
                                limpiarAutorizaciones = true;
                                
                            }else if (ticket.IdEstadoTicket == EstadoTicket.Rechazado)
                            {
                                ticket.IdEstadoTicket = EstadoTicket.PorAutorizar;
                            }

                            ticket.IdTipoSoporte = ticketViewModel.IdTipoSoporte;
                            ticket.IdModuloClasificacion = ticketViewModel.IdModuloClasificacion;
                            //ticket.FechaCreacion = DateTime.Now;
                            _context.SaveChanges();

                            if (limpiarAutorizaciones)
                            {
                                //eliminamos todas las autorizaciones anteriores

                                if (autorizaciones.Any())
                                {
                                    _context.Doc_TicketAutorizacion.RemoveRange(autorizaciones);
                                    _context.SaveChanges();
                                }

                                //agregamos las nuevas autorizaciones
                                if (areasAutorizacion.Count > 0)
                                {
                                    foreach (var auto in areasAutorizacion)
                                    {
                                        var ticketAutorizacion = new Doc_TicketAutorizacion
                                        {
                                            IdTicket = ticketViewModel.IdTicket,
                                            IdArea = auto.IdArea,
                                            IdEstadoAutorizacion = EstadoAutorizacion.PorAutorizar
                                        };

                                        _context.Add(ticketAutorizacion);
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            else
                            {
                                //actualizamos las autorizaciones existentes
                                foreach (var aut in autorizaciones)
                                {
                                    aut.IdEstadoAutorizacion = EstadoAutorizacion.PorAutorizar;
                                    _context.Update(aut);
                                }
                                _context.SaveChanges();
                            }

                        }
                        else
                        {
                            var limpiarAutorizaciones = (ticket.IdModuloClasificacion != ticketViewModel.IdModuloClasificacion);
                            ticket.IdTipoSoporte = ticketViewModel.IdTipoSoporte;
                            ticket.IdModuloClasificacion = ticketViewModel.IdModuloClasificacion;
                            _context.SaveChanges();
                            if(limpiarAutorizaciones)
                            {
                                //eliminamos todas las autorizaciones anteriores
                                var autorizaciones = _context.Doc_TicketAutorizacion.Where(au => au.IdTicket == ticket.IdTicket).ToList();

                                //revisamo si necesita autorización
                                var areasAutorizacion = _context.Cat_Autorizacion
                                    .Include(ra => ra.RelacionAreasAutorizan)
                                    .Where(au => au.IdModuloClasificacion == ticketViewModel.IdModuloClasificacion)
                                    .ToList();
                                if (autorizaciones.Any())
                                {
                                    _context.Doc_TicketAutorizacion.RemoveRange(autorizaciones);
                                    _context.SaveChanges();
                                }

                                //agregamos las nuevas autorizaciones
                                if (areasAutorizacion.Count > 0)
                                {
                                    foreach (var auto in areasAutorizacion)
                                    {
                                        var ticketAutorizacion = new Doc_TicketAutorizacion
                                        {
                                            IdTicket = ticketViewModel.IdTicket,
                                            IdArea = auto.IdArea,
                                            IdEstadoAutorizacion = EstadoAutorizacion.PorAutorizar
                                        };

                                        _context.Add(ticketAutorizacion);
                                        _context.SaveChanges();
                                    }
                                }
                            }
                        }

                        //guardamos log
                        var logTicket = new Log_TicketActividad
                        {
                            Accion = AccionLogTicket.Editar,
                            NumeroTicket = ticketViewModel.IdTicket,
                            IdUsuario = usuario.Id
                        };
                        _context.Log_TicketActividad.Add(logTicket);
                        _context.SaveChanges();

                    }
                }
                else
                {
                    //guardamos el ticket
                    //revisamo si necesita autorización
                    var areasAutorizacion = _context.Cat_Autorizacion
                        .Include(ra => ra.RelacionAreasAutorizan)
                        .Where(au => au.IdModuloClasificacion == ticketViewModel.IdModuloClasificacion)
                        .ToList();

                    var estadoTicket = EstadoTicket.Nuevo;

                    if (areasAutorizacion.Count > 0)
                    {
                        estadoTicket = EstadoTicket.PorAutorizar;
                    }
                    //último número de acuerdo al usuario, no total RN-28
                    var nuevoNumero = ObtenerMaximoTicketPorUsuario(usuario.Id) + 1;
                    var nuevoTurno = ObtenerMaximoTurno() + 1;

                    var nuevoTicket = new Doc_Ticket
                    {
                        IdUsuario = usuario.Id,
                        Asunto = ticketViewModel.Asunto,
                        Cuerpo = ticketViewModel.Cuerpo,
                        CorreoElectronico = ticketViewModel.CorreoElectronico,
                        Telefono = ticketViewModel.Telefono,
                        IdTipoSoporte = ticketViewModel.IdTipoSoporte,
                        IdModuloClasificacion = ticketViewModel.IdModuloClasificacion,
                        IdEstadoTicket = estadoTicket,
                        Numero = nuevoNumero,//
                        Turno = nuevoTurno,// temporal RN-30
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
                        IdUsuario = usuario.Id
                    };
                    _context.Log_TicketActividad.Add(logTicket);
                    _context.SaveChanges();

                    //guardamos las autorizaciones 

                    if (areasAutorizacion.Count > 0)
                    {
                        foreach (var auto in areasAutorizacion)
                        {
                            var ticketAutorizacion = new Doc_TicketAutorizacion
                            {
                                IdTicket = ticketViewModel.IdTicket,
                                IdArea = auto.IdArea,
                                IdEstadoAutorizacion = 1
                            };

                            _context.Add(ticketAutorizacion);
                            
                        }
                        _context.SaveChanges();
                    }
                }

                //sea cual sea el caso, se guarda el archivo si trae
                if (ticketViewModel.ArchivoAdjunto != null)
                {
                    ticketViewModel.IdUsuario = usuario.Id;
                    var result = await _ticketAdjuntoService.GuardarAdjunto(ticketViewModel);
                }

                return true;

            }
            catch (Exception ex)
            {

                throw new Exception("No se pudo guardar los cambios en el ticket",ex);
            }
        }

        public async Task<TicketViewModel> ObtenerPorId(int idTicket)
        {
            try
            {
                var ticket = await _context.Doc_Ticket
                    .Include(ad => ad.Adjuntos)
                    .Include(au => au.Autorizacion)
                    .ThenInclude(aru => aru.Area)
                    .Include(au => au.Autorizacion)
                    .ThenInclude(a => a.EstadoAutorizacion)
                    .Include(asi => asi.Asignacion)
                    .Include(ar => ar.Respuesta)
                    .ThenInclude(ra=>ra.Adjuntos)
                    .Include(u => u.Usuario)
                    .Include(mc=>mc.ModuloClasificacion)
                    .ThenInclude(ma => ma.Autorizaciones)
                    .ThenInclude(rau => rau.RelacionAreasAutorizan)
                    .ThenInclude(aa => aa.Area)
                    .FirstOrDefaultAsync(t => t.IdTicket == idTicket);

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

        public async Task<List<Cat_EstadoTicket>> ObtenerEstadosTicket()
        {
            try
            {
                var estados = await _context.Cat_EstadoTicket.ToListAsync();
                if (estados != null)
                {
                    return estados;
                }
                else
                {
                    throw new Exception("No se encontraron estados de ticket.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los estados de ticket: ", ex);
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

        private int ObtenerMaximoTurno()
        {
            return _context.Doc_Ticket
                .Select(t => (int?)t.Turno)
                .Max() ?? 0;
        }

        public async Task<bool> Validar(int idTicket, string idUsuario)
        {
            try
            {
                var ticket = await _context.Doc_Ticket.FirstOrDefaultAsync(t => t.IdTicket == idTicket);
                if (ticket != null)
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


            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar la atención del ticket", ex);
            }
        }

        public List<TicketsCantidadesPorEstadoViewModel> ObtenerCantidadesPorEstado(CatUsuarioViewModel usuario)
        {
            try
            {
                var esAgente = usuario.RolNombre != Roles.Usuario  ? 1 : 0;
                var param = new[]
                {
                    new SqlParameter("@IdUsuario", usuario.Id),
                    new SqlParameter("@EsAgente", esAgente)
                };

                var cantidades = _context.Sp_TicketsCantidadesPorEstado
                    .FromSqlRaw("EXEC sp_TicketsCantidadesPorEstado @IdUsuario = {0}, @EsAgente = {1}", param)
                    .ToList();
                if (cantidades != null)
                {
                    return _mapper.Map<List<TicketsCantidadesPorEstadoViewModel>>(cantidades);
                }
                else
                {
                    throw new Exception("No se encontraron tickets.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las cantidades: ", ex);
            }
        }

        public async Task<bool> Asignar(int idTicket, string idUsuario, string idUsuarioActual)
        {
            try
            {
                var ticket = await _context.Doc_Ticket
                    .Include(t => t.Usuario)
                    .Include(t => t.Asignacion)
                    .ThenInclude(a => a.Usuario)
                    .FirstOrDefaultAsync(t => t.IdTicket == idTicket);
                if (ticket != null)
                {

                    //cambio de estado al ticket
                    ticket.IdEstadoTicket = EstadoTicket.Abierto;
                    _context.Update(ticket);

                    //quitamos la asignación previa si existe
                    var asignacionExistente = _context.Doc_TicketAsignacion.Where(a => a.IdTicket == idTicket && a.Activo == true);
                    if(asignacionExistente.Any())
                    {
                        foreach (var asignacion in asignacionExistente)
                        {
                            asignacion.Activo = false; // desactivamos la asignación previa
                        }
                    }
                    //asignación del ticket
                    var ticketAsignacion = new Doc_TicketAsignacion
                    {
                        IdTicket = idTicket,
                        IdUsuario = idUsuario,
                        FechaAsignacion = DateTime.Now
                    };
                    _context.Doc_TicketAsignacion.Add(ticketAsignacion);

                    //correo
                    var asunto = "Centro de soporte";
                    var cuerpo = $"Estimado/a {ticket.Usuario.NombreCompleto}<br><br>"
                        + $"El ticket #{idTicket} será atendido por el agente {ticket.Asignacion.FirstOrDefault(a => a.Activo == true).Usuario.NombreCompleto}"
                        + " para su pronta atención."
                        + "<br><br>Para seguimiento ingresar al ticket en la página."
                        + "<br><br>No responder a este correo."
                        + $"<br>{DateTime.Now}";
                    var destinatario = ticket.Usuario.Email;

                    var correo = await _correoService.EnviarCorreoAsync(destinatario, asunto, cuerpo);

                    //log
                    var log = new Log_TicketActividad
                    {
                        Accion = (idUsuario == idUsuarioActual)? AccionLogTicket.Tomo : AccionLogTicket.Asignar,
                        NumeroTicket = idTicket,
                        IdUsuario = idUsuario
                    };
                    _context.Log_TicketActividad.Add(log);

                    await _context.SaveChangesAsync();

                    //actualizamos los turnos
                    await ActualizarTurnosPosteriores(idTicket, ticket.Turno??1);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al asignar el ticket", ex);
            }
        }

        public async Task<bool> CambiarEstado(int idTicket, int nuevoEstado, string idUsuario)
        {
            try
            {
                var ticket =  _context.Doc_Ticket
                    .Include(u => u.Usuario)
                    .Include(a => a.Asignacion)
                    .ThenInclude(a => a.Usuario)
                    .FirstOrDefault(t => t.IdTicket == idTicket);
                if (ticket != null)
                {
                    //cambio de estado al ticket
                    ticket.IdEstadoTicket = nuevoEstado;
                    if(nuevoEstado == EstadoTicket.Cerrado)
                    {
                        ticket.FechaCerrado = DateTime.Now;
                    }
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();

                    //mandamos correo al usuario cada cambio de estado
                    if(nuevoEstado == EstadoTicket.Validado)
                    {
                        var asunto = "Centro de soporte";
                        var cuerpo = $"Creado por: {ticket.Usuario.PrimerNombre} {ticket.Usuario.ApellidoPaterno}<br><br>"
                            + $"El ticket #{idTicket} {EstadoTicket.MensajeCorreo(nuevoEstado)}."
                            + "<br><br>No responder a este correo."
                            + $"<br>{DateTime.Now}";
                        var destinatario = ticket.Asignacion.FirstOrDefault(a => a.Activo == true).Usuario.Email;

                        var correo = await _correoService.EnviarCorreoAsync(destinatario, asunto, cuerpo);

                        await _hubContext.Clients.User(ticket.Asignacion.FirstOrDefault(a => a.Activo == true).Usuario.Id).SendAsync(
                            "RecibirNotificacion",
                            "Centro de Soporte",
                            $"El ticket #{idTicket} {EstadoTicket.MensajeCorreo(nuevoEstado)}."
                        );
                    }
                    else
                    {
                        var asunto = "Centro de soporte";
                        var cuerpo = $"Hola {ticket.Usuario.PrimerNombre} {ticket.Usuario.ApellidoPaterno}<br><br>"
                            + $"El estado del ticket #{idTicket} {EstadoTicket.MensajeCorreo(nuevoEstado)}."
                            + "<br><br>No responder a este correo."
                            + $"<br>{DateTime.Now}";
                        var destinatario = ticket.Usuario.Email;

                        var correo = await _correoService.EnviarCorreoAsync(destinatario, asunto, cuerpo);

                        await _hubContext.Clients.User(ticket.Usuario.Id).SendAsync(
                            "RecibirNotificacion",
                            "Centro de Soporte",
                            $"El estado del ticket #{idTicket} {EstadoTicket.MensajeCorreo(nuevoEstado)}."
                        );

                    }
                        

                    //log
                    if (nuevoEstado == EstadoTicket.Cerrado || nuevoEstado == EstadoTicket.Validado)
                    {
                        var log = new Log_TicketActividad
                        {
                            Accion = AccionLogTicket.AccionEstado(nuevoEstado),
                            NumeroTicket = idTicket,
                            IdUsuario = idUsuario
                        };
                        _context.Log_TicketActividad.Add(log);
                        _context.SaveChanges();
                        
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cambiar el estado del ticket", ex);
            }
        }

        #region [ Reportes ]
        public async Task<List<ReporteAgenteViewModel>> AtendidosPorAgente(DateTime fechaInicio, DateTime fechaFinal, string? idUsuario)
        {
            try
            {
              
                

                //traer los el total de tickets capturados
                var totalTickets = await _context.Doc_Ticket
                    .Where(t => t.FechaCreacion >= fechaInicio && t.FechaCreacion <= fechaFinal)
                    .CountAsync();

                var reporte = await _context.Cat_Usuario
                .Include(u => u.RolesUsuario)
                .ThenInclude(ru => ru.Rol)
                .Where(r => r.RolesUsuario.FirstOrDefault().Rol.Name != Roles.Usuario)
                .GroupJoin(
                    _context.Doc_Ticket
                        .Where(t => t.FechaCreacion >= fechaInicio && t.FechaCreacion <= fechaFinal),
                    user => user.Id,
                    ticket => ticket.IdUsuario,
                    (user, tickets) => new ReporteAgenteViewModel
                    {
                        IdUsuario = user.Id,
                        NombreAgente = user.PrimerNombre + " " +
                                       (user.SegundoNombre ?? "") + " " +
                                       user.ApellidoPaterno + " " +
                                       user.ApellidoMaterno,
                        Atendido = tickets.Count(),
                        Total = totalTickets 
                    }
                )
                .ToListAsync();



                return reporte;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el reporte de agentes", ex);
            }
        }

        public async Task<List<ReporteDetalladoViewModel>> ReporteDetallado(DateTime fechaInicio, DateTime fechaFinal)
        {
            try
            {
                var inicioParam = new SqlParameter("@FechaInicio", fechaInicio);
                var finParam = new SqlParameter("@FechaFin", fechaFinal);

                var resultado = await _context.Sp_ReporteDetallado
                    .FromSqlRaw("EXEC dbo.Sp_ReporteDetalladoPorPeriodo @FechaInicio, @FechaFin", inicioParam, finParam)
                    .ToListAsync();

                return resultado;

            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el reporte detallado", ex);
            }
        }
        #endregion [ Reportes ]

        private async Task<bool> ActualizarTurnosPosteriores(int idTicket, int turnoActual)
        {



            var ticketsPosteriores = await _context.Doc_Ticket
                .Where(t => t.Turno >= turnoActual && t.IdEstadoTicket == EstadoTicket.Nuevo && t.IdTicket != idTicket)
                .OrderBy(t => t.Turno)
                .ToListAsync();

            // 3. Recorrer y reasignar Turno
            
            foreach (var ticket in ticketsPosteriores)
            {
                ticket.Turno = turnoActual;
                turnoActual += 1;
            }

            // 4. Guardar cambios
            var exito = await _context.SaveChangesAsync();

            return true;
        }
    }
}
