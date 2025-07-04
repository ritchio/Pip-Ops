using AutoMapper;
using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Area.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Area.Services
{
    public class AreaService(
        ApplicationDbContext context,
        IMapper mapper) : IAreaService
    {
        #region [ Atributos ]
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        #endregion [ Atributos ]

        public async Task<List<CatAreaViewModel>> ObtenerAreasAsync()
        {
            try
            {
                var Areas = await _context.Cat_Area
                    .Include(a => a.Plaza)
                    .ToListAsync();
                return Areas.Select(p => new CatAreaViewModel
                {
                    IdArea = p.IdArea,
                    Nombre = p.Nombre,
                    Activo = p.Activo,
                    Estado = p.Activo? "Activo" : "Inactivo",
                    IdPlaza = p.IdPlaza,
                    NombrePlaza = p.Plaza?.Nombre
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
                Estado = Area.Activo? "Activo" : "Inactivo",
                IdPlaza = Area.IdPlaza
            };
        }

        public async Task<List<CatAreaViewModel>> ObtenerAreasPorPlazaAsync(int idPlaza)
        {
            try
            {
                var Areas = await _context.Cat_Area
                    .Where(a => a.IdPlaza == idPlaza)
                    .ToListAsync();
                return _mapper.Map<List<CatAreaViewModel>>(Areas);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las Areas por Plaza", ex);
            }
        }


        public async Task<RespuestaGenerica<bool>> CrearAreaAsync(Cat_Area Area)
        {
            var respuesta = new RespuestaGenerica<bool>();

            try
            {
                //revisamos si no existe un area con el mismo nombre en la misma plaza
                var areaExistente = await _context.Cat_Area
                    .FirstOrDefaultAsync(a => a.Nombre == Area.Nombre && a.IdPlaza == Area.IdPlaza);

                if (areaExistente != null)
                {
                    respuesta.Exito = false;
                    respuesta.Datos = false;
                    respuesta.Mensaje = "Ya existe un área con el mismo nombre en esta plaza.";
                }
                else
                {
                    var entity = new Cat_Area
                    {
                        Nombre = Area.Nombre,
                        Activo = true,
                        IdPlaza = Area.IdPlaza
                    };

                    _context.Cat_Area.Add(entity);
                    await _context.SaveChangesAsync();
                    
                    respuesta.Exito = true;
                    respuesta.Datos = true;
                    respuesta.Mensaje = "Área creada correctamente.";
                    
                }
                    
            }
            catch(Exception ex)
            {
                respuesta.Exito = false;
                respuesta.Datos = false;
                respuesta.Mensaje = "Error al crear el área: " + ex.Message;
            }

            return respuesta;
        }

        public async Task<RespuestaGenerica<bool>> ActualizarAreaAsync(Cat_Area Area)
        {
            var respuesta = new RespuestaGenerica<bool>();
            try
            {

                //revisamos si no existe un area con el mismo nombre en la misma plaza, excepto el area actual
                var areaExistente = await _context.Cat_Area
                    .FirstOrDefaultAsync(a => a.Nombre == Area.Nombre && a.IdPlaza == Area.IdPlaza && a.IdArea != Area.IdArea);
                if (areaExistente == null)
                {
                    var entity = await _context.Cat_Area.FindAsync(Area.IdArea);
                    if (entity == null)
                    {
                        respuesta.Exito = false;
                        respuesta.Datos = false;
                        respuesta.Mensaje = "Área no encontrada.";
                        return respuesta;
                    }

                    entity.Nombre = Area.Nombre;
                    entity.IdPlaza = Area.IdPlaza;

                    _context.Cat_Area.Update(entity);
                    await _context.SaveChangesAsync();

                    respuesta.Exito = true;
                    respuesta.Datos = true;
                    respuesta.Mensaje = "Área actualizada correctamente.";
                }
                else
                {
                    respuesta.Exito = false;
                    respuesta.Datos = false;
                    respuesta.Mensaje = "Ya existe un área con el mismo nombre en esta plaza.";

                }
            }
            catch(Exception ex)
            {
                respuesta.Exito = false;
                respuesta.Datos = false;
                respuesta.Mensaje = "Error al actualizar el área: " + ex.Message;
            }
           
            return respuesta;
        }

        public async Task<bool> CambiarEstadoAreaAsync(int id)
        {
            try
            {
                var entity = await _context.Cat_Area.FindAsync(id);
                if (entity == null) return false;

                entity.Activo = !entity.Activo;
                _context.Cat_Area.Update(entity);
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
