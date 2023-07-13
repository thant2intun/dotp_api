using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface IMDYCars
    {
        Task<List<MDYCars>> getMDYCarsList();
        Task<MDYCars> getMDYCarsById(int id);
        Task<bool> Create(MDYCarsVM mdyCarsVM);
        Task<bool> Delete(int id);
        Task<bool> Update(int id, MDYCarsVM mdyCarsVM);
    }
}
