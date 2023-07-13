using DOTP_BE.Interfaces;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleWeightController : ControllerBase
    {
        private readonly IVehicleWeight _iVehicle;

        public VehicleWeightController(IVehicleWeight iVehicle)
        {
            _iVehicle= iVehicle;
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAllVehicle()
        {
            var allVehicle = await _iVehicle.getVehicleWeight();
            return Ok(allVehicle);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicleById([FromRoute] int id)
        {
            var vehicle = await _iVehicle.getVehicleById(id);
            if(vehicle== null)
            {
                return BadRequest();
            }
            return Ok(vehicle);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleWeightVM model)
        {
            bool isExist = _iVehicle.IsExist(model);
            if (isExist)
            {
                return BadRequest("Vehicle Type already exists.");
            }
            else
            {
                await _iVehicle.Create(model);
                return Ok();
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] VehicleWeightVM model)
        {
            
            bool isExist = _iVehicle.IsExist(model);
            if(isExist)
            {
                return BadRequest("Vehicle Type already exists.");
            }
            else
            {
                await _iVehicle.Update(id,model);
                return Ok();
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _iVehicle.Delete(id);
            return Ok();
        }

    }
}
