using HelpDesk.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class CatAreaViewModel
    {
        public int IdArea{ get; set; }
        public required string Nombre { get; set; }
        public bool? Activo { get; set; }
        [Display(Name = "Estatus")]
        public string? Estado { get; set; }
        public required int IdPlaza { get; set; }

        public string? NombrePlaza { get; set; }

    }
}
