using AutoMapper;
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
        public List<TicketsNuevosViewModel> ObtenerNuevos(string idUsuario)
        {


            try
            {
                var result = _context.Vis_TicketsNuevos.Where(tn => tn.IdUsuario == idUsuario).ToList();

                return _mapper.Map<List<TicketsNuevosViewModel>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets por autorizar" + ex);
            }


        }

        public List<TicketsPorAutorizarViewModel> ObtenerPorAutorizar(string idUsuario)
        {
            try
            {
                var result = _context.Vis_TicketsPorAutorizar.Where(tn => tn.IdUsuario == idUsuario).ToList();

                return _mapper.Map<List<TicketsPorAutorizarViewModel>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets por autorizar" + ex);
            }
        }
        public List<TicketsRechazadosViewModel> ObtenerRechazados(string idUsuario)
        {
            try
            {
                var result = _context.Vis_TicketsRechazados.Where(tn => tn.IdUsuario == idUsuario).ToList();

                return _mapper.Map<List<TicketsRechazadosViewModel>>(result);
            }
            catch (Exception ex)
            {
                throw new Exception("No se puso obtener los registros de tickets rechazados" + ex);
            }
        }
    }
}
