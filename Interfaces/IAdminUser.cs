using DOTP_BE.ViewModel;
using DOTP_BE.ViewModel.AdminResponses;

namespace DOTP_BE.Interfaces
{
    public interface IAdminUser
    {
        List<AdminUserVM> GetAdminUser();
        bool CreateUser(AdminUserVM vm);
        bool UpdateUser(AdminUserVM vm);
        string GetById(int id);
        bool DeleteById(int id);
        Task<Responses> CheckUser(AdminUserVM vm);
        SelectedValuesVM GetSelectedValues();
    }
}
