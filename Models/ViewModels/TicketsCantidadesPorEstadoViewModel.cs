namespace HelpDesk.Models.ViewModels
{
    public class TicketsCantidadesPorEstadoViewModel
    {

        public string Estado { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public string Contexto { get; set; } = string.Empty;

    }
}
