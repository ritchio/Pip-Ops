using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.ModuloClasificacion.Interfaces;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace HelpDesk.Services.ModuloClasificacion.Services
{
    public class ModuloClasificacionService(
        ApplicationDbContext context
        ) : IModuloClasificacionService
    {
        private readonly ApplicationDbContext _context = context;


        public async Task<List<CatModuloClasificacionViewModel>> ObtenerModulosClasificacionesAsync()
        {
            try
            {
                var modulos = await _context.Cat_ModuloClasificacion
                    .Include(p => p.RelacionAreas)
                    .ThenInclude(ra => ra.Area)
                    .Include(p => p.TipoSoporte)
                    .Select(p => new CatModuloClasificacionViewModel
                    {
                        IdModuloClasificacion = p.IdModuloClasificacion,
                        Nombre = p.Nombre,
                        Activo = p.Activo,
                        IdAreas = p.RelacionAreas.Select(a => a.IdArea).ToList(),
                        Areas = p.RelacionAreas.Select(a => new CatAreaViewModel
                        {
                            IdArea = a.Area.IdArea,
                            Nombre = a.Area.Nombre,
                            Activo = a.Area.Activo,
                            IdPlaza = a.Area.IdPlaza,

                        }).ToList(),
                        IdTipoSoporte = p.IdTipoSoporte,
                        NombreTipoSoporte = p.TipoSoporte.Nombre
                    })
                    .ToListAsync();
                return modulos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los Modulos Clasificaciones", ex);
            }
        }
        public async Task<List<CatTipoSoporteViewModel>> ObtenerTiposSoporteAsync()
        {
            try
            {
                var tipos = await _context.Cat_TipoSoporte
                    .Select(p => new CatTipoSoporteViewModel
                    {
                        IdTipoSoporte = p.IdTipoSoporte,
                        Nombre = p.Nombre,
                        Activo = p.Activo,
                        Estado = p.Activo ? "Activo" : "Inactivo"
                    })
                    .ToListAsync();

                return tipos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public Task<bool> CrearModuloClasificacionAsync(CatModuloClasificacionViewModel moduloClasificacion)
        {
            //validamos si todas las areas estan seleccionadas
            if (moduloClasificacion.TodasAreas)
            {
                moduloClasificacion.IdAreas = _context.Cat_Area.Select(a => a.IdArea).ToList();
            }
            //convertimos el moduloClasificacion a Cat_ModuloClasificacion
            var nuevoModuloClasificacion = new Cat_ModuloClasificacion
            {
                Nombre = moduloClasificacion.Nombre,
                Activo = true,//albueys
                IdTipoSoporte = moduloClasificacion.IdTipoSoporte,
                RelacionAreas = moduloClasificacion.IdAreas.Select(a => new Rel_ModuloClasificacion_Area
                {
                    IdArea = a
                }).ToList()
            };

            try
            {
                _context.Cat_ModuloClasificacion.Add(nuevoModuloClasificacion);
                _context.SaveChanges();
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el Modulo Clasificacion", ex);
            }
        }

        public RespuestaGenerica<CatModuloClasificacionViewModel> CambiarEstado(int idModulo)
        {
            var respuesta = new RespuestaGenerica<CatModuloClasificacionViewModel>();
            try
            {
                var modulo = _context.Cat_ModuloClasificacion.FirstOrDefault(p => p.IdModuloClasificacion == idModulo);
                if (modulo != null)
                {
                    modulo.Activo = !modulo.Activo;
                    _context.SaveChanges();
                    respuesta.Datos = new CatModuloClasificacionViewModel
                    {
                        IdModuloClasificacion = modulo.IdModuloClasificacion,
                        Nombre = modulo.Nombre,
                        Activo = modulo.Activo,
                        IdTipoSoporte = modulo.IdTipoSoporte,
                        NombreTipoSoporte = string.Empty
                    };
                    respuesta.Exito = true;
                    respuesta.Mensaje = "El estado del modulo se ha cambiado correctamente";
                }
                else
                {
                    respuesta.Exito = false;
                    respuesta.Mensaje = "El modulo no existe";
                }
            }
            catch (Exception ex)
            {
                respuesta.Exito = false;
                respuesta.Mensaje = "Error al cambiar el estado del modulo, " + ex.Message;
            }
            return respuesta;
        }

        public Task<CatModuloClasificacionViewModel> ObtenerModuloClasificacionPorIdAsync(int idModulo)
        {
            try
            {
                var modulo = _context.Cat_ModuloClasificacion
                    .Include(p => p.RelacionAreas)
                    .ThenInclude(ra => ra.Area)
                    .Include(p => p.TipoSoporte)
                    .Select(p => new CatModuloClasificacionViewModel
                    {
                        IdModuloClasificacion = p.IdModuloClasificacion,
                        Nombre = p.Nombre,
                        Activo = p.Activo,
                        IdAreas = p.RelacionAreas.Select(a => a.IdArea).ToList(),
                        Areas = new List<CatAreaViewModel>(),
                        IdTipoSoporte = p.IdTipoSoporte,
                        NombreTipoSoporte = p.TipoSoporte.Nombre
                    })
                    .FirstOrDefault(p => p.IdModuloClasificacion == idModulo);

                if (modulo != null)
                {
                    return Task.FromResult(modulo);
                }
                else
                {
                    return Task.FromResult(new CatModuloClasificacionViewModel());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el Modulo Clasificacion", ex);
            }
        }

        public CatModuloClasificacionViewModel ModificarModuloClasificacion(CatModuloClasificacionViewModel moduloClasificacion)
        {
            try
            {
                var modulo = _context.Cat_ModuloClasificacion
                    .Include(p => p.RelacionAreas)
                    .ThenInclude(ra => ra.Area)
                    .FirstOrDefault(p => p.IdModuloClasificacion == moduloClasificacion.IdModuloClasificacion);
                if (modulo != null)
                {
                    modulo.Nombre = moduloClasificacion.Nombre;
                    modulo.Activo = modulo.Activo;
                    modulo.IdTipoSoporte = moduloClasificacion.IdTipoSoporte;
                    // Actualizar las áreas relacionadas
                    modulo.RelacionAreas.Clear();
                    if (moduloClasificacion.TodasAreas)
                    {
                        modulo.RelacionAreas = _context.Cat_Area.Select(a => new Rel_ModuloClasificacion_Area
                        {
                            IdArea = a.IdArea
                        }).ToList();
                    }
                    else
                    {
                        modulo.RelacionAreas = moduloClasificacion.IdAreas.Select(a => new Rel_ModuloClasificacion_Area
                        {
                            IdArea = a
                        }).ToList();
                    }
                    _context.SaveChanges();
                }
                return moduloClasificacion;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar el Modulo Clasificacion", ex);
            }
        }
    }
}
