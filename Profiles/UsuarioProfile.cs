using AutoMapper;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            // De entidad a ViewModel
            CreateMap<Cat_Usuario, AspNetUsersViewModel>()
                .ForMember(dest => dest.CorreoElectronico, opt => opt.MapFrom(src => src.Email));

            // De ViewModel a entidad (si necesitas guardar/actualizar datos desde el ViewModel)
            CreateMap<AspNetUsersViewModel, Cat_Usuario>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.CorreoElectronico));
        }
    }
}


