using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // Datos personales
        public required string PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public required string ApellidoPaterno { get; set; }
        public string? ApellidoMaterno { get; set; }
        // Estado
        public bool? Activo { get; set; }
        public DateTime? FechaActivacion { get; set; }
        public DateTime? FechaInactivacion { get; set; }

        // Relación con plaza
        public int? IdPlaza { get; set; }

        [ForeignKey("IdPlaza")]
        public virtual Cat_Plaza? Plaza { get; set; }
    }
}
