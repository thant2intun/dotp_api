using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface IVehicleWeightFee
    {
        Task<List<VehicleWeightFee>> getVehicleWeightFeeList();
        Task<VehicleWeightFee> getVehicleWeightFeeById(int id);
        Task<bool> Create(VehicleWeightFeeVM vehicleWeightFeeVM);
        void Delete(int id);
        Task<bool> Update(int id, VehicleWeightFeeVM vehicleWeightFeeVM);
    }
}
