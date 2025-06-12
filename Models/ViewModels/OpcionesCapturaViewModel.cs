namespace HelpDesk.Models.ViewModels
{
    public class OpcionesCapturaViewModel
    {
        public OpcionesMensajeDelDiaViewModel MensajeDelDia { get; set; } = new OpcionesMensajeDelDiaViewModel();
        public OpcionesDescargaAplicacionViewModel DescargaAplicacion { get; set; } = new OpcionesDescargaAplicacionViewModel();
    }
}
