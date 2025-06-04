using AutoMapper;
using HelpDesk.Authorization;
using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Area.Interfaces;
using HelpDesk.Services.Correo.Interfaces;
using HelpDesk.Services.Correo.Services;
using HelpDesk.Services.Plaza.Interfaces;
using HelpDesk.Services.Usuario.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HelpDesk.Services.Usuario.Services
{
    public class UsuarioService(
        ApplicationDbContext context,
        RoleManager<IdentityRole> roleManager,
        UserManager<Cat_Usuario> userManager,
        ICorreoService correoService,
        IMapper mapper) : IUsuarioService
    {
        #region [ Atributos ]
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly UserManager<Cat_Usuario> _userManager = userManager;
        private readonly ICorreoService _correoService = correoService;
        #endregion [ Atributos ]

        #region [ Usuario ]

        public async Task<CatUsuarioViewModel> ObtenerUsuarioActual(ClaimsPrincipal claim)
        {
            // Obtener el usuario actual de la sesión

            var usuarioActual = await _userManager.GetUserAsync(claim);
            

            
            if (usuarioActual == null)
            {
                throw new Exception("Usuario no encontrado.");
            }
            var usuario = await _context.Users
                   .Include(u => u.RolesUsuario)
                   .ThenInclude(ur => ur.Rol)
                   .Include(u => u.Plaza)
                   .Include(u => u.AreasUsuario)
                   .Where(u => u.Id == usuarioActual.Id)
                   .Select(u => new CatUsuarioViewModel
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

            return usuario;
        }
        public async Task<List<CatUsuarioViewModel>> ObtenerUsuarios()
        {
            try
            {
                var usuarios = await _context.Users
                    .Include(u => u.RolesUsuario)
                    .ThenInclude(ur => ur.Rol)
                    .Include(u => u.Plaza)
                    .Include(u => u.AreasUsuario)
                    .Select(u => new CatUsuarioViewModel
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

        public async Task<List<CatUsuarioViewModel>> ObtenerUsuariosPorRol(string rol)
        {
            try
            {
                var usuarios = await _context.Users
                    .Include(u => u.RolesUsuario)
                    .ThenInclude(ur => ur.Rol)
                    .Include(u => u.Plaza)
                    .Include(u => u.AreasUsuario)
                    .Where(u=>u.RolesUsuario.Any(ur => ur.Rol.Name == rol && u.Activo == true))
                    .ToListAsync();

                return _mapper.Map<List<CatUsuarioViewModel>>(usuarios);
            }
            catch
            {
                throw;
            }
        }

        public async Task<RespuestaGenerica<CatUsuarioViewModel>> ObtenerUsuarioPorId(string id)
        {
            var respuesta = new RespuestaGenerica<CatUsuarioViewModel>();
            try
            {
                var usuario = await _context.Users
                    .Include(u => u.RolesUsuario)
                    .ThenInclude(ur => ur.Rol)
                    .Include(u => u.Plaza)
                    .Include(u => u.AreasUsuario)
                    .Where(u => u.Id == id)
                    .Select(u => new CatUsuarioViewModel
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
                var usuario = new CatUsuarioViewModel
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

       
        public async Task<RespuestaGenerica<CatUsuarioViewModel>> Crear(CatUsuarioViewModel model)
        {
            var respuesta = new RespuestaGenerica<CatUsuarioViewModel>();
            try
            {
                // Verificar si el correo electrónico ya existe y está activo  
                var usuarioExistente = await _context.Users
                    .FirstOrDefaultAsync(u => u.NormalizedEmail == model.CorreoElectronico.ToUpper() && u.Activo == true);

                if (usuarioExistente != null)
                {
                    respuesta.Exito = false;
                    respuesta.Mensaje = "El correo electrónico ya está registrado con un usuario activo.";
                    return respuesta;
                }

                var usuario = _mapper.Map<CatUsuarioViewModel, Cat_Usuario>(model);

                usuario.Id = Guid.NewGuid().ToString();
                usuario.Activo = true;
                usuario.FechaActivacion = DateTime.Now;
                usuario.NormalizedEmail = model.CorreoElectronico.ToUpper();
                usuario.NormalizedUserName = model.CorreoElectronico.ToUpper();
                usuario.UserName = model.CorreoElectronico;
                usuario.EmailConfirmed = true;
                usuario.PhoneNumberConfirmed = true;
                usuario.TwoFactorEnabled = false;
                usuario.LockoutEnabled = true;
                usuario.LockoutEnd = null;
                usuario.ConcurrencyStamp = Guid.NewGuid().ToString();
                usuario.IdPlaza = model.IdPlaza ?? 0;
                usuario.CambiarContrasenia = true;
                var contrasenia = GenerarContraseniaAleatoria(10);
                // Generar contraseña aleatoria de 10 caracteres  
                usuario.PasswordHash = _userManager.PasswordHasher.HashPassword(usuario, contrasenia);

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

                    //mandamos correo con la contraseña
                    var asunto = "Bienvenido a HelpDesk";
                    var cuerpo = $"<p>Hola {usuario.PrimerNombre} {usuario.ApellidoPaterno},</p>" +
                                 $"<p>Tu cuenta ha sido creada con éxito. Tu contraseña es: <strong>{contrasenia}</strong></p>" +
                                 "<p>Por favor, cámbiala al iniciar sesión.</p>";
                    
                    var correoEnviado = await _correoService.EnviarCorreoAsync(usuario.Email, asunto, cuerpo, true);

                    respuesta.Exito = true;
                    respuesta.Mensaje = "Usuario creado con éxito.";
                    respuesta.Mensaje += correoEnviado ? "<br>Correo enviado con éxito." : "<br>Error al enviar correo.";

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

        public async Task<CatUsuarioViewModel> Modificar(CatUsuarioViewModel model)
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
                    var fechaInactivacion = usuarioAModificar.FechaInactivacion;
                    var usuarioMapeado = _mapper.Map(model, usuarioAModificar);
                    usuarioMapeado.UserName = usuarioMapeado.Email.ToUpper();
                    usuarioAModificar.FechaInactivacion = fechaInactivacion;
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

        public async Task<RespuestaGenerica<Dictionary<string, string>>> CambiarEstado(string idUsuario)
        {
            var respuesta = new RespuestaGenerica<Dictionary<string, string>>();
            var usuario = await _context.Users.FindAsync(idUsuario);
            if (usuario != null)
            {
                
                if (!usuario.Activo)
                {
                    // checa si existe otro usuario activo con el mismo correo electrónico  
                    var usuarioConMismoCorreo = await _context.Users
                        .FirstOrDefaultAsync(u => u.NormalizedEmail == usuario.Email && u.Activo == true && u.Id != usuario.Id);

                    if (usuarioConMismoCorreo != null)
                    {
                        respuesta.Mensaje =  "La habilitación no es posible debido a la duplicidad de correo electrónico asignado";
                        respuesta.Exito = false;
                        return respuesta;
                    }
                }
                
                usuario.Activo = !usuario.Activo;
                usuario.FechaInactivacion = usuario.Activo ? null : DateTime.Now;

                string correo = (usuario.Activo)?usuario.NormalizedEmail.Replace($"{usuario.Id}#", ""):usuario.Id+"#"+usuario.NormalizedEmail;

                //usuario.Email = correo.ToLower();
                usuario.LockoutEnabled = true;
                
                usuario.LockoutEnd = (usuario.Activo)?null:DateTime.Now.AddYears(100);//100 creo que son suficientes
                
                usuario.NormalizedEmail = correo;
                usuario.UserName = correo.ToLower();
                usuario.NormalizedUserName = usuario.NormalizedEmail;
                await _context.SaveChangesAsync();

                var datos =  new  Dictionary<string, string>();
                datos.Add("fechaInactivacion", usuario.FechaInactivacion.HasValue ? usuario.FechaInactivacion.Value.ToString() : "");
                datos.Add("estado", usuario.Activo ? "activado" : "desactivado");
                
                respuesta.Datos = datos;
                respuesta.Exito = true;
                respuesta.Mensaje = $"Usuario {datos["estado"]} con éxito.";

                return respuesta;
            }
            else
            {
                respuesta.Mensaje = "Usuario no encontrado";
                respuesta.Exito = false;
                return respuesta;
            }
        }

        #endregion [ Usuario ]

        #region [ Contraseña ]
        public async Task<RespuestaGenerica<string>> CambiarContrasenia(ClaimsPrincipal claim,string nuevaContrasenia)
        {
            var respuesta = new RespuestaGenerica<string>();
            try
            {
                var usuario = await _userManager.GetUserAsync(claim);
                if(usuario == null)
                {
                    respuesta.Exito = false;
                    respuesta.Mensaje = "Usuario no encontrado.";
                    return respuesta;
                }
                //token para poder resetear sin la contraseña actual
                var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);

                var resultado = await _userManager.ResetPasswordAsync(usuario, token, nuevaContrasenia);


                if (resultado.Succeeded)
                {
                    // Actualizar el campo CambiarContrasenia a false
                    usuario.CambiarContrasenia = false;
                    _context.Users.Update(usuario);
                    await _context.SaveChangesAsync();

                    respuesta.Exito = true;
                    respuesta.Mensaje = "Contraseña cambiada con éxito.";

                }
                else
                {
                    respuesta.Exito = false;
                    respuesta.Mensaje = "No se pudo cambiar la contraseña. " + string.Join(", ", resultado.Errors.Select(e => e.Description));
                }
            }
            catch(Exception ex)
            {
                respuesta.Exito = false;
                respuesta.Mensaje = "No se pudo cambiar la contraseña. " + ex.Message;
            }
            
            
            return respuesta;
        
        }
        #endregion [ Contraseña ]

        #region [ Permisos ]
        public async Task<List<PermisoViewModel>> ObtenerPermisosDeUsuario(string idUsuario)
        {
            var usuario = await _userManager.FindByIdAsync(idUsuario);
            if (usuario == null)
            {
                throw new Exception("Usuario no encontrado.");
            }

            var claims = await _userManager.GetClaimsAsync(usuario);
            var permisosAsignados = claims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value)
                .ToHashSet();

            var todosLosPermisos = ObtenerTodosLosPermisosDefinidos();

            var lista = todosLosPermisos
                .Select(p => new PermisoViewModel
                {
                    Valor = p.Valor,
                    Descripcion = p.Descripcion,
                    Asignado = permisosAsignados.Contains(p.Valor)
                })
                .ToList();

            return lista;
        }
        private List<(string Valor, string Descripcion)> ObtenerTodosLosPermisosDefinidos()
        {
            var permisos = new List<(string, string)>();
            var tipo = typeof(Permisos);

            foreach (var categoria in tipo.GetNestedTypes())
            {
                foreach (var campo in categoria.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
                {
                    var valor = campo.GetValue(null)?.ToString();
                    var descripcionAttr = campo.GetCustomAttributes(typeof(PermisoDescripcionAttribute), false)
                        .FirstOrDefault() as PermisoDescripcionAttribute;
                    var descripcion = descripcionAttr?.Descripcion ?? valor;
                    permisos.Add((valor??"", descripcion??""));
                }
            }

            return permisos;
        }

        public async Task<RespuestaGenerica<string>> ActualizarPermisoUsuario(string idUsuario, string permiso, bool asignar)
        {
            var respuesta = new RespuestaGenerica<string>();
            var usuario = await _userManager.FindByIdAsync(idUsuario);
            if (usuario == null)
            {
                respuesta.Exito = false;
                respuesta.Mensaje = "Usuario no encontrado.";
                return  respuesta;
            }
                
            var claim = new Claim("permission", permiso);

            if (asignar)
            {
                var resultado = await _userManager.AddClaimAsync(usuario, claim);
                if (!resultado.Succeeded)
                {
                    respuesta.Exito = false;
                    respuesta.Mensaje = "No se pudo asignar el permiso.";

                }
                else
                {
                    respuesta.Exito = true;
                    respuesta.Mensaje = "Permiso asignado con éxito.";

                }

                return respuesta;
            }
            else
            {
                var claimsExistentes = await _userManager.GetClaimsAsync(usuario);
                var claimExistente = claimsExistentes.FirstOrDefault(c => c.Type == "permission" && c.Value == permiso);
                if (claimExistente != null)
                {
                    var resultado = await _userManager.RemoveClaimAsync(usuario, claimExistente);
                    if (!resultado.Succeeded)
                    {
                        respuesta.Exito = false;
                        respuesta.Mensaje = "No se pudo quitar el permiso.";
                    }
                    else
                    {
                        respuesta.Exito = true;
                        respuesta.Mensaje = "Permiso quitado con éxito.";
                    }
                    return respuesta;
                }
                else
                {
                    respuesta.Exito = false;
                    respuesta.Mensaje = "El permiso no existe.";
                    return respuesta;

                }
            }
        }

        #endregion [ Permisos ]

        #region [ Auxiliares ]
        public string GenerarContraseniaAleatoria(int longitud)
        {
            if (longitud < 4)
                throw new ArgumentException("La longitud mínima es 4 para cumplir con el requisito.");

            const string mayusculas = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string minusculas = "abcdefghijklmnopqrstuvwxyz";
            const string numeros = "0123456789";
            const string simbolos = "#.,?¡¿!-_";
            string todos = mayusculas + minusculas + numeros + simbolos;

            var random = new Random();
            var contraseña = new List<char>
            {
                mayusculas[random.Next(mayusculas.Length)],
                minusculas[random.Next(minusculas.Length)],
                numeros[random.Next(numeros.Length)],
                simbolos[random.Next(simbolos.Length)]
            };

            // el resto de caracteres son de cualquiera
            for (int i = contraseña.Count; i < longitud; i++)
            {
                contraseña.Add(todos[random.Next(todos.Length)]);
            }

            // mezclamo para que no estén al inicio
            return new string(contraseña.OrderBy(c => random.Next()).ToArray());
        }

        #endregion [ Auxiliares ]

    }
}
