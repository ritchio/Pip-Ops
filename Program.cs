#region  [ Usings ]
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HelpDesk.Data;
using HelpDesk.Services.Usuario.Interfaces;
using HelpDesk.Services.Usuario.Services;
using HelpDesk.Models.Entities;
using HelpDesk.Services.Plaza.Services;
using HelpDesk.Services.Plaza.Interfaces;
using HelpDesk.Services.Area.Interfaces;
using HelpDesk.Services.Area.Services;
using HelpDesk.Services.Correo.Services;
using HelpDesk.Services.Correo.Interfaces;
using HelpDesk.Services.Identity.Services;
using HelpDesk.Services.Opcion.Interfaces;
using HelpDesk.Services.Opcion.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using HelpDesk.Services.Historial.Interfaces;
using HelpDesk.Services.Historial.Services;
using HelpDesk.Services.ModuloClasificacion.Services;
using HelpDesk.Services.ModuloClasificacion.Interfaces;
using HelpDesk.Services.Autorizacion.Interfaces;
using HelpDesk.Services.Autorizacion.Services;

#endregion [ Usings ]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
    .LogTo(Console.WriteLine, LogLevel.Information)
    );

//builder.Services.AddDefaultIdentity<Cat_Usuario>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<Cat_Usuario, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
    // Puedes agregar m�s configuraci�n aqu� si lo necesitas
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/";
    //guardamos el log de inicios para no molestar al ide
    options.Events = new CookieAuthenticationEvents
    {
        OnSignedIn = async context =>
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var userManager = serviceProvider.GetRequiredService<UserManager<Cat_Usuario>>();
            var db = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var user = await userManager.GetUserAsync(context.Principal);

            if (user != null)
            {
                db.Log_Acceso.Add(new Log_Acceso
                {
                    IdUsuario = user.Id,
                    Accion = "Inicio de sesión",
                    Fecha = DateTime.Now
                });

                await db.SaveChangesAsync();
            }
        },
        OnSigningOut = async context =>
        {
            var serviceProvider = context.HttpContext.RequestServices;
            var userManager = serviceProvider.GetRequiredService<UserManager<Cat_Usuario>>();
            var db = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var user = await userManager.GetUserAsync(context.HttpContext.User);

            if (user != null)
            {
                db.Log_Acceso.Add(new Log_Acceso
                {
                    IdUsuario = user.Id,
                    Accion = "Cierre de sesión",
                    Fecha = DateTime.Now
                });

                await db.SaveChangesAsync();
            }
        }
    };
    
});

#region [ Servicios ]
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<Cat_Usuario>, ApplicationUserClaimsPrincipalFactory>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPlazaService, PlazaService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<ICorreoService, CorreoService>();
builder.Services.AddScoped<IOpcionService,OpcionService>();
builder.Services.AddScoped<IHistorialService, HistorialService>();
builder.Services.AddScoped<IModuloClasificacionService, ModuloClasificacionService>();
builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();
#endregion [ Servicios ]

builder.Services.AddAutoMapper(typeof(Program)); 


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapGet("/Login", context =>
{
    context.Response.Redirect("/Identity/Account/Login");
    return Task.CompletedTask;
});

#region [ Configuracion inicial ]
// Inicializar la base de datos y los roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await IdentityDataInitializer.SeedRolesAsync(roleManager);
}

#endregion [ Configuracion inicial ]


app.Run();
