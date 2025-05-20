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
    }
}
