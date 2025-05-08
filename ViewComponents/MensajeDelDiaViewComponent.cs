using HelpDesk.Constants;
using HelpDesk.Data;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Opciones.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.ViewComponents
{
    public class MensajeDelDiaViewComponent(
        ApplicationDbContext context,
        IOpcionesService opcionesService) : ViewComponent
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IOpcionesService _opcionesService = opcionesService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var mensaje = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.MensajeDelDia.Mensaje);
            var posicion = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.MensajeDelDia.Posicion);

            var opciones = new OpcionesMensajeDelDiaViewModel
            {
                MensajeDelDia = mensaje?.Valor,
                Posicion = posicion?.Valor,
                FechaModificacion = mensaje?.FechaModificacion
            };

            return View(opciones);
        }
    }
}
