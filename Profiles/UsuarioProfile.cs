using AutoMapper;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            // pa'delante
            CreateMap<Cat_Usuario, AspNetUsersViewModel>()
                .ForMember(dest => dest.CorreoElectronico, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.NumeroUsuario, opt => opt.Ignore());

            //pa'trás
            CreateMap<AspNetUsersViewModel, Cat_Usuario>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.CorreoElectronico))
                .ForMember(dest => dest.NumeroUsuario, opt => opt.Ignore());

        }
    }
}


