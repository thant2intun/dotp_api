using DOTP_BE.Helpers;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.Models;
using DOTP_BE.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DOTP_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatorDetailController : ControllerBase
    {
        private readonly IOperatorDetail _iopeartorDetail;
        private readonly IConfiguration _iConfig;
        public OperatorDetailController(IOperatorDetail iopeartorDetail, IConfiguration iConfig)
        {
            _iopeartorDetail = iopeartorDetail;
            _iConfig = iConfig;
        }

        [HttpGet("OperatorDetailList")]
        public async Task<IActionResult> OperatorDetailList()
        {
            return Ok(await _iopeartorDetail.getOperatorDetailList());
        }

        [HttpPost("AddOperatorDetail")]
        public async Task<IActionResult> AddOperatorDetail(OperatorDetailVM operatorDetailVM)
        {
            await _iopeartorDetail.Create(operatorDetailVM);
            return Ok();
        }

        [HttpGet("OperatorDetailById")]
        public async Task<IActionResult> OperatorDetailID(int id)
        {
            if (id == 0)
                return BadRequest("Invalid Id");
            var op = await _iopeartorDetail.getOperatorDetailById(id);
            if (op == null)
                return NotFound();
            return Ok(op);
        }

        [HttpDelete("OperatorDetailDelete")]
        public async Task<IActionResult> OperatorDetailDelete(int id)
        {
            if (id == 0)
                return BadRequest("Invalid Id");
            bool oky = await _iopeartorDetail.Delete(id);
            if (oky)
                return Ok("Delete successful");
            return NotFound();
        }

        [HttpPut("OperatorDetailUpdate")]
        public async Task<IActionResult> OperatorDetailUpdate(int id, OperatorDetailVM operatorDetialVM)
        {
            if (id == 0)
                return BadRequest("Invalid Id");
            bool oky = await _iopeartorDetail.Update(id, operatorDetialVM);
            if (oky)
                return Ok("Update successful");
            return NotFound();
        }

        [HttpGet("getOperatorDetailByNRCAndLicenseNumberLong/{userId}/{operator_Id}/{license_num_long}")]
        public async Task<IActionResult> getOperatorDetailByNRCAndLicenseNumberLong(int userId, int operator_Id, string license_num_long)
        {
            if (userId == 0 || license_num_long == null)
                return BadRequest("Invalid request parameter");
            var op = await _iopeartorDetail.getOperatorDetailByNRCAndLicenseNumberLong(userId, operator_Id, license_num_long.Replace("**", "/"));
            if (op == null)
                return NotFound();
            return Ok(op);
        }

        //[HttpGet("getOperatorDetailByNRCAndLicenseNumberLong/{userId}/{license_num_long}")]
        //public async Task<IActionResult> getOperatorDetailByNRCAndLicenseNumberLong(string userId, string license_num_long)
        //{
        //    if (userId == null || license_num_long == null)
        //        return BadRequest("Invalid request parameter");
        //    var op = await _iopeartorDetail.getOperatorDetailByNRCAndLicenseNumberLong(userId, license_num_long.Replace("**", "/"));
        //    if (op == null)
        //        return NotFound();
        //    return Ok(op);
        //}

        #region work with Image
        //al_29_03_2023
        //[HttpPost("OperatorLicenseAttach")]
        //public async Task<IActionResult> AddOperatorLicenseAttach([FromForm] OperatorLicenseAttachVM operatorLicenseAttachVM)
        //{
        //    string sharedFolderIp = _iConfig.GetSection("ShareFolder:ipAddress").Value.Replace("/", "\\");
        //    string sharedOverHttp = _iConfig.GetSection("ShareFolder:ipOverHttp").Value;

        //    string firstFolderName = new CommonMethod().FilePathNameString(operatorLicenseAttachVM.licenseNumberLong);
        //    string dateFolderName = Path.Combine("Extention_Care", DateTime.Now.ToString("yyyyMMdd"));
        //    string rootFolder = Path.Combine(dateFolderName, firstFolderName);
        //    string savePath = Path.Combine(sharedFolderIp, rootFolder);


        //    try
        //    {
        //        if (!Directory.Exists(savePath))
        //            Directory.CreateDirectory(savePath);
        //    }
        //    catch (Exception e) { Console.WriteLine(e.ToString()); }

        //    string pathAttachFile_NRC = "";
        //    if (operatorLicenseAttachVM.AttachFile_NRC != null)
        //    {
        //        int index = 0;
        //        foreach (var item in operatorLicenseAttachVM.AttachFile_NRC)
        //        {
        //            if (index == 0)
        //            {
        //                bool okay = new CommonMethod().SaveImage(item, savePath + "\\" + (string.Format("{0}_{1}", new CommonMethod().FilePathNameString(operatorLicenseAttachVM.licenseNumberLong), "Font.jpg")));
        //                //share folder and shared folder Over path are not the same
        //                if (okay)
        //                    pathAttachFile_NRC = sharedOverHttp + rootFolder.Replace("\\", "/") + "/" + firstFolderName + "_Font.jpg<";
        //            }
        //            else if (index == 1)
        //            {
        //                bool okay = new CommonMethod().SaveImage(item, savePath + "\\" + (string.Format("{0}_{1}", new CommonMethod().FilePathNameString(operatorLicenseAttachVM.licenseNumberLong), "Back.jpg")));
        //                if (okay)
        //                    pathAttachFile_NRC += sharedOverHttp + rootFolder.Replace("\\", "/") + "/" + firstFolderName + "_Back.jpg";
        //            }
        //            else break;
        //            index++;
        //        }
        //    }

        //    string pathAttachFile_M10 = "";
        //    if (operatorLicenseAttachVM.AttachFile_M10 != null)
        //    {
        //        int index = 1;
        //        foreach (var item in operatorLicenseAttachVM.AttachFile_M10)
        //        {
        //            bool okay = new CommonMethod().SaveImage(item, savePath + "\\" + (string.Format("{0}_{1}", "AttachFile_M10", (index) + ".jpg")));
        //            if (okay)
        //                pathAttachFile_M10 += sharedOverHttp + rootFolder.Replace("\\", "/") + "/" + $"AttachFile_M10_{index}.jpg<";
        //            index++;
        //        }
        //    }

        //    string pathAttachFile_RecommandDoc1 = "";
        //    if (operatorLicenseAttachVM.AttachFile_RecommandDoc1 != null)
        //    {
        //        int index = 1;
        //        foreach (var item in operatorLicenseAttachVM.AttachFile_RecommandDoc1)
        //        {
        //            bool okay = new CommonMethod().SaveImage(item, savePath + "\\" + (string.Format("{0}_{1}", "pathAttachFile_RecommandDoc1", (index) + ".jpg")));
        //            if (okay)
        //                pathAttachFile_RecommandDoc1 += sharedOverHttp + rootFolder.Replace("\\", "/") + "/" + $"pathAttachFile_RecommandDoc1_{index}.jpg<";
        //            index++;
        //        }
        //    }

        //    string pathAttachFile_RecommandDoc2 = "";
        //    if (operatorLicenseAttachVM.AttachFile_RecommandDoc2 != null)
        //    {
        //        int index = 1;
        //        foreach (var item in operatorLicenseAttachVM.AttachFile_RecommandDoc2)
        //        {
        //            bool okay = new CommonMethod().SaveImage(item, savePath + "\\" + (string.Format("{0}_{1}", "AttachFile_RecommandDoc2", (index) + ".jpg")));
        //            if (okay)
        //                pathAttachFile_RecommandDoc2 += sharedOverHttp + rootFolder.Replace("\\", "/") + "/" + $"AttachFile_RecommandDoc2_{index}.jpg<";
        //            index++;
        //        }
        //    }

        //    string pathAttachFile_OperatorLicense = "";
        //    if (operatorLicenseAttachVM.AttachFile_OperatorLicense != null)
        //    {
        //        int index = 1;
        //        foreach (var item in operatorLicenseAttachVM.AttachFile_OperatorLicense)
        //        {
        //            bool okay = new CommonMethod().SaveImage(item, savePath + "\\" + (string.Format("{0}_{1}", "AttachFile_OperatorLicense", (index) + ".jpg")));
        //            if (okay)
        //                pathAttachFile_OperatorLicense += sharedOverHttp + rootFolder.Replace("\\", "/") + "/" + $"AttachFile_OperatorLicense_{index}.jpg<";
        //            index++;
        //        }
        //    }

        //    string pathAttachFile_Part1 = "";
        //    if (operatorLicenseAttachVM.AttachFile_Part1 != null)
        //    {
        //        int index = 1;
        //        foreach (var item in operatorLicenseAttachVM.AttachFile_Part1)
        //        {
        //            bool okay = new CommonMethod().SaveImage(item, savePath + "\\" + (string.Format("{0}_{1}", "AttachFile_Part1", (index) + ".jpg")));
        //            if (okay)
        //                pathAttachFile_Part1 += sharedOverHttp + rootFolder.Replace("\\", "/") + "/" + $"AttachFile_Part1_{index}.jpg<";
        //            index++;
        //        }
        //    }

        //    // car attached file
        //    if (operatorLicenseAttachVM.CarAttachedFiles != null)
        //    {
        //        bool va = _iopeartorDetail.UpdateVehicleAttach(operatorLicenseAttachVM.CarAttachedFiles, sharedFolderIp, sharedOverHttp);
        //    }

        //    var licenseOnly = new LicenseOnly()
        //    {
        //        Transaction_Id = operatorLicenseAttachVM.Transaction_Id,
        //        License_Number = operatorLicenseAttachVM.licenseNumberLong,
        //        LicenseOwner = operatorLicenseAttachVM.LicenseOwner,
        //        NRC_Number = operatorLicenseAttachVM.NRC,
        //        Address = operatorLicenseAttachVM.Address,
        //        Township_Name = operatorLicenseAttachVM.Township,
        //        Phone = operatorLicenseAttachVM.Phone,
        //        Fax = operatorLicenseAttachVM.Fax,
        //        AllowBusinessTitle = operatorLicenseAttachVM.AllowBusinessTitle,
        //        OtherRegistrationOffice_Id = operatorLicenseAttachVM.OtherRegistrationOffice_Id,
        //        IssueDate = operatorLicenseAttachVM.IssueDate,
        //        IsClosed = operatorLicenseAttachVM.IsClosed,
        //        FormMode = operatorLicenseAttachVM.FormMode,
        //        IsDeleted = operatorLicenseAttachVM.IsDeleted,

        //        AttachFile_NRC = pathAttachFile_NRC,
        //        AttachFile_M10 = pathAttachFile_M10,
        //        AttachFile_OperatorLicense = pathAttachFile_OperatorLicense,
        //        AttachFile_Part1 = pathAttachFile_Part1,
        //        AttachFile_RecommandDoc1 = pathAttachFile_RecommandDoc1,
        //        AttachFile_RecommandDoc2 = pathAttachFile_RecommandDoc1,
        //        AttachFile_RecommandDoc3 = "",
        //        AttachFile_RecommandDoc4 = "",
        //        AttachFile_RecommandDoc5 = "",

        //        RegistrationOfficeId = operatorLicenseAttachVM.RegistrationOffice_Id,
        //        JourneyTypeId = operatorLicenseAttachVM.JourneyType_Id,
        //        DeliveryId = 1,
        //        PersonInformationId = operatorLicenseAttachVM.PersonInformationId,
        //        CreatedDate = DateTime.Now,
        //        CreatedBy = operatorLicenseAttachVM.CreatedBy
        //    };
        //    bool a = await _iopeartorDetail.AddOperatorLicenseAttach(licenseOnly);

        //    return Ok(operatorLicenseAttachVM);
        //}
        #endregion

        //al_05_05_2023 (Save As PDF)
        [HttpPost("OperatorLicenseAttachNotUse")]
        public async Task<IActionResult> AddOperatorLicenseAttachNotUse([FromForm] OperatorLicenseAttachVM operatorLicenseAttachVM)
        {
            (bool, bool) response = await _iopeartorDetail.ExtendOperatorLicenseProcess(operatorLicenseAttachVM);
            return Ok(response);
        }

        [HttpPost("OperatorLicenseAttach")]
        public async Task<IActionResult> AddOperatorLicenseAttach([FromForm] OperatorLicenseAttachVM operatorLicenseAttachVM)
        {
            (bool, bool) response = await _iopeartorDetail.ExtendOperatorLicenseProcess(operatorLicenseAttachVM);
            return Ok(response);
        }

        //ok pdf
        ////al_05_05_2023 (Save As PDF)
        //[HttpPost("OperatorLicenseAttach")]
        //public async Task<IActionResult> AddOperatorLicenseAttach([FromForm] OperatorLicenseAttachVM operatorLicenseAttachVM)
        //{
        //    //folder create
        //    string rootPath = _iConfig.GetSection("Upload_FolderPath").Value;

        //    string firstFolderName = new CommonMethod().FilePathNameString(operatorLicenseAttachVM.licenseNumberLong);
        //    string dateFolderName = Path.Combine("Extention_Care", DateTime.Now.ToString("yyyyMMdd"));
        //    string rootFolder = Path.Combine(dateFolderName, firstFolderName);
        //    string savePath = Path.Combine(rootPath, rootFolder);
        //    string rootFolderR = rootFolder.Replace("\\", "/");
        //    try
        //    {
        //        if (!Directory.Exists(savePath))
        //            Directory.CreateDirectory(savePath);
        //    }
        //    catch (Exception e) { Console.WriteLine(e.ToString()); }

        //    string pathAttachFile_NRC = "";
        //    if (operatorLicenseAttachVM.AttachFile_NRC != null)
        //    {
        //        bool oky =  await _iopeartorDetail.AddOperatorLicenseAttachPDF(operatorLicenseAttachVM.AttachFile_NRC, savePath + "\\NRC.pdf");
        //        if (oky)
        //            pathAttachFile_NRC = rootFolderR + "/NRC.pdf";
        //    }

        //    string pathAttachFile_M10 = "";
        //    if (operatorLicenseAttachVM.AttachFile_M10 != null)
        //    {
        //        bool oky = await _iopeartorDetail.AddOperatorLicenseAttachPDF(operatorLicenseAttachVM.AttachFile_M10, savePath + "\\M10.pdf");
        //        if (oky)
        //            pathAttachFile_M10 = rootFolderR + "/M10.pdf";
        //    }

        //    string pathAttachFile_RecommandDoc1 = "";
        //    if (operatorLicenseAttachVM.AttachFile_RecommandDoc1 != null)
        //    {
        //        bool oky = await _iopeartorDetail.AddOperatorLicenseAttachPDF(operatorLicenseAttachVM.AttachFile_RecommandDoc1, savePath + "\\Doc1.pdf");
        //        if (oky)
        //            pathAttachFile_RecommandDoc1 = rootFolderR + "/Doc1.pdf";
        //    }

        //    string pathAttachFile_RecommandDoc2 = "";
        //    if (operatorLicenseAttachVM.AttachFile_RecommandDoc2 != null)
        //    {
        //       bool oky = await _iopeartorDetail.AddOperatorLicenseAttachPDF(operatorLicenseAttachVM.AttachFile_RecommandDoc2, savePath + "\\Doc2.pdf");
        //        if (oky)
        //            pathAttachFile_RecommandDoc2 = rootFolderR + "/Doc2.pdf";
        //    }

        //    string pathAttachFile_OperatorLicense = "";
        //    if (operatorLicenseAttachVM.AttachFile_OperatorLicense != null)
        //    {
        //        bool oky = await _iopeartorDetail.AddOperatorLicenseAttachPDF(operatorLicenseAttachVM.AttachFile_OperatorLicense, savePath + "\\OperatorLicense.pdf");
        //        if (oky)
        //            pathAttachFile_OperatorLicense = rootFolderR + "/OperatorLicense.pdf";
        //    }

        //    string pathAttachFile_Part1 = "";
        //    if (operatorLicenseAttachVM.AttachFile_Part1 != null)
        //    {
        //        bool oky = await _iopeartorDetail.AddOperatorLicenseAttachPDF(operatorLicenseAttachVM.AttachFile_Part1, savePath + "\\Part1.pdf");
        //        if (oky)
        //            pathAttachFile_Part1 = rootFolderR + "/Part1.pdf";
        //    }

        //    //car attached file
        //    string transactionId = "";
        //    if (operatorLicenseAttachVM.CarAttachedFiles != null)
        //      transactionId =  await _iopeartorDetail.VehicleAttach(operatorLicenseAttachVM, rootPath);

        //    // licenseOnly att Update
        //    var licenseOnlyDto = new ToUpdateLicenseOnlyVM()
        //    {
        //        pathAttachFile_NRC = pathAttachFile_NRC,
        //        pathAttachFile_M10 = pathAttachFile_M10,
        //        pathAttachFile_RecommandDoc1 = pathAttachFile_RecommandDoc1,
        //        pathAttachFile_RecommandDoc2 = pathAttachFile_RecommandDoc2,
        //        pathAttachFile_OperatorLicense = pathAttachFile_OperatorLicense,
        //        pathAttachFile_Part1 = pathAttachFile_Part1,
        //        transactionId = transactionId,
        //        licenseNumberLong = operatorLicenseAttachVM.licenseNumberLong,
        //        NRC = operatorLicenseAttachVM.NRC,
        //    };
        //    bool okLU = await _iopeartorDetail.UpdateLicenseAttach(licenseOnlyDto);




        //    #region changed logic so not use

        //    ////licenseOnly add (chnage to Update)
        //    //var licenseOnly = new LicenseOnly()
        //    //{
        //    //    Transaction_Id = transactionId,
        //    //    License_Number = operatorLicenseAttachVM.licenseNumberLong,
        //    //    LicenseOwner = operatorLicenseAttachVM.LicenseOwner,
        //    //    NRC_Number = operatorLicenseAttachVM.NRC,
        //    //    Address = operatorLicenseAttachVM.Address,
        //    //    Township_Name = operatorLicenseAttachVM.Township,
        //    //    Phone = operatorLicenseAttachVM.Phone,
        //    //    Fax = operatorLicenseAttachVM.Fax,
        //    //    AllowBusinessTitle = operatorLicenseAttachVM.AllowBusinessTitle,
        //    //    OtherRegistrationOffice_Id = operatorLicenseAttachVM.OtherRegistrationOffice_Id,
        //    //    IssueDate = operatorLicenseAttachVM.IssueDate,
        //    //    IsClosed = operatorLicenseAttachVM.IsClosed,
        //    //    FormMode = operatorLicenseAttachVM.FormMode,
        //    //    IsDeleted = operatorLicenseAttachVM.IsDeleted,

        //    //    AttachFile_NRC = pathAttachFile_NRC,
        //    //    AttachFile_M10 = pathAttachFile_M10,
        //    //    AttachFile_OperatorLicense = pathAttachFile_OperatorLicense,
        //    //    AttachFile_Part1 = pathAttachFile_Part1,
        //    //    AttachFile_RecommandDoc1 = pathAttachFile_RecommandDoc1,
        //    //    AttachFile_RecommandDoc2 = pathAttachFile_RecommandDoc1,
        //    //    AttachFile_RecommandDoc3 = "",
        //    //    AttachFile_RecommandDoc4 = "",
        //    //    AttachFile_RecommandDoc5 = "",

        //    //    RegistrationOfficeId = operatorLicenseAttachVM.RegistrationOffice_Id,
        //    //    JourneyTypeId = operatorLicenseAttachVM.JourneyType_Id,
        //    //    DeliveryId = 1,
        //    //    PersonInformationId = operatorLicenseAttachVM.PersonInformationId,
        //    //    CreatedDate = DateTime.Now,
        //    //    CreatedBy = operatorLicenseAttachVM.CreatedBy
        //    //};
        //    //bool a = await _iopeartorDetail.AddOperatorLicenseAttach(licenseOnly); //change to update status


        //    //add operator to db
        //    //var operatorDetail = new OperatorDetail()
        //    //{
        //    //    Transaction_Id = transactionId,
        //    //    LicenseHolderType =operatorLicenseAttachVM.LicenseHolderType,  //(need to update)
        //    //    OperatorName = operatorLicenseAttachVM.OperatorName, //(need to update)
        //    //    Address = operatorLicenseAttachVM.Address,
        //    //    ApplyDate = DateTime.Now,
        //    //    LicenseOwner = operatorLicenseAttachVM.LicenseOwner,
        //    //    RegistrationOffice_Id = operatorLicenseAttachVM.RegistrationOffice_Id,
        //    //    NRC = operatorLicenseAttachVM.NRC,
        //    //    applicant_Id = operatorLicenseAttachVM.applicant_Id, // don't know (need to update)
        //    //    Township = operatorLicenseAttachVM.Township,
        //    //    Phone = operatorLicenseAttachVM.Phone,
        //    //    Fax = operatorLicenseAttachVM.Fax,
        //    //    Email = operatorLicenseAttachVM.Email,
        //    //    ExpiredDate = DateTime.Now,
        //    //    JourneyType_Id = operatorLicenseAttachVM.JourneyType_Id,
        //    //    TotalCar = operatorLicenseAttachVM.CarAttachedFiles !=null? operatorLicenseAttachVM.CarAttachedFiles.Count:0,
        //    //    ApplyLicenseType = operatorLicenseAttachVM.licenseNumberLong.Substring(0, 1),
        //    //    FormMode = operatorLicenseAttachVM.FormMode,
        //    //    PersonInformationId = operatorLicenseAttachVM.PersonInformationId,
        //    //    VehicleId = operatorLicenseAttachVM.VehicleId,
        //    //    CreatedBy = operatorLicenseAttachVM.CreatedBy,
        //    //    CreatedDate = DateTime.Now
        //    //};

        //    //var operatorDetailVM = new OperatorDetailVM()
        //    //{
        //    //    Transaction_Id = operatorLicenseAttachVM.Transaction_Id,
        //    //    LicenseHolderType = operatorLicenseAttachVM.LicenseHolderType,
        //    //    OperatorName = operatorLicenseAttachVM.OperatorName,
        //    //    Address = operatorLicenseAttachVM.Address,
        //    //    ApplyDate = DateTime.Now,
        //    //    LicenseOwner = operatorLicenseAttachVM.LicenseOwner,
        //    //    RegistrationOffice_Id = operatorLicenseAttachVM.RegistrationOffice_Id,
        //    //    NRC = operatorLicenseAttachVM.NRC,
        //    //    applicant_Id = operatorLicenseAttachVM.applicant_Id,
        //    //    Township = operatorLicenseAttachVM.Township,
        //    //    Phone = operatorLicenseAttachVM.Phone,
        //    //    Fax = operatorLicenseAttachVM.Fax,
        //    //    Email = operatorLicenseAttachVM.Email,
        //    //    ExpiredDate = DateTime.Now.AddYears(operatorLicenseAttachVM.selectedExtenYear),
        //    //    JourneyType_Id = operatorLicenseAttachVM.JourneyType_Id,
        //    //    TotalCar = operatorLicenseAttachVM.CarAttachedFiles != null ? operatorLicenseAttachVM.CarAttachedFiles.Count : 0,
        //    //    ApplyLicenseType = operatorLicenseAttachVM.licenseNumberLong.Substring(0, 1),
        //    //    FormMode = operatorLicenseAttachVM.FormMode,
        //    //    PersonInformationId = operatorLicenseAttachVM.PersonInformationId,
        //    //    VehicleId = operatorLicenseAttachVM.VehicleId,
        //    //    CreatedBy = operatorLicenseAttachVM.CreatedBy,
        //    //    CreatedDate = DateTime.Now
        //    //};
        //    //bool okyOperator = await _iopeartorDetail.Create(operatorDetailVM);
        //    #endregion
        //    return Ok(okLU);
        //}

        #region WorkFilesOverHttpOrHttps
        //al_05_05_2023 (Save As PDF)
        //[HttpPost("OperatorLicenseAttach")]
        //public async Task<IActionResult> AddOperatorLicenseAttach([FromForm] OperatorLicenseAttachVM operatorLicenseAttachVM)
        //{
        //    //folder create
        //    string sharedFolderIp = _iConfig.GetSection("ShareFolder:ipAddress").Value.Replace("/", "\\");
        //    string sharedOverHttp = _iConfig.GetSection("ShareFolder:ipOverHttp").Value;

        //    string firstFolderName = new CommonMethod().FilePathNameString(operatorLicenseAttachVM.licenseNumberLong);
        //    string dateFolderName = Path.Combine("Extention_Care", DateTime.Now.ToString("yyyyMMdd"));
        //    string rootFolder = Path.Combine(dateFolderName, firstFolderName);
        //    string savePath = Path.Combine(sharedFolderIp, rootFolder);
        //    string rootFolderR = rootFolder.Replace("\\", "/");
        //    try
        //    {
        //        if (!Directory.Exists(savePath))
        //            Directory.CreateDirectory(savePath);
        //    }
        //    catch (Exception e) { Console.WriteLine(e.ToString()); }

        //    string pathAttachFile_NRC = "";
        //    if (operatorLicenseAttachVM.AttachFile_NRC != null)
        //    {
        //        bool oky = await _iopeartorDetail.AddOperatorLicenseAttachPDF(operatorLicenseAttachVM.AttachFile_NRC, savePath + "\\NRC.pdf");
        //        if (oky)
        //            pathAttachFile_NRC = rootFolderR + "/NRC.pdf";
        //    }

        //    string pathAttachFile_M10 = "";
        //    if (operatorLicenseAttachVM.AttachFile_M10 != null)
        //    {
        //        bool oky = await _iopeartorDetail.AddOperatorLicenseAttachPDF(operatorLicenseAttachVM.AttachFile_M10, savePath + "\\M10.pdf");
        //        if (oky)
        //            pathAttachFile_M10 = rootFolderR + "/M10.pdf";
        //    }

        //    string pathAttachFile_RecommandDoc1 = "";
        //    if (operatorLicenseAttachVM.AttachFile_RecommandDoc1 != null)
        //    {
        //        bool oky = await _iopeartorDetail.AddOperatorLicenseAttachPDF(operatorLicenseAttachVM.AttachFile_RecommandDoc1, savePath + "\\Doc1.pdf");
        //        if (oky)
        //            pathAttachFile_RecommandDoc1 = rootFolderR + "/Doc1.pdf";
        //    }

        //    string pathAttachFile_RecommandDoc2 = "";
        //    if (operatorLicenseAttachVM.AttachFile_RecommandDoc2 != null)
        //    {
        //        bool oky = await _iopeartorDetail.AddOperatorLicenseAttachPDF(operatorLicenseAttachVM.AttachFile_RecommandDoc2, savePath + "\\Doc2.pdf");
        //        if (oky)
        //            pathAttachFile_RecommandDoc2 = rootFolderR + "/Doc2.pdf";
        //    }

        //    string pathAttachFile_OperatorLicense = "";
        //    if (operatorLicenseAttachVM.AttachFile_OperatorLicense != null)
        //    {
        //        bool oky = await _iopeartorDetail.AddOperatorLicenseAttachPDF(operatorLicenseAttachVM.AttachFile_OperatorLicense, savePath + "\\OperatorLicense.pdf");
        //        if (oky)
        //            pathAttachFile_OperatorLicense = rootFolderR + "/OperatorLicense.pdf";
        //    }

        //    string pathAttachFile_Part1 = "";
        //    if (operatorLicenseAttachVM.AttachFile_Part1 != null)
        //    {
        //        bool oky = await _iopeartorDetail.AddOperatorLicenseAttachPDF(operatorLicenseAttachVM.AttachFile_Part1, savePath + "\\Part1.pdf");
        //        if (oky)
        //            pathAttachFile_Part1 = rootFolderR + "/Part1.pdf";
        //    }

        //    //car attached file
        //    string transactionId = "";
        //    if (operatorLicenseAttachVM.CarAttachedFiles != null)
        //        transactionId = await _iopeartorDetail.VehicleAttach(operatorLicenseAttachVM, sharedFolderIp, sharedOverHttp);

        //    // licenseOnly att Update
        //    var licenseOnlyDto = new ToUpdateLicenseOnlyVM()
        //    {
        //        pathAttachFile_NRC = pathAttachFile_NRC,
        //        pathAttachFile_M10 = pathAttachFile_M10,
        //        pathAttachFile_RecommandDoc1 = pathAttachFile_RecommandDoc1,
        //        pathAttachFile_RecommandDoc2 = pathAttachFile_RecommandDoc2,
        //        pathAttachFile_OperatorLicense = pathAttachFile_OperatorLicense,
        //        pathAttachFile_Part1 = pathAttachFile_Part1,
        //        transactionId = transactionId,
        //        licenseNumberLong = operatorLicenseAttachVM.licenseNumberLong,
        //        NRC = operatorLicenseAttachVM.NRC,
        //    };
        //    bool okLU = await _iopeartorDetail.UpdateLicenseAttach(licenseOnlyDto);




        //    #region changed logic so not use

        //    ////licenseOnly add (chnage to Update)
        //    //var licenseOnly = new LicenseOnly()
        //    //{
        //    //    Transaction_Id = transactionId,
        //    //    License_Number = operatorLicenseAttachVM.licenseNumberLong,
        //    //    LicenseOwner = operatorLicenseAttachVM.LicenseOwner,
        //    //    NRC_Number = operatorLicenseAttachVM.NRC,
        //    //    Address = operatorLicenseAttachVM.Address,
        //    //    Township_Name = operatorLicenseAttachVM.Township,
        //    //    Phone = operatorLicenseAttachVM.Phone,
        //    //    Fax = operatorLicenseAttachVM.Fax,
        //    //    AllowBusinessTitle = operatorLicenseAttachVM.AllowBusinessTitle,
        //    //    OtherRegistrationOffice_Id = operatorLicenseAttachVM.OtherRegistrationOffice_Id,
        //    //    IssueDate = operatorLicenseAttachVM.IssueDate,
        //    //    IsClosed = operatorLicenseAttachVM.IsClosed,
        //    //    FormMode = operatorLicenseAttachVM.FormMode,
        //    //    IsDeleted = operatorLicenseAttachVM.IsDeleted,

        //    //    AttachFile_NRC = pathAttachFile_NRC,
        //    //    AttachFile_M10 = pathAttachFile_M10,
        //    //    AttachFile_OperatorLicense = pathAttachFile_OperatorLicense,
        //    //    AttachFile_Part1 = pathAttachFile_Part1,
        //    //    AttachFile_RecommandDoc1 = pathAttachFile_RecommandDoc1,
        //    //    AttachFile_RecommandDoc2 = pathAttachFile_RecommandDoc1,
        //    //    AttachFile_RecommandDoc3 = "",
        //    //    AttachFile_RecommandDoc4 = "",
        //    //    AttachFile_RecommandDoc5 = "",

        //    //    RegistrationOfficeId = operatorLicenseAttachVM.RegistrationOffice_Id,
        //    //    JourneyTypeId = operatorLicenseAttachVM.JourneyType_Id,
        //    //    DeliveryId = 1,
        //    //    PersonInformationId = operatorLicenseAttachVM.PersonInformationId,
        //    //    CreatedDate = DateTime.Now,
        //    //    CreatedBy = operatorLicenseAttachVM.CreatedBy
        //    //};
        //    //bool a = await _iopeartorDetail.AddOperatorLicenseAttach(licenseOnly); //change to update status


        //    //add operator to db
        //    //var operatorDetail = new OperatorDetail()
        //    //{
        //    //    Transaction_Id = transactionId,
        //    //    LicenseHolderType =operatorLicenseAttachVM.LicenseHolderType,  //(need to update)
        //    //    OperatorName = operatorLicenseAttachVM.OperatorName, //(need to update)
        //    //    Address = operatorLicenseAttachVM.Address,
        //    //    ApplyDate = DateTime.Now,
        //    //    LicenseOwner = operatorLicenseAttachVM.LicenseOwner,
        //    //    RegistrationOffice_Id = operatorLicenseAttachVM.RegistrationOffice_Id,
        //    //    NRC = operatorLicenseAttachVM.NRC,
        //    //    applicant_Id = operatorLicenseAttachVM.applicant_Id, // don't know (need to update)
        //    //    Township = operatorLicenseAttachVM.Township,
        //    //    Phone = operatorLicenseAttachVM.Phone,
        //    //    Fax = operatorLicenseAttachVM.Fax,
        //    //    Email = operatorLicenseAttachVM.Email,
        //    //    ExpiredDate = DateTime.Now,
        //    //    JourneyType_Id = operatorLicenseAttachVM.JourneyType_Id,
        //    //    TotalCar = operatorLicenseAttachVM.CarAttachedFiles !=null? operatorLicenseAttachVM.CarAttachedFiles.Count:0,
        //    //    ApplyLicenseType = operatorLicenseAttachVM.licenseNumberLong.Substring(0, 1),
        //    //    FormMode = operatorLicenseAttachVM.FormMode,
        //    //    PersonInformationId = operatorLicenseAttachVM.PersonInformationId,
        //    //    VehicleId = operatorLicenseAttachVM.VehicleId,
        //    //    CreatedBy = operatorLicenseAttachVM.CreatedBy,
        //    //    CreatedDate = DateTime.Now
        //    //};

        //    //var operatorDetailVM = new OperatorDetailVM()
        //    //{
        //    //    Transaction_Id = operatorLicenseAttachVM.Transaction_Id,
        //    //    LicenseHolderType = operatorLicenseAttachVM.LicenseHolderType,
        //    //    OperatorName = operatorLicenseAttachVM.OperatorName,
        //    //    Address = operatorLicenseAttachVM.Address,
        //    //    ApplyDate = DateTime.Now,
        //    //    LicenseOwner = operatorLicenseAttachVM.LicenseOwner,
        //    //    RegistrationOffice_Id = operatorLicenseAttachVM.RegistrationOffice_Id,
        //    //    NRC = operatorLicenseAttachVM.NRC,
        //    //    applicant_Id = operatorLicenseAttachVM.applicant_Id,
        //    //    Township = operatorLicenseAttachVM.Township,
        //    //    Phone = operatorLicenseAttachVM.Phone,
        //    //    Fax = operatorLicenseAttachVM.Fax,
        //    //    Email = operatorLicenseAttachVM.Email,
        //    //    ExpiredDate = DateTime.Now.AddYears(operatorLicenseAttachVM.selectedExtenYear),
        //    //    JourneyType_Id = operatorLicenseAttachVM.JourneyType_Id,
        //    //    TotalCar = operatorLicenseAttachVM.CarAttachedFiles != null ? operatorLicenseAttachVM.CarAttachedFiles.Count : 0,
        //    //    ApplyLicenseType = operatorLicenseAttachVM.licenseNumberLong.Substring(0, 1),
        //    //    FormMode = operatorLicenseAttachVM.FormMode,
        //    //    PersonInformationId = operatorLicenseAttachVM.PersonInformationId,
        //    //    VehicleId = operatorLicenseAttachVM.VehicleId,
        //    //    CreatedBy = operatorLicenseAttachVM.CreatedBy,
        //    //    CreatedDate = DateTime.Now
        //    //};
        //    //bool okyOperator = await _iopeartorDetail.Create(operatorDetailVM);
        //    #endregion
        //    return Ok(okLU);
        //}
        #endregion

        #region file save old method
        //al_29_03_2023
        //[HttpPost("OperatorLicenseAttach1")]
        //public async Task<IActionResult> AddOperatorLicenseAttach1([FromForm] OperatorLicenseAttachVM operatorLicenseAttachVM)
        //{
        //string firsFolderName = new CommonMethod().FilePathNameString(operatorLicenseAttachVM.licenseNumberLong);
        //string dateFolderName = Path.Combine("Extention_Care", DateTime.Now.ToString("yyyyMMdd"));
        //string rootFolder = Path.Combine(dateFolderName, firsFolderName);
        //string savePath = Path.Combine("E:\\Data", rootFolder);
        ////string dir = Path.GetDirectoryName(savePath);
        //try{
        //    if (!Directory.Exists(savePath))
        //        Directory.CreateDirectory(savePath);
        //}
        //catch (Exception e) { Console.WriteLine(e.ToString()); }

        //string pathAttachFile_NRC = "";
        //if (operatorLicenseAttachVM.AttachFile_NRC != null)
        //{
        //    int index = 0;
        //    foreach(var item in operatorLicenseAttachVM.AttachFile_NRC)
        //    {
        //        if (index == 0) pathAttachFile_NRC = new CommonMethod().SaveImage(item, savePath+"\\" + (string.Format("{0}_{1}", new CommonMethod().FilePathNameString(operatorLicenseAttachVM.licenseNumberLong), "Font.jpg")));
        //        else if (index == 1) pathAttachFile_NRC += new CommonMethod().SaveImage(item, savePath+"\\" + (string.Format("{0}_{1}", new CommonMethod().FilePathNameString(operatorLicenseAttachVM.licenseNumberLong), "Back.jpg")));
        //        else break;
        //        index++;
        //    }
        //}

        //string pathAttachFile_M10 = "";
        //if (operatorLicenseAttachVM.AttachFile_M10 != null)
        //{
        //    int index = 1;
        //    foreach(var item in operatorLicenseAttachVM.AttachFile_M10)
        //    {
        //        pathAttachFile_M10 += new CommonMethod().SaveImage(item, savePath+"\\" + (string.Format("{0}_{1}", "AttachFile_M10", (index++) + ".jpg")));

        //    }
        //}

        //string pathAttachFile_RecommandDoc1 = "";
        //if (operatorLicenseAttachVM.AttachFile_RecommandDoc1 != null)
        //{
        //    int index = 1;
        //    foreach (var item in operatorLicenseAttachVM.AttachFile_RecommandDoc1)
        //    {
        //        pathAttachFile_RecommandDoc1 += new CommonMethod().SaveImage(item, savePath + "\\" + (string.Format("{0}_{1}", "pathAttachFile_RecommandDoc1", (index++) + ".jpg")));

        //    }
        //}

        //string pathAttachFile_RecommandDoc2 = "";
        //if (operatorLicenseAttachVM.AttachFile_RecommandDoc2 != null)
        //{
        //    int index = 1;
        //    foreach (var item in operatorLicenseAttachVM.AttachFile_RecommandDoc2)
        //    {
        //        pathAttachFile_RecommandDoc2 += new CommonMethod().SaveImage(item, savePath + "\\" + (string.Format("{0}_{1}", "AttachFile_RecommandDoc2", (index++) + ".jpg")));

        //    }
        //}

        //string pathAttachFile_OperatorLicense = "";
        //if (operatorLicenseAttachVM.AttachFile_OperatorLicense != null)
        //{
        //    int index = 1;
        //    foreach (var item in operatorLicenseAttachVM.AttachFile_OperatorLicense)
        //    {
        //        pathAttachFile_OperatorLicense += new CommonMethod().SaveImage(item, savePath + "\\" + (string.Format("{0}_{1}", "AttachFile_OperatorLicense", (index++) + ".jpg")));

        //    }
        //}

        //string pathAttachFile_Part1 = "";
        //if (operatorLicenseAttachVM.AttachFile_Part1 != null)
        //{
        //    int index = 1;
        //    foreach (var item in operatorLicenseAttachVM.AttachFile_Part1)
        //    {
        //        pathAttachFile_Part1 += new CommonMethod().SaveImage(item, savePath + "\\" + (string.Format("{0}_{1}", "AttachFile_Part1", (index++) + ".jpg")));

        //    }
        //}

        //var licenseOnly = new LicenseOnly()
        //{
        //    Transaction_Id = "3",
        //    License_Number = "Testing_License",
        //    LicenseOwner = "Aung Latt",
        //    NRC_Number = "1/BMMN(N)043223",
        //    Address = "Kachin",
        //    Township_Name = "Myitkyina",
        //    Phone = "09323423234",
        //    Fax = "String",
        //    AllowBusinessTitle = "Mandalar Min",
        //    OtherRegistrationOffice_Id = 1,
        //    IssueDate = DateTime.Now,
        //    IsClosed = false,
        //    FormMode = "Testing_Form",
        //    IsDeleted = false,
        //    AttachFile_NRC = pathAttachFile_NRC,
        //    AttachFile_M10 = pathAttachFile_M10,
        //    AttachFile_OperatorLicense = pathAttachFile_OperatorLicense,
        //    AttachFile_Part1 = pathAttachFile_Part1,
        //    AttachFile_RecommandDoc1 = pathAttachFile_RecommandDoc1,
        //    AttachFile_RecommandDoc2 = pathAttachFile_RecommandDoc1,
        //    AttachFile_RecommandDoc3 = "Testing",
        //    AttachFile_RecommandDoc4 = "Testing",
        //    AttachFile_RecommandDoc5 = "Testing",
        //    CreatedDate = DateTime.Now,
        //    CreatedBy = "Aung Latt",

        //    RegistrationOfficeId = 2,
        //    JourneyTypeId = 1,
        //    DeliveryId = 1,
        //    PersonInformationId = 27
        //};
        //bool a = await _iopeartorDetail.AddOperatorLicenseAttach(licenseOnly);
        //return Ok(operatorLicenseAttachVM);
        //}
        #endregion

        [HttpPost("DecreaseCar")]
        public async Task<IActionResult> DecreaseCarProcess([FromForm] DecreaseCarVMList decreaseCarVMList)
        {
            var decreaseCarData = await _iopeartorDetail.DecreaseCars(decreaseCarVMList);
            return Ok(decreaseCarData);

        }

        [HttpPost("ChangeLicenseOwnerAddress")]
        public async Task<IActionResult> ChangeLicenseOwnerAddressProcess([FromForm] ChangeLicenseOwnerAddressVM changeLicenseOwnerAddressVM)
        {
            var changeLOwnerAddressData = await _iopeartorDetail.ChangeLicenseOwnerAddress(changeLicenseOwnerAddressVM);

            return Ok(changeLOwnerAddressData);
        }

        [HttpPost("ChangeVehicleOwnerAddress")]
        public async Task<IActionResult> ChangeVehicleOwnerAddressProcess([FromForm] ChangeVehicleOwnerAddressVM changeVehicleOwnerAddressVM)
        {
            var changeVOAData = await _iopeartorDetail.ChangeVehicleOwnerAddress(changeVehicleOwnerAddressVM);

            return Ok(changeVOAData);
        }

        //[HttpPost("VehicleOwnerChangeName")]
        //public async Task<IActionResult> VehicleOwnerChangeName([FromForm] ChangeVehicleOwnerAddressVM changeVehicleOwnerAddressVM)
        //{
        //    var changeVOAData = await _iopeartorDetail.VehicleOwnerChangeName(changeVehicleOwnerAddressVM);

        //    return Ok(changeVOAData);
        //}

        [HttpPost("ExtenseCar")]
        public async Task<IActionResult> ExtenseCarProcess([FromForm] ExtenseCarVMList extenseCarVMLists)
        {
            //string transactionId = "";
            //if (extenseCarVMLists.ExtenseCarVMs != null)
            //{
            //    //transactionId = await _operatorDetail.VehicleAttach(extenseCarVMLists.ExtenseCarVMs)
            //}

            var addNewCar = await _iopeartorDetail.AddNewCars(extenseCarVMLists);

            return Ok(addNewCar);
        }

        //commented for to make same T&C from backend
        //[HttpPost("CommonChanges")]
        //public async Task<IActionResult> CommonChangesProcess([FromForm] CommonChangesVM dto)
        //{
        //    var response = await _iopeartorDetail.CommonChangesProcess(dto);
        //    return Ok(response);
        //    //return Ok(dto); // for testing only
        //}

        [HttpPost("CommonChanges")]
        public async Task<IActionResult> CommonChangesProcess([FromForm] CommonChangesVM dto)
        {
            var response = await _iopeartorDetail.CommonChangesProcess(dto);
            return Ok(response);
            //return Ok(dto); // for testing only
        }
        #region tzt 070723
        [HttpGet("getOperatorDetailByNRCAndLicenseNumberLongMobile")]
        public async Task<IActionResult> getOperatorDetailByNRCAndLicenseNumberLongMobile(OperatorDetailGetRequest opDetGetReq)
        {
            if (opDetGetReq.userId == 0 || opDetGetReq.licenseNumlong == null)
                return BadRequest(new { Status = false, Message = "Invalid request parameter" });
            var op = await _iopeartorDetail.getOperatorDetailByNRCAndLicenseNumberLongMobile(opDetGetReq);
            if (op.operatorDetailHead == null)
                return NotFound(new { Status = false, Message = "operator license not found!" });

            return Ok(new
            {
                Status = true,
                Message = "success",
                TotalCarCount = op.totalCarCount,
                OperatorDetail = op.operatorDetailHead,
                CarObjects = op.carObjects,
                CurrentPage = opDetGetReq.page,
                PageCount = opDetGetReq.countPerPage,
                TotalPage = op.totalPage
            }) ;
        }
        #endregion

        #region tzt 080823
        [HttpPost("ExtendOperatorMobile")]
        public async Task<IActionResult> ExtendOperatorMobile([FromForm] CommonChangesVM dto)
        {
            if(dto == null)
            {
                return BadRequest();
            }
            if(dto.FormMode == null || dto.NRC_Number == null || dto.LicenseNumberLong == null)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = "Invalid Request!"
                });
            }
            var response = await _iopeartorDetail.CommonChangesProcess(dto);
            return Ok(
                new
                {
                    Status = true,
                    Message = "success"
                }
            );
            
        }
        [HttpPost("ExtendVehicleMobile")]
        public async Task<IActionResult> ExtendVehicleMobile([FromForm] CommonChangesVM dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            if (dto.FormMode == null || dto.NRC_Number == null || dto.LicenseNumberLong == null)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = "Invalid Request!"
                });
            }
            var response = await _iopeartorDetail.CommonChangesProcess(dto);
            return Ok(
                new
                {
                    Status = true,
                    Message = "success"
                }
            );
            //return Ok(dto); // for testing only
        }
        #endregion
        #region TZT_090823
        [HttpPost("ChangeLOwnerAddressMobile")]
        public async Task<IActionResult> ChangeLOwnerAddressMobile([FromForm] CommonChangesVM dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            if (dto.FormMode == null || dto.NRC_Number == null || dto.LicenseNumberLong == null)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = "Invalid Request!"
                });
            }
            var response = await _iopeartorDetail.CommonChangesProcess(dto);
            return Ok(
                new
                {
                    Status = true,
                    Message = "success"
                }
            );
            
        }
        [HttpPost("ChangeVehicleAddressMobile")]
        public async Task<IActionResult> ChangeVehicleAddressMobile([FromForm] CommonChangesVM dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            if (dto.FormMode == null || dto.NRC_Number == null || dto.LicenseNumberLong == null)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = "Invalid Request!"
                });
            }
            var response = await _iopeartorDetail.CommonChangesProcess(dto);
            return Ok(
                new
                {
                    Status = true,
                    Message = "success"
                }
            );

        }
        [HttpPost("ChangeVehicleTypeMobile")]
        public async Task<IActionResult> ChangeVehicleTypeMobile([FromForm] CommonChangesVM dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            if (dto.FormMode == null || dto.NRC_Number == null || dto.LicenseNumberLong == null)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = "Invalid Request!"
                });
            }
            var response = await _iopeartorDetail.CommonChangesProcess(dto);
            return Ok(
                new
                {
                    Status = true,
                    Message = "success"
                }
            );

        }
        [HttpPost("ChangeVehOwnerNameMobile")]
        public async Task<IActionResult> ChangeVehOwnerNameMobile([FromForm] CommonChangesVM dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            if (dto.FormMode == null || dto.NRC_Number == null || dto.LicenseNumberLong == null)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = "Invalid Request!"
                });
            }
            var response = await _iopeartorDetail.CommonChangesProcess(dto);
            return Ok(
                new
                {
                    Status = true,
                    Message = "success"
                }
            );

        }
        [HttpPost("AddNewCarMobile")]
        public async Task<IActionResult> AddNewCarMobile([FromForm] CommonChangesVM dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            if (dto.FormMode == null || dto.NRC_Number == null || dto.LicenseNumberLong == null)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = "Invalid Request!"
                });
            }
            var response = await _iopeartorDetail.CommonChangesProcess(dto);
            return Ok(
                new
                {
                    Status = true,
                    Message = "success"
                }
            );

        }
        [HttpPost("DecreaseCarMobile")]
        public async Task<IActionResult> DecreaseCarMobile([FromForm] CommonChangesVM dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }
            if (dto.FormMode == null || dto.NRC_Number == null || dto.LicenseNumberLong == null)
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = "Invalid Request!"
                });
            }
            var response = await _iopeartorDetail.CommonChangesProcess(dto);
            return Ok(
                new
                {
                    Status = true,
                    Message = "success"
                }
            );

        }
        [HttpGet("LicenseDetailForOver2tonMobile/{dto}")]
        public async Task<IActionResult> LicenseDetailForOver2tonMobile(string dto)
        {
            if (dto == "" || dto == null)
                return BadRequest();
            var resp = await _iopeartorDetail.LicenseDetailForOver2ton(dto.Replace("*", "/"));
            if(resp.Item2 == null)
            {
                return Ok(new
                {
                    Status = true,
                    Message = "success",
                    Data = resp
                }); ;
            }
            else
            {
                return BadRequest(new
                {
                    Status = false,
                    Message = resp.Item2
                }); ;
            }
            
        }
        #endregion
        [HttpGet("LicenseDetailForOver2ton/{dto}")]
        public async Task<IActionResult> LicenseDetailForOver2ton(string dto)
        {
            var resp = await _iopeartorDetail.LicenseDetailForOver2ton(dto.Replace("*","/"));
            return Ok(resp);
        }

        [HttpPost("all_operation_done")]
        public async Task<IActionResult> AllOperationDoneProcess(AllOperationDoneVM allOperationDoneVM)
        {
            bool oky = await _iopeartorDetail.AllOperationDoneProcess(allOperationDoneVM);
            return Ok(allOperationDoneVM);
        }
    }
}
