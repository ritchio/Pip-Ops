using AutoMapper;
using HelpDesk.Models.Entities;
using HelpDesk.Models.ViewModels;

namespace HelpDesk.Profiles
{
    public class AreaProfile:Profile
    {
        public AreaProfile() 
        {
            CreateMap<Cat_Area, CatAreaViewModel>();        
        }
    }
}
