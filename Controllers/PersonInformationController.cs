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
    public class PersonInformationController : ControllerBase
    {
        private readonly IPersonInformation _repo;
        public PersonInformationController(IPersonInformation repo)
        {
            _repo = repo;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllPersonInformation()
        {
            var personInformations = await _repo.getPersonInformationList();
            return Ok(personInformations);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonInformationById([FromRoute] int id)
        {
            var personInformation = await _repo.getPersonInformationById(id);
            if (personInformation == null)
            {
                return BadRequest();
            }
            return Ok(personInformation);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PersonInformationVM personInformationVM, string IsPerson)
        {
            var personInformation = await _repo.Create(personInformationVM, IsPerson);
            if (personInformation)
            {
                return Ok();
            }
            return BadRequest("Person Information already exists.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePersonInformation(int id, [FromBody] PersonInformationVM personInformationVM)
        {
            var OUpdate = await _repo.Update(id, personInformationVM);
            if (OUpdate)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonInformationById([FromRoute] int id)
        {
            _repo.Delete(id);
            return Ok();
        }

    }
}
