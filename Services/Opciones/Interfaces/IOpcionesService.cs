using HelpDesk.Models.Entities;

namespace HelpDesk.Services.Opciones.Interfaces
{
    public interface IOpcionesService
    {
        Task<Cfg_Opciones> ObtenerOpcionAsync(string clave);
        Task<List<Cfg_Opciones>> ObtenerTodasAsync();
        Task ActualizarAsync(Cfg_Opciones config);
    }
}
