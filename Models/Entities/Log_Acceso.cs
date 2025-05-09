using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.Entities
{
    public class Log_Acceso
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Accion { get; set; } = null!; 

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("Usuario")]
        public string IdUsuario { get; set; } = null!;

        public Cat_Usuario Usuario { get; set; } = null!;
    }
}
