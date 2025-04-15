using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class AccesoLoginViewModel
    {
        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido.")]
        public required string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        public required string Contrasena { get; set; }

        [Display(Name = "Recordarme")]
        public bool Recordar { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
