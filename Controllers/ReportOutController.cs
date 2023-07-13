using DOTP_BE.Interfaces;
using DOTP_BE.Repositories;
using DOTP_BE.ViewModel;
using DOTP_BE.ViewModel.AdminResponses;
using DOTP_BE.ViewModel.ReportResponses;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportOutController : ControllerBase
    {
        private readonly IReportOut _reportOut;
        private readonly IConfiguration _configuration;

        public ReportOutController(IReportOut repo, IConfiguration config)
        {
            _reportOut = repo;
            _configuration = config;
        }

        [HttpPost("ReportData")]
        public IActionResult ReportData(ReportListData r)
        
        {
            //DashboardData res = new DashboardData();
            var res = _reportOut.GetReportData(r);
            return Ok(res);
        }        

    }
}
