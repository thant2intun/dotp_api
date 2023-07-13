using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface IVehicleWeight
    {
        Task<List<VehicleWeight>> getVehicleWeight();
        Task<VehicleWeight> getVehicleById(int id);
        Task Create(VehicleWeightVM model);
        bool IsExist(VehicleWeightVM vehicleModel);
        Task Update(int id, VehicleWeightVM model);
        Task Delete(int id);
    }
}
