using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    public class Rel_UsuarioRol : IdentityUserRole<string>
    {
        public virtual Cat_Usuario Usuario { get; set; } = default!;
        public virtual IdentityRole Rol { get; set; } = default!;
    }

}
