using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Opcion.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Opcion.Services
{
    public class OpcionService(ApplicationDbContext context) : IOpcionService
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<Cfg_Opcion> ObtenerOpcionAsync(string clave)
        {
            return (await _context.Cfg_Opcion
                .Where(c => c.Clave == clave)
                .FirstOrDefaultAsync()??new Cfg_Opcion());
        }

        public async Task<List<Cfg_Opcion>> ObtenerTodasAsync()
        {
            return await _context.Cfg_Opcion
                .OrderBy(c => c.Clave)
                .ToListAsync();
        }

        public async Task ActualizarAsync(Cfg_Opcion config)
        {
            config.FechaModificacion = DateTime.Now;
            _context.Cfg_Opcion.Update(config);
            await _context.SaveChangesAsync();
        }
    }
}
