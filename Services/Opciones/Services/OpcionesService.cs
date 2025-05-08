using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Opciones.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Opciones.Services
{
    public class OpcionesService(ApplicationDbContext context) : IOpcionesService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Cfg_Opciones> ObtenerOpcionAsync(string clave)
        {
            return (await _context.Cfg_Opciones
                .Where(c => c.Clave == clave)
                .FirstOrDefaultAsync()??new Cfg_Opciones());
        }

        public async Task<List<Cfg_Opciones>> ObtenerTodasAsync()
        {
            return await _context.Cfg_Opciones
                .OrderBy(c => c.Clave)
                .ToListAsync();
        }

        public async Task ActualizarAsync(Cfg_Opciones config)
        {
            config.FechaModificacion = DateTime.Now;
            _context.Cfg_Opciones.Update(config);
            await _context.SaveChangesAsync();
        }
    }
}
