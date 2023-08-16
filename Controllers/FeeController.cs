using DOTP_BE.Interfaces;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class FeeController : ControllerBase
    {
        private readonly IFee _repo;
        public FeeController(IFee repo)
        {
            _repo = repo;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllFee()
        {
            var fees = await _repo.getFeeList();
            return Ok(fees);
        }

        [HttpGet("GetAllFee")]
        public async Task<IActionResult> GetAllVehicleFee()
        {
            var fees = await _repo.getVehicleFeeList();
            return Ok(fees);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeeById([FromRoute] int id)
        {
            var fee = await _repo.getFeeById(id);
            if (fee == null)
            {
                return BadRequest();
            }
            return Ok(fee);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FeeVM feeVM)
        {
            var fee = await _repo.Create(feeVM);
            if (fee)
            {
                return Ok();
            }
            return BadRequest("Fee already exists.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFee(int id, [FromBody] FeeVM feeVM)
        {
            var OUpdate = await _repo.Update(id, feeVM);
            if (OUpdate)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeeById([FromRoute] int id)
        {
            _repo.Delete(id);
            return Ok();
        }
    }
}
