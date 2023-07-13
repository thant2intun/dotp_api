using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Xml.Linq;

namespace DOTP_BE.Repositories
{
    public class KALA_YGNCarsRepo : IKALA_YGNCars
    {
        private readonly ApplicationDbContext _context;
        public KALA_YGNCarsRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(KALA_YGNCarsVM kala_ygnCarVM)
        {
            var kal_ygnCar = new KALA_YGNCars()
            {
                REG_NO = kala_ygnCarVM.REG_NO,
                MAKE_MODEL = kala_ygnCarVM.MAKE_MODEL,
                TYPE = kala_ygnCarVM.TYPE,
                VEH_WT = kala_ygnCarVM.VEH_WT,
                PAYLOAD = kala_ygnCarVM.PAYLOAD,
                OWNER = kala_ygnCarVM.OWNER,
                D_E = kala_ygnCarVM.D_E,
                LOCATION = kala_ygnCarVM.LOCATION,
                NAME = kala_ygnCarVM.NAME,
                NRC_NO = kala_ygnCarVM.NRC_NO,
                HOUSE_NO = kala_ygnCarVM.HOUSE_NO,
                RD_ST = kala_ygnCarVM.RD_ST,
                QTR = kala_ygnCarVM.QTR,
                TSP = kala_ygnCarVM.TSP,
                Address = kala_ygnCarVM.Address 
            };
            await _context.Kala_YgnCars.AddAsync(kal_ygnCar);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var kal_ygnCar = await _context.Kala_YgnCars.FindAsync(id);
            if (kal_ygnCar == null) return false;

            _context.Kala_YgnCars.Remove(kal_ygnCar);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<KALA_YGNCars> getKALA_YGNCarsById(int id)
        {
            var kal_ygnCar = await _context.Kala_YgnCars.Where(c => c.Carid == id).FirstOrDefaultAsync();
            return kal_ygnCar;
        }

        public async Task<List<KALA_YGNCars>> getKALA_YGNCarsList()
        {
            return await _context.Kala_YgnCars.AsNoTracking().ToListAsync();
        }

        public async Task<bool> Update(int id, KALA_YGNCarsVM kala_ygnCarVM)
        {
            var kal_ygnCar = await _context.Kala_YgnCars.FindAsync(id);
            if(kal_ygnCar == null) return false;

            #region vm to model
            kal_ygnCar.REG_NO = kala_ygnCarVM.REG_NO;
            kal_ygnCar.MAKE_MODEL = kala_ygnCarVM.MAKE_MODEL;
            kal_ygnCar.TYPE = kala_ygnCarVM.TYPE;
            kal_ygnCar.VEH_WT = kala_ygnCarVM.VEH_WT;
            kal_ygnCar.PAYLOAD = kala_ygnCarVM.PAYLOAD;
            kal_ygnCar.OWNER = kala_ygnCarVM.OWNER;
            kal_ygnCar.D_E = kala_ygnCarVM.D_E;
            kal_ygnCar.LOCATION = kala_ygnCarVM.LOCATION;
            kal_ygnCar.NAME = kala_ygnCarVM.NAME;
            kal_ygnCar.NRC_NO = kala_ygnCarVM.NRC_NO;
            kal_ygnCar.HOUSE_NO = kala_ygnCarVM.HOUSE_NO;
            kal_ygnCar.RD_ST = kala_ygnCarVM.RD_ST;
            kal_ygnCar.QTR = kala_ygnCarVM.QTR;
            kal_ygnCar.TSP = kala_ygnCarVM.TSP;
            kal_ygnCar.Address = kala_ygnCarVM.Address;
            #endregion
            _context.Kala_YgnCars.Update(kal_ygnCar);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
