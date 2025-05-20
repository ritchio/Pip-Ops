using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class CatUsuarioViewModel
    {
        public string? Id { get; set; }
        public int? NumeroUsuario { get; set; }
        [Display(Name = "Correo Electrónico")]
        public required string CorreoElectronico { get; set; } = string.Empty;
        [Display(Name = "Primer Nombre")]
        public required string PrimerNombre { get; set; } = string.Empty;
        [Display(Name = "Segundo Nombre")]
        public string? SegundoNombre { get; set; }
        [Display(Name = "Apellido Paterno")]
        public required string ApellidoPaterno { get; set; } = string.Empty;
        [Display(Name = "Apellido Materno")]
        public string? ApellidoMaterno { get; set; }

        [Display(Name = "Nombre Completo")]
        public string? NombreCompleto => $"{PrimerNombre} {SegundoNombre} {ApellidoPaterno} {ApellidoMaterno}";

        public bool CambiarContrasenia { get; set; }

        // Estado del usuario
        public bool Activo { get; set; }
        [Display(Name = "Estado")]
        public string Estado => Activo? "Activo" : "Inactivo";

        [Display(Name = "Fecha de Activación")]
        public DateTime? FechaActivacion { get; set; }
        [Display(Name = "Fecha Inactivado")]
        public DateTime? FechaInactivacion { get; set; }

        // Relaciones
        //Plazas
        public int? IdPlaza { get; set; }
        [Display(Name = "Plaza")]
        public string? PlazaNombre { get; set; }
        public List<CatPlazaViewModel>? Plazas { get; set; }
        //Áreas
        public required List<int> IdAreas { get; set; } = [];
        [Display(Name = "Área")]
        public string? AreaNombre { get; set; }
        [Display(Name = "Área")]
        public List<CatAreaViewModel>? Areas { get; set; }

        //Roles
        [Display(Name = "Tipo de Usuario")]

        public required string IdRol { get; set; } = string.Empty;
        [Display(Name = "Tipo de Usuario")]
        public string? RolNombre { get; set; }

        public List<IdentityRole>? Roles { get; set; }
        
    }
}
