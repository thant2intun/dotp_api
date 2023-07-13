using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TownshipController : ControllerBase
    {
        private readonly ILogger<TownshipController> _logger;
        private readonly ITownship _township;

        public TownshipController(ILogger<TownshipController> logger, ITownship township)
        {
            _logger = logger;
            _township = township;
        }

        [HttpGet("GetTownshipList")]
        public async Task<IActionResult> GetTownshipList()
        {
            List<Township> lst = new List<Township>();
            lst = await _township.GetTownshipList();
            return Ok(lst);
        }

        [HttpGet("GetTownshipOnlyMyanmarNameList")]
        public async Task<IActionResult> GetTownshipMmNameList()
        {
            List<string> tList = new List<string>();
            tList = await _township.GetTownshipMyanmarNameList();
            return Ok(tList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTownshipByID(int id)
        {
            Township township = new Township();
            try
            {
                township = await _township.GetTownshipByID(id);
                if (township != null)
                {
                    return Ok(township);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Message : " + ex.Message.ToString(), ex);
                goto result;
            }
        result:
            return BadRequest();
        }

        [HttpPost("CreateTownship")]
        public async Task<IActionResult> CreateTownship(TownshipVM townshipVM)
        {
            var res = 0;
            try
            {
                res = await _township.CreateTownship(townshipVM);
                if (res != 0)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Message : " + ex.Message.ToString(), ex);
                goto result;
            }
        result:
            return BadRequest();
        }

        [HttpPut("UpdateTownship")]
        public async Task<IActionResult> UpdateTownship(int id, [FromBody] TownshipVM townshipVM)
        {
            var res = 0;
            try
            {
                res = await _township.UpdateTownship(id, townshipVM);
                if (res != 0)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Message : " + ex.Message.ToString(), ex);
                goto result;
            }
        result:
            return BadRequest();
        }

        [HttpDelete("DeleteTownship")]
        public async Task<IActionResult> DeleteTownship(int id)
        {
            var res = 0;
            try
            {
                res = await _township.DeleteTownship(id);
                if (res > 0)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error Message : " + ex.Message.ToString(), ex);
                goto result;
            }
        result:
            return BadRequest();
        }
   
    
        
    }
}
