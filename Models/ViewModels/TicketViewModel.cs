using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class TicketViewModel
    {
        public int IdTicket { get; set; }

        [Required]
        public int IdTipoSoporte { get; set; }

        [Required]
        public int IdModuloClasificacion { get; set; }

        public string IdUsuario { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(50)]
        public string? CorreoElectronico { get; set; }

        [Phone]
        [MaxLength(10)]
        public string? Telefono { get; set; }

        [Required]
        [MaxLength(140)]
        public string Asunto { get; set; } = string.Empty;

        [Required]
        [MaxLength(1000)]
        public string Cuerpo { get; set; } = string.Empty;

        public IFormFile? ArchivoAdjunto { get; set; }

        // para mostrar
        public string NombreTipoSoporte { get; set; } = string.Empty;
        public string NombreModuloClasificacion { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;

        public string Requisitos { get; set; } = string.Empty; //este viene de autorizaciones

        

        // Relaciones
        public List<CatTipoSoporteViewModel> TiposSoporte { get; set; } = [];
        public List<CatModuloClasificacionViewModel> ModulosClasificaciones { get; set; } = [];
        public List<TicketAdjuntoViewModel> Adjuntos { get; set; } = [];
    }
}
