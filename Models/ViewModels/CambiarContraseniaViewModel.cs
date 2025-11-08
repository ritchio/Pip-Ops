using System.ComponentModel.DataAnnotations;

namespace HelpDesk.Models.ViewModels
{
    public class CambiarContraseniaViewModel
    {
        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Debe tener al menos 8 caracteres")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[#\.,\-_¿?¡!]).{8,}$",
        ErrorMessage = "La contraseña debe tener al menos una mayúscula, una minúscula, un número y un símbolo válido (#.,-_¿?¡!)")]
        public required string ContraseniaNueva { get; set; }

        [Required(ErrorMessage = "La confirmación de la contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [Compare("ContraseniaNueva", ErrorMessage = "Las contraseñas no coinciden")]
        public required string ContraseniaConfirmada { get; set; }
    }
}
