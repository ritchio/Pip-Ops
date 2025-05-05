namespace HelpDesk.Services.Correo.Interfaces
{
    public interface ICorreoService
    {
       Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true);
    }
}
