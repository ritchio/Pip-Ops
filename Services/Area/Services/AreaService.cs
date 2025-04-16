using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Area.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Area.Services
{
    public class AreaService : IAreaService
    {
        #region [ Atributos ]
        private readonly ApplicationDbContext _context;
        #endregion [ Atributos ]

        public AreaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CatAreaViewModel>> ObtenerAreasAsync()
        {
            try
            {
                var Areas = await _context.Cat_Area.ToListAsync();
                return Areas.Select(p => new CatAreaViewModel
                {
                    IdArea = p.IdArea,
                    Nombre = p.Nombre,
                    Activo = p.Activo,
                    Estado = p.Activo.HasValue && p.Activo.Value ? "Activo" : "Inactivo"
                }).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las Areas", ex);
            }
        }

        public async Task<CatAreaViewModel> ObtenerAreaPorIdAsync(int id)
        {
            var Area = await _context.Cat_Area.FindAsync(id);
            if (Area == null) return null;

            return new CatAreaViewModel
            {
                IdArea = Area.IdArea,
                Nombre = Area.Nombre,
                Activo = Area.Activo,
                Estado = Area.Activo.HasValue && Area.Activo.Value ? "Activo" : "Inactivo"
            };
        }

        public async Task<bool> CrearAreaAsync(Cat_Area Area)
        {
            try
            {
                var entity = new Cat_Area
                {
                    Nombre = Area.Nombre,
                    Activo = Area.Activo
                };

                _context.Cat_Area.Add(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActualizarAreaAsync(Cat_Area Area)
        {
            try
            {
                var entity = await _context.Cat_Area.FindAsync(Area.IdArea);
                if (entity == null) return false;

                entity.Nombre = Area.Nombre;
                entity.Activo = Area.Activo;

                _context.Cat_Area.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarAreaAsync(int id)
        {
            try
            {
                var entity = await _context.Cat_Area.FindAsync(id);
                if (entity == null) return false;

                _context.Cat_Area.Remove(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
