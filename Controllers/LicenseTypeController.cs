using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseTypeController : ControllerBase
    {
        private readonly ILicenseType _licenseType;
        public LicenseTypeController(ILicenseType licenseType)
        {
           _licenseType= licenseType;   
        }

        [HttpGet("LicenseType-List")]
        public async Task<IActionResult> GetLicenseTypeList()
        {
            return Ok(await _licenseType.GetLicenseTypeList());
        }

        [HttpPost("Add-LicenseType")]
        public async Task<IActionResult> AddLicenseType(LicenseTypeVM licenseType)
        {
            await _licenseType.AddLicenseType(licenseType);
            return Ok("Added new LicenseType");
        }

        [HttpGet("Search-LicenseTypeById")]
        public async Task<IActionResult> GetLicenseTypeById(int? id)
        {
            if (id == null || id==0)
                return BadRequest("LicenseType Can't be null or 0");
            var jounery = await _licenseType.GetLicenseTypeById(id);
            if (jounery == null)
                return NotFound();
            return Ok(jounery);
        }

        [HttpDelete("Delete-LicenseType")]
        public async Task<IActionResult> DeleteLicenseType(int? id)
        {
            if (id == null || id == 0)
                return BadRequest("LicenseType Can't be null or 0");
            bool oky = await _licenseType.DeleteLicenseType(id);
            if (oky)
                return Ok("LicenseType Delete successful");
            return NotFound("The one that you are tring to delete was not in the list at the moment");
        }

        [HttpPut("Update-LicenseType")]
        public async Task<IActionResult> UpdateLicenseType(int? id, LicenseTypeVM licenseType)
        {
            bool oky = await _licenseType.UpdateLicenseType(id, licenseType);
            if (oky)
                return Ok("JourneyType Update successful");
            return NotFound("The one that you are tring to update is not in the list at the moment");
        }


    }
}
