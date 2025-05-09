using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class LogTicketActividadViewModel
    {
        public int Id { get; set; }
        public string Accion { get; set; } = null!;
        public int NumeroTicket { get; set; }
        public DateTime Fecha { get; set; } = DateTime.Now;
        public string IdUsuario { get; set; } = null!;
        public string NombreUsuario { get; set; } = null!;
    }
}
