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
                    .ThenInclude(pl => pl.Plaza)
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
                            Nombre = a.Area.Nombre + $" ({a.Area.Plaza.Nombre})",
                            Activo = a.Area.Activo,
                            IdPlaza = a.Area.IdPlaza,
                            NombrePlaza = a.Area.Plaza.Nombre

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
        
public async Task<List<CatModuloClasificacionViewModel>> ObtenerModulosClasificacionesPorTipoSoporteAsync(int idTipoSoporte)
        {
            try
            {
                var modulos = await _context.Cat_ModuloClasificacion
                    .Include(p => p.RelacionAreas)
                    .ThenInclude(ra => ra.Area)
                    .Include(p => p.TipoSoporte)
                    .Where(ts => ts.IdTipoSoporte == idTipoSoporte) 
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
                        NombreTipoSoporte = p.TipoSoporte.Nombre,
                        
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


        public async  Task<RespuestaGenerica<bool>> CrearModuloClasificacionAsync(CatModuloClasificacionViewModel moduloClasificacion)
        {
            var respuesta = new RespuestaGenerica<bool>();
            //verificamos si ya existe un módulo con el mismo nombre y tipo de soporte
            var moduloExistente = await _context.Cat_ModuloClasificacion
                .Where(mc => mc.Nombre.ToUpper() == moduloClasificacion.Nombre.ToUpper() 
                && mc.IdTipoSoporte == moduloClasificacion.IdTipoSoporte)
                .FirstOrDefaultAsync();

            if(moduloExistente == null)
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
                    
                    respuesta.Exito = true;
                    respuesta.Mensaje = "El módulo se ha creado con éxito";
                }
                catch (Exception ex)
                {
                    respuesta.Exito = false;
                    respuesta.Mensaje = "Error al crear el Modulo Clasificacion: " + ex.Message;
                }
            }
            else
            {
                respuesta.Exito = false;
                respuesta.Mensaje = "Ya existe un Modulo Clasificacion con el mismo nombre y tipo de soporte";
            }

            return respuesta;
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
                    respuesta.Mensaje = "El estatus del módulo cambió con éxito";
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

        public async Task<RespuestaGenerica<bool>> ModificarModuloClasificacion(CatModuloClasificacionViewModel moduloClasificacion)
        {
            var respuesta = new RespuestaGenerica<bool>();
            //verificamos si ya existe un módulo con el mismo nombre y tipo de soporte
            var moduloExistente = await _context.Cat_ModuloClasificacion
                .Where(mc => mc.Nombre.ToUpper() == moduloClasificacion.Nombre.ToUpper() 
                && mc.IdTipoSoporte == moduloClasificacion.IdTipoSoporte
                && mc.IdModuloClasificacion != moduloClasificacion.IdModuloClasificacion)
                .FirstOrDefaultAsync();
            if (moduloExistente == null)
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
                        respuesta.Exito = true;
                        respuesta.Datos = true;
                        respuesta.Mensaje = "El módulo se ha modificado con éxito";
                    }
                    else
                    {
                        respuesta.Exito = false;
                        respuesta.Mensaje = "El Modulo Clasificacion no existe";
                    }
                    
                }
                catch (Exception ex)
                {
                    respuesta.Exito = false;
                    respuesta.Mensaje = "Error al modificar el Modulo Clasificacion: " + ex.Message;
                }
            }
            else
            {
                respuesta.Exito = false;
                respuesta.Mensaje = "Ya existe un Modulo Clasificacion con el mismo nombre y tipo de soporte";
                
            }
            return respuesta;
        }
    }
}
