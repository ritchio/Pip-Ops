using HelpDesk.Constants;
using HelpDesk.Services.Opciones.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HelpDesk.ViewComponents
{
    public class FondoImagenViewComponent(IOpcionesService opcionesService) : ViewComponent
    {
        private readonly IOpcionesService _opcionesService = opcionesService;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var fondo = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.Fondo.Ruta);
            var version = await _opcionesService.ObtenerOpcionAsync(ConfiguracionOpciones.Fondo.Version);

            var ruta = fondo.Valor +"?"+ version.Valor;

            return View("Default", ruta);
        }
    }
}
