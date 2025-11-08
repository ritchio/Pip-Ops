using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.Entities
{
    public class Cat_TipoSoporte
    {
        [Key]
        public int IdTipoSoporte { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }

        public bool Activo { get; set; } = true;

        // Navegación
        public ICollection<Cat_ModuloClasificacion> ModulosClasificacion { get; set; }
        public ICollection<Doc_Ticket> Tickets { get; set; }

    }
}
