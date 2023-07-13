using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Repositories
{
    public class VehicleWeightRepo : IVehicleWeight
    {
        private readonly ApplicationDbContext _context;
        public VehicleWeightRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<VehicleWeight>> getVehicleWeight()
        {
            var result = await _context.VehicleWeights.ToListAsync();
            return result;
        }
        public async Task<VehicleWeight> getVehicleById(int id)
        {
            var vehicle = await _context.VehicleWeights.Where(s => s.VehicleWeightId == id).FirstOrDefaultAsync();
            return vehicle;
        }
        public bool IsExist(VehicleWeightVM vehicleWeightVM)
        {
            bool isExist = false;
            isExist = _context.VehicleWeights.Any(x => x.VehicleType == vehicleWeightVM.VehicleType);
            return isExist;
        }
        public async Task Create(VehicleWeightVM modelVM)
        {
            VehicleWeight vehicle = new VehicleWeight();
            vehicle.VehicleType = modelVM.VehicleType;
            vehicle.CreatedDate = DateTime.Now;            
            vehicle.CreatedBy = "Admin";
            _context.VehicleWeights.Add(vehicle);
            await _context.SaveChangesAsync();
        }
        public async Task Update(int id, VehicleWeightVM modelVM)
        {
            var model = await _context.VehicleWeights.Where(x => x.VehicleWeightId == id).FirstOrDefaultAsync();
            if(model != null)
            {
                model.VehicleType = modelVM.VehicleType;
                model.UpdatedDate = DateTime.Now;
                _context.VehicleWeights.Update(model);
                await _context.SaveChangesAsync();
            }
        }
        public async Task Delete(int id)
        {
            var vehicleWeight = _context.VehicleWeights.Find(id);
            if (vehicleWeight != null)
            {
                _context.VehicleWeights.Remove(vehicleWeight);
                await _context.SaveChangesAsync();
            }
        }
    }
}
