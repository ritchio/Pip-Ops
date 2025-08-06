#region  [ Usings ]
using HelpDesk.Authorization;
using HelpDesk.Data;
using HelpDesk.Hubs;
using HelpDesk.Models.Entities;
using HelpDesk.Services.Area.Interfaces;
using HelpDesk.Services.Area.Services;
using HelpDesk.Services.Autorizacion.Interfaces;
using HelpDesk.Services.Autorizacion.Services;
using HelpDesk.Services.Correo.Interfaces;
using HelpDesk.Services.Correo.Services;
using HelpDesk.Services.Historial.Interfaces;
using HelpDesk.Services.Historial.Services;
using HelpDesk.Services.Identity.Services;
using HelpDesk.Services.ModuloClasificacion.Interfaces;
using HelpDesk.Services.ModuloClasificacion.Services;
using HelpDesk.Services.Opcion.Interfaces;
using HelpDesk.Services.Opcion.Services;
using HelpDesk.Services.Plaza.Interfaces;
using HelpDesk.Services.Plaza.Services;
using HelpDesk.Services.Ticket.Interfaces;
using HelpDesk.Services.Ticket.Services;
using HelpDesk.Services.TipoSoporte.Interfaces;
using HelpDesk.Services.TipoSoporte.Services;
using HelpDesk.Services.Usuario.Interfaces;
using HelpDesk.Services.Usuario.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

#endregion [ Usings ]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
    .LogTo(Console.WriteLine, LogLevel.Information)
    );

builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Error);

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
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/Login";
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
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IAutorizacionService, AutorizacionService>();
builder.Services.AddScoped<ICorreoService, CorreoService>();
builder.Services.AddScoped<IHistorialService, HistorialService>();
builder.Services.AddScoped<IModuloClasificacionService, ModuloClasificacionService>();
builder.Services.AddScoped<IOpcionService, OpcionService>();
builder.Services.AddScoped<IPlazaService, PlazaService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ITicketAdjuntoService, TicketAdjuntoService>();
builder.Services.AddScoped<ITicketAdministradorService, TicketAdministradorService>();
builder.Services.AddScoped<ITicketRespuestaService, TicketRespuestaService>();
builder.Services.AddScoped<ITicketRespuestaAdjuntoService, TicketRespuestaAdjuntoService>();
builder.Services.AddScoped<ITicketSoporteService, TicketSoporteService>();
builder.Services.AddScoped<ITicketUsuarioService, TicketUsuarioService>();
builder.Services.AddScoped<ITipoSoporteService, TipoSoporteService>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<Cat_Usuario>, ApplicationUserClaimsPrincipalFactory>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<Cat_Usuario>, CustomClaimsPrincipalFactory>();


//autenticación de doble factor
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim("Is2FAAuthenticated", "true")
        .Build();
});

//notificaciones
builder.Services.AddSignalR();

//servicio en segundo plano
builder.Services.AddHostedService<TicketAutoValidarService>();

#endregion [ Servicios ]

builder.Services.AddAutoMapper(typeof(Program));

var pathBase = builder.Configuration.GetSection("AppSettings")["PathBase"];


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

app.UseStaticFiles();

// Leer PathBase desde config

if (!string.IsNullOrEmpty(pathBase))
{
    app.UsePathBase(pathBase);
    app.Use((context, next) =>
    {
        context.Request.PathBase = pathBase;
        return next();
    });
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

//middleware para autenticación de doble factor
app.Use(async (context, next) =>
{
    // Ya está autenticado (cookie ok)
    var user = context.User;
    var path = context.Request.Path;

    if (user.Identity.IsAuthenticated &&
        !user.HasClaim(c => c.Type == "Is2FAAuthenticated" && c.Value == "true") &&
        !path.StartsWithSegments("/Home/CambiarContrasenia") &&
        !path.StartsWithSegments("/Acceso/VerificaTOTP") &&
        !path.StartsWithSegments("/Acceso/RegistroTOTP") &&
        !path.StartsWithSegments("/Acceso/Logout") &&
        !path.StartsWithSegments("/Acceso/Login") &&
        !path.StartsWithSegments("/Identity/Account/Login") && 
        !path.StartsWithSegments("/Identity/Account/Logout") && 
        !path.StartsWithSegments("/Login"))
    {
        // Redirige al paso de verificación
        context.Response.Redirect(pathBase + "/Acceso/VerificaTOTP");
        return;
    }

    await next();
});

app.UseAuthorization();

app.MapHub<NotificacionHub>("/notificacionesHub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapGet("/Login", context =>
{
    context.Response.Redirect(pathBase+"/Identity/Account/Login");
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
