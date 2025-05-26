using HelpDesk.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class TicketRespuestaViewModel
    {
        public int IdTicketRespuesta { get; set; }
        public int IdTicket { get; set; }
        public string? Mensaje { get; set; }
        public string IdUsuario { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Now;
        public bool DelSolicitante { get; set; } = false;
        public IFormFile? ArchivoAdjunto { get; set; }
        public List<Doc_TicketRespuestaAdjunto>? Adjuntos { get; set; } = [];
    }
}
