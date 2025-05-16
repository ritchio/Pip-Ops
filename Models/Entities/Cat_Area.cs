using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    [Table("Cat_Area")]
    public class Cat_Area
    {
        [Key]
        public int IdArea { get; set; }

        public required string Nombre { get; set; }

        public bool Activo { get; set; }

        public required int IdPlaza { get; set; }

        [ForeignKey("IdPlaza")]
        public virtual Cat_Plaza? Plaza { get; set; }

        //navegacion
        //public virtual ICollection<Rel_UsuarioArea>? UsuarioAreas { get; set; }  

        public virtual ICollection<Cat_Autorizacion>? Autorizaciones { get; set; }
        public virtual ICollection<Rel_ModuloClasificacion_Area>? ModuloClasificacionAreas { get; set; }
        public virtual ICollection<Rel_Autorizacion_AreaAutoriza>? AutorizacionAreas { get; set; }
        public virtual ICollection<Doc_TicketAutorizacion>? TicketAutorizacion { get; set; } 
    }

}
