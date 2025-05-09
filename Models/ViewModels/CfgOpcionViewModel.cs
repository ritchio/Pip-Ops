using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class CfgOpcionViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(100, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string Clave { get; set; } = string.Empty;
        public string? Valor { get; set; }
        [StringLength(255, ErrorMessage = "El campo {0} no puede tener más de {1} caracteres.")]
        public string? Descripcion { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaModificacion { get; set; } = DateTime.Now;
    }
}
