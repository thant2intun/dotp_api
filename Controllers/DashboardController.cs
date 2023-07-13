using DOTP_BE.Interfaces;
using DOTP_BE.ViewModel.AdminResponses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        //tzt
        private readonly IDashboard _dashboardRepo;
        private readonly IConfiguration _configuration;
        public DashboardController(IDashboard dashboard, IConfiguration config)
        {
            _dashboardRepo = dashboard;
            _configuration = config;
        }

        [HttpGet("DashboardDataByOffId")]
        public async Task<IActionResult> DashboardDataByOffId(int id)
        {
            var res = await _dashboardRepo.GetDataByOffId(id);
            return Ok(res);
        }

        [HttpPost("DashboardData")]
        public async Task<IActionResult> DashboardData(DashboardData data)
        {
            var res = await _dashboardRepo.GetDataByOffId(data);
            return Ok(res);
        }
        #region tzt 300523
        [HttpGet("DashboardLicenseDataByOffId")]
        public async Task<IActionResult> DashboardLicenseDataByOffId(int id)
        {
            var res = await _dashboardRepo.GetLicenseDataByOffId(id);
            return Ok(res);
        }

        [HttpPost("DashboardLicenseData")]
        public async Task<IActionResult> DashboardLicenseData(Filter filter)
        {
            filter.fromDate = filter.fromDate.AddDays(1);
            filter.toDate = filter.toDate.AddDays(1);
            var res = await _dashboardRepo.GetLicenseData(filter);
            return Ok(res);
        }

        [HttpGet("DashboardRestOfLicenseDataByOffId")]
        public IActionResult DashboardRestOfLicenseDataByOffId(int id)
        {
            var res = _dashboardRepo.GetRestLicenseDataToCheckByOffId(id);
            return Ok(res);
        }

        [HttpPost("DashboardRestOfLicenseData")]
        public IActionResult DashboardRestOfLicenseData(Filter filter)
        {
            filter.fromDate = filter.fromDate.AddDays(1);
            filter.toDate = filter.toDate.AddDays(1);
            var res = _dashboardRepo.GetRestLicenseDataToCheckByFilter(filter);
            return Ok(res);
        }

        #endregion

        [HttpGet("DashboardTotalExtendValuesByOffId")]
        public IActionResult DashboardTotalExtendValuesByOffId(int id)
        {
            var res = _dashboardRepo.GetDataForTotalExtendValuesById(id);
            return Ok(res);
        }

        [HttpPost("GetDataForTotalExtendValuesByFilter")]
        public IActionResult GetDataForTotalExtendValuesByFilter(Filter filter)
        {
            var res = _dashboardRepo.GetDataForTotalExtendValuesByFilter(filter);
            return Ok(res);
        }
    }
}
