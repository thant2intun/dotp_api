using DOTP_BE.Interfaces;
using DOTP_BE.ViewModel;
using DOTP_BE.ViewModel.MobileResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class NRCController : ControllerBase
    {
        private readonly INRC _repo;
        public NRCController(INRC repo)
        {
            _repo = repo;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllNRC()
        {
            var nrc = await _repo.getNRCList();
            return Ok(nrc);
        }
        [HttpGet("GetAllNRCByNRCNumber/{nrc_number}")]
        public async Task<IActionResult> GetAllNRCByNRCNumber([FromRoute] int nrc_number)
        {
            var nrc = await _repo.getNRCListByNRCNumber(nrc_number);
            if (nrc == null)
            {
                return BadRequest();
            }
            return Ok(nrc);
        }

        [HttpGet("GetAllNRCByNRCNumberMobile/{nrc_number}")] //TZT For Mobile Added 040723
        public async Task<IActionResult> GetAllNRCByNRCNumberMobile([FromRoute] int nrc_number)
        {
            var nrc = await _repo.getNRCListByNRCNumber(nrc_number);
            if (nrc.Count() == 0 || nrc == null)
              return BadRequest(new ResponseMessage() { Status=false,Message= "This nrc number does not exist!" });
            else
              return Ok(new NRCMobileResponse() { Status=true,Message="success",Data=nrc});
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNRCById([FromRoute] int id)
        {
            var nrc = await _repo.getNRCById(id);
            if (nrc == null)
            {
                return BadRequest();
            }
            return Ok(nrc);
        }
        [HttpPost]
        public IActionResult Create([FromBody] NRCVM nrcVM)
        {
            var nrc = _repo.Create(nrcVM);
            if (nrc)
            {
                return Ok();
            }
            return BadRequest("NRC already exists.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNRC(int id, [FromBody] NRCVM nrcVM)
        {
            var OUpdate = _repo.Update(id, nrcVM);
            if (OUpdate)
            {
                return Ok();
            }
            return BadRequest("NRC already exists.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNRCById([FromRoute] int id)
        {
            _repo.Delete(id);
            return Ok();
        }

    }
}
