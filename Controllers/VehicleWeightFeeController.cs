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
    public class VehicleWeightFeeController : ControllerBase
    {
        private readonly IVehicleWeightFee _repo;
        public VehicleWeightFeeController(IVehicleWeightFee repo)
        {
            _repo = repo;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllVehicleWeightFee()
        {
            var vehicleWeightFee = await _repo.getVehicleWeightFeeList();
            return Ok(vehicleWeightFee);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleWeightFeeById([FromRoute] int id)
        {
            var vehicleWeightFee = await _repo.getVehicleWeightFeeById(id);
            if (vehicleWeightFee == null)
            {
                return BadRequest();
            }
            return Ok(vehicleWeightFee);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleWeightFeeVM vehicleWeightFeeVM)
        {
            var vehicleWeightFee = await _repo.Create(vehicleWeightFeeVM);
            if (vehicleWeightFee)
            {
                return Ok();
            }
            return BadRequest("Vehicle Weight Fee already exists.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicleWeightFee(int id, [FromBody] VehicleWeightFeeVM vehicleWeightFeeVM)
        {
            var OUpdate = await _repo.Update(id, vehicleWeightFeeVM);
            if (OUpdate)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleWeightFeeById([FromRoute] int id)
        {
            _repo.Delete(id);
            return Ok();
        }

    }
}
