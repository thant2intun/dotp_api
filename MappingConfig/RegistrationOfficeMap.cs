using AutoMapper;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.MappingConfig
{
    public class RegistrationOfficeMap : Profile
    {
        public RegistrationOfficeMap()
        {
            CreateMap<RegistrationOffice, RegistrationOfficeVM>().ReverseMap();
        }
    }
}
