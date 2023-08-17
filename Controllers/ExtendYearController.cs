using DOTP_BE.Interfaces;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtendYearController : ControllerBase
    {
        private readonly IExtendYear _repo;

        public ExtendYearController(IExtendYear repo)
        {
            _repo = repo;
        }

        [HttpGet("GetAllExtendYear")]
        public async Task<IActionResult> GetAllExtendYear()
        {
            var extendYear = await _repo.getExtendYear();

            return Ok(extendYear);
        }


        [HttpPost("CreateExtendYear")]
        public async Task<IActionResult> CreateExtendYear([FromBody] ExtendYearVM extend_YearVM)
        {
            var data = _repo.CreateExtendYear(extend_YearVM);

            return Ok(data);
        }


        [HttpGet("GetExtendYearById/{id}")]
        public async Task<IActionResult> GetExtendYearById(int id)
        {
            try
            {
                string data = ConvertToMyanmarNumeral(id);
                var res = await _repo.GetExtendYearById(id);

                if (res == null)
                {
                    return NotFound();
                }
                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        static string ConvertToMyanmarNumeral(int number)
        {
            string[] myanmarNumerals = { "၁", "၂", "၃", "၄", "၅", "၆", "၇", "၈", "၉", "၁၀" };
            string numberString = number.ToString();
            string myanmarNumeral = string.Join("", numberString.Select(digit => myanmarNumerals[digit - '0']));
            return myanmarNumeral;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExtendYear(int id, [FromBody] ExtendYearVM extend_YearVM)
        {
            var data = _repo.UpdateExtendYear(id, extend_YearVM);
            return Ok(data);
        }

        [HttpDelete("DeleteByID/{id}")]
        public IActionResult DeleteByID(int id)
        {
            bool res = _repo.DeleteByID(id);
            return Ok(res);
        }


    }
}
