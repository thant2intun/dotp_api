using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Repositories
{
    public class ExtendYearRepo : IExtendYear
    {
        private readonly ApplicationDbContext _context;

        public ExtendYearRepo(ApplicationDbContext context)
        {
           _context = context;
        }

        public async Task<List<Years>> getExtendYear()
        {
            var extendYearList = await _context.ExtendYear.ToListAsync();
            return extendYearList;
        }

        public bool DeleteByID(int id)
        {
            var data = _context.ExtendYear.Find(id);
            _context.ExtendYear.Remove(data);

            bool res = _context.SaveChanges() > 0 ? true : false;
            return res;
        }

        public bool CreateExtendYear(ExtendYearVM extend_YearVM)
        {
            var year = new Years()
            {
                EngYear = extend_YearVM.EngYear,
                MyanYear = ConvertToMyanmarNumeral(extend_YearVM.EngYear),
                CreatedAt = DateTime.Now
            };

            if (!ExistOrNot(extend_YearVM))
            {
                _context.ExtendYear.Add(year);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<Years> GetExtendYearById(int id)
        {

            var data = await _context.ExtendYear.FindAsync(id);

            return data;
        }

        public bool UpdateExtendYear(int id, ExtendYearVM extendYearVM)
        {
            var data = _context.ExtendYear.Find(id);

            if (data != null)
            {
                data.EngYear = extendYearVM.EngYear;
                data.MyanYear = ConvertToMyanmarNumeral(extendYearVM.EngYear);
                data.UpdatedAt = DateTime.Now;

                if (!ExistOrNot(extendYearVM))
                {
                    _context.ExtendYear.Update(data);
                    _context.SaveChanges();
                    return true;
                }

            }
            return false;
        }

        public bool ExistOrNot(ExtendYearVM extend)
        {
            return _context.ExtendYear.Any(y => y.EngYear == extend.EngYear);
        }
        static string ConvertToMyanmarNumeral(int number)
        {
            string[] myanmarNumerals = { "၀", "၁", "၂", "၃", "၄", "၅", "၆", "၇", "၈", "၉", "၁၀" };
            string numberString = number.ToString();
            string myanmarNumeral = string.Join("", numberString.Select(digit => myanmarNumerals[digit - '0']));
            return myanmarNumeral;
        }


    }
}
