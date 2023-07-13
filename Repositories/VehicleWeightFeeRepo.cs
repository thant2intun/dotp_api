using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Repositories
{
    public class VehicleWeightFeeRepo : IVehicleWeightFee
    {
        private readonly ApplicationDbContext _context;
        public VehicleWeightFeeRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<VehicleWeightFee>> getVehicleWeightFeeList()
        {
            var result = await _context.VehicleWeightFees.ToListAsync();
            return result;
        }
        public async Task<VehicleWeightFee> getVehicleWeightFeeById(int id)
        {
            var vehicleWeightFee= await _context.VehicleWeightFees.Where(s => s.VehicleWeightFeeId == id).FirstOrDefaultAsync();
            return vehicleWeightFee;
        }
        public async Task<bool> Create(VehicleWeightFeeVM vehicleWeightFeeVM)
        {
            
            var vehicleWeightFee = new VehicleWeightFee()
            {
                VehicleWeightId = vehicleWeightFeeVM.VehicleWeightId,
                RegistrationTitle = vehicleWeightFeeVM.RegistrationTitle,
                OneToFive = vehicleWeightFeeVM.OneToFive,
                SixToTen = vehicleWeightFeeVM.SixToTen,
                ElevenToTwenty = vehicleWeightFeeVM.ElevenToTwenty,
                TwentyOneToThirty = vehicleWeightFeeVM.TwentyOneToThirty,
                ThirtyOneToFourty = vehicleWeightFeeVM.ThirtyOneToFourty,
                FourtyOneToHundred = vehicleWeightFeeVM.FourtyOneToHundred,
                HundredOneToFiveHundred = vehicleWeightFeeVM.HundredOneToFiveHundred,
                FiveHundredOneToThousand = vehicleWeightFeeVM.FiveHundredOneToThousand,
                ThousandOneAndAbove = vehicleWeightFeeVM.ThousandOneAndAbove,
                CreatedDate = DateTime.Now,
                CreatedBy = vehicleWeightFeeVM.CreatedBy
            };
            await _context.VehicleWeightFees.AddAsync(vehicleWeightFee);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Update(int id, VehicleWeightFeeVM vehicleWeightFeeVM)
        {
            var vehicleWeightFee= _context.VehicleWeightFees.Find(id);
            if (vehicleWeightFee != null)
            {
                vehicleWeightFee.VehicleWeightId = vehicleWeightFeeVM.VehicleWeightId;
                vehicleWeightFee.RegistrationTitle = vehicleWeightFeeVM.RegistrationTitle;
                vehicleWeightFee.OneToFive = vehicleWeightFeeVM.OneToFive;
                vehicleWeightFee.SixToTen = vehicleWeightFeeVM.SixToTen;
                vehicleWeightFee.ElevenToTwenty = vehicleWeightFeeVM.ElevenToTwenty;
                vehicleWeightFee.TwentyOneToThirty = vehicleWeightFeeVM.TwentyOneToThirty;
                vehicleWeightFee.ThirtyOneToFourty = vehicleWeightFeeVM.ThirtyOneToFourty;
                vehicleWeightFee.FourtyOneToHundred = vehicleWeightFeeVM.FourtyOneToHundred;
                vehicleWeightFee.HundredOneToFiveHundred = vehicleWeightFeeVM.HundredOneToFiveHundred;
                vehicleWeightFee.FiveHundredOneToThousand = vehicleWeightFeeVM.FiveHundredOneToThousand;
                vehicleWeightFee.ThousandOneAndAbove = vehicleWeightFeeVM.ThousandOneAndAbove;
                _context.VehicleWeightFees.Update(vehicleWeightFee);
                await _context.SaveChangesAsync();
                return true;
            };
            return false;
        }

        public void Delete(int id)
        {
            var vehicleWeightFee= _context.VehicleWeightFees.Find(id);
            if (vehicleWeightFee != null)
            {
                _context.VehicleWeightFees.Remove(vehicleWeightFee);
                _context.SaveChangesAsync();
            }

        }
        
    }
}
