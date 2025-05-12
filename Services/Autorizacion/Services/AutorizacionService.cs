using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Autorizacion.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Autorizacion.Services
{
    public class AutorizacionService : IAutorizacionService
    {
        #region [ Atributos ]
        private readonly ApplicationDbContext _context;

        public AutorizacionService(ApplicationDbContext context)
        {
            _context = context;
        }
        #endregion [ Atributos ]

        #region [ Métodos ]
        public async Task<List<CatAutorizacionViewModel>> ObtenerAutorizacionesAsync()
        {
            try
            {
                var autorizaciones = await _context.Cat_Autorizacion
                    .Include(a => a.AreaPertenece)
                    .Include(a => a.ModuloClasificacion)
                    .Include(a => a.RelacionAreasAutorizan)
                        .ThenInclude(r => r.Area)
                    .Select(a => new CatAutorizacionViewModel
                    {
                        IdAutorizacion = a.IdAutorizacion,
                        IdArea = a.IdArea,
                        NombreAreaPertenece = a.AreaPertenece.Nombre,
                        NombreModuloClasificacion = a.ModuloClasificacion.Nombre,
                        IdModuloClasificacion = a.IdModuloClasificacion,
                        Requisitos = a.Requisitos,
                        IdAreasAutorizan = a.RelacionAreasAutorizan.Select(r => r.IdArea).ToList(),
                        AreasAutorizan = a.RelacionAreasAutorizan.Select(r => new CatAreaViewModel
                        {
                            IdPlaza = 0,
                            IdArea = r.Area.IdArea,
                            Nombre = r.Area.Nombre
                        }).ToList()
                    })
                    .ToListAsync();

                return autorizaciones;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las autorizaciones", ex);
            }
        }

        public async Task<CatAutorizacionViewModel> ObtenerAutorizacionPorIdAsync(int idAutorizacion)
        {
            try
            {
                var autorizacion = await _context.Cat_Autorizacion
                    .Include(a => a.AreaPertenece)
                    .Include(a => a.ModuloClasificacion)
                    .Include(a => a.RelacionAreasAutorizan)
                        .ThenInclude(r => r.Area)
                    .FirstOrDefaultAsync(a => a.IdAutorizacion == idAutorizacion);
                if (autorizacion == null)
                {
                    throw new Exception("No se encontró la autorización");
                }
                return new CatAutorizacionViewModel
                {
                    IdAutorizacion = autorizacion.IdAutorizacion,
                    IdArea = autorizacion.IdArea,
                    NombreAreaPertenece = autorizacion.AreaPertenece.Nombre,
                    NombreModuloClasificacion = autorizacion.ModuloClasificacion.Nombre,
                    IdModuloClasificacion = autorizacion.IdModuloClasificacion,
                    Requisitos = autorizacion.Requisitos,
                    IdAreasAutorizan = autorizacion.RelacionAreasAutorizan.Select(r => r.IdArea).ToList(),
                    AreasAutorizan = autorizacion.RelacionAreasAutorizan.Select(r => new CatAreaViewModel
                    {
                        IdPlaza = 0,
                        IdArea = r.Area.IdArea,
                        Nombre = r.Area.Nombre
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la autorización", ex);
            }
        }

        public async Task<bool> CrearAutorizacionAsync(CatAutorizacionViewModel autorizacion)
        {
            try
            {
                var catAuto = new Cat_Autorizacion
                {
                    IdModuloClasificacion = autorizacion.IdModuloClasificacion,
                    IdArea = autorizacion.IdArea,
                    Requisitos = autorizacion.Requisitos,
                    RelacionAreasAutorizan = autorizacion.IdAreasAutorizan
                        .Select(idArea => new Rel_Autorizacion_AreaAutoriza
                        {
                            IdArea = idArea,
                        }).ToList()
                };

                _context.Cat_Autorizacion.Add(catAuto);
                var exito = await _context.SaveChangesAsync();
                return exito > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la autorización", ex);
            }
        }

        public async Task<bool> ActualizarAutorizacionAsync(CatAutorizacionViewModel autorizacion)
        {
            try
            {
                var catAuto = await _context.Cat_Autorizacion
                    .Include(a => a.RelacionAreasAutorizan)
                    .FirstOrDefaultAsync(a => a.IdAutorizacion == autorizacion.IdAutorizacion);
                if (catAuto == null)
                {
                    throw new Exception("No se encontró la autorización");
                }
                catAuto.IdModuloClasificacion = autorizacion.IdModuloClasificacion;
                catAuto.IdArea = autorizacion.IdArea;
                catAuto.Requisitos = autorizacion.Requisitos;
                // Actualizar las áreas que autorizan
                catAuto.RelacionAreasAutorizan.Clear();
                foreach (var idArea in autorizacion.IdAreasAutorizan)
                {
                    catAuto.RelacionAreasAutorizan.Add(new Rel_Autorizacion_AreaAutoriza
                    {
                        IdArea = idArea,
                    });
                }
                _context.Cat_Autorizacion.Update(catAuto);
                var exito = await _context.SaveChangesAsync();
                return exito > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al editar la autorización", ex);
            }
        }
        #endregion [ Métodos ]
    }
}
