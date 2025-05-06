using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using HelpDesk.Models.Entities;

namespace HelpDesk.Services.Identity.Services
{
    public class ApplicationUserClaimsPrincipalFactory
        : UserClaimsPrincipalFactory<Cat_Usuario, IdentityRole>
    {
        public ApplicationUserClaimsPrincipalFactory(
            UserManager<Cat_Usuario> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Cat_Usuario user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // el nombre completo como claim personalizado
            if (!string.IsNullOrWhiteSpace(user.NombreCompleto))
            {
                identity.AddClaim(new Claim("NombreCompleto", user.NombreCompleto));
            }
            //...
            return identity;
        }
    }
}
