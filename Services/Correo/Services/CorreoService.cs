using HelpDesk.Services.Correo.Interfaces;
using System.Net;
using System.Net.Mail;

namespace HelpDesk.Services.Correo.Services
{
    public class CorreoService : ICorreoService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;

        public CorreoService(IConfiguration configuration)
        {
            var smtpConfig = configuration.GetSection("Smtp");
            _smtpServer = smtpConfig["Server"];
            _smtpPort = int.Parse(smtpConfig["Port"]);
            _smtpUser = smtpConfig["User"];
            _smtpPassword = smtpConfig["Password"];
        }

        public async Task<bool> EnviarCorreoAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true)
        {
            try
            {
                using (var cliente = new SmtpClient(_smtpServer, _smtpPort))
                {
                    cliente.Credentials = new NetworkCredential(_smtpUser, _smtpPassword);
                    //cliente.EnableSsl = true;

                    var mensaje = new MailMessage
                    {
                        From = new MailAddress(_smtpUser),
                        Subject = asunto,
                        Body = cuerpo,
                        IsBodyHtml = esHtml
                    };

                    mensaje.To.Add(destinatario);

                   await cliente.SendMailAsync(mensaje);
                }

                return true;
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
           
}
