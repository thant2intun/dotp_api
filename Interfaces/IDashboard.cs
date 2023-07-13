using DOTP_BE.ViewModel.AdminResponses;

namespace DOTP_BE.Interfaces
{
    public interface IDashboard
    {
        Task<DashboardData> GetDataByOffId(int offId);
        Task<DashboardData> GetDataByOffId(DashboardData data);

        Task<TotalLicense> GetLicenseDataByOffId(int offId);//tzt 290523
        Task<TotalLicense> GetLicenseData(Filter filter);//tzt 300523

        List<RestLicenseToCheck> GetRestLicenseDataToCheckByOffId(int offId); // tzt 010623
        List<RestLicenseToCheck> GetRestLicenseDataToCheckByFilter(Filter filter); // tzt 020623

        ThirdCardData GetDataForTotalExtendValuesById(int offid); //tzt 070623
        ThirdCardData GetDataForTotalExtendValuesByFilter(Filter filter); //tzt 070623
    }
}