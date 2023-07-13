using DOTP_BE.Model;
using DOTP_BE.ViewModel;

namespace DOTP_BE.Interfaces
{
    public interface IJourneyType
    {
        IEnumerable<JourneyType> GetJourneyTypeList();

        void AddJourneyType(JourneyTypeVM journeyType);

        JourneyType GetJourneyTypeById(int? id);

        bool DeleteJourneyType(int? id);

        bool UpdateJourneyType(int? id, JourneyTypeVM journeyType);
    }
}
