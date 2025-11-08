namespace HelpDesk.Models.ViewModels
{
    public class ReporteDetalladoViewModel
    {
        public int? IdTicket { get; set; }
        public int? Numero { get; set; }
        public string? NombreSolicitante { get; set; }
        public string? NombreAgente { get; set; }
        public string? Plaza { get; set; }
        public string? Asunto { get; set; }
        public string? Cuerpo { get; set; }
        public string? TipoSoporte { get; set; }
        public string? Modulo { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaCerrado { get; set; }
        public DateTime? FechaValidado { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public string? EstadoTicket { get; set; }
        public string? IdUsuario { get; set; }
        public string? IdUsuarioAgente { get; set; }
    }
}
