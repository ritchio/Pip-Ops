using AutoMapper;
using HelpDesk.Authorization;
using HelpDesk.Services.Ticket.Interfaces;
using HelpDesk.Services.Usuario.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.ViewComponents
{
    public class TicketsPorAutorizarViewComponent(
        IUsuarioService usuarioService,
        ITicketSoporteService ticketSoporteService,
        ITicketUsuarioService ticketUsuarioService,
        IMapper mapper
        ) : ViewComponent
    {
        private readonly IUsuarioService _usuarioService = usuarioService;
        private readonly ITicketSoporteService _ticketSoporteService = ticketSoporteService;
        private readonly ITicketUsuarioService _ticketUsuarioService = ticketUsuarioService;
        private readonly IMapper _mapper = mapper;
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var usuario = await _usuarioService.ObtenerUsuarioActual(HttpContext.User);

            //if (usuario.RolNombre == Roles.Usuario)
            if(usuario.RolNombre == Roles.Soporte || usuario.RolNombre == Roles.Administrador || usuario.RolNombre == Roles.AdministradorSupervisor)
                {
                var respuesta = _ticketSoporteService.ObtenerPorAutorizar(usuario);
                return View(respuesta);
            }
            else
            {
                var respuesta = _ticketUsuarioService.ObtenerPorAutorizar(usuario);
                return View("Usuario",respuesta);
            }
      
        }
    }
}
