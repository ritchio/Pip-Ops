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
#endregion [ Usings ]

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
    .LogTo(Console.WriteLine, LogLevel.Information)
    );
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<Cat_Usuario>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

#region [ Servicios ]
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPlazaService, PlazaService>();
builder.Services.AddScoped<IAreaService, AreaService>();
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

#region [ Configuracion inicial ]
//// Inicializar la base de datos y los roles
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//    await IdentityDataInitializer.SeedRolesAsync(roleManager);
//}

#endregion [ Configuracion inicial ]


app.Run();
