using DOTP_BE.Data;
using DOTP_BE.Helpers;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Repositories
{
    public class LicenseOnlyRepo : ILicenseOnly
    {
        private readonly ApplicationDbContext _context;
        public LicenseOnlyRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<LicenseOnly>> getLicenseOnlyList()
        {
            //var result = await _context.LicenseOnlys.Include(x => x.JourneyType).ToListAsync();
            var result = await _context.LicenseOnlys.ToListAsync();
            return result;
        }
        public async Task<LicenseOnly> getLicenseOnlyById(int id)
        {
            var licenseOnly = await _context.LicenseOnlys
                                        .Include(x => x.RegistrationOffice)
                                        .Include(x => x.PersonInformation)
                                        .Include(x => x.JourneyType)
                                        .Where(s => s.LicenseOnlyId == id)
                                        .FirstOrDefaultAsync();
            return licenseOnly;
        }
        public async Task<bool> Create(LicenseOnlyVM licenseOnlyVM)
        {
            var licenseOnly = new LicenseOnly()
            {
                Transaction_Id = licenseOnlyVM.Transaction_Id,
                License_Number = licenseOnlyVM.License_Number,
                LicenseOwner = licenseOnlyVM.LicenseOwner,
                NRC_Number = licenseOnlyVM.NRC_Number,
                Address = licenseOnlyVM.Address,
                Township_Name = licenseOnlyVM.Township_Name,
                Phone = licenseOnlyVM.Phone,
                Fax = licenseOnlyVM.Fax,
                AllowBusinessTitle = licenseOnlyVM.AllowBusinessTitle,
                OtherRegistrationOffice_Id = licenseOnlyVM.OtherRegistrationOffice_Id,
                IssueDate = licenseOnlyVM.IssueDate,
                IsClosed = false,
                FormMode = licenseOnlyVM.FormMode,
                IsDeleted = false,
                AttachFile_NRC = licenseOnlyVM.AttachFile_NRC,
                AttachFile_M10 = licenseOnlyVM.AttachFile_M10,
                AttachFile_OperatorLicense = licenseOnlyVM.AttachFile_OperatorLicense,
                AttachFile_Part1 = licenseOnlyVM.AttachFile_Part1,
                AttachFile_RecommandDoc1 = licenseOnlyVM.AttachFile_RecommandDoc1,
                AttachFile_RecommandDoc2 = licenseOnlyVM.AttachFile_RecommandDoc2,
                AttachFile_RecommandDoc3 = licenseOnlyVM.AttachFile_RecommandDoc3,
                AttachFile_RecommandDoc4 = licenseOnlyVM.AttachFile_RecommandDoc4,
                AttachFile_RecommandDoc5 = licenseOnlyVM.AttachFile_RecommandDoc5,
                CreatedDate = DateTime.Now,
                CreatedBy = licenseOnlyVM.CreatedBy,

                RegistrationOfficeId = licenseOnlyVM.RegistrationOfficeId,
                JourneyTypeId = licenseOnlyVM.JourneyTypeId,
                DeliveryId = licenseOnlyVM.DeliveryId,
                PersonInformationId = licenseOnlyVM.PersonInformationId
            };
            await _context.LicenseOnlys.AddAsync(licenseOnly);
            await _context.SaveChangesAsync();
            return true;         

        }
        public async Task<bool> Update(int id, LicenseOnlyVM licenseOnlyVM)
        {
            var licenseOnly = _context.LicenseOnlys.Find(id);            
            if (licenseOnly != null)
            {
                licenseOnly.Transaction_Id = licenseOnlyVM.Transaction_Id;
                licenseOnly.License_Number = licenseOnlyVM.License_Number;
                licenseOnly.LicenseOwner = licenseOnlyVM.LicenseOwner;
                licenseOnly.NRC_Number = licenseOnlyVM.NRC_Number;
                licenseOnly.Address = licenseOnlyVM.Address;
                licenseOnly.Township_Name = licenseOnlyVM.Township_Name;
                licenseOnly.Phone = licenseOnlyVM.Phone;
                licenseOnly.Fax = licenseOnlyVM.Fax;
                licenseOnly.AllowBusinessTitle = licenseOnlyVM.AllowBusinessTitle;
                licenseOnly.OtherRegistrationOffice_Id = licenseOnlyVM.OtherRegistrationOffice_Id;
                licenseOnly.IssueDate = licenseOnlyVM.IssueDate;
                licenseOnly.IsClosed = licenseOnlyVM.IsClosed;
                licenseOnly.FormMode = licenseOnlyVM.FormMode;
                licenseOnly.AttachFile_NRC = licenseOnlyVM.AttachFile_NRC;
                licenseOnly.AttachFile_M10 = licenseOnlyVM.AttachFile_M10;
                licenseOnly.AttachFile_OperatorLicense = licenseOnlyVM.AttachFile_OperatorLicense;
                licenseOnly.AttachFile_Part1 = licenseOnlyVM.AttachFile_Part1;
                licenseOnly.AttachFile_RecommandDoc1 = licenseOnlyVM.AttachFile_RecommandDoc1;
                licenseOnly.AttachFile_RecommandDoc2 = licenseOnlyVM.AttachFile_RecommandDoc2;
                licenseOnly.AttachFile_RecommandDoc3 = licenseOnlyVM.AttachFile_RecommandDoc3;
                licenseOnly.AttachFile_RecommandDoc4 = licenseOnlyVM.AttachFile_RecommandDoc4;
                licenseOnly.AttachFile_RecommandDoc5 = licenseOnlyVM.AttachFile_RecommandDoc5;
                licenseOnly.UpdatedDate = DateTime.Now;

                licenseOnly.RegistrationOfficeId = licenseOnlyVM.RegistrationOfficeId;
                licenseOnly.JourneyTypeId = licenseOnlyVM.JourneyTypeId;
                licenseOnly.DeliveryId = licenseOnlyVM.DeliveryId;
                licenseOnly.PersonInformationId = licenseOnlyVM.PersonInformationId;
                _context.LicenseOnlys.Update(licenseOnly);
                await _context.SaveChangesAsync();
                return true;
            };
            return false;
        }

        public void Delete(int id)
        {
            var licenseOnly = _context.LicenseOnlys.Find(id);
            if (licenseOnly != null)
            {
                _context.LicenseOnlys.Remove(licenseOnly);
                _context.SaveChanges();
            }

        }
        //Check Validtaion methods
        public bool PersonInformationExists(string nrcnumber, string name) // For Create
        {
            return _context.PersonInformations.Any(e => e.NRC_Number == nrcnumber &&
           e.Name == name);
        }
                
    }
}
