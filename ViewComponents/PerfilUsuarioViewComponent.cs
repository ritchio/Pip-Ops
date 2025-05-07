using HelpDesk.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HelpDesk.ViewComponents
{
    public class PerfilUsuarioViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public PerfilUsuarioViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

            var usuario = await _context.Users
                .Include(u => u.Plaza)
                .Include(u => u.AreasUsuario)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (usuario == null) return View(null);

            var model = new
            {
                usuario.NombreCompleto,
                usuario.Email,
                Plaza = usuario.Plaza?.Nombre,
                Areas = string.Join(", ", usuario.AreasUsuario.Select(au => _context.Cat_Area.FirstOrDefault(a => a.IdArea == au.IdArea).Nombre))
            };

            return View(model);
        }
    }
}
