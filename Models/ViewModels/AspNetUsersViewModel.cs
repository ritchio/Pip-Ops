using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class AspNetUsersViewModel
    {
        public string Id { get; set; }
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
        [Display(Name = "Fecha de Activación")]

        public DateTime? FechaActivacion { get; set; }
        [Display(Name = "Fecha de Inactivación")]

        public DateTime? FechaInactivacion { get; set; }

        // Relación con Plaza
        public int? IdPlaza { get; set; }
        public string? PlazaNombre { get; set; } // Campo opcional si necesitas mostrar el nombre de la plaza
    }
}
