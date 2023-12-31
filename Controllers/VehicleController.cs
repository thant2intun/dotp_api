﻿using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.ViewModel;
using DOTP_BE.ViewModel.AdminResponses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicle _repo;
        private readonly ApplicationDbContext _context;
        public VehicleController(IVehicle repo, ApplicationDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicle = await _repo.getVehicleList();
            return Ok(vehicle);
        }

        [HttpGet("{formMode}/{transactionId}/{status}")]
        public async Task<IActionResult> GetVehicleById([FromRoute]  string formMode, string transactionId, string status)
        {
            var vehicles = await _repo.getVehicleById(formMode, transactionId, status);
            if (vehicles == null)
                return NotFound();
            return Ok(vehicles);
        }

        //[HttpGet("{formMode}/{transactionId}/{status}")]
        //public async Task<IActionResult> GetVehicleById([FromRoute] string formMode, string transactionId, string status)
        //{
        //    var vehicles = await _repo.getVehicleById(formMode, transactionId, status);
        //    if (vehicles == null)
        //        return NotFound();
        //    return Ok(vehicles);
        //}

        [HttpGet("{transactionId}/{status}")]
        public async Task<IActionResult> GetVehicleById([FromRoute] string transactionId, string status)
         {
            var vehicles = await _repo.getVehicleById(transactionId, status);
            if (vehicles == null)
                return NotFound();
            return Ok(vehicles);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] VehicleVM vehicleVM)
        {
            var vehicle = await _repo.Create(vehicleVM);
            if (vehicle)
            {
                return Ok();
            }
            return BadRequest("Vehicle already exists.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] VehicleVM vehicleVM)
        {
            var OUpdate = await _repo.Update(id, vehicleVM);
            if (OUpdate)
            {
                return Ok();
            }
            return BadRequest("Vehicle already exists.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicleById([FromRoute] int id)
        {
            _repo.Delete(id);
            return Ok();
        }

        #region get VehicleListByStatus Worked
        //[HttpGet("VehicleListByStatus/{status}")]
        //public async Task<IActionResult> VehicleListByStatus(string status)
        //{         
        //    return Ok(await _repo.getVehicleListByStatus(status));
        //}
        #endregion

        //get data fromDate toDate is more light weight than get data without duration
        //[HttpGet("VehicleListByStatus/{status}/{fromDate}/{toDate}")]
        //public async Task<IActionResult> getVehicleListByStatusAndDate(string status, string fromDate, string toDate)
        //{
        //    var result = await _repo.getVehicleListByStatusAndDate(status, fromDate, toDate);
        //    if (result == null)
        //        return NotFound();
        //    return Ok(result);
        //}

        #region ****** search by all parameters ******
        //get data fromDate toDate is more light weight than get data without duration(06/07/2023)
        [HttpPost("VehicleListByStatus")]
        public async Task<IActionResult> VehicleListByStatus(ExtenLicenseDbSearchVM dto)
        {
            var result = await _repo.getVehicleListByStatus(dto);   
            return Ok(result);
        }

        [HttpPost("VehicleListByOtherStatus")]
        public async Task<IActionResult> VehicleListByOtherStatus(ExtenLicenseDbSearchVM dto)
        {
            var result = await _repo.getVehicleListByOtherStatus(dto);
            return Ok(result);
        }

        [HttpGet("VehicleListByStatusNOTUSE")]
        public async Task<IActionResult> VehicleListByStatusNOTUSE([FromQuery] ExtenLicenseDbSearchVM dto)
        {
            var result = await _repo.getVehicleListByStatus(dto);
            return Ok(result);
        }
        #endregion

        [HttpGet("vehicleDetailToCheckById/{vId}")]
        public async Task<IActionResult> VehicleDetailToCheckById(int vId)
        {
            return Ok(await _repo.VehicleDetailToCheckById(vId));
        }

        [HttpPut("UpdateStatusById/{id}")]
        public async Task<IActionResult> UpdateStatusById(int id, [FromForm] string statusDto)
        {
            bool res = await _repo.UpdateStatusById(id, statusDto);
            if (res)
                return Ok(true);
            return BadRequest(false);
        }

        [HttpPut("OperatorLicenseConfirmReject")]
        public async Task<IActionResult> OperatorLicenseConfirmReject(OLConfirmOrRejectVM oLConfirmOrRejectVM)
        {
            (bool, string?) oky = await _repo.OperatorLicenseConfirmReject(oLConfirmOrRejectVM);
            return Ok(oky);
        }

        [HttpGet("GetVehiclByPagination/{pageNumber}/{pageSize}")]
        public async Task<IActionResult> GetVehicleByPagination(int pageNumber, int pageSize)
        {
            var res = await _repo.GetVehiclListByPagination(pageNumber, pageSize);
            return Ok(res);
        }


        //testing only
        [HttpGet("GetVehiclesDataTable")]
        public async Task<IActionResult> GetVehiclesByModel(ExtenLicenseDbSearchVM dto)
        {
            return Ok();
        }
    }
}
