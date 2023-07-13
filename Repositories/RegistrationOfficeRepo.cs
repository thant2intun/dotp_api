using DOTP_BE.Data;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Repositories
{
    public class RegistrationOfficeRepo : IRegistrationOffice
    {
        private readonly ApplicationDbContext _context;

        public RegistrationOfficeRepo(ApplicationDbContext context)
        {
                _context = context;
        }

        public void Delete(int id)
        {
            var office =  _context.RegistrationOffices.Find(id);
            if (office != null)
            {
                _context.RegistrationOffices.Remove(office);
                _context.SaveChanges();
            }

        }

        public async Task<RegistrationOffice> getRegistrationOfficeById(int? id)
        {
            var Office = await _context.RegistrationOffices.FindAsync(id);
            return Office;
        }

        public async Task<List<RegistrationOffice>> getRegistrationOfficeList()
        {
            var officeList = await _context.RegistrationOffices.ToListAsync();
            return officeList;
        }

        public  bool Create(RegistrationOfficeVM registrationOfficeVM)
        {
            var office = new RegistrationOffice()
            {
                OfficeLongName = registrationOfficeVM.OfficeLongName,
                OfficeShortName = registrationOfficeVM.OfficeShortName
            };
                if (!OfficeExists(registrationOfficeVM.OfficeLongName, registrationOfficeVM.OfficeShortName))
                {
                    _context.RegistrationOffices.Add(office);
                     _context.SaveChanges();
                    return true;
                }

               return false;
    
        }

        public bool Update(int id, RegistrationOfficeVM registrationOfficeVM)
        {
            var office = _context.RegistrationOffices.Find(id);
            if (office != null)
            {
                office.OfficeLongName = registrationOfficeVM.OfficeLongName;
                office.OfficeShortName = registrationOfficeVM.OfficeShortName;
            };
            if (!OfficeExistsForUpdate(id , registrationOfficeVM))
            {
                _context.RegistrationOffices.Update(office);
                _context.SaveChanges();
                return true;
            }
            return false;
        }


        //Check Validtaion methods
        public bool OfficeExists(string longName ,string shortName ) // For Create
        {
            return _context.RegistrationOffices.Any( e => e.OfficeLongName == longName || 
            e.OfficeShortName == shortName );
        }

        public bool OfficeExistsForUpdate(int id , RegistrationOfficeVM rVM) // For Update
        {
            var count = _context.RegistrationOffices.Count(e => e.OfficeId != id && e.OfficeLongName == rVM.OfficeLongName);
            count = count + (_context.RegistrationOffices.Count(e => e.OfficeId != id && e.OfficeLongName == rVM.OfficeLongName));
            
            if (count < 1)
            {
                return false;
            }
            return true;
        }

    }
}
