using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    public class Log_TicketAutorizacion
    {
       
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Accion { get; set; } = null!;

        public int NumeroTicket { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        public string NombreUsuario { get; set; }

    }
    

}

