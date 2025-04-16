using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    public class Rel_UsuarioArea 
    {
        public string IdUsuario { get; set; }  // Relación con Cat_Usuario
        public int IdArea { get; set; }  // Relación con Cat_Area

        //[ForeignKey(nameof(IdUsuario))]
        //public virtual Cat_Usuario Usuario { get; set; }

        //[ForeignKey(nameof(IdArea))]
        //public virtual Cat_Area Area { get; set; }
    }
}
