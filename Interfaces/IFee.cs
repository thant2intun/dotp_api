using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface IFee
    {
        Task<List<Fee>> getFeeList();

        Task<List<Fee>> getVehicleFeeList();

        Task<Fee> getFeeById(int id);
        Task<bool> Create(FeeVM feeVM);
        void Delete(int id);
        Task<bool> Update(int id, FeeVM feeVM);
    }
}
