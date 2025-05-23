namespace HelpDesk.Models.Entities
{
    public class Sp_TicketsCantidadesPorEstado
    {
        public string Estado { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public string Contexto { get; set; } = string.Empty;

    }
}
