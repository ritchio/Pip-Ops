using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Usuario.Interfaces;
using HelpDesk.Services.Usuario.Services;
using Microsoft.AspNetCore.Mvc;
using OtpNet;

namespace HelpDesk.ViewComponents
{
    public class MostrarQRTOTPViewComponent(
        IUsuarioService usuarioService,
        IConfiguration appSettings
        ) :ViewComponent
    {
        private readonly IUsuarioService _usuarioService = usuarioService;
        private readonly IConfiguration _appSettings = appSettings;
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var usuario = await _usuarioService.ObtenerUsuarioActual(HttpContext.User);

            // Genera secreto si no existe
            if (!usuario.TOTPActivo)
            {
                var key = KeyGeneration.GenerateRandomKey(20);
                string base32Secret = Base32Encoding.ToString(key);
                usuario.ClaveTOTP = base32Secret;
                //Console.WriteLine($"Clave: {usuario.ClaveTOTP}");
                usuario.TOTPActivo = true; //se activa al generar el TOTP
                var respuesta = await _usuarioService.ActualizarTOTP(usuario);//se guarda en la BD
            }
            Console.WriteLine($"Clave recién creada: {usuario.ClaveTOTP}");
            var config = _appSettings.GetSection("Security:Totp");
            string issuer = config["Issuer"];
            string accountName = usuario.CorreoElectronico;
            string secret = usuario.ClaveTOTP;

            string otpauthUrl = $"otpauth://totp/{Uri.EscapeDataString(issuer)}:{Uri.EscapeDataString(accountName)}" +
                                $"?secret={secret}&issuer={Uri.EscapeDataString(issuer)}";

            // Genera QR
            var qrGenerator = new QRCoder.QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(otpauthUrl, QRCoder.QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCoder.PngByteQRCode(qrCodeData);
            byte[] qrCodeImage = qrCode.GetGraphic(20);

            var model = new TOTPViewModel
            {
                QRBase64 = Convert.ToBase64String(qrCodeImage),//para mostrar la imagen
                Clave = usuario.ClaveTOTP??string.Empty
            };

            return View(model);
        }
    }
}
