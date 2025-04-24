using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class AspNetUsersViewModel
    {
        public string Id { get; set; }
        public int NumeroUsuario { get; set; }
        [Display(Name = "Correo Electrónico")]
        public required string CorreoElectronico { get; set; }
        [Display(Name = "Primer Nombre")]
        public required string PrimerNombre { get; set; }
        [Display(Name = "Segundo Nombre")]
        public string? SegundoNombre { get; set; }
        [Display(Name = "Apellido Paterno")]
        public required string ApellidoPaterno { get; set; }
        [Display(Name = "Apellido Materno")]
        public string? ApellidoMaterno { get; set; }

        [Display(Name = "Nombre Completo")]
        public string? NombreCompleto => $"{PrimerNombre} {SegundoNombre} {ApellidoPaterno} {ApellidoMaterno}";

        // Estado del usuario
        public bool? Activo { get; set; }
        [Display(Name = "Estado")]
        public string Estado => Activo.HasValue && Activo.Value ? "Activo" : "Inactivo";

        [Display(Name = "Fecha de Activación")]
        public DateTime? FechaActivacion { get; set; }
        [Display(Name = "Fecha Inactivado")]
        public DateTime? FechaInactivacion { get; set; }

        // Relaciones
        public int? IdPlaza { get; set; }
        [Display(Name = "Plaza")]
        public string? PlazaNombre { get; set; }
        [Display(Name = "Área")]
        public string? AreaNombre { get; set; }
        [Display(Name = "Tipo de Usuario")]
        public string? RolNombre { get; set; }
    }
}
