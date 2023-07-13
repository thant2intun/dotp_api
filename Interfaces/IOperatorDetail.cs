using DOTP_BE.Model;
using DOTP_BE.Models;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface IOperatorDetail
    {
        Task<List<OperatorDetail>> getOperatorDetailList();
        Task<OperatorDetail> getOperatorDetailById(int id);
        Task<bool> Create(OperatorDetailVM operatorDetailVM);
        Task<bool> Delete(int id);
        Task<bool> Update(int id, OperatorDetailVM operatorDetailVM);
        Task<OperatorDetailVM> getOperatorDetailByNRCAndLicenseNumberLong(int userId, int operatorId, string license_num_long);
        Task<bool> AddOperatorLicenseAttach(LicenseOnly licenseOnly);
        Task<bool> UpdateLicenseAttach(OperatorLicenseAttachVM operatorLicenseAttachVM, string transactionId);

        //Task<bool> UpdateLicenseAttach(ToUpdateLicenseOnlyVM doUpdateLDto); //ok pdf

        Task<(bool, bool)> ExtendOperatorLicenseProcess(OperatorLicenseAttachVM operatorLicenseAttachVM);
        //Task<string> VehicleAttach(List<CarAttachedFileVM> carAttachedFilelVM);
        //Task<string> VehicleAttach(OperatorLicenseAttachVM carAttachedFilelVM, string rootPath); //ok pdf

        //Task<string> VehicleAttach(OperatorLicenseAttachVM carAttachedFilelVM, string sharedFolderIp, string sharedOverHttp);

        //bool UpdateVehicleAttachPDF(List<CarAttachedFileVM> carAttachedFilelVM, string sharedFolderIp, string sharedOverHttp);

        //Task<bool> AddOperatorLicenseAttachPDF(List<IFormFile> formFiles, string savePath);

        Task<(string, string, string, DateTime)> DecreaseCars(DecreaseCarVMList decreaseCarVMList);
        //Task<(string, string, string, List<int>)> DecreaseCars(DecreaseCarVMList decreaseCarVMList);

        Task<(string, string, string, DateTime)> AddNewCars(ExtenseCarVMList extenseCarVMList);
        //Task<(string, string, string)> AddNewCars(ExtenseCarVMList extenseCarVMList);
        Task<object> ChangeLicenseOwnerAddress(ChangeLicenseOwnerAddressVM changeLicenseOwnerAddressVM);
        Task<object> ChangeVehicleOwnerAddress(ChangeVehicleOwnerAddressVM changeVehicleOwnerAddressVM);
        //Task<object> VehicleOwnerChangeName(ChangeVehicleOwnerAddressVM changeVehicleOwnerAddressVM);

        //commented to make same T&C From BE
        //Task<(string, string, string, DateTime)> CommonChangesProcess(CommonChangesVM dto);
        Task<(bool, bool)> CommonChangesProcess(CommonChangesVM dto);
    }
}
