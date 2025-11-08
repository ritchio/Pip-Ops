using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.Entities
{
    public class Cat_EstadoTicket
    {
        [Key]
        public int IdEstadoTicket { get; set; }
        public required string Nombre { get; set; }
        public bool Activo { get; set; }

        //relaciones navegacion
        public virtual ICollection<Doc_Ticket>? Tickets { get; set; } = [];
    }
}
