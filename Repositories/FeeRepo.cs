using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Repositories
{
    public class FeeRepo : IFee
    {
        private readonly ApplicationDbContext _context;
        public FeeRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Fee>> getFeeList()
        {
            var result = await _context.Fees.ToListAsync();
            return result;
        }
        public async Task<Fee> getFeeById(int id)
        {
            var fee= await _context.Fees.Where(s => s.FeesId == id).FirstOrDefaultAsync();
            return fee;
        }
        public async Task<bool> Create(FeeVM feeVM)
        {
            
            var fee = new Fee()
            {
                RegistrationFees = feeVM.RegistrationFees,
                RegistrationCharges = feeVM.RegistrationCharges,
                CertificateFees = feeVM.CertificateFees,
                PartOneFees = feeVM.PartOneFees,
                PartTwoFees = feeVM.PartTwoFees,
                TriangleFees = feeVM.TriangleFees,                
                MinCars = feeVM.MinCars,
                MaxCars = feeVM.MaxCars,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now,
                VehicleWeightId = feeVM.VehicleWeightId,
                JourneyTypeId = feeVM.JourneyTypeId
            };
            await _context.Fees.AddAsync(fee);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Update(int id, FeeVM feeVM)
        {
            var fee = _context.Fees.Find(id);
            if (fee != null)
            {
                fee.RegistrationFees = feeVM.RegistrationFees;
                fee.RegistrationCharges = feeVM.RegistrationCharges;
                fee.CertificateFees = feeVM.CertificateFees;
                fee.PartOneFees = feeVM.PartOneFees;
                fee.PartTwoFees = feeVM.PartTwoFees;
                fee.TriangleFees = feeVM.TriangleFees;
                fee.MinCars = feeVM.MinCars;
                fee.MaxCars = feeVM.MaxCars;
                fee.UpdatedDate = DateTime.Now;
                fee.VehicleWeightId = feeVM.VehicleWeightId;
                fee.JourneyTypeId = feeVM.JourneyTypeId;
                _context.Fees.Update(fee);
                await _context.SaveChangesAsync();
                return true;
            };
            return false;
        }

        public void Delete(int id)
        {
            var fee= _context.Fees.Find(id);
            if (fee != null)
            {
                _context.Fees.Remove(fee);
                _context.SaveChangesAsync();
            }

        }
       
    }
}
