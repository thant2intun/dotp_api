using DOTP_BE.Data;
using DOTP_BE.Helpers;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Repositories
{
    public class PersonInformationRepo : IPersonInformation
    {
        private readonly ApplicationDbContext _context;
        public PersonInformationRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<PersonInformation>> getPersonInformationList()
        {
            var result = await _context.PersonInformations.ToListAsync();
            return result;
        }
        public async Task<PersonInformation> getPersonInformationById(int id)
        {
            var personInfo = await _context.PersonInformations.Where(s => s.PersonInformationId == id).FirstOrDefaultAsync();
            return personInfo;
        }
        public async Task<bool> Create(PersonInformationVM personInfoVM ,string IsPerson)
        {
            string str_NRc = "";
            if (!PersonInformationExists(personInfoVM.NRC_Number, personInfoVM.Name))
            {
                if (IsPerson == "Business") // to check Is User is Business or Person
                {
                    str_NRc = personInfoVM.NRC_Number;
                }
                else
                {
                    str_NRc = personInfoVM.NRC_Number;
                    int lstindex = str_NRc.Length - 6;
                    str_NRc = personInfoVM.NRC_Number.Substring(0, lstindex) + NRCHelper.ChangeNRC_MyanToEnglish(personInfoVM.NRC_Number.Substring(lstindex, 6));

                }
                var personInfo = new PersonInformation()
                {
                    Name = personInfoVM.Name,
                    NRC_Number = str_NRc,
                    Address = personInfoVM.Address,
                    Tsp_Name = personInfoVM.Tsp_Name,
                    Phone = personInfoVM.Phone,
                    Fax = personInfoVM.Fax,
                    Email = personInfoVM.Email,
                    Notes = personInfoVM.Notes,
                    Status = "Active",
                    RegisterDate = personInfoVM.RegisterDate,
                    CreatedDate = DateTime.Now,
                    CreatedBy = personInfoVM.CreatedBy,
                    //TownshipId = personInfoVM.TownshipId,   -- temp Comment
                };
                await _context.PersonInformations.AddAsync(personInfo);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;

        }
        public async Task<bool> Update(int id, PersonInformationVM personInfoVM)
        {
            var personInfo = _context.PersonInformations.Find(id);
            string str_NRc = personInfoVM.NRC_Number;
            int lstindex = str_NRc.Length - 6;
            string str_ConvertedNRC = personInfoVM.NRC_Number.Substring(0, lstindex) + NRCHelper.ChangeNRC_MyanToEnglish(personInfoVM.NRC_Number.Substring(lstindex, 6));
            if (personInfo != null)
            {
                personInfo.Name = personInfoVM.Name;
                personInfo.Address = personInfoVM.Address;
                personInfo.NRC_Number = personInfoVM.NRC_Number;
                personInfo.Tsp_Name = personInfoVM.Tsp_Name;
                personInfo.Phone = personInfoVM.Phone;
                personInfo.Fax = personInfoVM.Fax;
                personInfo.Email = personInfoVM.Email;
                personInfo.Status = personInfoVM.Status; ;
                personInfo.Notes = personInfoVM.Notes;
                personInfo.RegisterDate = personInfoVM.RegisterDate;
                personInfo.UpdatedDate = DateTime.Now;
                //personInfo.TownshipId = personInfoVM.TownshipId; -- temp comment
                _context.PersonInformations.Update(personInfo);
                await _context.SaveChangesAsync();
                return true;
            };
            return false;
        }

        public void Delete(int id)
        {
            var personInfo = _context.PersonInformations.Find(id);
            if (personInfo != null)
            {
                _context.PersonInformations.Remove(personInfo);
                _context.SaveChangesAsync();
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
