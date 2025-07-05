using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Historial.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Historial.Services
{
    public class HistorialService(
        ApplicationDbContext context
        ) : IHistorialService
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<List<LogAccesoViewModel>> ObtenerAccesosAsync()
        {
            try
            {
                //obtenemos todos los registros de acceso
                var accesos = await _context.Log_Acceso
                .Include(u => u.Usuario)
                .OrderByDescending(a => a.Fecha)
                .Select(a => new LogAccesoViewModel
                {
                    Id = a.Id,
                    Fecha = a.Fecha,
                    NombreUsuario = a.Usuario.NombreCompleto,
                    Accion = a.Accion
                })
                .ToListAsync();

                return accesos;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los accesos", ex);
            }
        }
        public async Task<List<LogTicketActividadViewModel>> ObtenerTicketsActividadesAsync()
        {

            try
            {
                // Consulta de actividades de tickets
                var actividades = await _context.Log_TicketActividad
                    .Include(u => u.Usuario)
                    .OrderByDescending(a => a.Fecha)
                    .Select(t => new LogTicketActividadViewModel
                    {
                        Id = t.Id,
                        Fecha = t.Fecha,
                        NombreUsuario = t.Usuario.NombreCompleto,
                        Accion = t.Accion,
                        NumeroTicket = t.NumeroTicket,
                    })
                    .ToListAsync();

                // Consulta de logs de autorizaciones
                var autorizaciones = await _context.Log_TicketAutorizacion
                    .OrderByDescending(a => a.Fecha)
                    .Select(a => new LogTicketActividadViewModel
                    {
                        Id = a.Id,
                        Fecha = a.Fecha,
                        NombreUsuario = a.NombreUsuario,
                        Accion = a.Accion,
                        NumeroTicket = a.NumeroTicket,
                    })
                    .ToListAsync();

                // Unir ambas listas
                var resultado = actividades.Concat(autorizaciones)
                    .OrderByDescending(x => x.Fecha)
                    .ToList();

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las actividades", ex);
            }
        }
    }
}
