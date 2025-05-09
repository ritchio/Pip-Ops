using HelpDesk.Models.Entities;

namespace HelpDesk.Services.Opcion.Interfaces
{
    public interface IOpcionService
    {
        Task<Cfg_Opcion> ObtenerOpcionAsync(string clave);
        Task<List<Cfg_Opcion>> ObtenerTodasAsync();
        Task ActualizarAsync(Cfg_Opcion config);
    }
}
