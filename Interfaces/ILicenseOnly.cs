using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface ILicenseOnly
    {
        Task<List<LicenseOnly>> getLicenseOnlyList();
        Task<LicenseOnly> getLicenseOnlyById(int id);
        Task<bool> Create(LicenseOnlyVM licenseOnlyVM);
        void Delete(int id);
        Task<bool> Update(int id, LicenseOnlyVM licenseOnlyVM);
    }
}
