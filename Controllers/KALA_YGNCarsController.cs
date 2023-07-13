using DOTP_BE.Interfaces;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KALA_YGNCarsController : ControllerBase
    {
        private readonly IKALA_YGNCars _iKala_YagnCars;
        public KALA_YGNCarsController(IKALA_YGNCars iKala_YagnCars)
        {
            _iKala_YagnCars = iKala_YagnCars;
        }

        [HttpGet("KALA_YGNCarsList")]
        public async Task<IActionResult> KALA_YGNCarsList()
        {
            return Ok(await _iKala_YagnCars.getKALA_YGNCarsList());
        }

        [HttpPost("AddKALA_YGNCars")]
        public async Task<IActionResult> AddMDYCar(KALA_YGNCarsVM kala_ygncarVM)
        {
            await _iKala_YagnCars.Create(kala_ygncarVM);
            return Ok();
        }

        [HttpGet("KALA_YGNCarsById")]
        public async Task<IActionResult> KALA_YGNCarsID(int id)
        {
            if (id == null || id == 0)
                return BadRequest("Invalid Id");
            var kala_ygnCar = await _iKala_YagnCars.getKALA_YGNCarsById(id);
            if (kala_ygnCar == null)
                return NotFound();
            return Ok(kala_ygnCar);
        }

        [HttpDelete("KALA_YGNCarsDelete")]
        public async Task<IActionResult> KALA_YGNCarsDelete(int id)
        {
            if (id == null || id == 0)
                return BadRequest("Invalid Id");
            bool oky = await _iKala_YagnCars.Delete(id);
            if (oky)
                return Ok("Delete successful");
            return NotFound();
        }

        [HttpPut("KALA_YGNCarsUpdate")]
        public async Task<IActionResult> KALA_YGNCarsUpdate(int id, KALA_YGNCarsVM kala_ygncarVM)
        {
            if (id == null || id == 0)
                return BadRequest("Invalid Id");
            bool oky = await _iKala_YagnCars.Update(id, kala_ygncarVM);
            if (oky)
                return Ok("Update successful");
            return NotFound();
        }
    }
}
