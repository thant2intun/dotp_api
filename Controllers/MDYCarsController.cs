using DOTP_BE.Interfaces;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MDYCarsController : ControllerBase
    {
        private readonly IMDYCars _iMdyCar;
        public MDYCarsController(IMDYCars iMdyCar)
        {
            _iMdyCar = iMdyCar;
        }

        [HttpGet("MDYCarsList")]
        public async Task<IActionResult> GetMDYCarsList()
        {
            return Ok(await _iMdyCar.getMDYCarsList());
        }

        [HttpPost("AddMDYCar")]
        public async Task<IActionResult> AddMDYCar(MDYCarsVM mdyCarVM)
        {
            await _iMdyCar.Create(mdyCarVM);
            return Ok();
        }

        [HttpGet("MDYCarById")]
        public async Task<IActionResult> MDYCarByID(int id)
        {
            if (id == null || id == 0)
                return BadRequest("Invalid Id");
            var mdyCar = await _iMdyCar.getMDYCarsById(id);
            if (mdyCar == null)
                return NotFound();
            return Ok(mdyCar);
        }

        [HttpDelete("MDYCarDelete")]
        public async Task<IActionResult> MDYCarDelete(int id)
        {
            if (id == null || id == 0)
                return BadRequest("Invalid Id");
            bool oky = await _iMdyCar.Delete(id);
            if (oky)
                return Ok("Delete successful");
            return NotFound();
        }

        [HttpPut("MDYCarUpdate")]
        public async Task<IActionResult> MDYCarUpdate(int id, MDYCarsVM mdyCarVM)
        {
            if (id == null || id == 0)
                return BadRequest("Invalid Id");
            bool oky = await _iMdyCar.Update(id, mdyCarVM);
            if (oky)
                return Ok("Update successful");
            return NotFound();
        }
    }
}
