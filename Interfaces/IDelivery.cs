using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface IDelivery

    {
        Task<List<Delivery>> getDeliveryList();
        Task<Delivery> getDeliveryById(int id);
        bool Create(DeliveryVM deliveryVM);
        void Delete(int id);
        bool Update(int id, DeliveryVM deliveryVM);
    }
}
