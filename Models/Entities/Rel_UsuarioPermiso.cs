using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace HelpDesk.Models.Entities
{
    public class Rel_UsuarioPermiso : IdentityUserClaim<string>
    {
        //[ForeignKey("UserId")]
        //public Cat_Usuario Usuario { get; set; }
    }

}
