using HelpDesk.Constants;
using HelpDesk.Data;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Opcion.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.ViewComponents
{
    public class DescargaAplicacionViewComponent(ApplicationDbContext context,
        IOpcionService opcionesService) : ViewComponent
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IOpcionService _opcionesService = opcionesService;

        public async Task<IViewComponentResult> InvokeAsync(bool login = false)
        {
            var mensaje = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.DescargaAplicacion.Mensaje);
            var enlace = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.DescargaAplicacion.Enlace);
            var contrasenia = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.DescargaAplicacion.Contrasenia);

            var opciones = new OpcionesDescargaAplicacionViewModel
            {
                Mensaje = mensaje?.Valor,
                Enlace = enlace?.Valor,
                Contrasenia = contrasenia?.Valor,
                FechaModificacion = enlace?.FechaModificacion
            };

            return View(opciones);
        }
    }
}
