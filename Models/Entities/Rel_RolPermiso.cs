using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    public class Rel_RolPermiso : IdentityRoleClaim<string>
    {
        //[ForeignKey("RoleId")]
        //public Cat_Rol Rol { get; set; }
    }

}
