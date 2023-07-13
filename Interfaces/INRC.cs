using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface INRC
    {
        Task<List<NRC>> getNRCList();
        Task<List<NRC>> getNRCListByNRCNumber(int nrcNumber);
        Task<NRC> getNRCById(int id);
        bool Create(NRCVM nrcVM);
        void Delete(int id);
        bool Update(int id, NRCVM nrcVM);

    }
}
