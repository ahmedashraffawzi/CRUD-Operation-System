using AutoMapper;
using P.DataAccessLayer.Models;
using P.PresentationLayer.ViewModels;

namespace P.PresentationLayer.MappingProfiles
{
    public class EmployeeProfile:Profile
    {
        public EmployeeProfile()
        {
            CreateMap<EmployeeViewModel,Employee>().ReverseMap();
        }
    }
}
