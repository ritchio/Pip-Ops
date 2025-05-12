namespace HelpDesk.Models.ViewModels
{
    public class CatAutorizacionViewModel
    {
        public int IdAutorizacion { get; set; }
        public int IdArea { get; set; }
        public string NombreAreaPertenece { get; set; } = string.Empty;
        public int IdModuloClasificacion { get; set; }
        public string NombreModuloClasificacion { get; set; } = string.Empty;
        public string Requisitos { get; set; } = string.Empty;
        public List<int> IdAreasAutorizan { get; set; } = [];
        public List<CatAreaViewModel> AreasAutorizan { get; set; } = [];
        public List<CatAreaViewModel> CatalogoAreas { get; set; } = [];
        public List<CatModuloClasificacionViewModel> CatalogoModulosClasificaciones { get; set; } = [];
    }
}
