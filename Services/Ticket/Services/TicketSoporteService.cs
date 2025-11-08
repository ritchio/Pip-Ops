using AutoMapper;
using HelpDesk.Authorization;
using HelpDesk.Data;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Ticket.Interfaces;

namespace HelpDesk.Services.Ticket.Services
{
    public class TicketSoporteService(
        ApplicationDbContext _context,
        IMapper mapper
        ) : ITicketSoporteService
    {
        private readonly IMapper _mapper = mapper;
        public List<TicketsNuevosViewModel> ObtenerNuevos(CatUsuarioViewModel usuario)
        {
           

            try
            {
                var result = _context.Vis_TicketsNuevos.ToList();

                return _mapper.Map<List<TicketsNuevosViewModel>>(result);
            }catch(Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets nuevos"+ex);
            }

            
        }

        public List<TicketsPorAutorizarViewModel> ObtenerPorAutorizar(CatUsuarioViewModel usuario)
        {
            try
            {
                if (usuario.RolNombre == Roles.Soporte)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsPorAutorizar.Where(ta => ta.FechaSolicitud > fechaMinima).ToList();
                    return _mapper.Map<List<TicketsPorAutorizarViewModel>>(result);
                }
                else
                {
                    var result = _context.Vis_TicketsPorAutorizar.ToList();

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
                if (usuario.RolNombre == Roles.Soporte)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsRechazados.Where(ta => ta.FechaSolicitud > fechaMinima).ToList();
                    return _mapper.Map<List<TicketsRechazadosViewModel>>(result);
                }
                else
                {
                    var result = _context.Vis_TicketsRechazados.ToList();
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
                if (usuario.RolNombre == Roles.Soporte)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsAbiertos.Where(ta => ta.FechaSolicitud > fechaMinima).ToList();
                    return _mapper.Map<List<TicketsAbiertosViewModel>>(result);
                }
                else
                {
                    var result = _context.Vis_TicketsAbiertos.ToList();
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
                if (usuario.RolNombre == Roles.Soporte)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsPendientes.Where(ta => ta.FechaSolicitud > fechaMinima).ToList();
                    return _mapper.Map<List<TicketsPendientesViewModel>>(result);
                }
                else
                {
                    var result = _context.Vis_TicketsPendientes.ToList();
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
                if(usuario.RolNombre == Roles.Soporte)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsCerrados.Where(ta => ta.FechaSolicitud > fechaMinima).ToList();
                    return _mapper.Map<List<TicketsCerradosViewModel>>(result);
                }
                else
                {
                    var result = _context.Vis_TicketsCerrados.ToList();
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
                if (usuario.RolNombre == Roles.Soporte)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsValidados.Where(ta => ta.FechaSolicitud > fechaMinima).ToList();
                    return _mapper.Map<List<TicketsValidadosViewModel>>(result);
                }
                else
                {
                    var result = _context.Vis_TicketsValidados.ToList();
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
                if(usuario.RolNombre == Roles.Soporte)
                {
                    var fechaMinima = DateTime.Now.AddDays(-60);
                    var result = _context.Vis_TicketsRespuesta.Where(ta => ta.FechaSolicitud > fechaMinima).ToList();
                    return _mapper.Map<List<TicketsRespuestaViewModel>>(result);
                }
                else
                {
                    var result = _context.Vis_TicketsRespuesta.ToList();
                    return _mapper.Map<List<TicketsRespuestaViewModel>>(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets respuesta" + ex);
            }
        }

       
    }
}
