using AutoMapper;
using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Area.Interfaces;
using HelpDesk.Services.Plaza.Interfaces;
using HelpDesk.Services.Usuario.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Usuario.Services
{
    public class UsuarioService(
        ApplicationDbContext context,
        RoleManager<IdentityRole> roleManager,
        UserManager<Cat_Usuario> userManager,
        IMapper mapper) : IUsuarioService
    {
        #region [ Atributos ]
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly UserManager<Cat_Usuario> _userManager = userManager;
        #endregion [ Atributos ]
        public async Task<List<AspNetUsersViewModel>> ObtenerUsuarios()
        {
            try
            {
                var usuarios = await _context.Users
                    .Include(u => u.RolesUsuario)
                    .ThenInclude(ur => ur.Rol)
                    .Include(u => u.Plaza)
                    .Include(u => u.AreasUsuario)
                    .Select(u => new AspNetUsersViewModel
                    {
                        Id = u.Id,
                        NumeroUsuario = u.NumeroUsuario,
                        PrimerNombre = u.PrimerNombre,
                        SegundoNombre = u.SegundoNombre,
                        ApellidoPaterno = u.ApellidoPaterno,
                        ApellidoMaterno = u.ApellidoMaterno,
                        CorreoElectronico = u.Email ?? "",
                        PlazaNombre = u.Plaza.Nombre,
                        RolNombre = u.RolesUsuario.FirstOrDefault().Rol.Name ?? "Sin Rol",
                        AreaNombre = string.Join(", ", u.AreasUsuario.Select(au => _context.Cat_Area.FirstOrDefault(a => a.IdArea == au.IdArea).Nombre)),
                        FechaInactivacion = u.FechaInactivacion,
                        IdRol = u.RolesUsuario.FirstOrDefault().Rol.Id,
                        IdAreas = u.AreasUsuario.Select(au => au.IdArea).ToList(),
                        Activo = u.Activo
                        
                    })
                    .ToListAsync();

                return usuarios;
            }
            catch
            {
                throw;
            }
        }

        public async Task<RespuestaGenerica<AspNetUsersViewModel>> ObtenerUsuarioPorId(string id)
        {
            var respuesta = new RespuestaGenerica<AspNetUsersViewModel>();
            try
            {
                var usuario = await _context.Users
                    .Include(u => u.RolesUsuario)
                    .ThenInclude(ur => ur.Rol)
                    .Include(u => u.Plaza)
                    .Include(u => u.AreasUsuario)
                    .Where(u => u.Id == id)
                    .Select(u => new AspNetUsersViewModel
                    {
                        Id = u.Id,
                        NumeroUsuario = u.NumeroUsuario,
                        PrimerNombre = u.PrimerNombre,
                        SegundoNombre = u.SegundoNombre,
                        ApellidoPaterno = u.ApellidoPaterno,
                        ApellidoMaterno = u.ApellidoMaterno,
                        CorreoElectronico = u.Email ?? "",
                        PlazaNombre = u.Plaza.Nombre,
                        RolNombre = u.RolesUsuario.FirstOrDefault().Rol.Name ?? "Sin Rol",
                        AreaNombre = string.Join(", ", u.AreasUsuario.Select(au => _context.Cat_Area.FirstOrDefault(a => a.IdArea == au.IdArea).Nombre)),
                        FechaInactivacion = u.FechaInactivacion,
                        IdRol = u.RolesUsuario.FirstOrDefault().Rol.Id,
                        IdAreas = u.AreasUsuario.Select(au => au.IdArea).ToList(),
                        IdPlaza = u.IdPlaza,
                        Activo = u.Activo,
                        FechaActivacion = u.FechaActivacion
                    })
                    .FirstOrDefaultAsync();


                respuesta.Exito = true;
                respuesta.Datos = usuario;
            }
            catch
            {
                var usuario = new AspNetUsersViewModel
                {
                    CorreoElectronico = string.Empty,
                    PrimerNombre = string.Empty,
                    ApellidoPaterno = string.Empty,
                    IdAreas = new List<int>(),
                    IdRol = string.Empty
                };

                respuesta.Exito = false;
                respuesta.Datos = usuario;
            }

            return respuesta;
        }

       
        public async Task<RespuestaGenerica<AspNetUsersViewModel>> Crear(AspNetUsersViewModel model)
        {
            var respuesta = new RespuestaGenerica<AspNetUsersViewModel>();
            try
            {
                // Verificar si el correo electrónico ya existe y está activo  
                var usuarioExistente = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.CorreoElectronico && u.Activo == true);

                if (usuarioExistente != null)
                {
                    respuesta.Exito = false;
                    respuesta.Mensaje = "El correo electrónico ya está registrado con un usuario activo.";
                    return respuesta;
                }

                var usuario = _mapper.Map<AspNetUsersViewModel, Cat_Usuario>(model);

                usuario.Id = Guid.NewGuid().ToString();
                usuario.Activo = true;
                usuario.FechaActivacion = DateTime.Now;
                usuario.NormalizedEmail = model.CorreoElectronico.ToUpper();
                usuario.NormalizedUserName = model.CorreoElectronico.ToUpper();
                usuario.UserName = model.CorreoElectronico;
                usuario.EmailConfirmed = true;
                usuario.PhoneNumberConfirmed = true;
                usuario.TwoFactorEnabled = false;
                usuario.LockoutEnabled = false;
                usuario.LockoutEnd = null;
                usuario.ConcurrencyStamp = Guid.NewGuid().ToString();
                usuario.IdPlaza = model.IdPlaza ?? 0;

                // Generar contraseña aleatoria de 10 caracteres  
                usuario.PasswordHash = _userManager.PasswordHasher.HashPassword(usuario, GenerarContrasenaAleatoria(10));

                _context.Users.Add(usuario);
                var nuevoUsuario = await _context.SaveChangesAsync();
                if (nuevoUsuario > 0)
                {
                    // Guardamos el rol asignado  
                    var rol = await _roleManager.FindByIdAsync(model.IdRol);
                    if (rol == null)
                    {
                        respuesta.Exito = false;
                        respuesta.Mensaje = "El rol no existe.";
                        return respuesta;
                    }

                    // Asignamos el rol al usuario  
                    var resultado = await _userManager.AddToRoleAsync(usuario, rol.Name);
                    if (!resultado.Succeeded)
                    {
                        respuesta.Exito = false;
                        respuesta.Mensaje = "No se pudo asignar el rol al usuario";
                        return respuesta;
                    }

                    // Guardamos la relación de áreas  
                    var existentes = _context.Rel_UsuarioArea.Where(r => r.IdUsuario == usuario.Id);
                    _context.Rel_UsuarioArea.RemoveRange(existentes);

                    // Agregar nuevas relaciones  
                    var nuevasRelaciones = model.IdAreas.Select(areaId => new Rel_UsuarioArea
                    {
                        IdUsuario = usuario.Id,
                        IdArea = areaId
                    }).ToList();

                    await _context.Rel_UsuarioArea.AddRangeAsync(nuevasRelaciones);
                    await _context.SaveChangesAsync();

                    respuesta.Exito = true;
                    respuesta.Mensaje = "Usuario creado con éxito.";

                    return respuesta;
                }
                else
                {
                    respuesta.Exito = false;
                    respuesta.Mensaje = "Error al crear el usuario.";
                    return respuesta;
                }
            }
            catch (Exception ex)
            {
                respuesta.Exito = false;
                respuesta.Mensaje = "Error al crear el usuario." + ex.Message;
                return respuesta;
            }
        }

        public async Task<AspNetUsersViewModel> Modificar(AspNetUsersViewModel model)
        {
            try
            {
                var usuarioAModificar = await _context.Users.FindAsync(model.Id);

                if (usuarioAModificar != null)
                {
                    //Verifica si el correo que se está modificando ya existe para otro usuariop activo
                    var emailExiste = await _context.Users
                        .FirstOrDefaultAsync(u => u.Email == model.CorreoElectronico && u.Activo == true && u.Id != model.Id);
                    if(emailExiste != null)
                    {
                        throw new Exception("El correo ya existe en otro usuario activo.");
                    }

                    var usuarioMapeado = _mapper.Map(model, usuarioAModificar);
                    usuarioMapeado.UserName = usuarioMapeado.Email.ToUpper();

                    await _context.SaveChangesAsync();

                    // quitamo la relación de áreas
                    var existentes = _context.Rel_UsuarioArea.Where(r => r.IdUsuario == usuarioMapeado.Id);
                    _context.Rel_UsuarioArea.RemoveRange(existentes);
                    // agregamo nuevas relaciones
                    var nuevasRelaciones = model.IdAreas.Select(areaId => new Rel_UsuarioArea
                    {
                        IdUsuario = usuarioMapeado.Id,
                        IdArea = areaId
                    }).ToList();
                    await _context.Rel_UsuarioArea.AddRangeAsync(nuevasRelaciones);
                    await _context.SaveChangesAsync();

                    // guaradamo el rol
                    var rol = await _roleManager.FindByIdAsync(model.IdRol);
                    if (rol == null)
                    {
                        throw new Exception("El rol no existe.");
                    }

                    // quitamo los roles
                    var rolesActuales = await _userManager.GetRolesAsync(usuarioMapeado);
                    if (rolesActuales.Any())
                    {
                        var resultadoEliminacion = await _userManager.RemoveFromRolesAsync(usuarioMapeado, rolesActuales);
                        if (!resultadoEliminacion.Succeeded)
                        {
                            throw new Exception("No se pudieron eliminar los roles anteriores del usuario.");
                        }
                    }

                    // agregamo el nuevo rol
                    var resultadoAsignacion = await _userManager.AddToRoleAsync(usuarioMapeado, rol.Name);
                    if (!resultadoAsignacion.Succeeded)
                    {
                        throw new Exception("No se pudo asignar el rol al usuario.");
                    }

                    return model;
                }
                else
                {
                    throw new Exception("Usuario no encontrado.");
                }
            }
            catch
            {
                throw;
            } 
            
        }

        public async Task<Dictionary<string,string>> CambiarEstado(string idUsuario)
        {
            var usuario = await _context.Users.FindAsync(idUsuario);
            if (usuario!=null)
            {
                usuario.Activo = !usuario.Activo;
                usuario.FechaInactivacion = usuario.Activo ? null : DateTime.Now;
                await _context.SaveChangesAsync();
                var respuesta = new Dictionary<string, string>();
                respuesta.Add("fechaInactivacion", usuario.FechaInactivacion.HasValue?usuario.FechaInactivacion.Value.ToString():"");
                respuesta.Add("estado", usuario.Activo ? "activado" : "desactivado");
                
                return respuesta;
            }
            else
            {
                throw new Exception("Usuario no encontrado.");
            }
        }
        #region [ Privadas ]
        private string GenerarContrasenaAleatoria(int longitud)
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(caracteres, longitud)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #endregion [ Privadas ]

    }
}
