using DOTP_BE.Interfaces;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRole _roleRepo;
        public RolesController(IRole rolerepo)
        {
            _roleRepo = rolerepo;
        }

        [HttpGet]
        public JsonResult GetRoles()
        {
            var res = _roleRepo.Rolelst();
            return new JsonResult(res);
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var res = _roleRepo.GetById(id);
            return Ok(res);
        }

        [HttpPost("RoleCreOrUpd")]
        public async Task<IActionResult> CreOrUpd(RolesVM vm)
        {
            if(vm != null)
            {
                var res = _roleRepo.CreOrUpd(vm);
                if(res != null)
                {
                    return Ok(res);
                }
                else
                {
                    return BadRequest(res);
                }
            }
            else
            {
                return BadRequest("Don't have data!");
            }
        }

        [HttpDelete("RoleDel/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            bool res = _roleRepo.Delete(id);
            return Ok(res);
        }

    }
}
