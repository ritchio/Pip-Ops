using Microsoft.AspNetCore.Mvc.Rendering;

namespace HelpDesk.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static bool TienePermiso(this IHtmlHelper htmlHelper, string permiso)
        {
            var user = htmlHelper.ViewContext.HttpContext.User;
            return user.HasClaim("permission", permiso);
        }
    }
}
