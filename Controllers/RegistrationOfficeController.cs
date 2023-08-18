using DOTP_BE.Data;
using DOTP_BE.Model;
using DOTP_BE.Repositories;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DOTP_BE.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationOfficeController : ControllerBase
    {
        private readonly IRegistrationOffice _repo;
        public RegistrationOfficeController(IRegistrationOffice repo)
        {
            _repo = repo;
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAllOffice()
        {
            var allOffice =await _repo.getRegistrationOfficeList();
            return Ok(allOffice);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOfficeById([FromRoute] int id)
        {
            var Office = await _repo.getRegistrationOfficeById(id);
            if (Office == null)
            {
                return BadRequest();
            }
            return Ok(Office);
        }

        [HttpPost]
        public  IActionResult Create([FromBody] RegistrationOfficeVM registrationOfficevm)
        {
            var office =  _repo.Create(registrationOfficevm);
            if (office){
                return Ok();
            }
            return BadRequest("Office Name already exists.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateOffice(int id, [FromBody] RegistrationOfficeVM registrationOfficevm)
        {
            var OUpdate = _repo.Update(id, registrationOfficevm);
            if(OUpdate)
            {
                return Ok();
            }
            return BadRequest("Office Name already exists.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOfficeById([FromRoute] int id)
        {
            _repo.Delete(id);
            return Ok();
        }

    }
}
