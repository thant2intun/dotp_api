using DOTP_BE.Interfaces;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDelivery _repo;
        public DeliveryController(IDelivery repo)
        {
            _repo = repo;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllDeliveries()
        {
            var deliveries = await _repo.getDeliveryList();
            return Ok(deliveries);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeliveryById([FromRoute] int id)
        {
            var delivery = await _repo.getDeliveryById(id);
            if (delivery == null)
            {
                return BadRequest();
            }
            return Ok(delivery);
        }
        [HttpPost]
        public IActionResult Create([FromBody] DeliveryVM deliveryVM)
        {
            var delivery = _repo.Create(deliveryVM);
            if (delivery)
            {
                return Ok();
            }
            return BadRequest("Delivery already exists.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDelivery(int id, [FromBody] DeliveryVM deliveryVM)
        {
            var OUpdate = _repo.Update(id, deliveryVM);
            if (OUpdate)
            {
                return Ok();
            }
            return BadRequest("Delivery already exists.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeliveryById([FromRoute] int id)
        {
            _repo.Delete(id);
            return Ok();
        }

    }
}
