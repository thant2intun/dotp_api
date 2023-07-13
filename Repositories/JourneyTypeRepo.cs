using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Repositories
{
    public class JourneyTypeRepo : IJourneyType
    {
        private readonly ApplicationDbContext _context;
        public JourneyTypeRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddJourneyType(JourneyTypeVM journeyType)
        {
            var journey = new JourneyType()
            {
                JourneyTypeLong = journeyType.JourneyTypeLong,
                JourneyTypeShort = journeyType.JourneyTypeShort
            };
            _context.JourneyTypes.Add(journey);
            _context.SaveChanges();
        }

        public bool DeleteJourneyType(int? id)
        {
            var journey = _context.JourneyTypes.Find(id); 

            if(journey== null) return false;

            _context.JourneyTypes.Remove(journey);
            _context.SaveChanges();
            return true;
        }

        public JourneyType GetJourneyTypeById(int? id)
        {
            var journey = _context.JourneyTypes.Where(x => x.JourneyTypeId == id).FirstOrDefault();
            return journey;
        }

        public IEnumerable<JourneyType> GetJourneyTypeList()
        {
            return _context.JourneyTypes.AsNoTracking().ToList();
        }

        public bool UpdateJourneyType(int? id, JourneyTypeVM journeyType)
        {
            var journey = _context.JourneyTypes.Find(id);

            if (journey == null) return false;

            journey.JourneyTypeLong = journeyType.JourneyTypeLong;
            journey.JourneyTypeShort = journeyType.JourneyTypeShort;

            _context.JourneyTypes.Update(journey);
            _context.SaveChanges();
            return true;
        }
    }
}
