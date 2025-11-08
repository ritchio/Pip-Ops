using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.Entities
{
    public class Cat_EstadoAutorizacion
    {
        [Key]
        public int IdEstadoAutorizacion { get; set; }
        public required string Nombre { get; set; }
        public bool Activo { get; set; }

        //relaciones navegacion
        public virtual ICollection<Doc_TicketAutorizacion>? TicketAutorizacion { get; set; } = [];
    }
}
