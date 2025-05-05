using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    public class Cat_Usuario : IdentityUser
    {
        // Datos personales
        public int? NumeroUsuario { get; set; }
        public required string PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public required string ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        public bool CambiarContrasenia { get; set; }

        // Estado
        public bool Activo { get; set; }
        public DateTime? FechaActivacion { get; set; }
        public DateTime? FechaInactivacion { get; set; }

        // Relación con plaza
        public int IdPlaza { get; set; }

        [ForeignKey("IdPlaza")]
        public virtual Cat_Plaza Plaza { get; set; }

        // Relación con roles
        //public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; } = new List<IdentityUserRole<string>>();

        public virtual ICollection<Rel_UsuarioRol> RolesUsuario { get; set; } = new List<Rel_UsuarioRol>();

        //Areas
        public virtual ICollection<Rel_UsuarioArea> AreasUsuario { get; set; } = new List<Rel_UsuarioArea>();

    }

}
