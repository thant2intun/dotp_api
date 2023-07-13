using AutoMapper;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.MappingConfig
{
    public class AdminUserMap : Profile
    {
        public AdminUserMap()
        {
            CreateMap<AdminUser, AdminUserVM>().ReverseMap();
        }
    }
}
