using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.Entities
{
    public class Cat_Autorizacion
    {
        [Key]
        public int IdAutorizacion { get; set; }
        public int IdArea { get; set; }
        public int IdModuloClasificacion { get; set; }
        public string Requisitos { get; set; }

        //relaciones
        public Cat_Area AreaPertenece { get; set; }
        public Cat_ModuloClasificacion ModuloClasificacion { get; set; }
        // Navegación
        public ICollection<Rel_Autorizacion_AreaAutoriza> RelacionAreasAutorizan { get; set; }
    }
}
