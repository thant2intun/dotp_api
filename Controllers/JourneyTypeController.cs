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
    /*[Authorize] *///comment by al 26_04_2023
    public class JourneyTypeController : ControllerBase
    {
        private readonly IJourneyType _ijourneyType;
        public JourneyTypeController(IJourneyType ijourneyType)
        {
            _ijourneyType = ijourneyType;
        }

        [HttpGet("JourneyType-List")]
        public IActionResult GetAllUser()
        {
            return Ok(_ijourneyType.GetJourneyTypeList());
        }

        [HttpPost("Add-JourneyType")]
        public IActionResult AddUser(JourneyTypeVM journeyType)
        {
            _ijourneyType.AddJourneyType(journeyType);
            return Ok("Added new journeyType");
        }

        [HttpGet("Search-JourneyById")]
        public IActionResult GetJourneyTypeById(int? id)
        {
            if (id == null || id == 0)
                return BadRequest("Journey Can't be null");
            var jounery = _ijourneyType.GetJourneyTypeById(id);
            if (jounery == null)
                return NotFound();
            return Ok(jounery);
        }

        [HttpDelete("Delete-JourneyType")]
        public IActionResult DeleteJourneyType(int? id)
        {
            if (id == null || id == 0)
                return BadRequest("Journey Can't be null");
            bool oky = _ijourneyType.DeleteJourneyType(id);
            if (oky)
                return Ok("JourneyType Delete successful");
            return NotFound("The one that you are tring to delete was not in the list at the moment");
        }

        [HttpPut("Update-JourneyType")]
        public IActionResult UpdateJourneyType(int? id, JourneyTypeVM journeyType)
        {
            bool oky = _ijourneyType.UpdateJourneyType(id, journeyType);
            if (oky)
                return Ok("JourneyType Update successful");
            return NotFound("The one that you are tring to update is not in the list at the moment");
        }

    }
}
