using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace HelpDesk.Authorization
{
    /// <summary>
    /// Aplica los filtros de permisos y roles para los controladores y acciones
    /// </summary>
    /// <example>
    /// Para permisos
    /// [VerificaPermiso("Usuario.Crear")] 
    /// Para roles
    /// [VerificaPermiso(roles: new[] { "Administrador" })]
    /// Para permiso o rol
    /// [VerificaPermiso("Usuario.Crear", "Administrador")]
    /// </example>
    public class VerificaPermisoAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string? _permiso;
        private readonly string[] _rolesPermitidos;

        public VerificaPermisoAttribute(string? permiso = null, params string[] roles)
        {
            _permiso = permiso;
            _rolesPermitidos = roles ?? new string[0];
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var usuario = context.HttpContext.User;

            if (!usuario.Identity?.IsAuthenticated ?? false)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            bool tienePermiso = string.IsNullOrEmpty(_permiso) ||
                                usuario.Claims.Any(c =>
                                    c.Type == ClaimTypes.AuthorizationDecision &&
                                    c.Value == _permiso);

            bool tieneRolPermitido = !_rolesPermitidos.Any() ||
                                     _rolesPermitidos.Any(rol => usuario.IsInRole(rol));

            // con que cumpla una condición
            if (!tienePermiso && !tieneRolPermitido)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
