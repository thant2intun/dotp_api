using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface ITownship
    {
        Task<List<Township>> GetTownshipList();

        Task<List<string>> GetTownshipMyanmarNameList();
        Task<Township> GetTownshipByID(int id);
        Task<int> CreateTownship(TownshipVM township);
        Task<int> UpdateTownship(int id, TownshipVM model);
        Task<int> DeleteTownship(int id);
    }
}
