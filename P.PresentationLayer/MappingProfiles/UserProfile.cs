using AutoMapper;
using P.DataAccessLayer.Models;
using P.PresentationLayer.ViewModels;

namespace P.PresentationLayer.MappingProfiles
{
    public class UserProfile:Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>().ReverseMap();
        }
    }
}
