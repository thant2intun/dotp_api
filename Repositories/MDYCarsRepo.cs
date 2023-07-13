using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Threading.Channels;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DOTP_BE.Repositories
{
    public class MDYCarsRepo : IMDYCars
    {
        private readonly ApplicationDbContext _context;
        public MDYCarsRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(MDYCarsVM mdyCarsVM)
        {
            var mdyCar = new MDYCars()
            {
                L_IRG = mdyCarsVM.L_IRG,
                V_DATE = mdyCarsVM.V_DATE,
                CANCEL = mdyCarsVM.CANCEL,
                AIR = mdyCarsVM.AIR,
                I_RG = mdyCarsVM.I_RG,
                MAKE_MODEL = mdyCarsVM.MAKE_MODEL,
                WHEEL = mdyCarsVM.WHEEL,
                M_YEAR = mdyCarsVM.M_YEAR,
                TYPE = mdyCarsVM.TYPE,
                TYPE_8 = mdyCarsVM.TYPE_8,
                VEH_WT = mdyCarsVM.VEH_WT,
                PAYLOAD = mdyCarsVM.PAYLOAD,
                EP = mdyCarsVM.EP,
                ENGINE_NO = mdyCarsVM.ENGINE_NO,
                COLOUR = mdyCarsVM.COLOUR,
                FUEL = mdyCarsVM.FUEL,
                DR = mdyCarsVM.DR,
                OWNER = mdyCarsVM.OWNER,
                D_E = mdyCarsVM.D_E,
                LOCATION = mdyCarsVM.LOCATION,
                NAME = mdyCarsVM.NAME,
                NRC_NO = mdyCarsVM.NRC_NO,
                HOUSE_NO = mdyCarsVM.HOUSE_NO,
                RD_ST = mdyCarsVM.RD_ST,
                QTR = mdyCarsVM.QTR,
                TSP = mdyCarsVM.TSP,
                LXWXH = mdyCarsVM.LXWXH,
                WB = mdyCarsVM.WB,
                OH = mdyCarsVM.OH,
                ENGINE = mdyCarsVM.ENGINE,
                GEAR = mdyCarsVM.GEAR,
                FRAMEH = mdyCarsVM.FRAMEH,
                F_AXLE = mdyCarsVM.F_AXLE,
                B_AXLE = mdyCarsVM.B_AXLE,
                SERVICE = mdyCarsVM.SERVICE,
                STATUS = mdyCarsVM.STATUS,
                Millage = mdyCarsVM.Millage,
                VIC_no = mdyCarsVM.VIC_no,
                VIC_DE = mdyCarsVM.VIC_DE,
                CYL = mdyCarsVM.CYL,
                m_axle = mdyCarsVM.m_axle,
                f_rta = mdyCarsVM.f_rta,
                b_rta = mdyCarsVM.b_rta,
                d_rta = mdyCarsVM.d_rta,
                CypherNo = mdyCarsVM.CypherNo,
                imgFileLoc = mdyCarsVM.imgFileLoc,
                REG_NO = mdyCarsVM.REG_NO,
                Address = mdyCarsVM.Address
            };
            await _context.MdyCars.AddAsync(mdyCar);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete (int id)
        {
            var mdyCar =await _context.MdyCars.FindAsync(id);
            if (mdyCar == null) return false;

            _context.MdyCars.Remove(mdyCar);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Model.MDYCars> getMDYCarsById(int id)
        {
            var mydCar = await _context.MdyCars.Where(c => c.CarId == id).FirstOrDefaultAsync();
            return mydCar;
        }

        public async Task<List<Model.MDYCars>> getMDYCarsList()
        {
            return await _context.MdyCars.AsNoTracking().ToListAsync();
        }

        public async Task<bool> Update(int id, MDYCarsVM mdyCarsVM)
        {
            var mdyCar = await _context.MdyCars.FindAsync(id);
            if(mdyCar == null) return false;

            #region dto to Model
            mdyCar.L_IRG = mdyCarsVM.L_IRG;
            mdyCar.V_DATE = mdyCarsVM.V_DATE;
            mdyCar.CANCEL = mdyCarsVM.CANCEL;
            mdyCar.AIR = mdyCarsVM.AIR;
            mdyCar.I_RG = mdyCarsVM.I_RG;
            mdyCar.MAKE_MODEL = mdyCarsVM.MAKE_MODEL;
            mdyCar.WHEEL = mdyCarsVM.WHEEL;
            mdyCar.M_YEAR = mdyCarsVM.M_YEAR;
            mdyCar.TYPE = mdyCarsVM.TYPE;
            mdyCar.TYPE_8 = mdyCarsVM.TYPE_8;
            mdyCar.VEH_WT = mdyCarsVM.VEH_WT;
            mdyCar.PAYLOAD = mdyCarsVM.PAYLOAD;
            mdyCar.EP = mdyCarsVM.EP;
            mdyCar.ENGINE_NO = mdyCarsVM.ENGINE_NO;
            mdyCar.COLOUR = mdyCarsVM.COLOUR;
            mdyCar.FUEL = mdyCarsVM.FUEL;
            mdyCar.DR = mdyCarsVM.DR;
            mdyCar.OWNER = mdyCarsVM.OWNER;
            mdyCar.D_E = mdyCarsVM.D_E;
            mdyCar.LOCATION = mdyCarsVM.LOCATION;
            mdyCar.NAME = mdyCarsVM.NAME;
            mdyCar.NRC_NO = mdyCarsVM.NRC_NO;
            mdyCar.HOUSE_NO = mdyCarsVM.HOUSE_NO;
            mdyCar.RD_ST = mdyCarsVM.RD_ST;
            mdyCar.QTR = mdyCarsVM.QTR;
            mdyCar.TSP = mdyCarsVM.TSP;
            mdyCar.LXWXH = mdyCarsVM.LXWXH;
            mdyCar.WB = mdyCarsVM.WB;
            mdyCar.OH = mdyCarsVM.OH;
            mdyCar.ENGINE = mdyCarsVM.ENGINE;
            mdyCar.GEAR = mdyCarsVM.GEAR;
            mdyCar.FRAMEH = mdyCarsVM.FRAMEH;
            mdyCar.F_AXLE = mdyCarsVM.F_AXLE;
            mdyCar.B_AXLE = mdyCarsVM.B_AXLE;
            mdyCar.SERVICE = mdyCarsVM.SERVICE;
            mdyCar.STATUS = mdyCarsVM.STATUS;
            mdyCar.Millage = mdyCarsVM.Millage;
            mdyCar.VIC_no = mdyCarsVM.VIC_no;
            mdyCar.VIC_DE = mdyCarsVM.VIC_DE;
            mdyCar.CYL = mdyCarsVM.CYL;
            mdyCar.m_axle = mdyCarsVM.m_axle;
            mdyCar.f_rta = mdyCarsVM.f_rta;
            mdyCar.b_rta = mdyCarsVM.b_rta;
            mdyCar.d_rta = mdyCarsVM.d_rta;
            mdyCar.CypherNo = mdyCarsVM.CypherNo;
            mdyCar.imgFileLoc = mdyCarsVM.imgFileLoc;
            mdyCar.REG_NO = mdyCarsVM.REG_NO;
            mdyCar.Address = mdyCarsVM.Address;
            #endregion
            _context.MdyCars.Update(mdyCar);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
