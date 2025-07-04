namespace HelpDesk.Models.ViewModels
{
    public class TOTPViewModel
    {
        public string QRBase64 { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
    }
}
