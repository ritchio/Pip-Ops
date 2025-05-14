using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Ticket.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace HelpDesk.Services.Ticket.Services
{
    public class TicketAdjuntoService(
        ApplicationDbContext _context
        ) : ITicketAdjuntoService
    {
        public Task<bool> EliminarAdjunto(int idAdjunto)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> GuardarAdjunto(TicketViewModel model)
        {
            if (model.ArchivoAdjunto == null || model.ArchivoAdjunto.Length == 0)
                return false;

            // Verificar que el tamaño del archivo sea menor a 1MB  
            const long maxFileSize = 1 * 1024 * 1024; // 1MB en bytes  
            if (model.ArchivoAdjunto.Length > maxFileSize)
                throw new Exception("El archivo excede el tamaño máximo permitido de 1MB.");

            try
            {
                using var memoryStream = new MemoryStream();
                await model.ArchivoAdjunto.CopyToAsync(memoryStream);
                var archivoBytes = memoryStream.ToArray();

                // Calcular el hash SHA-256  
                using var sha256 = SHA256.Create();
                var hashBytes = sha256.ComputeHash(archivoBytes);
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                var adjunto = new Doc_TicketAdjunto
                {
                    IdTicket = model.IdTicket,
                    Nombre = model.ArchivoAdjunto.FileName,
                    TipoMime = model.ArchivoAdjunto.ContentType,
                    Archivo = archivoBytes,
                    Hash = hashString,
                    IdUsuario = model.IdUsuario
                };

                _context.Doc_TicketAdjunto.Add(adjunto);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al guardar el adjunto", ex);
            }
        }

        public Task<TicketAdjuntoViewModel> ObtenerAdjuntoPorId(int idAdjunto)
        {
            throw new NotImplementedException();
        }

        public Task<List<TicketAdjuntoViewModel>> ObtenerAdjuntosPorTicketId(int idTicket)
        {
            throw new NotImplementedException();
        }
    }
}
