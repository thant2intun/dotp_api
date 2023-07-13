using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface IPersonInformation
    {
        Task<List<PersonInformation>> getPersonInformationList();
        Task<PersonInformation> getPersonInformationById(int id);
        Task<bool> Create(PersonInformationVM personInfoVM , string IsPerson);
        void Delete(int id);
        Task<bool> Update(int id, PersonInformationVM personInfoVM);
        bool PersonInformationExists(string NRC_Number, string Name);
    }
}
