using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class LicenseOnlyController : ControllerBase
    {
        private readonly ILicenseOnly _repo;
        public LicenseOnlyController(ILicenseOnly repo)
        {
            _repo = repo;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllLicenseOnly()
        {
            var licenseOnlys = await _repo.getLicenseOnlyList();
            return Ok(licenseOnlys);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLicenseOnlyById([FromRoute] int id)
        {
            var licenseOnly = await _repo.getLicenseOnlyById(id);
            if (licenseOnly == null)
            {
                return BadRequest();
            }
            return Ok(licenseOnly);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LicenseOnlyVM licenseOnlyVM)
        {
            var licenseOnly = await _repo.Create(licenseOnlyVM);
            if (licenseOnly)
            {
                return Ok();
            }
            return BadRequest("Already exists.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicenseOnly(int id, [FromBody] LicenseOnlyVM licenseOnlyVM)
        {
            var OUpdate = await _repo.Update(id, licenseOnlyVM);
            if (OUpdate)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLicenseOnlyById([FromRoute] int id)
        {
            _repo.Delete(id);
            return Ok();
        }

    }
}
