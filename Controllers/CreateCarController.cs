using DOTP_BE.Interfaces;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateCarController : ControllerBase
    {
        private readonly ICreateCar _repo;
        private readonly IOperatorDetail _repoOperatorDetail;
        public CreateCarController(ICreateCar repo, IOperatorDetail operatorDetail)
        {
            _repo = repo;
            _repoOperatorDetail = operatorDetail;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAllCreateCars()
        {
            var createCars = await _repo.getCreateCarList();
            return Ok(createCars);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCreateCarById([FromRoute] int id)
        {
            var createCar = await _repo.getCreateCarById(id);
            if (createCar == null)
            {
                return BadRequest();
            }
            return Ok(createCar);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCarVM createCarVM)
        {
            var createCar = _repo.Create(createCarVM);
            if (createCar)
            {
                return Ok();
            }
            return BadRequest("CreateCar already exists.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCreateCar(int id, [FromBody] CreateCarVM createCarVM)
        {
            var OUpdate = _repo.Update(id, createCarVM);
            if (OUpdate)
            {
                return Ok();
            }
            return BadRequest("CreateCar already exists.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCreateCarById([FromRoute] int id)
        {
            _repo.Delete(id);
            return Ok();
        }


        [HttpPost("ExtenseCar")]
        public async Task<IActionResult> GetDataFromAngular([FromForm] ExtenseCarVMList extenseCarVMLists)
        {
            return Ok(extenseCarVMLists);
        }


        [HttpGet("CheckVehicleNumber/{vehicleNumber}")]
        public async Task<IActionResult> CheckVehicleNumber(string vehicleNumber)
        {
            var result = await _repo.CheckVehicleNumber(vehicleNumber);
            return Ok(result);
        }

        //[HttpPost("UpdateCar")]
        //public async Task<IActionResult> UpdateCar([FromForm] OperatorLicenseAttachVM operatorLicenseAttachVM)
        //{
        //    var OUpdate = _repo.UpdateCar(operatorLicenseAttachVM.CreateCar);

        //    string transactionId = "";
        //    string chalenNumber = "";
        //    if (operatorLicenseAttachVM.CarAttachedFiles != null)
        //        (transactionId, chalenNumber) = await _repoOperatorDetail.VehicleAttach(operatorLicenseAttachVM);

        //    // licenseOnly att Update
        //    bool okLU = await _repoOperatorDetail.UpdateLicenseAttach(operatorLicenseAttachVM, transactionId);

        //    if (OUpdate != null)
        //    {
        //        return Ok();
        //    }
        //    return BadRequest("CreateCar already exists.");
        //}

        //[HttpGet("checkVehicleNumberGoodToSaveOrNot")]
        //public async Task<IActionResult> CheckVehicleNoGoodToSave(string vehicleNumber)
        //{
        //    bool oky = await _repo.CheckVehicleNoGoodToSave(vehicleNumber);
        //    return Ok(oky);
        //}
    }
}
