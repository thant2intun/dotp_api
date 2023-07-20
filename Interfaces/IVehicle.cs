using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using DOTP_BE.ViewModel.AdminResponses;

namespace DOTP_BE.Interfaces
{
    public interface IVehicle

    {
        Task<List<Vehicle>> getVehicleList();
        Task<List<Vehicle>> getVehicleById(string formMode, string transactionId, string status);

        Task<List<Vehicle>> getVehicleById(string transactionId, string status);
        //Task<(Vehicle, List<Vehicle>)> getVehicleById(int transactionId);
        Task<bool> Create(VehicleVM vehicleVM);
        void Delete(int id);
        Task<bool> Update(int id, VehicleVM vehicleVM);

        //Task<List<ExtendLicenseVMAdmin>> getVehicleListByStatus(string status);

        //Task<ExtendLicenseDashBoardVMAdmin> getVehicleListByStatusAndDate(string status, string fromDate, string toDate);
        Task<(int, ExtendLicenseDashBoardVMAdmin)> getVehicleListByStatus(ExtenLicenseDbSearchVM dto);

        Task<Vehicle> VehicleDetailToCheckById(int id);
        Task<bool> UpdateStatusById(int id, string statusDto);

        Task<(bool, string?)> OperatorLicenseConfirmReject(OLConfirmOrRejectVM oLConfirmOrRejectVM);

        Task<(int, int, int, List<Vehicle>)> GetVehiclListByPagination(int page, int pageSize);
    }
}
