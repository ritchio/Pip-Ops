using AutoMapper;
using HelpDesk.Data;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;
using HelpDesk.Services.Usuario.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HelpDesk.Services.Usuario.Services
{
    public class UsuarioService : IUsuarioService
    {
        #region [ Atributos ]
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        #endregion [ Atributos ]

        #region [ Constructor ]
        public UsuarioService(
            ApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion [ Constructor ]

        public async Task<List<AspNetUsersViewModel>> ObtenerUsuarios()
        {
            return await _context.Users  
                .Select(u => new AspNetUsersViewModel
                {
                    Id = u.Id,
                    PrimerNombre = u.PrimerNombre,
                    SegundoNombre = u.SegundoNombre,
                    ApellidoPaterno = u.ApellidoPaterno,
                    ApellidoMaterno = u.ApellidoMaterno,
                    CorreoElectronico = u.Email ?? ""
                })
                .ToListAsync();
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
