using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Interfaces
{
    public interface ILicenseType
    {
        Task<List<LicenseType>> GetLicenseTypeList();

        Task<bool> AddLicenseType(LicenseTypeVM licenseType);

        Task<LicenseType> GetLicenseTypeById(int? id);

        Task<bool> DeleteLicenseType(int? id);

        Task<bool> UpdateLicenseType(int? id, LicenseTypeVM licenseType);
    }
}
