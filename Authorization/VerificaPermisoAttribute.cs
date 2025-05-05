using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace HelpDesk.Authorization
{
    public class VerificaPermisoAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _permiso;

        public VerificaPermisoAttribute(string permiso)
        {
            _permiso = permiso;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var usuario = context.HttpContext.User;

            if (!usuario.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var VerificaPermiso = usuario.Claims.Any(c =>
                c.Type == ClaimTypes.AuthorizationDecision && c.Value == _permiso);

            if (!VerificaPermiso)
            {
                context.Result = new ForbidResult();
            }
        }
    }

}
