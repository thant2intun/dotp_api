using AutoMapper;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.MappingConfig
{
    public class RoleMap : Profile
    {
        public RoleMap()
        {
            CreateMap<Role, RolesVM>().ReverseMap();
        }
    }
}
