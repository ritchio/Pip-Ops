using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.Entities
{
    public class Cat_ModuloClasificacion
    {
        [Key]
        public int IdModuloClasificacion { get; set; }

        [Required]
        [ForeignKey("TipoSoporte")]
        public int IdTipoSoporte { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        public bool Activo { get; set; } = true;

        // Navegación
        public Cat_TipoSoporte TipoSoporte { get; set; }

        public ICollection<Rel_ModuloClasificacion_Area> RelacionAreas { get; set; }
        public ICollection<Cat_Autorizacion> Autorizaciones { get; set; }
        public ICollection<Doc_Ticket> Tickets { get; set; }
    }
}
