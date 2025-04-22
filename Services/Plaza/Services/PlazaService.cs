using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Plaza.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Plaza.Services
{
    public class PlazaService : IPlazaService
    {
        #region [ Atributos ]
        private readonly ApplicationDbContext _context;
        #endregion [ Atributos ]

        public PlazaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CatPlazaViewModel>> ObtenerPlazasAsync()
        {
            try
            {
                var plazas = await _context.Cat_Plaza.ToListAsync();
                return plazas.Select(p => new CatPlazaViewModel
                {
                    IdPlaza = p.IdPlaza,
                    Nombre = p.Nombre,
                    Activo = p.Activo,
                    Estado = p.Activo? "Activo" : "Inactivo"
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las plazas", ex);
            }
        }

        public async Task<CatPlazaViewModel> ObtenerPlazaPorIdAsync(int id)
        {
            var plaza = await _context.Cat_Plaza.FindAsync(id);
            if (plaza == null) return null;

            return new CatPlazaViewModel
            {
                IdPlaza = plaza.IdPlaza,
                Nombre = plaza.Nombre,
                Activo = plaza.Activo,
                Estado = plaza.Activo? "Activo" : "Inactivo"
            };
        }

        public async Task<bool> CrearPlazaAsync(Cat_Plaza plaza)
        {
            try
            {
                var entity = new Cat_Plaza
                {
                    Nombre = plaza.Nombre,
                    Activo = true
                };

                _context.Cat_Plaza.Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActualizarPlazaAsync(Cat_Plaza plaza)
        {
            try
            {
                var entity = await _context.Cat_Plaza.FindAsync(plaza.IdPlaza);
                if (entity == null) return false;

                entity.Nombre = plaza.Nombre;

                _context.Cat_Plaza.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CambiarEstadoPlazaAsync(int id)
        {
            try
            {
                var entity = await _context.Cat_Plaza.FindAsync(id);
                if (entity == null) return false;
                entity.Activo = !entity.Activo;
                _context.Cat_Plaza.Update(entity);
                await _context.SaveChangesAsync();
                return entity.Activo;
            }
            catch
            {
                return false;
            }
        }
    }
}
