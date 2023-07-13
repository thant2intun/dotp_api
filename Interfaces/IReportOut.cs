using DOTP_BE.ViewModel;
using DOTP_BE.ViewModel.AdminResponses;
using DOTP_BE.ViewModel.ReportResponses;

namespace DOTP_BE.Interfaces
{
    public interface IReportOut
    {
        //ReportOutListVM GetReportOut();

        ReportListData GetReportData(ReportListData data);
    }
}
