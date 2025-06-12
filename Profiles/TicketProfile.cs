using AutoMapper;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Profiles
{
    public class TicketProfile: Profile
    {
        public TicketProfile()
        {
            CreateMap<Doc_Ticket, TicketViewModel>()
            .ForMember(dest => dest.IdTipoSoporte, opt => opt.MapFrom(src => src.IdTipoSoporte ?? 0))
            .ForMember(dest => dest.IdModuloClasificacion, opt => opt.MapFrom(src => src.IdModuloClasificacion ?? 0))
            .ForMember(dest => dest.Autorizaciones, opt => opt.MapFrom(src => src.Autorizacion != null ? src.Autorizacion : null))
            .ForMember(dest => dest.Adjuntos, opt => opt.MapFrom(src => src.Adjuntos != null ? src.Adjuntos : null))
            .ForMember(dest => dest.Respuestas, opt => opt.MapFrom(src => src.Respuesta != null ? src.Respuesta : null))
            .ForMember(dest => dest.Asignacion, opt => opt.MapFrom(src => src.Asignacion != null ? src.Asignacion : null))
            .ForMember(dest=>dest.Requisitos, opt => opt.MapFrom(src => src.ModuloClasificacion!= null? src.ModuloClasificacion.Autorizaciones.FirstOrDefault().Requisitos : null));

            CreateMap<Doc_TicketAdjunto, TicketAdjuntoViewModel>()
                .ForMember(dest => dest.IdTickeAdjunto, opt => opt.MapFrom(src => src.IdTicketAdjunto));

            CreateMap<Doc_TicketAutorizacion, TicketAutorizacionViewModel>()
                .ForMember(dest => dest.NombreArea, opt => opt.MapFrom(src => src.Area != null ? src.Area.Nombre : string.Empty))
                .ForMember(dest => dest.NombreEstado, opt => opt.MapFrom(src => src.EstadoAutorizacion != null ? src.EstadoAutorizacion.Nombre : string.Empty));

            CreateMap<Doc_TicketRespuesta, TicketRespuestaViewModel>();
            CreateMap<Doc_TicketAsignacion, TicketAsignacionViewModel>();
               
            CreateMap<Vis_TicketsNuevos, TicketsNuevosViewModel>();
            CreateMap<Vis_TicketsPorAutorizar, TicketsPorAutorizarViewModel>();
            CreateMap<Vis_TicketsAbiertos, TicketsAbiertosViewModel>();
            CreateMap<Vis_TicketsPendientes, TicketsPendientesViewModel>();
            CreateMap<Vis_TicketsCerrados, TicketsCerradosViewModel>();
            CreateMap<Vis_TicketsValidados, TicketsValidadosViewModel>();
            CreateMap<Vis_TicketsRespuesta, TicketsRespuestaViewModel>();
            CreateMap<Vis_TicketsRechazados, TicketsRechazadosViewModel>();

            CreateMap<Sp_TicketsCantidadesPorEstado, TicketsCantidadesPorEstadoViewModel>();
            CreateMap<Sp_TicketsPendientesPorUsuario, TicketsPendientesViewModel>();
            CreateMap<Sp_TicketsRespuestaPorUsuario, TicketsRespuestaViewModel>();

        }
    }
}
