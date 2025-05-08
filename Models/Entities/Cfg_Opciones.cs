using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.Entities
{
    public class Cfg_Opciones
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Clave { get; set; } = string.Empty;

        public string? Valor { get; set; }

        [StringLength(255)]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaModificacion { get; set; } = DateTime.Now;
    }
}
