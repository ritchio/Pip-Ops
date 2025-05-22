using AutoMapper;
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
        public List<TicketsNuevosViewModel> ObtenerNuevos()
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

        public List<TicketsPorAutorizarViewModel> ObtenerPorAutorizar()
        {
            try
            {
                var result = _context.Vis_TicketsPorAutorizar.ToList();

                return _mapper.Map<List<TicketsPorAutorizarViewModel>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets por autorizar" + ex);
            }
        }

        public List<TicketsRechazadosViewModel> ObtenerRechazados()
        {
            try
            {
                var result = _context.Vis_TicketsRechazados.ToList();

                return _mapper.Map<List<TicketsRechazadosViewModel>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets rechazados" + ex);
            }
        }
        public List<TicketsAbiertosViewModel> ObtenerAbiertos()
        {
            try
            {
                var result = _context.Vis_TicketsAbiertos.ToList();

                return _mapper.Map<List<TicketsAbiertosViewModel>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets abiertos" + ex);
            }
        }
        public List<TicketsPendientesViewModel> ObtenerPendientes()
        {
            try
            {
                var result = _context.Vis_TicketsPendientes.ToList();

                return _mapper.Map<List<TicketsPendientesViewModel>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets pendientes" + ex);
            }
        }
        public List<TicketsCerradosViewModel> ObtenerCerrados()
        {
            try
            {
                var result = _context.Vis_TicketsCerrados.ToList();

                return _mapper.Map<List<TicketsCerradosViewModel>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets cerrados" + ex);
            }
        }
        public List<TicketsValidadosViewModel> ObtenerValidados()
        {
            try
            {
                var result = _context.Vis_TicketsValidados.ToList();

                return _mapper.Map<List<TicketsValidadosViewModel>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets validados" + ex);
            }
        }
        public List<TicketsRespuestaViewModel> ObtenerRespuesta()
        {
            try
            {
                var result = _context.Vis_TicketsRespuesta.ToList();

                return _mapper.Map<List<TicketsRespuestaViewModel>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets respuesta" + ex);
            }
        }

       
    }
}
