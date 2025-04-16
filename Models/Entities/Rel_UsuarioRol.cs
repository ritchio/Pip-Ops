using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    public class Rel_UsuarioRol : IdentityUserRole<string>
    {
        //[ForeignKey(nameof(UserId))]
        //public virtual Cat_Usuario Usuario { get; set; }

        //[ForeignKey(nameof(RoleId))]
        //public virtual Cat_Rol Rol { get; set; }
    }

}
