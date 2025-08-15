using HelpDesk.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class TicketViewModel
    {
        public int IdTicket { get; set; }
        public int Turno { get; set; }
        public int Numero { get; set; }

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

        public int IdEstadoTicket { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Cuerpo { get; set; } = string.Empty;

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaValidado { get; set; }
        public DateTime FechaCerrado { get; set; }

        public IFormFile? ArchivoAdjunto { get; set; }

        // para mostrar
        public string NombreTipoSoporte { get; set; } = string.Empty;
        public string NombreModuloClasificacion { get; set; } = string.Empty;
        public string NombreUsuario { get; set; } = string.Empty;
        public CatUsuarioViewModel? Usuario { get; set; } 
        public string Requisitos { get; set; } = string.Empty; //este viene de autorizaciones

        public bool EsAgente { get; set; } = false; //para que en la vista sepamos si el usuario actual es el agente asignado
        public bool EsSolicitante { get; set; } = false; //para que en la vista sepamos si el usuario actual es el solicitante

        // Relaciones
        public List<Cat_EstadoTicket> EstadosTicket { get; set; } = [];
        public List<CatTipoSoporteViewModel> TiposSoporte { get; set; } = [];
        public List<CatModuloClasificacionViewModel> ModulosClasificaciones { get; set; } = [];
        public List<TicketAdjuntoViewModel> Adjuntos { get; set; } = [];
        public List<TicketRespuestaViewModel> Respuestas { get; set; } = [];
        public List<TicketAutorizacionViewModel> Autorizaciones { get; set; } = [];
        public List<Doc_TicketAsignacion>? Asignacion { get; set; }

        //misc
        public bool Editable { get; set; } = false;
    }
}
