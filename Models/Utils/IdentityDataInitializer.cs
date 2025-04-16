using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

public static class IdentityDataInitializer
{
    public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roleNames = { "Administrador","Administrador Supervisor", "Soporte", "Usuario" };

        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
