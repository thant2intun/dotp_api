using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface IExtendYear
    {
        Task<List<Years>> getExtendYear();

        bool CreateExtendYear(ExtendYearVM extendYearVM);
        bool DeleteByID(int id);

        Task<Years> GetExtendYearById(int id);

        bool UpdateExtendYear(int id, ExtendYearVM extendYearVM);

        bool ExistOrNot(ExtendYearVM extendYearVM);
    }
}
