using AutoMapper;
using P.DataAccessLayer.Models;
using P.PresentationLayer.ViewModels;

namespace P.PresentationLayer.MappingProfiles
{
    public class DepartmentProfile:Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentViewModel,Department>().ReverseMap();
        }
    }
}
