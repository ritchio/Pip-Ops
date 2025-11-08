namespace HelpDesk.Models.Entities
{
    public class Rel_Autorizacion_AreaAutoriza
    {
        public int IdArea { get; set; }
        public int IdAutorizacion { get; set; }

        // Navegación
        public Cat_Autorizacion Autorizacion { get; set; } = null!;
        public Cat_Area Area { get; set; } = null!;
    }
}
