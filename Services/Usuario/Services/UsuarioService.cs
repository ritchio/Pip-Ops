using AutoMapper;
using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Usuario.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Usuario.Services
{
    public class UsuarioService(
        ApplicationDbContext context,
        IMapper mapper) : IUsuarioService
    {
        #region [ Atributos ]
        private readonly ApplicationDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        #endregion [ Atributos ]
        public async Task<List<AspNetUsersViewModel>> ObtenerUsuarios()
        {
            try
            {
                var usuarios = await _context.Users
                .Include(u => u.RolesUsuario)
                    .ThenInclude(ur => ur.Rol)
                .Include(u => u.Plaza)
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
                    AreaNombre = "Área no definida",
                    FechaInactivacion = u.FechaInactivacion
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
