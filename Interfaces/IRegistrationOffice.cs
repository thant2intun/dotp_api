using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Repositories
{
    public interface IRegistrationOffice
    {
        Task<RegistrationOffice> getRegistrationOfficeById(int? id);
        Task<List<RegistrationOffice>> getRegistrationOfficeList();
        //Task<RegistrationOffice> Create(RegistrationOfficeVM registrationOfficeVM);
        bool Create(RegistrationOfficeVM registrationOfficeVM);
        bool Update(int id ,RegistrationOfficeVM registrationOfficeVM);
        void Delete(int id);
        bool OfficeExists(string longName, string shortName);
        bool OfficeExistsForUpdate(int id, RegistrationOfficeVM rVM);

    }
}
