using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
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
                }).OrderBy(p=>p.Nombre).ToList();
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

        public async Task<RespuestaGenerica<bool>> CrearPlazaAsync(Cat_Plaza plaza)
        {
            var respuesta = new RespuestaGenerica<bool>();
            try
            {
                //revisamos si no hay una plaza con el mismo nombre
                var existingPlaza = await _context.Cat_Plaza
                    .FirstOrDefaultAsync(p => p.Nombre == plaza.Nombre);
                if (existingPlaza == null)
                {
                    var entity = new Cat_Plaza
                    {
                        Nombre = plaza.Nombre,
                        Activo = true
                    };

                    _context.Cat_Plaza.Add(entity);
                    await _context.SaveChangesAsync();

                    respuesta.Mensaje = "Plaza creada exitosamente.";
                    respuesta.Exito = true;
                    respuesta.Datos = true;
                    return respuesta;
                }
                else
                {
                    // Si ya existe una plaza con el mismo nombre, no se crea una nueva
                    respuesta.Exito = false;
                    respuesta.Mensaje = "Ya existe una plaza con el mismo nombre.";
                    respuesta.Datos = false;
                    return respuesta;
                }

                    
            }
            catch(Exception ex)
            {
                respuesta.Exito = false;
                respuesta.Mensaje = $"Error al crear la plaza: {ex.Message}";
                respuesta.Datos = false;
                return respuesta;
            }
           
        }

        public async Task<RespuestaGenerica<bool>> ActualizarPlazaAsync(Cat_Plaza plaza)
        {
            var respuesta = new RespuestaGenerica<bool>();
            try
            {
                //revisamos si hay alguna plaza diferente a la modificada que tenga el mismo nombre
                var plazaExistente = await _context.Cat_Plaza.Where(p => p.Nombre.ToUpper() == plaza.Nombre.ToUpper() && p.IdPlaza != plaza.IdPlaza).FirstOrDefaultAsync();

                if(plazaExistente != null)
                {
                    respuesta.Exito = false;
                    respuesta.Mensaje = "Ya existe una plaza con el mismo nombre.";
                }
                else
                {
                    var entity = await _context.Cat_Plaza.FindAsync(plaza.IdPlaza);
                    if (entity == null)
                    {
                        respuesta.Exito = false;
                        respuesta.Mensaje = "Plaza no encontrada.";
                    }

                    entity.Nombre = plaza.Nombre;

                    _context.Cat_Plaza.Update(entity);
                    await _context.SaveChangesAsync();
                    respuesta.Exito = true;
                   respuesta.Mensaje = "Plaza actualizada exitosamente.";
                }
                   
            }
            catch(Exception ex)
            {
                respuesta.Exito = false;
                respuesta.Mensaje = $"Error al actualizar la plaza: {ex.Message}";
            }

            return respuesta;
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
