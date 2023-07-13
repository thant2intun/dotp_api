using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Repositories
{
    public class DeliveryRepo : IDelivery
    {
        private readonly ApplicationDbContext _context;
        public DeliveryRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Delivery>> getDeliveryList()
        {
            var result = await _context.Deliveries.ToListAsync();
            return result;
        }
        public async Task<Delivery> getDeliveryById(int id)
        {
            var delivery = await _context.Deliveries.Where(s => s.DeliveryId == id).FirstOrDefaultAsync();
            return delivery;
        }
        public bool Create(DeliveryVM deliveryVM)
        {
            var delivery = new Delivery()
            {
                Name = deliveryVM.Name,
                Address = deliveryVM.Address,
                Phone = deliveryVM.Phone,
                Township_Name = deliveryVM.Township_Name
            };
            _context.Deliveries.Add(delivery);
            _context.SaveChanges();
            return true;
        }
        public bool Update(int id, DeliveryVM deliveryVM)
        {
            var delivery = _context.Deliveries.Find(id);
            if (delivery != null)
            {
                delivery.Name = deliveryVM.Name;
                delivery.Address = deliveryVM.Address;
                delivery.Phone = deliveryVM.Phone;
                delivery.Township_Name = deliveryVM.Township_Name;
                _context.Update(delivery);
                _context.SaveChanges();
                return true;
            };
            return false;
        }

        public void Delete(int id)
        {
            var delivery = _context.Deliveries.Find(id);
            if (delivery != null)
            {
                _context.Deliveries.Remove(delivery);
                _context.SaveChangesAsync();
            }

        }

    }
}
