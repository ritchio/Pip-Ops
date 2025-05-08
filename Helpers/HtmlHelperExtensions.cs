using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Security.Claims;

namespace HelpDesk.Helpers
{
    /// <summary>
    /// Aplica los filtros de permisos y roles para las vistas
    /// </summary>
    /// <example>
    /// Para permisos
    /// Html.TieneAcceso("Usuario.Crear")
    /// Para roles
    /// Html.TieneAcceso(roles: new[] { "Administrador" })
    /// Para permiso o rol
    /// Html.TieneAcceso("Usuario.Crear", "Administrador")
    /// </example>
    public static class HtmlHelperExtensions
    {
        public static bool TieneAcceso(this IHtmlHelper htmlHelper, string? permiso = null, params string[] roles)
        {
            var user = htmlHelper.ViewContext.HttpContext.User;

            if (!user.Identity?.IsAuthenticated ?? false)
                return false;

            bool tienePermiso = !string.IsNullOrEmpty(permiso) && user.HasClaim("permission", permiso);

            bool tieneRol = roles == null || roles.Length == 0 ||
                            roles.Any(rol => user.IsInRole(rol));

            return tienePermiso || tieneRol;
        }
    }
}
