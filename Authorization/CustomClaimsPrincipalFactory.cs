using HelpDesk.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace HelpDesk.Authorization
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<Cat_Usuario, IdentityRole>
    {
        public CustomClaimsPrincipalFactory(
            UserManager<Cat_Usuario> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, roleManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Cat_Usuario user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // doble factor
            identity.AddClaim(new Claim("Is2FAAuthenticated", "false"));

            return identity;
        }
    }

}
