using AutoMapper;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.MappingConfig
{
    public class MenuMap : Profile
    {
        public MenuMap()
        {
            CreateMap<Menu, MenuVM>().ReverseMap();
        }
    }
}
