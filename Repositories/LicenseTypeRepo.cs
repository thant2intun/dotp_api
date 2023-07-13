using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Repositories
{
    public class LicenseTypeRepo : ILicenseType
    {
        private readonly ApplicationDbContext _context;

        public LicenseTypeRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddLicenseType(LicenseTypeVM licenseType)
        {
            var license = new LicenseType()
            {
                LicenseTypeLong = licenseType.LicenseTypeLong,
                LicenseTypeShort = licenseType.LicenseTypeShort
            };
            _context.LicenseTypes.Add(license);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteLicenseType(int? id)
        {
            var license = await _context.LicenseTypes.FindAsync(id);

            if (license == null) return false;

            _context.LicenseTypes.Remove(license);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<LicenseType> GetLicenseTypeById(int? id)
        {
            var license = await _context.LicenseTypes.Where(x => x.LicenseTypeId == id).FirstOrDefaultAsync();
            return license;
        }

        public async Task<List<LicenseType>> GetLicenseTypeList()
        {
            return await _context.LicenseTypes.AsNoTracking().ToListAsync();
        }

        public async Task<bool> UpdateLicenseType(int? id, LicenseTypeVM licenseType)
        {
            var journey = await _context.LicenseTypes.FindAsync(id);

           if (journey == null) return false;

            journey.LicenseTypeLong= licenseType.LicenseTypeLong;
            journey.LicenseTypeShort= licenseType.LicenseTypeShort;

            _context.LicenseTypes.Update(journey);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
