using HelpDesk.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class CatModuloClasificacionViewModel
    {
        public int IdModuloClasificacion { get; set; }
        public int IdTipoSoporte { get; set; }
        public string Nombre { get; set; } = string.Empty; 
        public string NombreTipoSoporte { get; set; } = string.Empty;  
        public bool Activo { get; set; }
        public List<CatTipoSoporteViewModel> TiposSoporte { get; set; } = [];  
        public bool TodasAreas { get; set; } = false;
        public List<int> IdAreas { get; set; } = [];   
        public List<CatAreaViewModel> Areas { get; set; } = [];
        public string Estado => Activo ? "Activo" : "Inactivo";
    }
}
