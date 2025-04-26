using AutoMapper;
using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.Utils;
using HelpDesk.Models.ViewModels;
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
                        
                    })
                    .ToListAsync();

                return usuarios;
            }
            catch
            {
                throw;
            }
        }

        public async Task<AspNetUsersViewModel> ObtenerUsuarioPorId(string id)
        {
            var AspNetUsers = await _context.Users.FindAsync(id);

            var response = _mapper.Map<AspNetUsersViewModel>(AspNetUsers);

            return response;
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
                    respuesta.Datos = await ObtenerUsuarioPorId(usuario.Id);
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
                    var usuarioMapeado = _mapper.Map(model, usuarioAModificar);

                    await _context.SaveChangesAsync();
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


    }
}
