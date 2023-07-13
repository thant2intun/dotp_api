using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface IRole
    {
        Task<string> CreOrUpd(RolesVM vm);
        List<RolesVM> Rolelst();
        bool Delete(int id);
        string GetById(int id);
    }
}
