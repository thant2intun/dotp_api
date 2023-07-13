using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface IKALA_YGNCars
    {
        Task<List<KALA_YGNCars>> getKALA_YGNCarsList();
        Task<KALA_YGNCars> getKALA_YGNCarsById(int id);
        Task<bool> Create(KALA_YGNCarsVM kala_ygnCarVM);
        Task<bool> Delete(int id);
        Task<bool> Update(int id, KALA_YGNCarsVM kala_ygnCarVM);
    }
}
