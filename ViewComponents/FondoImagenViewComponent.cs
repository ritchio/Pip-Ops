using HelpDesk.Constants;
using HelpDesk.Services.Opcion.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;

namespace HelpDesk.ViewComponents
{
    public class FondoImagenViewComponent(
        IOpcionService opcionesService
        ) : ViewComponent
    {
        private readonly IOpcionService _opcionesService = opcionesService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            var fondo = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.Fondo.Ruta);
            var version = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.Fondo.Version);

            var ruta = fondo.Valor +"?"+ version.Valor;

            return View("Default", ruta);
        }
    }
}
