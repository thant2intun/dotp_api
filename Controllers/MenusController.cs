using DOTP_BE.Interfaces;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenusController : ControllerBase
    {
        private readonly IMenus _menuRepo;
        public MenusController(IMenus menu)
        {
            _menuRepo = menu;
        }

        [HttpGet]
        public JsonResult GetMenus()
        {
            var res = _menuRepo.GetMenusLst();
            return new JsonResult(res);
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var res = _menuRepo.GetById(id);
            return Ok(res);
        }

        [HttpPost("MenuCreOrUpd")]
        public async Task<IActionResult> CreOrUpd(MenuVM vm)
        {
            if(vm != null)
            {
                string res = await _menuRepo.CreOrUpd(vm);
                return Ok(res);
                //if(res != null)
                //{
                //    return Ok(res);
                //}
                //else
                //{
                //    return BadRequest(res);
                //}
            }
            else
            {
                return BadRequest("Don't have data!");
            }
            
        }

        [HttpDelete("MenuDel/{id}")]
        public IActionResult Delete(int id)
        {
            bool res = _menuRepo.Delete(id);
            return Ok(res);
        }
    }
}
