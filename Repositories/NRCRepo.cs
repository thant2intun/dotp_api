using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Repositories
{
    public class NRCRepo : INRC
    {
        private readonly ApplicationDbContext _context;
        public NRCRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<NRC>> getNRCList()
        {
            var result = await _context.NRCs.ToListAsync();
            return result;
        }
        public async Task<List<NRC>> getNRCListByNRCNumber(int nrcNumber)
        {
            var result = await _context.NRCs.Where(s => s.NRCNumber == nrcNumber).ToListAsync();
            return result;
        }
        public async Task<NRC> getNRCById(int id)
        {
            var nrc = await _context.NRCs.Where(s => s.NRCId == id).FirstOrDefaultAsync();
            return nrc;
        }
        public bool Create(NRCVM nrcVM)
        {
            var nrc = new NRC()
            {
                NRCCode = nrcVM.NRCCode,
                NRCEnglishCode = nrcVM.NRCEnglishCode,
                NRCMyanmarCode = nrcVM.NRCMyanmarCode,
                NRCNumber = nrcVM.NRCNumber
            };
            _context.NRCs.Add(nrc);
             _context.SaveChanges();
            return true;
        }
        public bool Update(int id, NRCVM nrcVM)
        {
            var nrc = _context.NRCs.Find(id);
            if (nrc != null)
            {
                nrc.NRCCode = nrcVM.NRCCode;
                nrc.NRCEnglishCode = nrcVM.NRCEnglishCode;
                nrc.NRCMyanmarCode = nrcVM.NRCMyanmarCode;
                nrc.NRCNumber = nrcVM.NRCNumber;
                _context.NRCs.Update(nrc);
                _context.SaveChanges();
                return true;
            };
            return false;
        }

        public void Delete(int id)
        {
            var nrc = _context.NRCs.Find(id);
            if (nrc != null)
            {
                _context.NRCs.Remove(nrc);
                _context.SaveChanges();
            }

        }

    }
}
