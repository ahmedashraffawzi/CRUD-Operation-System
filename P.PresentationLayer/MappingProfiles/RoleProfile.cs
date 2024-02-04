using AutoMapper;
using Microsoft.AspNetCore.Identity;
using P.PresentationLayer.ViewModels;

namespace P.PresentationLayer.MappingProfiles
{
    public class RoleProfile:Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole,RoleViewModel>().ForMember(d=>d.RoleName,o=>o.MapFrom(s=>s.Name))
                                                   .ReverseMap();
            
        }
    }
}
