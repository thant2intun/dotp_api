using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Org.BouncyCastle.Asn1;

namespace DOTP_BE.Interfaces
{
    public interface ICreateCar

    {
        Task<List<CreateCar>> getCreateCarList();
        Task<CreateCar> getCreateCarById(int id);
        bool Create(CreateCarVM createCarVM);
        void Delete(int id);
        bool Update(int id, CreateCarVM createCarVM);

        //List<CreateCar> AddNewCars(ExtenseCarVMList extenseCarVMList);

        //Task<string> NewCarAttach(List<ExtenseCarVM> extenseCarVMList);
        bool UpdateCar(List<UpdateCreateCar> updateCreateCarVM);
        //Task<CreateCar?> CheckVehicleNumber(string vehicleNumber);

        Task<(bool, VehicleNumberCheckVM?)> CheckVehicleNumber(string vehicleNumber);

        Task<bool> CheckVehicleNoGoodToSave(string vehicleNumber);
    }
}
