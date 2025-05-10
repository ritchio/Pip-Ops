using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    public class Rel_ModuloClasificacion_Area
    {
        public int IdModuloClasificacion { get; set; }

        public int IdArea { get; set; }

        // Navegación
        public Cat_ModuloClasificacion ModuloClasificacion { get; set; } = null!;
        public Cat_Area Area { get; set; }  = null!;
    }
}
