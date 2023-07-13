using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface IUser
    {
        Task<List<User>> getUserList();
        Task<User> getUserById(int id);
        bool Create(UserVM userVM);
        void Delete(int id);
        bool Update(int id, UserVM userVM);
        bool UserExists(string Email, string Phone);
        string GetOTP(int id);
        Task<List<ExtendsLicenseVM>?> GetExtendsLicenses(string userId);
        User? FindUserByEmailOrPhone(string EmailOrPhone);
    }
}
