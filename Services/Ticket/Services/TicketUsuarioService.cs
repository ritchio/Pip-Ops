using AutoMapper;
using HelpDesk.Authorization;
using HelpDesk.Data;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Ticket.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Ticket.Services
{
    [Authorize]
    public class TicketUsuarioService(
        ApplicationDbContext _context,
        IMapper mapper
        ) : ITicketUsuarioService
    {
        private readonly IMapper _mapper = mapper;
        public List<TicketsNuevosViewModel> ObtenerNuevos(CatUsuarioViewModel usuario)
        {


            try
            {
                if (usuario.RolNombre == Roles.Soporte || usuario.RolNombre == Roles.Usuario)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsNuevos.Where(tn => tn.FechaSolicitud > fechaMinima && tn.IdUsuario == usuario.Id).ToList();
                    return _mapper.Map<List<TicketsNuevosViewModel>>(result);
                }
                else
                {
                    var result = _context.Vis_TicketsNuevos.Where(tr => tr.IdUsuario == usuario.Id).ToList();
                    return _mapper.Map<List<TicketsNuevosViewModel>>(result);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets por autorizar" + ex);
            }


        }

        public List<TicketsPorAutorizarViewModel> ObtenerPorAutorizar(CatUsuarioViewModel usuario)
        {
            try
            {
                if (usuario.RolNombre == Roles.Soporte || usuario.RolNombre == Roles.Usuario)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsPorAutorizar.Where(ta => ta.FechaSolicitud > fechaMinima && ta.IdUsuario == usuario.Id).ToList();
                    return _mapper.Map<List<TicketsPorAutorizarViewModel>>(result);
                }
                else
                {
                    var result = _context.Vis_TicketsPorAutorizar.Where(tr => tr.IdUsuario == usuario.Id).ToList();
                    return _mapper.Map<List<TicketsPorAutorizarViewModel>>(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets por autorizar" + ex);
            }
        }
        public List<TicketsRechazadosViewModel> ObtenerRechazados(CatUsuarioViewModel usuario)
        {
            try
            {
                if(usuario.RolNombre == Roles.Soporte || usuario.RolNombre == Roles.Usuario)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsRechazados.Where(tn => tn.FechaSolicitud > fechaMinima && tn.IdUsuario == usuario.Id).ToList();
                    return _mapper.Map<List<TicketsRechazadosViewModel>>(result);
                }
                else
                {
                    var result = _context.Vis_TicketsRechazados.Where(tr => tr.IdUsuario == usuario.Id).ToList();
                    return _mapper.Map<List<TicketsRechazadosViewModel>>(result);
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets rechazados" + ex);
            }
        }

        public List<TicketsAbiertosViewModel> ObtenerAbiertos(CatUsuarioViewModel usuario)
        {
            try
            {
                if(usuario.RolNombre == Roles.Usuario)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsAbiertos
                        .Where(tn => tn.FechaSolicitud > fechaMinima && tn.IdUsuario == usuario.Id)
                        .ToList();

                    return _mapper.Map<List<TicketsAbiertosViewModel>>(result);
                }
                else if (usuario.RolNombre == Roles.Soporte)//en el caaaaso de que alguien deje de ser soporte y sea solo usuario
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsAbiertos
                        .Where(tn => tn.FechaSolicitud > fechaMinima &&
                                     (tn.IdUsuarioAgente == usuario.Id || tn.IdUsuario == usuario.Id))
                        .ToList();
                    return _mapper.Map<List<TicketsAbiertosViewModel>>(result);
                }
                else
                {
                    //admin sin restricción de fechas
                    var result = _context.Vis_TicketsAbiertos
                        .Where(tn => tn.IdUsuario == usuario.Id &&
                        (tn.IdUsuarioAgente == usuario.Id || tn.IdUsuario == usuario.Id))
                        .ToList();

                    return _mapper.Map<List<TicketsAbiertosViewModel>>(result);
                }
                    
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets abiertos" + ex);
            }
        }
        public List<TicketsPendientesViewModel> ObtenerPendientes(CatUsuarioViewModel usuario)
        {
            try
            {
                if (usuario.RolNombre == Roles.Usuario)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsPendientes
                        .Where(tn => tn.FechaSolicitud > fechaMinima && tn.IdUsuario == usuario.Id)
                        .ToList();

                    return _mapper.Map<List<TicketsPendientesViewModel>>(result);
                }
                else if (usuario.RolNombre == Roles.Soporte)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsPendientes
                        .Where(tn => tn.FechaSolicitud > fechaMinima &&
                                     (tn.IdUsuarioAgente == usuario.Id || tn.IdUsuario == usuario.Id))
                        .ToList();
                    return _mapper.Map<List<TicketsPendientesViewModel>>(result);
                }
                else
                {
                    // admin sin restricción de fechas
                    var result = _context.Vis_TicketsPendientes
                        .Where(tn => tn.IdUsuario == usuario.Id &&
                                     (tn.IdUsuarioAgente == usuario.Id || tn.IdUsuario == usuario.Id))
                        .ToList();

                    return _mapper.Map<List<TicketsPendientesViewModel>>(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets pendientes" + ex);
            }
        }
        public List<TicketsCerradosViewModel> ObtenerCerrados(CatUsuarioViewModel usuario)
        {
            try
            {

                if (usuario.RolNombre == Roles.Usuario)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsCerrados
                        .Where(tn => tn.FechaSolicitud > fechaMinima && tn.IdUsuario == usuario.Id)
                        .ToList();

                    return _mapper.Map<List<TicketsCerradosViewModel>>(result);
                }
                else if (usuario.RolNombre == Roles.Soporte)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsCerrados
                        .Where(tn => tn.FechaSolicitud > fechaMinima &&
                                     (tn.IdUsuarioAgente == usuario.Id || tn.IdUsuario == usuario.Id))
                        .ToList();
                    return _mapper.Map<List<TicketsCerradosViewModel>>(result);
                }
                else
                {
                    // admin sin restricción de fechas
                    var result = _context.Vis_TicketsCerrados
                        .Where(tn => tn.IdUsuario == usuario.Id &&
                                     (tn.IdUsuarioAgente == usuario.Id || tn.IdUsuario == usuario.Id))
                        .ToList();

                    return _mapper.Map<List<TicketsCerradosViewModel>>(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets cerrados" + ex);
            }
        }
        public List<TicketsValidadosViewModel> ObtenerValidados(CatUsuarioViewModel usuario)
        {
            try
            {

                if (usuario.RolNombre == Roles.Usuario)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsValidados
                        .Where(tn => tn.FechaSolicitud > fechaMinima && tn.IdUsuario == usuario.Id)
                        .ToList();

                    return _mapper.Map<List<TicketsValidadosViewModel>>(result);
                }
                else if (usuario.RolNombre == Roles.Soporte)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsValidados
                        .Where(tn => tn.FechaSolicitud > fechaMinima &&
                                     (tn.IdUsuarioAgente == usuario.Id || tn.IdUsuario == usuario.Id))
                        .ToList();
                    return _mapper.Map<List<TicketsValidadosViewModel>>(result);
                }
                else
                {
                    // admin sin restricción de fechas
                    var result = _context.Vis_TicketsValidados
                        .Where(tn => tn.IdUsuario == usuario.Id &&
                                     (tn.IdUsuarioAgente == usuario.Id || tn.IdUsuario == usuario.Id))
                        .ToList();

                    return _mapper.Map<List<TicketsValidadosViewModel>>(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets validados" + ex);
            }
        }
        public List<TicketsRespuestaViewModel> ObtenerRespuesta(CatUsuarioViewModel usuario)
        {
            try
            {

                if (usuario.RolNombre == Roles.Usuario)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsRespuesta
                        .Where(tn => tn.FechaSolicitud > fechaMinima && tn.IdUsuario == usuario.Id)
                        .ToList();

                    return _mapper.Map<List<TicketsRespuestaViewModel>>(result);
                }
                else if (usuario.RolNombre == Roles.Soporte)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsRespuesta
                        .Where(tn => tn.FechaSolicitud > fechaMinima &&
                                     (tn.IdUsuarioAgente == usuario.Id || tn.IdUsuario == usuario.Id))
                        .ToList();
                    return _mapper.Map<List<TicketsRespuestaViewModel>>(result);
                }
                else
                {
                    // admin sin restricción de fechas
                    var result = _context.Vis_TicketsRespuesta
                        .Where(tn => tn.IdUsuario == usuario.Id &&
                                     (tn.IdUsuarioAgente == usuario.Id || tn.IdUsuario == usuario.Id))
                        .ToList();

                    return _mapper.Map<List<TicketsRespuestaViewModel>>(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets Respuesta" + ex);
            }
        }
    }
}
