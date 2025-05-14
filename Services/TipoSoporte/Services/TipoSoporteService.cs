using HelpDesk.Data;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.TipoSoporte.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.TipoSoporte.Services
{
    public class TipoSoporteService(
        ApplicationDbContext _context) : ITipoSoporteService
    {
        public async Task<List<CatTipoSoporteViewModel>> ObtenerTiposSoporteAsync()
        {
            try
            {
                var tiposSoporte = await _context.Cat_TipoSoporte
                    .Where(x => x.Activo)
                    .Select(x => new CatTipoSoporteViewModel
                    {
                        IdTipoSoporte = x.IdTipoSoporte,
                        Nombre = x.Nombre,
                        Activo = x.Activo,
                        Estado = x.Activo ? "Activo" : "Inactivo"
                    })
                    .ToListAsync();
                return tiposSoporte;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los tipos de soporte", ex);
            }
        }
    }
}
