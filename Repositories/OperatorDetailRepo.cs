using DOTP_BE.Common;
using DOTP_BE.Data;
using DOTP_BE.Helpers;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.Models;
using DOTP_BE.ViewModel;
using DOTP_BE.ViewModel.ReportResponses;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Org.BouncyCastle.Utilities;
using System.ComponentModel;
using System.Diagnostics;
using System.Numerics;
using static iTextSharp.text.pdf.AcroFields;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace DOTP_BE.Repositories
{
    public class OperatorDetailRepo : IOperatorDetail
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _iConfig;
        public OperatorDetailRepo(ApplicationDbContext context, IConfiguration iConfig)
        {
            _context = context;
            _iConfig = iConfig;
        }

        public async Task<bool> Create(OperatorDetailVM operatorDetailVM)
        {
            #region VM to Model
            var op = new OperatorDetail()
            {
                Transaction_Id = operatorDetailVM.Transaction_Id,
                LicenseHolderType = operatorDetailVM.LicenseHolderType,
                OperatorName = operatorDetailVM.OperatorName,
                AllowBusinessTitle = operatorDetailVM.AllowBusinessTitle,
                Address = operatorDetailVM.Address,
                ApplyDate = operatorDetailVM.ApplyDate,
                LicenseOwner = operatorDetailVM.LicenseOwner,
                RegistrationOffice_Id = operatorDetailVM.RegistrationOffice_Id,
                NRC = operatorDetailVM.NRC,
                applicant_Id = operatorDetailVM.applicant_Id,
                Township = operatorDetailVM.Township,
                Phone = operatorDetailVM.Phone,
                Fax = operatorDetailVM.Fax,
                Email = operatorDetailVM.Email,
                ExpiredDate = operatorDetailVM.ExpiredDate,
                JourneyType_Id = operatorDetailVM.JourneyType_Id,
                TotalCar = operatorDetailVM.TotalCar,
                DesiredRoute = operatorDetailVM.DesiredRoute,
                Notes = operatorDetailVM.Notes,
                ApplyLicenseType = operatorDetailVM.ApplyLicenseType,
                IsClosed = operatorDetailVM.IsClosed,
                IsDeleted = operatorDetailVM.IsDeleted,
                FormMode = operatorDetailVM.FormMode,
                CreatedDate = DateTime.Now,
                CreatedBy = operatorDetailVM.CreatedBy,
                UpdatedDate = DateTime.Now,
                PersonInformationId = operatorDetailVM.PersonInformationId,
                VehicleId = null // mwl cmm
            };
            #endregion
            //await _context.OperatorDetails.AddAsync(op);
            //await _context.SaveChangesAsync();
            _context.OperatorDetails.Add(op);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var op = await _context.OperatorDetails.FindAsync(id);
            if (op == null) return false;

            _context.OperatorDetails.Remove(op);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<OperatorDetail> getOperatorDetailById(int id)
        {
            var op = await _context.OperatorDetails.Where(o => o.OperatorId == id).FirstOrDefaultAsync();
            return op;
        }

        public async Task<List<OperatorDetail>> getOperatorDetailList()
        {
            return await _context.OperatorDetails.AsNoTracking().ToListAsync();
        }

        public async Task<bool> Update(int id, OperatorDetailVM operatorDetailVM)
        {
            var op = await _context.OperatorDetails.FindAsync(id);
            if (op == null) return false;

            #region VM to Model
            op.Transaction_Id = operatorDetailVM.Transaction_Id;
            op.LicenseHolderType = operatorDetailVM.LicenseHolderType;
            op.OperatorName = operatorDetailVM.OperatorName;
            op.AllowBusinessTitle = operatorDetailVM.AllowBusinessTitle;
            op.Address = operatorDetailVM.Address;
            op.ApplyDate = operatorDetailVM.ApplyDate;
            op.LicenseOwner = operatorDetailVM.LicenseOwner;
            op.RegistrationOffice_Id = operatorDetailVM.RegistrationOffice_Id;
            op.NRC = operatorDetailVM.NRC;
            op.applicant_Id = operatorDetailVM.applicant_Id;
            op.Township = operatorDetailVM.Township;
            op.Phone = operatorDetailVM.Phone;
            op.Fax = operatorDetailVM.Fax;
            op.Email = operatorDetailVM.Email;
            op.ExpiredDate = operatorDetailVM.ExpiredDate;
            op.JourneyType_Id = operatorDetailVM.JourneyType_Id;
            op.TotalCar = operatorDetailVM.TotalCar;
            op.DesiredRoute = operatorDetailVM.DesiredRoute;
            op.Notes = operatorDetailVM.Notes;
            op.ApplyLicenseType = operatorDetailVM.ApplyLicenseType;
            op.IsClosed = operatorDetailVM.IsClosed;
            op.IsDeleted = operatorDetailVM.IsDeleted;
            op.FormMode = operatorDetailVM.FormMode;
            op.CreatedBy = operatorDetailVM.CreatedBy;
            op.UpdatedDate = DateTime.Now;
            op.PersonInformationId = operatorDetailVM.PersonInformationId;
            // op.VehicleId = operatorDetailVM.VehicleId; //mwl
            #endregion
            _context.OperatorDetails.Update(op);
            await _context.SaveChangesAsync();
            return true;
        }

        //tzt 29_12_2022
        //public async Task<OperatorDetailVM> getOperatorDetailByNRCAndLicenseNumberLong(string userId, string license_num_long)
        //{
        //    var op = new OperatorDetail();
        //    var reg_office = new RegistrationOffice();
        //    var journeyType = new JourneyType();
        //    var opVM = new OperatorDetailVM();

        //    var vehicle = await _context.Vehicles.Include(x => x.OperatorDetail).Where(v => v.LicenseNumberLong == license_num_long).FirstOrDefaultAsync();
        //    if (vehicle != null)
        //    {
        //        var userNrc = await _context.Users.Where(x => x.UserId == Int16.Parse(userId)).Select(x => x.NRC_Number).FirstOrDefaultAsync();
        //        op = await _context.OperatorDetails.Where(o => o.VehicleId == vehicle.VehicleId && o.NRC == userNrc).FirstOrDefaultAsync();
        //        //op = await _context.OperatorDetails.Where(o => o.NRC == userNrc).FirstOrDefaultAsync();
        //        reg_office = _context.RegistrationOffices.Find(op != null ? op.RegistrationOffice_Id : 0);
        //        journeyType = _context.JourneyTypes.Find(op != null ? op.JourneyType_Id : 0);

        //        #region To VM
        //        opVM.LicenseNumberLong = vehicle.LicenseNumberLong;
        //        opVM.OperatorName = op !=null? op.OperatorName:"";
        //        opVM.OfficeName = reg_office != null ? reg_office.OfficeLongName : "";
        //        opVM.TotalCar = op != null? op.TotalCar:0;
        //        opVM.NRC = op !=null? op.NRC : "";
        //        opVM.Phone = op !=null? op.Phone : "";
        //        opVM.Email = op !=null? op.Email : "";
        //        opVM.Address = op!=null? op.Address:"";
        //        opVM.JourneyTypeName = journeyType!=null? journeyType.JourneyTypeLong:"";
        //        opVM.ExpiredDate = op!=null? op.ExpiredDate : DateTime.Now;
        //        #endregion
        //    }

        //    return opVM;
        //}

        //al 20_4_2023
        public async Task<OperatorDetailVM> getOperatorDetailByNRCAndLicenseNumberLong(int userId, int operatorId, string license_num_long)
        {
            var opVM = new OperatorDetailVM();
            var userNrc = await _context.Users.FindAsync(userId);
            if (userNrc != null)
            {
                var operatorDetail = await _context.OperatorDetails.FindAsync(operatorId);

                //var distNRCs = await _context.OperatorDetails.Include(x => x.Vehicle).ThenInclude(x => x.LicenseOnly).ThenInclude(x => x.RegistrationOffice)
                //                                       .Where(x => x.NRC == userNrc.NRC_Number &&
                //                                       (x.FormMode == ConstantValue.CreateNew_FM || x.FormMode == ConstantValue.EOPL_FM) &&
                //                                        x.IsDeleted == false)
                //                                       .ToListAsync(); //get all data where formMode is 'Creae New' or 'ExtendOperatorLicense'

                //distNRCs = distNRCs.GroupBy(d => d.ApplyLicenseType)
                //                   .Select(g => g.OrderByDescending(d => d.ApplyDate).First())
                //                   .ToList(); // groupd by and select greatest date

                //var operatorDetail = distNRCs[0];

                if (operatorDetail != null)
                {
                    int addCar = await _context.OperatorDetails.AsNoTracking()
                                               .Where(x => x.NRC == operatorDetail.NRC &&
                                                           x.ApplyDate.Date >= operatorDetail.ApplyDate.Date &&
                                                           x.FormMode == ConstantValue.AddNewCar_FM &&
                                                           x.ApplyLicenseType == operatorDetail.ApplyLicenseType /*&&*/
                                                           /*x.IsDeleted == false*/)
                                               .SumAsync(x => x.TotalCar);

                    int decCar = await _context.OperatorDetails.AsNoTracking()
                                               .Where(x => x.NRC == operatorDetail.NRC &&
                                                           x.ApplyDate.Date >= operatorDetail.ApplyDate.Date &&
                                                           x.FormMode == ConstantValue.DecreaseCar_FM &&
                                                           x.ApplyLicenseType == operatorDetail.ApplyLicenseType /*&&*/
                                                          /* x.Transaction_Id != operatorDetail.Transaction_Id*/) // commited by al(11/07/2023)
                                               .SumAsync(x => x.TotalCar);
                    #region not use 
                    //int decCar = await _context.OperatorDetails.AsNoTracking()
                    //                           .Where(x => x.NRC == operatorDetail.NRC &&
                    //                                       x.ApplyDate >= operatorDetail.ApplyDate &&
                    //                                       x.FormMode == ConstantValue.DecreaseCar_FM &&
                    //                                       x.ApplyLicenseType == operatorDetail.ApplyLicenseType /*&&*/
                    //                                       /*x.IsDeleted == false*/)
                    //                           .SumAsync(x => x.TotalCar);
                    //get the first one

                    //var vehicleobj = await _context.Vehicles.AsNoTracking()
                    //                               .Include(x => x.LicenseOnly).ThenInclude(x => x.RegistrationOffice)
                    //                               .Include(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
                    //                               .Where(x => x.Transaction_Id == operatorDetail.Transaction_Id)
                    //                               .FirstOrDefaultAsync();
                    //if (vehicleobj != null)
                    //{
                    //    opVM.LicenseNumberLong = vehicleobj.LicenseNumberLong;
                    //    opVM.OfficeName = vehicleobj.LicenseOnly.RegistrationOffice.OfficeLongName;
                    //    opVM.JourneyTypeName = vehicleobj.LicenseOnly.JourneyType.JourneyTypeLong;
                    //    opVM.ChalenNumber = vehicleobj.ChalenNumber;
                    //}
                    #endregion
                    opVM.OperatorName = operatorDetail.OperatorName;
                    opVM.TotalCar = operatorDetail.TotalCar + addCar - decCar;
                    opVM.NRC = operatorDetail.NRC;
                    opVM.Phone = operatorDetail.Phone;
                    opVM.Email = operatorDetail.Email;
                    opVM.Address = operatorDetail.Address;
                    opVM.ExpiredDate = operatorDetail.ExpiredDate;

                    //for licenOnly table
                    //opVM.Transaction_Id = operatorDetail.Transaction_Id; (al c)
                    //opVM.Fax = operatorDetail.Fax; (al c)
                    opVM.AllowBusinessTitle = operatorDetail.AllowBusinessTitle;
                    opVM.LicenseOwner = operatorDetail.LicenseOwner;
                    opVM.Township = operatorDetail.Township;
                    opVM.RegistrationOffice_Id = operatorDetail.RegistrationOffice_Id;
                    opVM.CreatedDate = operatorDetail.CreatedDate;
                    opVM.IsClosed = operatorDetail.IsClosed;
                    opVM.FormMode = operatorDetail.FormMode;
                    opVM.IsDeleted = operatorDetail.IsDeleted;
                    opVM.JourneyType_Id = operatorDetail.JourneyType_Id;
                    opVM.PersonInformationId = operatorDetail.PersonInformationId;
                    opVM.CreatedBy = operatorDetail.CreatedBy; //userNrc.Name
                    opVM.LicenseHolderType = operatorDetail.LicenseHolderType;
                    opVM.ApplyLicenseType = operatorDetail.ApplyLicenseType;
                    opVM.Email = operatorDetail.Email;
                    opVM.VehicleId = operatorDetail.VehicleId;
                    opVM.applicant_Id = operatorDetail.applicant_Id;


                    //get all dec car Vehicle No and filter all that car for total car
                    var decCarT_Id = await _context.OperatorDetails.AsNoTracking()
                                                 .Where(x => x.NRC == operatorDetail.NRC &&
                                                             x.ApplyDate.Date >= operatorDetail.ApplyDate.Date &&
                                                             x.FormMode == ConstantValue.DecreaseCar_FM &&
                                                             x.ApplyLicenseType == operatorDetail.ApplyLicenseType /*&&*/
                                                             /*x.IsDeleted == false*/)
                                                 .Select(x => x.Transaction_Id)
                                                 .ToListAsync();


                    var vehicleNo = await _context.Vehicles.AsNoTracking()
                                                  .Where(x => decCarT_Id.Contains(x.Transaction_Id) && x.FormMode == ConstantValue.DecreaseCar_FM)
                                                  .Select(x => x.VehicleNumber)
                                                  .ToListAsync();

                    var vehiclesObject = await _context.Vehicles.AsNoTracking()
                                                       .Include(x => x.CreateCar)
                                                       .Include(x => x.VehicleWeight)
                                                       .Include(x => x.LicenseOnly).ThenInclude(x => x.RegistrationOffice)
                                                       .Include(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
                                                       .Where(x => x.NRC_Number == userNrc.NRC_Number &&
                                                                   x.LicenseNumberLong == license_num_long &&
                                                                   x.ApplyDate.Date >= operatorDetail.ApplyDate.Date &&
                                                                   !vehicleNo.Contains(x.VehicleNumber) &&
                                                                   x.LicenseTypeId == operatorDetail.ApplyLicenseType &&
                                                                   //x.FormMode != ConstantValue.DecreaseCar_FM &&
                                                                   x.Status == ConstantValue.Status_Approved)
                                                       .ToListAsync();
                    #region not use
                    //var decCarT_Id = await _context.OperatorDetails.AsNoTracking()
                    //                               .Where(x => x.ApplyDate >= operatorDetail.ApplyDate &&
                    //                                           x.FormMode == ConstantValue.DecreaseCar_FM &&
                    //                                           x.ApplyLicenseType == operatorDetail.ApplyLicenseType)
                    //                               .Select(x => x.Transaction_Id)
                    //                               .ToListAsync();

                    //var vehicleNo = await _context.Vehicles.AsNoTracking()
                    //                              .Where(x => x.FormMode == ConstantValue.DecreaseCar_FM && 
                    //                                          decCarT_Id.Contains(x.Transaction_Id))
                    //                              .Select(x => x.VehicleNumber)
                    //                              .ToListAsync();

                    //var vehicleNo = await _context.Vehicles.AsNoTracking()
                    //                          .Where(x => decCarT_Id.Contains(x.Transaction_Id))
                    //                          .Select(x => x.VehicleNumber)
                    //                          .ToListAsync();

                    //for total Car Object
                    //var vehiclesObject = await _context.Vehicles.AsNoTracking()
                    //                                   .Include(x => x.CreateCar)
                    //                                   .Include(x => x.VehicleWeight)
                    //                                   //.Where(x => x.NRC_Number == userNrc.NRC_Number && x.LicenseNumberLong == license_num_long)
                    //                                   .Where(x => x.LicenseNumberLong == license_num_long &&
                    //                                               x.ApplyDate.Date >= operatorDetail.ApplyDate.Date &&
                    //                                               !vehicleNo.Contains(x.VehicleNumber) &&
                    //                                               x.Status == ConstantValue.Status_Approved)
                    //.ToListAsync();


                    //var vehiclesObject = await _context.Vehicles.AsNoTracking()
                    //                                   .Include(x => x.CreateCar)
                    //                                   .Include(x => x.VehicleWeight)
                    //                                   //.Where(x => x.NRC_Number == userNrc.NRC_Number && x.LicenseNumberLong == license_num_long)
                    //                                   .Where(x => x.NRC_Number == userNrc.NRC_Number &&
                    //                                               x.LicenseNumberLong == license_num_long &&
                    //                                               x.ApplyDate.Date >= operatorDetail.ApplyDate.Date &&
                    //                                               !vehicleNo.Contains(x.VehicleNumber) &&
                    //                                               x.Status == ConstantValue.Status_Approved)
                    //                                   .ToListAsync();
                    #endregion

                    if (vehiclesObject != null)
                    {
                        List<CarObject> cars = new List<CarObject>();
                        foreach (var item in vehiclesObject)
                        {
                            var a = (new CarObject
                            {
                                //CreateCarId = item.CreateCar.CreateCarId,
                                CreateCarId = item.VehicleId,
                                VehicleNumber = item.CreateCar.VehicleNumber,
                                VehicleBrand = item.CreateCar.VehicleBrand,
                                VehicleType = item.CreateCar.VehicleType,
                                VehicleOwnerName = item.CreateCar.VehicleOwnerName,
                                ExpiredDate = item.ExpiryDate,
                                //AllowedWeight = item.VehicleWeight.VehicleType,

                                vehicleOwnerAddress = item.CreateCar.VehicleOwnerAddress,
                                vehicleAddress = item.VehicleLocation,
                                vehicleWeight = item.CreateCar.VehicleWeight,
                                vehicleOwnerNRC = item.CreateCar.VehicleOwnerNRC,
                                OwnerBook = item.OwnerBook,
                                Triangle = item.Triangle,
                                AttachedFile1 = item.AttachedFile1,
                                AttachedFile2 = item.AttachedFile2,
                                Id = item.CreateCar.CreateCarId,
                                AllowedWeight = item.CreateCar.VehicleWeight,
                            });
                            cars.Add(a);
                        }
                        opVM.CarObjects = cars;
                        opVM.LicenseNumberLong = vehiclesObject[0].LicenseNumberLong;
                        opVM.OfficeName = vehiclesObject[0].LicenseOnly.RegistrationOffice.OfficeLongName;
                        opVM.JourneyTypeName = vehiclesObject[0].LicenseOnly.JourneyType.JourneyTypeLong;
                        opVM.ChalenNumber = vehiclesObject[0].ChalenNumber;
                    }
                }
            }
            return opVM;
        }

        #region Old one

        ////al 20_4_2023
        //public async Task<OperatorDetailVM> getOperatorDetailByNRCAndLicenseNumberLong(string userId, string license_num_long)
        //{
        //    var opVM = new OperatorDetailVM();
        //    var userNrc = await _context.Users.FindAsync(int.Parse(userId));
        //    if (userNrc != null)
        //    {
        //        var operatorDetails = await _context.OperatorDetails
        //                            .Include(x => x.Vehicle).ThenInclude(x => x.LicenseOnly).ThenInclude(x => x.RegistrationOffice)
        //                            .Include(x => x.Vehicle).ThenInclude(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
        //                            .Where(x => x.NRC == userNrc.NRC_Number &&
        //                                        x.Vehicle.LicenseNumberLong == license_num_long &&
        //                                        x.FormMode == ConstantValue.EOPL_FM)
        //                            .ToListAsync();

        //        //.FirstOrDefaultAsync
        //        //var vehicle = await _context.Vehicles.Include(x => x.OperatorDetail).Where(v => v.NRC_Number == userNrc).ToListAsync();
        //        //vehicle = vehicle.OrderByDescending(x => x.CreatedDate).ToList();

        //        operatorDetails = operatorDetails.OrderByDescending(x => x.ExpiredDate).ToList(); // to get greatest expire date record

        //        var operatorDetail = operatorDetails[0];
        //        if (operatorDetail != null)
        //        {
        //            //var reg_office = _context.RegistrationOffices.Find(operatorDetail.RegistrationOffice_Id);
        //            //var journeyType = _context.JourneyTypes.Find(operatorDetail.JourneyType_Id);
        //            #region To VM
        //            opVM.LicenseNumberLong = operatorDetail.Vehicle.LicenseNumberLong;
        //            opVM.OperatorName = operatorDetail.OperatorName;
        //            //opVM.OfficeName = reg_office != null ? reg_office.OfficeLongName : "";
        //            opVM.OfficeName = operatorDetail.Vehicle.LicenseOnly.RegistrationOffice.OfficeLongName;
        //            opVM.TotalCar = operatorDetail.TotalCar;
        //            opVM.NRC = operatorDetail.NRC;
        //            opVM.Phone = operatorDetail.Phone;
        //            opVM.Email = operatorDetail.Email;
        //            opVM.Address = operatorDetail.Address;
        //            //opVM.JourneyTypeName = journeyType != null ? journeyType.JourneyTypeLong : "";
        //            opVM.JourneyTypeName = operatorDetail.Vehicle.LicenseOnly.JourneyType.JourneyTypeLong;
        //            opVM.ExpiredDate = operatorDetail.ExpiredDate;

        //            //for licenOnly table
        //            opVM.Transaction_Id = DateTime.Now.ToString("ddMMyyyy") + operatorDetail.Transaction_Id;
        //            opVM.ChalenNumber = operatorDetail.Vehicle.ChalenNumber;
        //            opVM.Fax = operatorDetail.Fax;
        //            opVM.AllowBusinessTitle = operatorDetail.AllowBusinessTitle;
        //            opVM.LicenseOwner = operatorDetail.LicenseOwner;
        //            opVM.Township = operatorDetail.Township;
        //            opVM.RegistrationOffice_Id = operatorDetail.RegistrationOffice_Id;
        //            opVM.CreatedDate = operatorDetail.CreatedDate;
        //            opVM.IsClosed = operatorDetail.IsClosed;
        //            opVM.FormMode = operatorDetail.FormMode;
        //            opVM.IsDeleted = operatorDetail.IsDeleted;
        //            opVM.JourneyType_Id = operatorDetail.JourneyType_Id;
        //            opVM.PersonInformationId = operatorDetail.PersonInformationId;
        //            opVM.CreatedBy = operatorDetail.CreatedBy; //userNrc.Name
        //            opVM.LicenseHolderType = operatorDetail.LicenseHolderType;
        //            opVM.ApplyLicenseType = operatorDetail.ApplyLicenseType;
        //            opVM.Email = operatorDetail.Email;
        //            opVM.VehicleId = operatorDetail.VehicleId;
        //            opVM.applicant_Id = operatorDetail.applicant_Id;

        //            //for total Car Object
        //            var carsObject = await _context.Vehicles.Include(x => x.CreateCar)
        //                                                    .Include(x => x.VehicleWeight)
        //                                                    //.Where(x => x.NRC_Number == userNrc.NRC_Number && x.LicenseNumberLong == license_num_long)
        //                                                    .Where(x => x.NRC_Number == userNrc.NRC_Number && x.LicenseNumberLong == license_num_long &&
        //                                                                x.Status == ConstantValue.Status_Approved)
        //                                                    .ToListAsync();
        //            if (carsObject != null)
        //            {
        //                List<CarObject> cars = new List<CarObject>();
        //                foreach (var item in carsObject)
        //                {
        //                    var a = (new CarObject
        //                    {
        //                        //CreateCarId = item.CreateCar.CreateCarId,
        //                        CreateCarId = item.VehicleId,
        //                        VehicleNumber = item.CreateCar.VehicleNumber,
        //                        VehicleBrand = item.CreateCar.VehicleBrand,
        //                        VehicleType = item.CreateCar.VehicleType,
        //                        VehicleOwnerName = item.CreateCar.VehicleOwnerName,
        //                        ExpiredDate = item.ExpiryDate,
        //                        AllowedWeight = item.VehicleWeight.VehicleType
        //                    });
        //                    cars.Add(a);
        //                }
        //                opVM.CarObjects = cars;
        //            }
        //            #endregion
        //        }
        //    }
        //    return opVM;
        //}

        #endregion

        public async Task<bool> AddOperatorLicenseAttach(LicenseOnly licenseOnly)
        {
            await _context.LicenseOnlys.AddAsync(licenseOnly);
            await _context.SaveChangesAsync();
            return true;
        }
        #region TZT 120723 for mobile api
        public async Task<OperatorDetailMobileVM> getOperatorDetailByNRCAndLicenseNumberLongMobile(OperatorDetailGetRequest opGetReq)
        {
            var opVM = new OperatorDetailMobileVM();
            var opHead = new OperatorDetailHead();
            var userNrc = await _context.Users.FindAsync(opGetReq.userId);
            if (userNrc != null)
            {
                var operatorDetail = await _context.OperatorDetails.FindAsync(opGetReq.operatorId);


                if (operatorDetail != null)
                {
                    int addCar = await _context.OperatorDetails.AsNoTracking()
                                               .Where(x => x.NRC == operatorDetail.NRC &&
                                                           x.ApplyDate.Date >= operatorDetail.ApplyDate.Date &&
                                                           x.FormMode == ConstantValue.AddNewCar_FM &&
                                                           x.ApplyLicenseType == operatorDetail.ApplyLicenseType /*&&*/
                                                           /*x.IsDeleted == false*/)
                                               .SumAsync(x => x.TotalCar);

                    int decCar = await _context.OperatorDetails.AsNoTracking()
                                               .Where(x => x.NRC == operatorDetail.NRC &&
                                                           x.ApplyDate.Date >= operatorDetail.ApplyDate.Date &&
                                                           x.FormMode == ConstantValue.DecreaseCar_FM &&
                                                           x.ApplyLicenseType == operatorDetail.ApplyLicenseType &&
                                                           x.Transaction_Id != operatorDetail.Transaction_Id)
                                               .SumAsync(x => x.TotalCar);


                    opHead.OperatorName = operatorDetail!.OperatorName;
                    opHead.TotalCar = operatorDetail.TotalCar + addCar - decCar;
                    opHead.NRC = operatorDetail.NRC;
                    opHead.Phone = operatorDetail.Phone;
                    opHead.Email = operatorDetail.Email;
                    opHead.Address = operatorDetail.Address;
                    opHead.ExpiredDate = operatorDetail.ExpiredDate;

                    //for licenOnly table
                    //opHead.Transaction_Id = operatorDetail.Transaction_Id; (al c)
                    //opHead.Fax = operatorDetail.Fax; (al c)
                    opHead.AllowBusinessTitle = operatorDetail.AllowBusinessTitle;
                    opHead.LicenseOwner = operatorDetail.LicenseOwner;
                    opHead.Township = operatorDetail.Township;
                    opHead.RegistrationOffice_Id = operatorDetail.RegistrationOffice_Id;
                    opHead.CreatedDate = operatorDetail.CreatedDate;
                    opHead.IsClosed = operatorDetail.IsClosed;
                    opHead.FormMode = operatorDetail.FormMode;
                    opHead.IsDeleted = operatorDetail.IsDeleted;
                    opHead.JourneyType_Id = operatorDetail.JourneyType_Id;
                    opHead.PersonInformationId = operatorDetail.PersonInformationId;
                    opHead.CreatedBy = operatorDetail.CreatedBy; //userNrc.Name
                    opHead.LicenseHolderType = operatorDetail.LicenseHolderType;
                    opHead.ApplyLicenseType = operatorDetail.ApplyLicenseType;
                    opHead.Email = operatorDetail.Email;
                    opHead.VehicleId = operatorDetail.VehicleId;
                    opHead.applicant_Id = operatorDetail.applicant_Id;


                    //get all dec car Vehicle No and filter all that car for total car
                    var decCarT_Id = await _context.OperatorDetails.AsNoTracking()
                                                 .Where(x => x.NRC == operatorDetail.NRC &&
                                                             x.ApplyDate.Date >= operatorDetail.ApplyDate.Date &&
                                                             x.FormMode == ConstantValue.DecreaseCar_FM &&
                                                             x.ApplyLicenseType == operatorDetail.ApplyLicenseType /*&&*/
                                                             /*x.IsDeleted == false*/)
                                                 .Select(x => x.Transaction_Id)
                                                 .ToListAsync();


                    var vehicleNo = await _context.Vehicles.AsNoTracking()
                                                  .Where(x => decCarT_Id.Contains(x.Transaction_Id) && x.FormMode == ConstantValue.DecreaseCar_FM)
                                                  .Select(x => x.VehicleNumber)
                                                  .ToListAsync();

                    var vehiclesObject = await _context.Vehicles.AsNoTracking()
                                                       .Include(x => x.CreateCar)
                                                       .Include(x => x.VehicleWeight)
                                                       .Include(x => x.LicenseOnly).ThenInclude(x => x.RegistrationOffice)
                                                       .Include(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
                                                       .Where(x => x.NRC_Number == userNrc.NRC_Number &&
                                                                   x.LicenseNumberLong == opGetReq.licenseNumlong &&
                                                                   x.ApplyDate.Date >= operatorDetail.ApplyDate.Date &&
                                                                   !vehicleNo.Contains(x.VehicleNumber) &&
                                                                   x.LicenseTypeId == operatorDetail.ApplyLicenseType &&
                                                                   //x.FormMode != ConstantValue.DecreaseCar_FM &&
                                                                   x.Status == ConstantValue.Status_Approved)
                                                       .ToListAsync();

                    if (vehiclesObject != null)
                    {
                        List<CarObject> cars = new List<CarObject>();
                        foreach (var item in vehiclesObject)
                        {
                            var a = (new CarObject
                            {
                                //CreateCarId = item.CreateCar.CreateCarId,
                                CreateCarId = item.VehicleId,
                                VehicleNumber = item.CreateCar.VehicleNumber,
                                VehicleBrand = item.CreateCar.VehicleBrand,
                                VehicleType = item.CreateCar.VehicleType,
                                VehicleOwnerName = item.CreateCar.VehicleOwnerName,
                                ExpiredDate = item.ExpiryDate,
                                //AllowedWeight = item.VehicleWeight.VehicleType,

                                vehicleOwnerAddress = item.CreateCar.VehicleOwnerAddress,
                                vehicleAddress = item.VehicleLocation,
                                vehicleWeight = item.CreateCar.VehicleWeight,
                                vehicleOwnerNRC = item.CreateCar.VehicleOwnerNRC,
                                OwnerBook = item.OwnerBook,
                                Triangle = item.Triangle,
                                AttachedFile1 = item.AttachedFile1,
                                AttachedFile2 = item.AttachedFile2,
                                Id = item.CreateCar.CreateCarId,
                                AllowedWeight = item.CreateCar.VehicleWeight,
                            });
                            cars.Add(a);
                        }
                        var pageCount = cars.Count() / opGetReq.countPerPage;
                        opVM.carObjects = cars.Skip((opGetReq.page - 1) * opGetReq.countPerPage)
                                      .Take(opGetReq.countPerPage)
                                      .ToList();
                        opHead.LicenseNumberLong = vehiclesObject[0].LicenseNumberLong;
                        opHead.OfficeName = vehiclesObject[0].LicenseOnly.RegistrationOffice.OfficeLongName;
                        opHead.JourneyTypeName = vehiclesObject[0].LicenseOnly.JourneyType.JourneyTypeLong;
                        opHead.ChalenNumber = vehiclesObject[0].ChalenNumber;
                        opVM.operatorDetailHead = opHead;
                        opVM.totalCarCount = cars.Count();
                    }
                }
            }
            return opVM;
        }
        #endregion TZT 120723 for mobile api
        //public async Task<string> VehicleAttach(List<CarAttachedFileVM> carAttachedFilelVM)
        public async Task<(bool, bool)> ExtendOperatorLicenseProcessNotUse(OperatorLicenseAttachVM dto)
        {
            if (dto.TakeNewRecord != null && dto.TakeNewRecord == true)
            {
                var dataToDelete = await _context.Vehicles.AsNoTracking()
                    .Where(x => x.LicenseNumberLong == dto.licenseNumberLong &&
                                x.FormMode == dto.FormMode &&
                                x.Status == ConstantValue.Status_Pending &&
                                x.CreatedDate.Date == DateTime.Now.Date)
                    .ToListAsync();
                _context.Vehicles.RemoveRange(dataToDelete);
                //await _context.SaveChangesAsync(); // if with T_id only have one fromMode and delete it cause duplicate then will change new T_id and prevent if something wrong not delete it
            }
            else
            {
                bool checkFormModeDuplicate = await _context.Vehicles.AsNoTracking()
                           .AnyAsync(x => x.CreatedDate.Date == DateTime.Now.Date &&
                                          x.LicenseNumberLong == dto.licenseNumberLong &&
                                          x.FormMode == dto.FormMode &&
                                          x.Status == ConstantValue.Status_Pending);

                if (checkFormModeDuplicate)
                    return (false, true); //duplicate formMode in one single day (not done, duplicate)
            }

            var licenOnlys = await _context.LicenseOnlys.Where(x => x.License_Number == dto.licenseNumberLong.Replace("**", "/") &&
                                                                      x.NRC_Number == dto.NRC)
                                                          .OrderByDescending(x => x.CreatedDate)
                                                          .FirstOrDefaultAsync();

            if (licenOnlys != null)
            {
                #region *** generate new Transaction and Chalen ID ***

                //var checkTandC = await _context.Vehicles.AsNoTracking()
                //        .FirstOrDefaultAsync(x => x.LicenseNumberLong == dto.licenseNumberLong &&
                //                                  x.CreatedDate.Date == DateTime.Now.Date);

                var checkTandC = await _context.Vehicles.AsNoTracking()
                       .Where(x => x.LicenseNumberLong == dto.licenseNumberLong &&
                                                 x.CreatedDate.Date == DateTime.Now.Date)
                       .Select(x => new {x.Transaction_Id, x.ChalenNumber})
                       .FirstOrDefaultAsync();

                string TransactionIdN = string.Empty;
                string ChalenNumberN = string.Empty;

                if (checkTandC == null)
                {
                    var vehicleObj = await _context.Vehicles.AsNoTracking().ToListAsync();

                    int tG = vehicleObj.OrderByDescending(x => x.Transaction_Id)
                                       .Select(x => x.Transaction_Id.Split('_').LastOrDefault())
                                       .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
                                       .FirstOrDefault(); //order by year and then other number

                    int cG = vehicleObj.OrderByDescending(x => x.ChalenNumber)
                                       .Select(x => x.ChalenNumber.Split('_').LastOrDefault())
                                       .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
                                       .FirstOrDefault();

                    TransactionIdN = new CommonMethod().GenerateT_IdandC_Id("T", ++tG, 9); //generate Id
                    ChalenNumberN = new CommonMethod().GenerateT_IdandC_Id("C", ++cG, 7); //generate I
                }
                else
                {
                    TransactionIdN = checkTandC.Transaction_Id;
                    ChalenNumberN = checkTandC.ChalenNumber;
                }
                #endregion

                #region *** create for license only folder ***

                string rootPath = _iConfig.GetSection("Upload_FolderPath").Value;
                string firstFolderName = new CommonMethod().FilePathNameString(dto.licenseNumberLong);
                string dateFolderName = Path.Combine("Extention_Care", DateTime.Now.ToString("yyyyMMdd"));
                string rootFolder = Path.Combine(dateFolderName, firstFolderName);
                string savePath = Path.Combine(rootPath, rootFolder);
                string rootFolderR = rootFolder.Replace("\\", "/");
                try
                {
                    if (!Directory.Exists(savePath))
                        Directory.CreateDirectory(savePath);
                }
                catch (Exception e) { Console.WriteLine(e.ToString()); }
                #endregion

                #region *** Save License Attached Files ***

                string pathAttachFile_NRC = "";
                if (dto.AttachFile_NRC != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.AttachFile_NRC, savePath + "\\NRC.pdf");
                    if (oky)
                        pathAttachFile_NRC = rootFolderR + "/NRC.pdf";
                }

                string pathAttachFile_M10 = "";
                if (dto.AttachFile_M10 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.AttachFile_M10, savePath + "\\M10.pdf");
                    if (oky)
                        pathAttachFile_M10 = rootFolderR + "/M10.pdf";
                }

                string pathAttachFile_RecommandDoc1 = "";
                if (dto.AttachFile_RecommandDoc1 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.AttachFile_RecommandDoc1, savePath + "\\Doc1.pdf");
                    if (oky)
                        pathAttachFile_RecommandDoc1 = rootFolderR + "/Doc1.pdf";
                }

                string pathAttachFile_RecommandDoc2 = "";
                if (dto.AttachFile_RecommandDoc2 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.AttachFile_RecommandDoc2, savePath + "\\Doc2.pdf");
                    if (oky)
                        pathAttachFile_RecommandDoc2 = rootFolderR + "/Doc2.pdf";
                }

                string pathAttachFile_OperatorLicense = "";
                if (dto.AttachFile_OperatorLicense != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.AttachFile_OperatorLicense, savePath + "\\OperatorLicense.pdf");
                    if (oky)
                        pathAttachFile_OperatorLicense = rootFolderR + "/OperatorLicense.pdf";
                }

                string pathAttachFile_Part1 = "";
                if (dto.AttachFile_Part1 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.AttachFile_Part1, savePath + "\\Part1.pdf");
                    if (oky)
                        pathAttachFile_Part1 = rootFolderR + "/Part1.pdf";
                }
                #endregion

                #region *** LicenseOnly Process ***
                licenOnlys.Transaction_Id = TransactionIdN;
                licenOnlys.AttachFile_NRC = pathAttachFile_NRC;
                licenOnlys.AttachFile_M10 = pathAttachFile_M10;
                licenOnlys.AttachFile_Part1 = pathAttachFile_Part1;
                licenOnlys.AttachFile_RecommandDoc1 = pathAttachFile_RecommandDoc1;
                licenOnlys.AttachFile_RecommandDoc2 = pathAttachFile_RecommandDoc2;
                licenOnlys.AttachFile_OperatorLicense = pathAttachFile_OperatorLicense;
                licenOnlys.UpdatedDate = DateTime.Now;
                licenOnlys.FormMode = dto.FormMode;
                _context.LicenseOnlys.Update(licenOnlys);
                #endregion

                #region *** ExtendOperator record add in Vehicle Table ***
                foreach (var item in dto.CarAttachedFiles)
                {
                    var vehicle = _context.Vehicles.Find(item.CreateCarId);
                    if (vehicle != null)
                    {
                        //vehicleWeightIdObj = vehicle.VehicleWeightId; //each license number long of weight are same
                        //create folder
                        string vehicleFolderName = "VehicleId_" + item.CreateCarId;
                        //string dateFolderName = Path.Combine("Vehicle_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), vehicleFolderName);
                        string vehicleSavePath = Path.Combine(rootPath, dateFolderName);
                        string dateFolderNameR = dateFolderName.Replace("\\", "/");

                        try
                        {
                            if (!Directory.Exists(vehicleSavePath))
                                Directory.CreateDirectory(vehicleSavePath);
                        }
                        catch (Exception e) { Console.WriteLine(e.ToString()); }

                        // save TriangleFiles
                        string pathTriangleFiles = string.Empty;
                        if (item.TriangleFiles != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.TriangleFiles, vehicleSavePath + "\\Triangle.pdf");
                            if (oky)
                                pathTriangleFiles = dateFolderNameR + "/Triangle.pdf";
                        }

                        // save OwnerBookFiles
                        string pathOwnerBookFiles = string.Empty;
                        if (item.OwnerBookFiles != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.OwnerBookFiles, vehicleSavePath + "\\OwnerBook.pdf");
                            if (oky)
                                pathOwnerBookFiles = dateFolderNameR + "/OwnerBook.pdf";
                        }

                        // save AttachedFiles1
                        string pathAttachedFiles1 = string.Empty;
                        if (item.AttachedFiles1 != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.AttachedFiles1, vehicleSavePath + "\\CarAttached1.pdf");
                            if (oky)
                                pathAttachedFiles1 = dateFolderNameR + "/CarAttached1.pdf";
                        }

                        // save AttachedFiles2
                        string pathAttachedFiles2 = string.Empty;
                        if (item.AttachedFiles2 != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.AttachedFiles2, vehicleSavePath + "\\CarAttached2.pdf");
                            if (oky)
                                pathAttachedFiles2 = dateFolderNameR + "/CarAttached2.pdf";
                        }


                        vehicle.VehicleId = ConstantValue.Zero; //for new insert
                        vehicle.Transaction_Id = TransactionIdN;
                        vehicle.ChalenNumber = ChalenNumberN;
                        vehicle.Triangle = pathTriangleFiles;
                        vehicle.OwnerBook = pathOwnerBookFiles;
                        vehicle.AttachedFile1 = pathAttachedFiles1;
                        vehicle.AttachedFile2 = pathAttachedFiles2;
                        vehicle.CreatedDate = DateTime.Now;
                        //vehicle.CreatedDate = DateTime.Now.Date;
                        vehicle.CertificatePrinted = false;
                        vehicle.Part1Printed = false;
                        vehicle.Part2Printed = false;
                        vehicle.TrianglePrinted = false;
                        vehicle.License_Number = vehicle.LicenseNumberLong.Substring(2, vehicle.LicenseNumberLong.IndexOf("(") - 3);
                        vehicle.Status = ConstantValue.Status_Pending;
                        vehicle.FormMode = ConstantValue.EOPL_FM; //form mode for extendoperatorlicense

                        _context.Vehicles.Add(vehicle);
                    }
                }
                #endregion

                await _context.SaveChangesAsync();
                return (true, false); // (done, not duplicate)
            }
            return (false, false); //(not done, not duplicate)
        }

        public async Task<(bool, bool)> ExtendOperatorLicenseProcess(OperatorLicenseAttachVM dto)
        {
            if (dto.TakeNewRecord != null && dto.TakeNewRecord == true)
            {
                var dataToDelete = await _context.Vehicles.AsNoTracking()
                    .Where(x => x.LicenseNumberLong == dto.licenseNumberLong &&
                                x.FormMode == dto.FormMode &&
                                x.Status == ConstantValue.Status_Pending &&
                                x.CreatedDate.Date == DateTime.Now.Date)
                    .ToListAsync();
                _context.Vehicles.RemoveRange(dataToDelete);
                //await _context.SaveChangesAsync(); // if with T_id only have one fromMode and delete it cause duplicate then will change new T_id and prevent if something wrong not delete it
            }
            else
            {
                bool checkFormModeDuplicate = await _context.Vehicles.AsNoTracking()
                           .AnyAsync(x => x.CreatedDate.Date == DateTime.Now.Date &&
                                          x.LicenseNumberLong == dto.licenseNumberLong &&
                                          x.FormMode == dto.FormMode &&
                                          x.Status == ConstantValue.Status_Pending);

                if (checkFormModeDuplicate)
                    return (false, true); //duplicate formMode in one single day (not done, duplicate)
            }

            var licenOnlys = await _context.LicenseOnlys.AsNoTracking()
                    .Where(x => x.License_Number == dto.licenseNumberLong.Replace("**", "/") &&
                                x.NRC_Number == dto.NRC)
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefaultAsync();

            if (licenOnlys != null)
            {
                #region *** generate new Transaction and Chalen ID ***

                //var checkTandC = await _context.Vehicles.AsNoTracking()
                //        .FirstOrDefaultAsync(x => x.LicenseNumberLong == dto.licenseNumberLong &&
                //                                  x.CreatedDate.Date == DateTime.Now.Date);

                var checkTandC = await _context.Vehicles.AsNoTracking()
                       .Where(x => x.LicenseNumberLong == dto.licenseNumberLong &&
                                                 x.CreatedDate.Date == DateTime.Now.Date)
                       .Select(x => new { x.Transaction_Id, x.ChalenNumber })
                       .FirstOrDefaultAsync();

                string TransactionIdN = string.Empty;
                string ChalenNumberN = string.Empty;

                if (checkTandC == null)
                {
                    //var vehicleObj = await _context.Vehicles.AsNoTracking().ToListAsync();
                    var vehicleObj = await _context.Vehicles.AsNoTracking().Where(x => x.CreatedDate >= DateTime.Now.Date.AddMonths(-1)).ToListAsync();
                    //ရှိတမျ data အကုန်ဆွဲ ထုတ်တာထက် လက်ရှိရောက်နေတဲ့ ခုနှစ်ထက် တစ်လလောက် နောက်ဆုတ်ပီး ဆွဲထုတ်တာ ပိုပေါ့ (if there is no operation duing last month it would wrong)


                    int tG = vehicleObj.OrderByDescending(x => x.Transaction_Id)
                                       .Select(x => x.Transaction_Id.Split('_').LastOrDefault())
                                       .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
                                       .FirstOrDefault(); //order by year and then other number

                    int cG = vehicleObj.OrderByDescending(x => x.ChalenNumber)
                                       .Select(x => x.ChalenNumber.Split('_').LastOrDefault())
                                       .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
                                       .FirstOrDefault();

                    TransactionIdN = new CommonMethod().GenerateT_IdandC_Id("T", ++tG, 9); //generate Id
                    ChalenNumberN = new CommonMethod().GenerateT_IdandC_Id("C", ++cG, 7); //generate I
                }
                else
                {
                    TransactionIdN = checkTandC.Transaction_Id;
                    ChalenNumberN = checkTandC.ChalenNumber;
                }
                #endregion

                #region *** create for license only folder ***

                string rootPath = _iConfig.GetSection("Upload_FolderPath").Value;
                string firstFolderName = new CommonMethod().FilePathNameString(dto.licenseNumberLong);
                string dateFolderName = Path.Combine("Extention_Care", DateTime.Now.ToString("yyyyMMdd"));
                string rootFolder = Path.Combine(dateFolderName, firstFolderName);
                string savePath = Path.Combine(rootPath, rootFolder);
                string rootFolderR = rootFolder.Replace("\\", "/");
                try
                {
                    if (!Directory.Exists(savePath))
                        Directory.CreateDirectory(savePath);
                }
                catch (Exception e) { Console.WriteLine(e.ToString()); }
                #endregion

                #region *** Save License Attached Files ***

                string pathAttachFile_NRC = "";
                if (dto.AttachFile_NRC != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.AttachFile_NRC, savePath + "\\NRC.pdf");
                    if (oky)
                        pathAttachFile_NRC = rootFolderR + "/NRC.pdf";
                }

                string pathAttachFile_M10 = "";
                if (dto.AttachFile_M10 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.AttachFile_M10, savePath + "\\M10.pdf");
                    if (oky)
                        pathAttachFile_M10 = rootFolderR + "/M10.pdf";
                }

                string pathAttachFile_RecommandDoc1 = "";
                if (dto.AttachFile_RecommandDoc1 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.AttachFile_RecommandDoc1, savePath + "\\Doc1.pdf");
                    if (oky)
                        pathAttachFile_RecommandDoc1 = rootFolderR + "/Doc1.pdf";
                }

                string pathAttachFile_RecommandDoc2 = "";
                if (dto.AttachFile_RecommandDoc2 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.AttachFile_RecommandDoc2, savePath + "\\Doc2.pdf");
                    if (oky)
                        pathAttachFile_RecommandDoc2 = rootFolderR + "/Doc2.pdf";
                }

                string pathAttachFile_OperatorLicense = "";
                if (dto.AttachFile_OperatorLicense != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.AttachFile_OperatorLicense, savePath + "\\OperatorLicense.pdf");
                    if (oky)
                        pathAttachFile_OperatorLicense = rootFolderR + "/OperatorLicense.pdf";
                }

                string pathAttachFile_Part1 = "";
                if (dto.AttachFile_Part1 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.AttachFile_Part1, savePath + "\\Part1.pdf");
                    if (oky)
                        pathAttachFile_Part1 = rootFolderR + "/Part1.pdf";
                }
                #endregion

                Temp_Table tempTable = new Temp_Table();

                #region *** LicenseOnly Process ***
                //licenOnlys.Transaction_Id = TransactionIdN;
                //licenOnlys.AttachFile_NRC = pathAttachFile_NRC;
                //licenOnlys.AttachFile_M10 = pathAttachFile_M10;
                //licenOnlys.AttachFile_Part1 = pathAttachFile_Part1;
                //licenOnlys.AttachFile_RecommandDoc1 = pathAttachFile_RecommandDoc1;
                //licenOnlys.AttachFile_RecommandDoc2 = pathAttachFile_RecommandDoc2;
                //licenOnlys.AttachFile_OperatorLicense = pathAttachFile_OperatorLicense;
                //licenOnlys.UpdatedDate = DateTime.Now;
                //licenOnlys.FormMode = dto.FormMode;
                //_context.LicenseOnlys.Update(licenOnlys);

                tempTable.LicenseOnlyId = licenOnlys.LicenseOnlyId;
                tempTable.Transaction_Id = TransactionIdN;
                tempTable.LicenseNumberLong = licenOnlys.License_Number;
                tempTable.NRC_Number = licenOnlys.NRC_Number;
                tempTable.AttachFile_NRC = pathAttachFile_NRC;
                tempTable.AttachFile_M10 = pathAttachFile_M10;
                tempTable.AttachFile_Part1 = pathAttachFile_Part1;
                tempTable.AttachFile_RecommandDoc1 = pathAttachFile_RecommandDoc1;
                tempTable.AttachFile_RecommandDoc2 = pathAttachFile_RecommandDoc2;
                tempTable.FormMode = dto.FormMode;
                tempTable.CreatedDate = DateTime.Now;

                #endregion

                #region *** ExtendOperator record add in Vehicle Table ***
                foreach (var item in dto.CarAttachedFiles)
                {
                    var vehicle = await _context.Vehicles.AsNoTracking().SingleOrDefaultAsync(x => x.VehicleId == item.CreateCarId);
                    if (vehicle != null)
                    {
                        //vehicleWeightIdObj = vehicle.VehicleWeightId; //each license number long of weight are same
                        //create folder
                        string vehicleFolderName = "VehicleId_" + item.CreateCarId;
                        //string dateFolderName = Path.Combine("Vehicle_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), vehicleFolderName);
                        string vehicleSavePath = Path.Combine(rootPath, dateFolderName);
                        string dateFolderNameR = dateFolderName.Replace("\\", "/");

                        try
                        {
                            if (!Directory.Exists(vehicleSavePath))
                                Directory.CreateDirectory(vehicleSavePath);
                        }
                        catch (Exception e) { Console.WriteLine(e.ToString()); }

                        // save TriangleFiles
                        string pathTriangleFiles = string.Empty;
                        if (item.TriangleFiles != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.TriangleFiles, vehicleSavePath + "\\Triangle.pdf");
                            if (oky)
                                pathTriangleFiles = dateFolderNameR + "/Triangle.pdf";
                        }

                        // save OwnerBookFiles
                        string pathOwnerBookFiles = string.Empty;
                        if (item.OwnerBookFiles != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.OwnerBookFiles, vehicleSavePath + "\\OwnerBook.pdf");
                            if (oky)
                                pathOwnerBookFiles = dateFolderNameR + "/OwnerBook.pdf";
                        }

                        // save AttachedFiles1
                        string pathAttachedFiles1 = string.Empty;
                        if (item.AttachedFiles1 != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.AttachedFiles1, vehicleSavePath + "\\CarAttached1.pdf");
                            if (oky)
                                pathAttachedFiles1 = dateFolderNameR + "/CarAttached1.pdf";
                        }

                        // save AttachedFiles2
                        string pathAttachedFiles2 = string.Empty;
                        if (item.AttachedFiles2 != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.AttachedFiles2, vehicleSavePath + "\\CarAttached2.pdf");
                            if (oky)
                                pathAttachedFiles2 = dateFolderNameR + "/CarAttached2.pdf";
                        }


                        vehicle.VehicleId = ConstantValue.Zero; //for new insert
                        vehicle.Transaction_Id = TransactionIdN;
                        vehicle.ChalenNumber = ChalenNumberN;
                        vehicle.Triangle = pathTriangleFiles;
                        vehicle.OwnerBook = pathOwnerBookFiles;
                        vehicle.AttachedFile1 = pathAttachedFiles1;
                        vehicle.AttachedFile2 = pathAttachedFiles2;
                        vehicle.CreatedDate = DateTime.Now;
                        vehicle.CertificatePrinted = false;
                        vehicle.Part1Printed = false;
                        vehicle.Part2Printed = false;
                        vehicle.TrianglePrinted = false;
                        vehicle.License_Number = vehicle.LicenseNumberLong.Substring(2, vehicle.LicenseNumberLong.IndexOf("(") - 3);
                        vehicle.Status = ConstantValue.Status_Pending;
                        vehicle.FormMode = ConstantValue.EOPL_FM; //form mode for extendoperatorlicense
                        _context.Vehicles.Add(vehicle);
                    }
                }
                #endregion

                _context.Temp_Tables.Add(tempTable);
                await _context.SaveChangesAsync();
                return (true, false); // (done, not duplicate)
            }
            return (false, false); //(not done, not duplicate)
        }


        //ok pdf
        //public async Task<string> VehicleAttach(OperatorLicenseAttachVM carAttachedFilelVM, string rootPath)
        //{
        //    var vehicleObj = await _context.Vehicles.ToListAsync();

        //    int tG = vehicleObj.OrderByDescending(x => x.Transaction_Id)
        //                       .Select(x => x.Transaction_Id.Split('_').LastOrDefault())
        //                       .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
        //                       .FirstOrDefault(); //order by year and then other number

        //    int cG = vehicleObj.OrderByDescending(x => x.ChalenNumber)
        //                       .Select(x => x.ChalenNumber.Split('_').LastOrDefault())
        //                       .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
        //                       .FirstOrDefault();

        //    string TransactionIdN = new CommonMethod().GenerateT_IdandC_Id("T", ++tG, 9); //generate Id
        //    string ChalenNumberN = new CommonMethod().GenerateT_IdandC_Id("C", ++cG, 7); //generate Id

        //    int vehicleWeightIdObj = 0;

        //    foreach (var item in carAttachedFilelVM.CarAttachedFiles)
        //    {
        //        var vehicle = _context.Vehicles.Find(item.CreateCarId);
        //        if (vehicle != null)
        //        {
        //            vehicleWeightIdObj = vehicle.VehicleWeightId; //each license number long of weight are same
        //            //create folder
        //            string vehicleFolderName = "VehicleId_" + item.CreateCarId;
        //            string dateFolderName = Path.Combine("Vehicle_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), vehicleFolderName);
        //            string vehicleSavePath = Path.Combine(rootPath, dateFolderName);
        //            string dateFolderNameR = dateFolderName.Replace("\\", "/");

        //            try
        //            {
        //                if (!Directory.Exists(vehicleSavePath))
        //                    Directory.CreateDirectory(vehicleSavePath);
        //            }
        //            catch (Exception e) { Console.WriteLine(e.ToString()); }

        //            // save TriangleFiles
        //            string pathTriangleFiles = string.Empty;
        //            if (item.TriangleFiles != null)
        //            {
        //                bool oky = await AddOperatorLicenseAttachPDF(item.TriangleFiles, vehicleSavePath + "\\Triangle.pdf");
        //                if (oky)
        //                    pathTriangleFiles = dateFolderNameR + "/Triangle.pdf";
        //            }

        //            // save OwnerBookFiles
        //            string pathOwnerBookFiles = string.Empty;
        //            if (item.OwnerBookFiles != null)
        //            {
        //                bool oky = await AddOperatorLicenseAttachPDF(item.OwnerBookFiles, vehicleSavePath + "\\OwnerBook.pdf");
        //                if (oky)
        //                    pathOwnerBookFiles = dateFolderNameR + "/OwnerBook.pdf";
        //            }


        //            // save AttachedFiles1
        //            string pathAttachedFiles1 = string.Empty;
        //            if (item.AttachedFiles1 != null)
        //            {
        //                bool oky = await AddOperatorLicenseAttachPDF(item.AttachedFiles1, vehicleSavePath + "\\CarAttached1.pdf");
        //                if (oky)
        //                    pathAttachedFiles1 = dateFolderNameR + "/CarAttached1.pdf";
        //            }

        //            // save AttachedFiles1
        //            string pathAttachedFiles2 = string.Empty;
        //            if (item.AttachedFiles2 != null)
        //            {
        //                bool oky = await AddOperatorLicenseAttachPDF(item.AttachedFiles2, vehicleSavePath + "\\CarAttached2.pdf");
        //                if (oky)
        //                    pathAttachedFiles2 = dateFolderNameR + "/CarAttached2.pdf";
        //            }


        //            vehicle.VehicleId = ConstantValue.Zero; //for new insert
        //            vehicle.Transaction_Id = TransactionIdN;
        //            vehicle.ChalenNumber = ChalenNumberN;
        //            vehicle.Triangle = pathTriangleFiles;
        //            vehicle.OwnerBook = pathOwnerBookFiles;
        //            vehicle.AttachedFile1 = pathAttachedFiles1;
        //            vehicle.AttachedFile2 = pathAttachedFiles2;
        //            vehicle.License_Number = vehicle.LicenseNumberLong.Substring(2, vehicle.LicenseNumberLong.IndexOf("(") - 3);
        //            vehicle.Status = ConstantValue.Status_Pending;
        //            vehicle.FormMode = ConstantValue.EOPL_FM; //form mode for extendoperatorlicense

        //            _context.Vehicles.Add(vehicle);
        //            await _context.SaveChangesAsync();
        //        }
        //    }

        //    return TransactionIdN;
        //}

        #region workOverHttpOrHttps
        //public async Task<string> VehicleAttach(OperatorLicenseAttachVM carAttachedFilelVM, string sharedFolderIp, string sharedOverHttp)
        //{
        //    var vehicleObj = await _context.Vehicles.ToListAsync();

        //    int tG = vehicleObj.OrderByDescending(x => x.Transaction_Id)
        //                       .Select(x => x.Transaction_Id.Split('_').LastOrDefault())
        //                       .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
        //                       .FirstOrDefault(); //order by year and then other number

        //    //int tG = vehicleObj.Select(x => x.Transaction_Id.Split('_').LastOrDefault())
        //    //                   .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)    
        //    //                   .Max(); //get largest number

        //    int cG = vehicleObj.OrderByDescending(x => x.ChalenNumber)
        //                       .Select(x => x.ChalenNumber.Split('_').LastOrDefault())
        //                       .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
        //                       .FirstOrDefault();

        //    //int cG = vehicleObj.Select(x => x.ChalenNumber.Split('_').LastOrDefault())
        //    //                   .Select(x => int.TryParse(x, out int val)? val : int.MaxValue)
        //    //                   .Max(); //get largest number


        //    string TransactionIdN = new CommonMethod().GenerateT_IdandC_Id("T", ++tG, 9); //generate Id
        //    string ChalenNumberN = new CommonMethod().GenerateT_IdandC_Id("C", ++cG, 7); //generate Id
        //    #region old T_Id and C_Id generate method
        //    //int tGreatest = vehicleObj.Max(x => int.TryParse(x.Transaction_Id, out int val) ? val : int.MinValue); //get greates number
        //    //int cGreatest = vehicleObj.Max(x => int.TryParse(x.ChalenNumber, out int val) ? val : int.MinValue);  //get greates number
        //    //string TransactionIdN = new CommonMethod().IntToStringByL(++tGreatest, 9); //generate new transaction
        //    //string ChalenNumberN = new CommonMethod().IntToStringByL(++cGreatest, 6); //generate new chalenNumber
        //    #endregion
        //    int vehicleWeightIdObj =0;

        //    foreach (var item in carAttachedFilelVM.CarAttachedFiles)
        //    {
        //        var vehicle = _context.Vehicles.Find(item.CreateCarId);
        //        if (vehicle != null)
        //        {
        //            vehicleWeightIdObj = vehicle.VehicleWeightId; //each license number long of weight are same
        //            //create folder
        //            string vehicleFolderName = "VehicleId_" + item.CreateCarId;
        //            string dateFolderName = Path.Combine("Vehicle_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), vehicleFolderName);
        //            string vehicleSavePath = Path.Combine(sharedFolderIp, dateFolderName);
        //            string dateFolderNameR = dateFolderName.Replace("\\", "/");

        //            try
        //            {
        //                if (!Directory.Exists(vehicleSavePath))
        //                    Directory.CreateDirectory(vehicleSavePath);
        //            }
        //            catch (Exception e) { Console.WriteLine(e.ToString()); }

        //            // save TriangleFiles
        //            string pathTriangleFiles = string.Empty;
        //            if (item.TriangleFiles != null)
        //            {
        //                bool oky = await AddOperatorLicenseAttachPDF(item.TriangleFiles, vehicleSavePath +"\\Triangle.pdf");
        //                if (oky)
        //                    pathTriangleFiles = dateFolderNameR + "/Triangle.pdf";
        //            }

        //            // save OwnerBookFiles
        //            string pathOwnerBookFiles = string.Empty;
        //            if (item.OwnerBookFiles != null)
        //            {
        //                bool oky = await AddOperatorLicenseAttachPDF(item.OwnerBookFiles, vehicleSavePath + "\\OwnerBook.pdf");
        //                if (oky)
        //                    pathOwnerBookFiles = dateFolderNameR + "/OwnerBook.pdf";
        //            }


        //            // save AttachedFiles1
        //            string pathAttachedFiles1 = string.Empty;
        //            if (item.AttachedFiles1 != null)
        //            {
        //                bool oky = await AddOperatorLicenseAttachPDF(item.AttachedFiles1, vehicleSavePath + "\\CarAttached1.pdf");
        //                if (oky)
        //                    pathAttachedFiles1 = dateFolderNameR + "/CarAttached1.pdf";
        //            }

        //            // save AttachedFiles1
        //            string pathAttachedFiles2 = string.Empty;
        //            if (item.AttachedFiles2 != null)
        //            {
        //                bool oky = await AddOperatorLicenseAttachPDF(item.AttachedFiles2, vehicleSavePath + "\\CarAttached2.pdf");
        //                if (oky)
        //                    pathAttachedFiles2 = dateFolderNameR + "/CarAttached2.pdf";
        //            }


        //            vehicle.VehicleId = ConstantValue.Zero; //for new insert
        //            vehicle.Transaction_Id = TransactionIdN;
        //            vehicle.ChalenNumber = ChalenNumberN;
        //            vehicle.Triangle = pathTriangleFiles;
        //            vehicle.OwnerBook = pathOwnerBookFiles;
        //            vehicle.AttachedFile1 = pathAttachedFiles1;
        //            vehicle.AttachedFile2 = pathAttachedFiles2;
        //            vehicle.License_Number = vehicle.LicenseNumberLong.Substring(2, vehicle.LicenseNumberLong.IndexOf("(") - 3);
        //            vehicle.Status = ConstantValue.Status_Pending;
        //            vehicle.FormMode = ConstantValue.EOPL_FM; //form mode for extendoperatorlicense

        //            _context.Vehicles.Add(vehicle);
        //            await _context.SaveChangesAsync();
        //        }
        //    }

        //    #region Transaction Add
        //    //get Fee data
        //    //var feesObj = await _context.Fees.Where(x => x.JourneyTypeId == (carAttachedFilelVM.licenseNumberLong.Contains(ConstantValue.Twin) ? ConstantValue.One : ConstantValue.Two) 
        //    //                                    && x.VehicleWeightId == vehicleWeightIdObj).FirstOrDefaultAsync();
        //    //int partOneFeesObj = ConstantValue.twoThousand * carAttachedFilelVM.CarAttachedFiles.Count;
        //    //int partTwoFeesObj = ConstantValue.oneThousand * carAttachedFilelVM.CarAttachedFiles.Count;
        //    //var transcationObj = new Transaction()
        //    //{
        //    //    Transaction_Id = TransactionIdN,
        //    //    ChalenNumber = ChalenNumberN,
        //    //    NRC_Number = carAttachedFilelVM.NRC,
        //    //    RegistrationFees = feesObj != null ? feesObj.RegistrationFees : ConstantValue.Nero,
        //    //    RegistrationCharges = feesObj != null ? feesObj.RegistrationCharges : ConstantValue.Nero,
        //    //    TriangleFees = feesObj !=null? feesObj.TriangleFees : ConstantValue.Nero,
        //    //    CertificateFees = ConstantValue.twoThousand,
        //    //    PartOneFees = partOneFeesObj,
        //    //    PartTwoFees = partTwoFeesObj,
        //    //    ModifiedCharges = ConstantValue.Nero,
        //    //    TotalCars = carAttachedFilelVM.CarAttachedFiles.Count,
        //    //    Total_WithoutCertificate = (feesObj != null ? feesObj.RegistrationCharges : ConstantValue.Nero) + ConstantValue.twoThousand,
        //    //    Total = partOneFeesObj + partTwoFeesObj + (feesObj != null ? feesObj.TriangleFees : ConstantValue.Nero),
        //    //    Status = ConstantValue.Status_Pending,
        //    //};
        //    //_context.Transactions.Add(transcationObj);
        //    //await _context.SaveChangesAsync();
        //    #endregion

        //    return TransactionIdN;
        //}
        #endregion

        #region not Use for PDF File Converter
        //public bool VehicleAttach(List<CarAttachedFileVM> carAttachedFilelVM, string sharedFolderIp, string sharedOverHttp)
        //{
        //    foreach (var item in carAttachedFilelVM)
        //    {
        //        var vehicle = _context.Vehicles.Find(item.CreateCarId);
        //        if (vehicle != null)
        //        {
        //            //create folder
        //            string vehicleFolderName = Path.Combine("Vehicle_AttachedFiles", "VehicleId_" + item.CreateCarId);
        //            string vehicleSavePath = Path.Combine(sharedFolderIp, vehicleFolderName);
        //            try
        //            {
        //                if (!Directory.Exists(vehicleSavePath))
        //                    Directory.CreateDirectory(vehicleSavePath);
        //            }
        //            catch (Exception e) { Console.WriteLine(e.ToString()); }

        //            // save TriangleFiles
        //            string pathTriangleFiles = "";
        //            if (item.TriangleFiles != null)
        //            {
        //                int index = 1;
        //                foreach (var file in item.TriangleFiles)
        //                {
        //                    bool okay = new CommonMethod().SaveImage(file, vehicleSavePath + "\\" + "Triangle_" + index + "_.jpg");
        //                    if (okay)
        //                        pathTriangleFiles += sharedOverHttp + vehicleFolderName.Replace("\\", "/") + "/Triangle_" + index + "_.jpg<";
        //                    index++;
        //                }
        //            }

        //            // save OwnerBookFiles
        //            string pathOwnerBookFiles = "";
        //            if (item.OwnerBookFiles != null)
        //            {
        //                int index = 1;
        //                foreach (var file in item.OwnerBookFiles)
        //                {
        //                    bool okay = new CommonMethod().SaveImage(file, vehicleSavePath + "\\" + "OwnerBook_" + index + "_.jpg");
        //                    if (okay)
        //                        pathOwnerBookFiles += sharedOverHttp + vehicleFolderName.Replace("\\", "/") + "/OwnerBook_" + index + "_.jpg<";
        //                    index++;
        //                }
        //            }

        //            // save AttachedFiles1
        //            string pathAttachedFiles1 = "";
        //            if (item.AttachedFiles1 != null)
        //            {
        //                int index = 1;
        //                foreach (var file in item.AttachedFiles1)
        //                {
        //                    bool okay = new CommonMethod().SaveImage(file, vehicleSavePath + "\\" + "AttachedFile1_" + index + "_.jpg");
        //                    if (okay)
        //                        pathAttachedFiles1 += sharedOverHttp + vehicleFolderName.Replace("\\", "/") + "/AttachedFile1_" + index + "_.jpg<";
        //                    index++;
        //                }
        //            }

        //            // save AttachedFiles1
        //            string pathAttachedFiles2 = "";
        //            if (item.AttachedFiles2 != null)
        //            {
        //                int index = 1;
        //                foreach (var file in item.AttachedFiles2)
        //                {
        //                    bool okay = new CommonMethod().SaveImage(file, vehicleSavePath + "\\" + "AttachedFile2_" + index + "_.jpg");
        //                    if (okay)
        //                        pathAttachedFiles2 += sharedOverHttp + vehicleFolderName.Replace("\\", "/") + "/AttachedFile2_" + index + "_.jpg<";
        //                    index++;
        //                }
        //            }
        //            vehicle.Triangle = pathTriangleFiles;
        //            vehicle.OwnerBook = pathOwnerBookFiles;
        //            vehicle.AttachedFile1 = pathAttachedFiles1;
        //            vehicle.AttachedFile2 = pathAttachedFiles2;
        //            _context.Vehicles.Update(vehicle);
        //            _context.SaveChanges();
        //        }
        //    }
        //    return true;
        //}


        //public async Task<bool> AddOperatorLicenseAttachPDF(List<IFormFile> formFiles, string savePath)
        //{
        //    bool oky = true;
        //    try
        //    {
        //        Document document = new Document();
        //        PdfWriter.GetInstance(document, new FileStream(savePath, FileMode.Create));

        //        document.Open();
        //        foreach (var item in formFiles)
        //        {
        //            if (item.ContentType == "image/png" || item.ContentType == "image/jpeg" || item.ContentType == "image/gif" || item.ContentType == "image/bmp")
        //            {
        //                byte[] bytes;
        //                using (var ms = new MemoryStream())
        //                {
        //                    await item.CopyToAsync(ms);
        //                    bytes = ms.ToArray();
        //                }

        //                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(bytes);
        //                //image.ScaleAbsolute(575f, 820.25f);
        //                image.Alignment = Element.ALIGN_CENTER;
        //                image.ScaleToFit(document.PageSize.Width - 10, document.PageSize.Height - 10);
        //                document.Add(image);

        //                //var paragraph = new Paragraph("Accept your self and just move on.");
        //                //document.Add(paragraph);

        //                document.NewPage();
        //            }
        //        }
        //        document.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        oky = false;
        //    }

        //    return oky;
        //}
        #endregion

        public async Task<bool> UpdateLicenseAttach(OperatorLicenseAttachVM operatorLicenseAttachVM, string transactionId)
        {
            //Create Folder
            string rootPath = _iConfig.GetSection("Upload_FolderPath").Value;

            string firstFolderName = new CommonMethod().FilePathNameString(operatorLicenseAttachVM.licenseNumberLong);
            string dateFolderName = Path.Combine("Extention_Care", DateTime.Now.ToString("yyyyMMdd"));
            string rootFolder = Path.Combine(dateFolderName, firstFolderName);
            string savePath = Path.Combine(rootPath, rootFolder);
            string rootFolderR = rootFolder.Replace("\\", "/");
            try
            {
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }

            #region save image files
            string pathAttachFile_NRC = "";
            if (operatorLicenseAttachVM.AttachFile_NRC != null)
            {
                bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(operatorLicenseAttachVM.AttachFile_NRC, savePath + "\\NRC.pdf");
                if (oky)
                    pathAttachFile_NRC = rootFolderR + "/NRC.pdf";
            }

            string pathAttachFile_M10 = "";
            if (operatorLicenseAttachVM.AttachFile_M10 != null)
            {
                bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(operatorLicenseAttachVM.AttachFile_M10, savePath + "\\M10.pdf");
                if (oky)
                    pathAttachFile_M10 = rootFolderR + "/M10.pdf";
            }

            string pathAttachFile_RecommandDoc1 = "";
            if (operatorLicenseAttachVM.AttachFile_RecommandDoc1 != null)
            {
                bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(operatorLicenseAttachVM.AttachFile_RecommandDoc1, savePath + "\\Doc1.pdf");
                if (oky)
                    pathAttachFile_RecommandDoc1 = rootFolderR + "/Doc1.pdf";
            }

            string pathAttachFile_RecommandDoc2 = "";
            if (operatorLicenseAttachVM.AttachFile_RecommandDoc2 != null)
            {
                bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(operatorLicenseAttachVM.AttachFile_RecommandDoc2, savePath + "\\Doc2.pdf");
                if (oky)
                    pathAttachFile_RecommandDoc2 = rootFolderR + "/Doc2.pdf";
            }

            string pathAttachFile_OperatorLicense = "";
            if (operatorLicenseAttachVM.AttachFile_OperatorLicense != null)
            {
                bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(operatorLicenseAttachVM.AttachFile_OperatorLicense, savePath + "\\OperatorLicense.pdf");
                if (oky)
                    pathAttachFile_OperatorLicense = rootFolderR + "/OperatorLicense.pdf";
            }

            string pathAttachFile_Part1 = "";
            if (operatorLicenseAttachVM.AttachFile_Part1 != null)
            {
                bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(operatorLicenseAttachVM.AttachFile_Part1, savePath + "\\Part1.pdf");
                if (oky)
                    pathAttachFile_Part1 = rootFolderR + "/Part1.pdf";
            }
            #endregion

            var licenOnlys = await _context.LicenseOnlys.Where(x => x.License_Number == operatorLicenseAttachVM.licenseNumberLong &&
                                                                   x.NRC_Number == operatorLicenseAttachVM.NRC)
                                                       .OrderByDescending(x => x.CreatedDate)
                                                       .ToListAsync();

            licenOnlys[0].Transaction_Id = transactionId;
            licenOnlys[0].AttachFile_NRC = pathAttachFile_NRC;
            licenOnlys[0].AttachFile_M10 = pathAttachFile_M10;
            licenOnlys[0].AttachFile_OperatorLicense = pathAttachFile_OperatorLicense;
            licenOnlys[0].AttachFile_Part1 = pathAttachFile_Part1;
            licenOnlys[0].AttachFile_RecommandDoc1 = pathAttachFile_RecommandDoc1;
            licenOnlys[0].AttachFile_RecommandDoc2 = pathAttachFile_RecommandDoc2;
            licenOnlys[0].UpdatedDate = DateTime.Now;

            _context.LicenseOnlys.Update(licenOnlys[0]);
            await _context.SaveChangesAsync();

            return true;
        }
                
        public async Task<(string, string, string, DateTime)> DecreaseCars(DecreaseCarVMList decreaseCarVMList)
        {
            try
            {
                string TransactionIdN = string.Empty;
                string ChalenNumberN = string.Empty;
                if (decreaseCarVMList.ChalenNumber == null || decreaseCarVMList.Transaction_Id == null)
                {
                    var vehicleObj = await _context.Vehicles.AsNoTracking().ToListAsync();

                    int tG = vehicleObj.OrderByDescending(x => x.Transaction_Id)
                                       .Select(x => x.Transaction_Id.Split('_').LastOrDefault())
                                       .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
                                       .FirstOrDefault(); //order by year and then other number

                    int cG = vehicleObj.OrderByDescending(x => x.ChalenNumber)
                                       .Select(x => x.ChalenNumber.Split('_').LastOrDefault())
                                       .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
                                       .FirstOrDefault();

                    TransactionIdN = new CommonMethod().GenerateT_IdandC_Id("T", ++tG, 9); //generate Id
                    ChalenNumberN = new CommonMethod().GenerateT_IdandC_Id("C", ++cG, 7); //generate Id
                }
                else
                {
                    TransactionIdN = decreaseCarVMList.Transaction_Id;
                    ChalenNumberN = decreaseCarVMList.ChalenNumber;
                }

                //int latestLicenseId = await _context.LicenseOnlys.Where(x => x.License_Number == decreaseCarVMList.LicenseNumberLong.Replace("**","/") && x.NRC_Number == decreaseCarVMList.NRC_Number)
                //                        .Select(x => x.LicenseOnlyId).FirstOrDefaultAsync();
                var licenOnlys = await _context.LicenseOnlys.Where(x => x.License_Number == decreaseCarVMList.LicenseNumberLong.Replace("**", "/") &&
                                                                        x.NRC_Number == decreaseCarVMList.NRC_Number)
                                                            .OrderByDescending(x => x.CreatedDate)
                                                            .FirstOrDefaultAsync();
                #region license update
                LicenseOnly newLicenseOnly = new LicenseOnly();
                string rootPath = _iConfig.GetSection("Upload_FolderPath").Value;
                string pathAttachFile_NRC = string.Empty;
                string AttachFile_M10 = string.Empty;
                string AttachFile_RecommandDoc1 = string.Empty;
                string AttachFile_RecommandDoc2 = string.Empty;
                string AttachFile_OperatorLicense = string.Empty;
                string AttachFile_Part1 = string.Empty;


                //create for license only folder
                string licenseOnlyFolderName = "VehicleId_" + licenOnlys.LicenseOnlyId;
                string licenseOnlyDateFolerName = Path.Combine("LicenseOnly_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), licenseOnlyFolderName);
                string licenseOnlySavePath = Path.Combine(rootPath, licenseOnlyDateFolerName);
                string licenseOnlyDateFolderNameR = licenseOnlyDateFolerName.Replace("\\", "/");

                try
                {
                    if (!Directory.Exists(licenseOnlySavePath))
                        Directory.CreateDirectory(licenseOnlySavePath);
                }
                catch (Exception e) { Console.WriteLine(decreaseCarVMList.ToString()); }


                // Save AttachFile_NRC
                if (decreaseCarVMList.AttachFile_NRC != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(decreaseCarVMList.AttachFile_NRC, licenseOnlySavePath + "\\AttachFile_NRC.pdf");
                    if (oky)
                        pathAttachFile_NRC = licenseOnlyDateFolderNameR + "/AttachFile_NRC.pdf";
                }

                // Save AttachFile_M10
                if (decreaseCarVMList.AttachFile_M10 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(decreaseCarVMList.AttachFile_M10, licenseOnlySavePath + "\\AttachFile_M10.pdf");
                    if (oky)
                        AttachFile_M10 = licenseOnlyDateFolderNameR + "/AttachFile_M10.pdf";
                }

                // Save AttachFile_RecommandDoc1
                if (decreaseCarVMList.AttachFile_RecommandDoc1 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(decreaseCarVMList.AttachFile_RecommandDoc1, licenseOnlySavePath + "\\AttachFile_RecommandDoc1.pdf");
                    if (oky)
                        AttachFile_RecommandDoc1 = licenseOnlyDateFolderNameR + "/AttachFile_RecommandDoc1.pdf";
                }

                // Save AttachFile_RecommandDoc2
                if (decreaseCarVMList.AttachFile_RecommandDoc2 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(decreaseCarVMList.AttachFile_RecommandDoc2, licenseOnlySavePath + "\\AttachFile_RecommandDoc2.pdf");
                    if (oky)
                        AttachFile_RecommandDoc2 = licenseOnlyDateFolderNameR + "/AttachFile_RecommandDoc2.pdf";
                }

                // Save AttachFile_OperatorLicense
                if (decreaseCarVMList.AttachFile_OperatorLicense != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(decreaseCarVMList.AttachFile_OperatorLicense, licenseOnlySavePath + "\\AttachFile_OperatorLicense.pdf");
                    if (oky)
                        AttachFile_OperatorLicense = licenseOnlyDateFolderNameR + "/AttachFile_OperatorLicense.pdf";
                }

                // Save AttachFile_Part1
                if (decreaseCarVMList.AttachFile_Part1 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(decreaseCarVMList.AttachFile_Part1, licenseOnlySavePath + "\\AttachFile_Part1.pdf");
                    if (oky)
                        AttachFile_Part1 = licenseOnlyDateFolderNameR + "/AttachFile_Part1.pdf";
                }

                if (licenOnlys != null)
                {
                    licenOnlys.Transaction_Id = TransactionIdN;
                    licenOnlys.AttachFile_NRC = pathAttachFile_NRC;
                    licenOnlys.AttachFile_M10 = AttachFile_M10;
                    licenOnlys.AttachFile_Part1 = AttachFile_Part1;
                    licenOnlys.AttachFile_RecommandDoc1 = AttachFile_RecommandDoc1;
                    licenOnlys.AttachFile_RecommandDoc2 = AttachFile_RecommandDoc2;
                    licenOnlys.AttachFile_OperatorLicense = AttachFile_OperatorLicense;
                    licenOnlys.CreatedDate = DateTime.Now;
                    licenOnlys.UpdatedDate = DateTime.Now;
                    _context.LicenseOnlys.Update(licenOnlys);
                }
                #endregion

                #region  Prepar for new decrease car record
                List<int> decreaseVehicleIdList = new List<int>();
                foreach (var item in decreaseCarVMList.DecreaseCarVMs)
                {
                    // Vehicle Process
                    var vehicleToDecrease = await _context.Vehicles.FindAsync(item.VehicleID);
                    if (vehicleToDecrease == null)
                        continue;

                    //folder variable
                    string vehicleFolderName = string.Empty;
                    string vehicleDateFolerName = string.Empty;
                    string vehicleSavePath = string.Empty;
                    string vehicleLateFolderNameR = string.Empty;

                    //file path variable
                    string ownerBookFile = string.Empty;
                    string triangelFile = string.Empty;
                    string pathAttachedFile2 = string.Empty;

                    if (decreaseCarVMList.DecreaseCarVMs[0].NewOwnerBook != null || decreaseCarVMList.DecreaseCarVMs[0].NewTriangle != null || decreaseCarVMList.DecreaseCarVMs[0].NewAttachedFile2 != null)
                    {
                        //create folder
                        vehicleFolderName = "VehicleId_" + item.VehicleID;
                        vehicleDateFolerName = Path.Combine("Vehicle_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), vehicleFolderName);
                        vehicleSavePath = Path.Combine(rootPath, vehicleDateFolerName);
                        vehicleLateFolderNameR = vehicleDateFolerName.Replace("\\", "/");

                        try
                        {
                            if (!Directory.Exists(vehicleSavePath))
                                Directory.CreateDirectory(vehicleSavePath);
                        }
                        catch (Exception e) { Console.WriteLine(e.ToString()); }
                    }


                    // Save OwnerBook
                    if (item.NewOwnerBook != null)
                    {
                        bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewOwnerBook, vehicleSavePath + "\\Owner.pdf");
                        if (oky)
                            ownerBookFile = vehicleLateFolderNameR + "/Owner.pdf";
                    }

                    // Save Triangle 
                    if (item.NewTriangle != null)
                    {
                        bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewTriangle, vehicleSavePath + "\\Triangle.pdf");
                        if (oky)
                            triangelFile = vehicleLateFolderNameR + "/Triangle.pdf";
                    }

                    // Save AttachedFile2
                    if (item.NewAttachedFile2 != null)
                    {
                        bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewAttachedFile2, vehicleSavePath + "\\AttachedFile2.pdf");
                        if (oky)
                            pathAttachedFile2 = vehicleLateFolderNameR + "/AttachedFile2.pdf";
                    }

                    vehicleToDecrease.VehicleId = ConstantValue.Zero;
                    vehicleToDecrease.Transaction_Id = TransactionIdN;
                    vehicleToDecrease.ChalenNumber = ChalenNumberN;
                    vehicleToDecrease.Status = ConstantValue.Status_Pending;
                    vehicleToDecrease.CertificatePrinted = false;
                    vehicleToDecrease.Part1Printed = false;
                    vehicleToDecrease.Part2Printed = false;
                    vehicleToDecrease.TrianglePrinted = false;
                    vehicleToDecrease.CreatedDate = DateTime.Now;
                    vehicleToDecrease.ApplyDate = DateTime.Now;
                    vehicleToDecrease.FormMode = ConstantValue.DecreaseCar_FM;
                    vehicleToDecrease.OwnerBook = ownerBookFile == string.Empty ? vehicleToDecrease.OwnerBook : ownerBookFile;
                    vehicleToDecrease.Triangle = triangelFile == string.Empty ? vehicleToDecrease.Triangle : triangelFile;
                    vehicleToDecrease.AttachedFile2 = pathAttachedFile2 == string.Empty ? vehicleToDecrease.AttachedFile2 : pathAttachedFile2;
                    vehicleToDecrease.LicenseOnlyId = licenOnlys == null ? vehicleToDecrease.LicenseOnlyId : licenOnlys.LicenseOnlyId;

                    _context.Vehicles.Add(vehicleToDecrease);
                    decreaseVehicleIdList.Add(item.VehicleID); //no need at the moment
                }
                #endregion

                await _context.SaveChangesAsync();
                return (TransactionIdN, ChalenNumberN, decreaseCarVMList.LicenseNumberLong, DateTime.Now);
                //return (TransactionIdN, ChalenNumberN, decreaseCarVMList.LicenseNumberLong, decreaseVehicleIdList);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return (string.Empty, string.Empty, string.Empty, DateTime.Now);
                //return (string.Empty, string.Empty, string.Empty, new List<int>());
            }
        }

        //string, string, string
        public async Task<(string, string, string, DateTime)> AddNewCars(ExtenseCarVMList extenseCarVMList)
        {
            try
            {
                //int latestLicenseId = _context.LicenseOnlys.OrderByDescending(l => l.LicenseOnlyId).Select(l => l.LicenseOnlyId).FirstOrDefault();
                //int latestLicenseId = await _context.LicenseOnlys.OrderByDescending(x => x.LicenseOnlyId).Select(x => x.LicenseOnlyId).FirstOrDefaultAsync();
                var licenOnlys = await _context.LicenseOnlys.Where(x => x.License_Number == extenseCarVMList.LicenseNumberLong.Replace("**", "/") &&
                                                                       x.NRC_Number == extenseCarVMList.NRC_Number)
                                                           .OrderByDescending(x => x.CreatedDate)
                                                           .FirstOrDefaultAsync();

                LicenseOnly newLicenseOnly = new LicenseOnly();

                string rootPath = _iConfig.GetSection("Upload_FolderPath").Value;
                string pathAttachFile_NRC = string.Empty;
                string AttachFile_M10 = string.Empty;
                string AttachFile_RecommandDoc1 = string.Empty;
                string AttachFile_RecommandDoc2 = string.Empty;
                string AttachFile_OperatorLicense = string.Empty;
                string AttachFile_Part1 = string.Empty;


                //create for license only folder
                string licenseOnlyFolderName = "VehicleId_" + licenOnlys.LicenseOnlyId;
                string licenseOnlyDateFolerName = Path.Combine("LicenseOnly_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), licenseOnlyFolderName);
                string licenseOnlySavePath = Path.Combine(rootPath, licenseOnlyDateFolerName);
                string licenseOnlyDateFolderNameR = licenseOnlyDateFolerName.Replace("\\", "/");

                try
                {
                    if (!Directory.Exists(licenseOnlySavePath))
                        Directory.CreateDirectory(licenseOnlySavePath);
                }
                catch (Exception e) { Console.WriteLine(extenseCarVMList.ToString()); }


                // Save AttachFile_NRC
                if (extenseCarVMList.AttachFile_NRC != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(extenseCarVMList.AttachFile_NRC, licenseOnlySavePath + "\\AttachFile_NRC.pdf");
                    if (oky)
                        pathAttachFile_NRC = licenseOnlyDateFolderNameR + "/AttachFile_NRC.pdf";
                }


                // Save AttachFile_M10
                if (extenseCarVMList.AttachFile_M10 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(extenseCarVMList.AttachFile_M10, licenseOnlySavePath + "\\AttachFile_M10.pdf");
                    if (oky)
                        AttachFile_M10 = licenseOnlyDateFolderNameR + "/AttachFile_M10.pdf";
                }


                // Save AttachFile_RecommandDoc1
                if (extenseCarVMList.AttachFile_RecommandDoc1 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(extenseCarVMList.AttachFile_RecommandDoc1, licenseOnlySavePath + "\\AttachFile_RecommandDoc1.pdf");
                    if (oky)
                        AttachFile_RecommandDoc1 = licenseOnlyDateFolderNameR + "/AttachFile_RecommandDoc1.pdf";
                }


                // Save AttachFile_RecommandDoc2
                if (extenseCarVMList.AttachFile_RecommandDoc2 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(extenseCarVMList.AttachFile_RecommandDoc2, licenseOnlySavePath + "\\AttachFile_RecommandDoc2.pdf");
                    if (oky)
                        AttachFile_RecommandDoc2 = licenseOnlyDateFolderNameR + "/AttachFile_RecommandDoc2.pdf";
                }


                // Save AttachFile_OperatorLicense
                if (extenseCarVMList.AttachFile_OperatorLicense != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(extenseCarVMList.AttachFile_OperatorLicense, licenseOnlySavePath + "\\AttachFile_OperatorLicense.pdf");
                    if (oky)
                        AttachFile_OperatorLicense = licenseOnlyDateFolderNameR + "/AttachFile_OperatorLicense.pdf";
                }


                // Save AttachFile_Part1
                if (extenseCarVMList.AttachFile_Part1 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(extenseCarVMList.AttachFile_Part1, licenseOnlySavePath + "\\AttachFile_Part1.pdf");
                    if (oky)
                        AttachFile_Part1 = licenseOnlyDateFolderNameR + "/AttachFile_Part1.pdf";
                }

                string TransactionIdN = string.Empty;
                string ChalenNumberN = string.Empty;

                if (extenseCarVMList.Transaction_Id == null || extenseCarVMList.ChalenNumber == null)
                {
                    var vehicleObj = _context.Vehicles.ToList();

                    int tG = vehicleObj.OrderByDescending(x => x.Transaction_Id)
                                       .Select(x => x.Transaction_Id.Split('_').LastOrDefault())
                                       .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
                                       .FirstOrDefault(); //order by year and then other number

                    int cG = vehicleObj.OrderByDescending(x => x.ChalenNumber)
                                       .Select(x => x.ChalenNumber.Split('_').LastOrDefault())
                                       .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
                                       .FirstOrDefault();

                    TransactionIdN = new CommonMethod().GenerateT_IdandC_Id("T", ++tG, 9); //generate Id
                    ChalenNumberN = new CommonMethod().GenerateT_IdandC_Id("C", ++cG, 7); //generate I
                }
                else
                {
                    TransactionIdN = extenseCarVMList.Transaction_Id;
                    ChalenNumberN = extenseCarVMList.ChalenNumber;
                }

                licenOnlys.Transaction_Id = TransactionIdN;
                licenOnlys.AttachFile_NRC = pathAttachFile_NRC;
                licenOnlys.AttachFile_M10 = AttachFile_M10;
                licenOnlys.AttachFile_RecommandDoc1 = AttachFile_RecommandDoc1;
                licenOnlys.AttachFile_RecommandDoc2 = AttachFile_RecommandDoc2;
                licenOnlys.AttachFile_OperatorLicense = AttachFile_OperatorLicense;
                licenOnlys.AttachFile_Part1 = AttachFile_Part1;
                licenOnlys.UpdatedDate = DateTime.Now;
                _context.LicenseOnlys.Update(licenOnlys);

                //int vehicleWeightIdObj = 0;
                var vehicleObjToAdd = await _context.Vehicles.AsNoTracking()
                                                        .FirstOrDefaultAsync(x => x.LicenseNumberLong == extenseCarVMList.LicenseNumberLong &&
                                                                                  x.NRC_Number == extenseCarVMList.NRC_Number);
                foreach (var i in extenseCarVMList.ExtenseCarVMs)
                {
                    var newCar = new CreateCar();
                    newCar.VehicleNumber = i.vehicleNumber;
                    newCar.VehicleBrand = i.vehicleBrand;
                    newCar.VehicleType = i.vehicleType;
                    newCar.VehicleLocation = i.vehicleAddress;
                    newCar.VehicleOwnerName = i.vehicleOwnerName;
                    newCar.VehicleOwnerNRC = i.vehicleOwnerNRC;
                    newCar.VehicleOwnerAddress = i.ownerAddress;
                    newCar.IsDeleted = false;
                    newCar.CreatedDate = DateTime.Now;
                    newCar.CreatedBy = extenseCarVMList.CreatedBy;
                    newCar.VehicleWeight = i.vehicleWeight;

                    await _context.CreateCars.AddAsync(newCar);
                    await _context.SaveChangesAsync();

                    // Vehicle Process
                    string pathAttachedFile1 = string.Empty;
                    string pathAttachedFile2 = string.Empty;


                    if (extenseCarVMList.ExtenseCarVMs != null && vehicleObjToAdd != null)
                    {
                        //vehicleWeightIdObj = 1;

                        //create folder
                        string vehicleFolderName = "VehicleId_" + i.createCarId;
                        string vehicleDateFolerName = Path.Combine("Vehicle_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), vehicleFolderName);
                        string vehicleSavePath = Path.Combine(rootPath, vehicleDateFolerName);
                        string vehicleLateFolderNameR = vehicleDateFolerName.Replace("\\", "/");

                        try
                        {
                            if (!Directory.Exists(vehicleSavePath))
                                Directory.CreateDirectory(vehicleSavePath);
                        }
                        catch (Exception e) { Console.WriteLine(e.ToString()); }


                        // Save AttachedFile1
                        if (i.AttachedFile1 != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(i.AttachedFile1, vehicleSavePath + "\\CarAttached1.pdf");
                            if (oky)
                                pathAttachedFile1 = vehicleLateFolderNameR + "/CarAttached1.pdf";
                        }

                        // Save AttachedFile2
                        if (i.AttachedFile2 != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(i.AttachedFile2, vehicleSavePath + "\\CarAttached2.pdf");
                            if (oky)
                                pathAttachedFile2 = vehicleLateFolderNameR + "/CarAttached2.pdf";
                        }

                        #region not use
                        //var newVehicle = new Vehicle();
                        //newVehicle.VehicleNumber = i.vehicleNumber;
                        //newVehicle.Transaction_Id = TransactionIdN;
                        //newVehicle.ChalenNumber = ChalenNumberN;
                        //newVehicle.NRC_Number = extenseCarVMList.NRC_Number;
                        //newVehicle.ApplicantId = 10;
                        //newVehicle.License_Number = extenseCarVMList.License_Number;
                        //newVehicle.LicenseNumberLong = extenseCarVMList.LicenseNumberLong;
                        //newVehicle.VehicleDesiredRoute = "-";
                        //newVehicle.CertificatePrinted = false;
                        //newVehicle.Part1Printed = false;
                        //newVehicle.Part2Printed = false;
                        //newVehicle.TrianglePrinted = false;
                        //newVehicle.ApplyDate = DateTime.Now;
                        //newVehicle.ExpiryDate = i.expiredDate;
                        //newVehicle.RefTransactionId = 0;
                        //newVehicle.Status = "Pending";
                        //newVehicle.VehicleLineTitle = "-";
                        //newVehicle.CarryLogisticType = "-";
                        //newVehicle.VehicleLocation = "MDY";
                        //newVehicle.Notes = "-";
                        //newVehicle.IsCurrent = false;
                        //newVehicle.IsDeleted = false;
                        //newVehicle.FormMode = ConstantValue.AddNewCar_FM;
                        //newVehicle.Triangle = "-";
                        //newVehicle.OwnerBook = "-";
                        //newVehicle.AttachedFile1 = pathAttachedFile1;
                        //newVehicle.AttachedFile2 = pathAttachedFile2;

                        //newVehicle.LicenseTypeId = 8;
                        //newVehicle.VehicleWeightId = 5;
                        //newVehicle.LicenseOnlyId = licenOnlys.LicenseOnlyId;
                        //newVehicle.CreatedDate = DateTime.Now;
                        //newVehicle.CreatedBy = extenseCarVMList.CreatedBy;
                        #endregion
                        vehicleObjToAdd.VehicleId = ConstantValue.Zero;
                        vehicleObjToAdd.Transaction_Id = TransactionIdN;
                        vehicleObjToAdd.ChalenNumber = ChalenNumberN;
                        vehicleObjToAdd.VehicleNumber = i.vehicleNumber;
                        vehicleObjToAdd.CertificatePrinted = false;
                        vehicleObjToAdd.Part1Printed = false;
                        vehicleObjToAdd.Part2Printed = false;
                        vehicleObjToAdd.TrianglePrinted = false;
                        vehicleObjToAdd.Status = "Pending";
                        vehicleObjToAdd.IsCurrent = false;
                        vehicleObjToAdd.IsDeleted = false;
                        vehicleObjToAdd.FormMode = ConstantValue.AddNewCar_FM;
                        vehicleObjToAdd.Triangle = null;
                        vehicleObjToAdd.OwnerBook = null;
                        vehicleObjToAdd.AttachedFile1 = pathAttachedFile1;
                        vehicleObjToAdd.AttachedFile2 = pathAttachedFile2;
                        vehicleObjToAdd.LicenseOnlyId = licenOnlys.LicenseOnlyId;
                        vehicleObjToAdd.CreatedBy = extenseCarVMList.CreatedBy;
                        vehicleObjToAdd.CreatedDate = DateTime.Now;
                        vehicleObjToAdd.CreateCarId = newCar.CreateCarId;

                        _context.Vehicles.Add(vehicleObjToAdd);
                        await _context.SaveChangesAsync();
                        //newCarList.Add(newCar);
                    }
                }

                //await _context.SaveChangesAsync();

                //return new { CarLists = newCarList, LicenseOnly = newLicenseOnly, Transaction_Id = TransactionIdN, ChalenNumber=ChalenNumberN };
                return (TransactionIdN, ChalenNumberN, extenseCarVMList.LicenseNumberLong, DateTime.Now);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return (string.Empty, string.Empty, string.Empty, DateTime.Now);
            }
        }


        public async Task<object> ChangeLicenseOwnerAddress(ChangeLicenseOwnerAddressVM changeLicenseOwnerAddressVM)
        {
            try
            {
                var vehicleObj = _context.Vehicles.ToList();

                int tG = vehicleObj.OrderByDescending(x => x.Transaction_Id)
                                   .Select(x => x.Transaction_Id.Split('_').LastOrDefault())
                                   .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
                                   .FirstOrDefault(); //order by year and then other number

                int cG = vehicleObj.OrderByDescending(x => x.ChalenNumber)
                                   .Select(x => x.ChalenNumber.Split('_').LastOrDefault())
                                   .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
                                   .FirstOrDefault();

                string TransactionIdN = new CommonMethod().GenerateT_IdandC_Id("T", ++tG, 9); //generate Id
                string ChalenNumberN = new CommonMethod().GenerateT_IdandC_Id("C", ++cG, 7); //generate Id

                //int latestLicenseId = await _context.LicenseOnlys.Where(x => x.License_Number == decreaseCarVMList.LicenseNumberLong.Replace("**","/") && x.NRC_Number == decreaseCarVMList.NRC_Number)
                //                        .Select(x => x.LicenseOnlyId).FirstOrDefaultAsync();
                var licenOnlys = await _context.LicenseOnlys.Where(x => x.License_Number == changeLicenseOwnerAddressVM.LicenseNumberLong.Replace("**", "/") &&
                                                                        x.NRC_Number == changeLicenseOwnerAddressVM.NRC_Number)
                                                            .OrderByDescending(x => x.CreatedDate)
                                                            .FirstOrDefaultAsync();
                #region license update
                LicenseOnly newLicenseOnly = new LicenseOnly();
                string rootPath = _iConfig.GetSection("Upload_FolderPath").Value;
                string pathAttachFile_NRC = string.Empty;
                string AttachFile_M10 = string.Empty;
                string AttachFile_RecommandDoc1 = string.Empty;
                string AttachFile_RecommandDoc2 = string.Empty;
                string AttachFile_OperatorLicense = string.Empty;
                string AttachFile_Part1 = string.Empty;


                //create for license only folder
                string licenseOnlyFolderName = "VehicleId_" + licenOnlys.LicenseOnlyId;
                string licenseOnlyDateFolerName = Path.Combine("LicenseOnly_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), licenseOnlyFolderName);
                string licenseOnlySavePath = Path.Combine(rootPath, licenseOnlyDateFolerName);
                string licenseOnlyDateFolderNameR = licenseOnlyDateFolerName.Replace("\\", "/");

                try
                {
                    if (!Directory.Exists(licenseOnlySavePath))
                        Directory.CreateDirectory(licenseOnlySavePath);
                }
                catch (Exception e) { Console.WriteLine(changeLicenseOwnerAddressVM.ToString()); }


                // Save AttachFile_NRC
                if (changeLicenseOwnerAddressVM.AttachFile_NRC != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeLicenseOwnerAddressVM.AttachFile_NRC, licenseOnlySavePath + "\\AttachFile_NRC.pdf");
                    if (oky)
                        pathAttachFile_NRC = licenseOnlyDateFolderNameR + "/AttachFile_NRC.pdf";
                }

                // Save AttachFile_M10
                if (changeLicenseOwnerAddressVM.AttachFile_M10 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeLicenseOwnerAddressVM.AttachFile_M10, licenseOnlySavePath + "\\AttachFile_M10.pdf");
                    if (oky)
                        AttachFile_M10 = licenseOnlyDateFolderNameR + "/AttachFile_M10.pdf";
                }

                // Save AttachFile_RecommandDoc1
                if (changeLicenseOwnerAddressVM.AttachFile_RecommandDoc1 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeLicenseOwnerAddressVM.AttachFile_RecommandDoc1, licenseOnlySavePath + "\\AttachFile_RecommandDoc1.pdf");
                    if (oky)
                        AttachFile_RecommandDoc1 = licenseOnlyDateFolderNameR + "/AttachFile_RecommandDoc1.pdf";
                }

                // Save AttachFile_RecommandDoc2
                if (changeLicenseOwnerAddressVM.AttachFile_RecommandDoc2 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeLicenseOwnerAddressVM.AttachFile_RecommandDoc2, licenseOnlySavePath + "\\AttachFile_RecommandDoc2.pdf");
                    if (oky)
                        AttachFile_RecommandDoc2 = licenseOnlyDateFolderNameR + "/AttachFile_RecommandDoc2.pdf";
                }

                // Save AttachFile_OperatorLicense
                if (changeLicenseOwnerAddressVM.AttachFile_OperatorLicense != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeLicenseOwnerAddressVM.AttachFile_OperatorLicense, licenseOnlySavePath + "\\AttachFile_OperatorLicense.pdf");
                    if (oky)
                        AttachFile_OperatorLicense = licenseOnlyDateFolderNameR + "/AttachFile_OperatorLicense.pdf";
                }

                // Save AttachFile_Part1
                if (changeLicenseOwnerAddressVM.AttachFile_Part1 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeLicenseOwnerAddressVM.AttachFile_Part1, licenseOnlySavePath + "\\AttachFile_Part1.pdf");
                    if (oky)
                        AttachFile_Part1 = licenseOnlyDateFolderNameR + "/AttachFile_Part1.pdf";
                }

                if (licenOnlys != null)
                {
                    licenOnlys.Transaction_Id = TransactionIdN;
                    licenOnlys.AttachFile_NRC = pathAttachFile_NRC;
                    licenOnlys.AttachFile_M10 = AttachFile_M10;
                    licenOnlys.AttachFile_Part1 = AttachFile_Part1;
                    licenOnlys.AttachFile_RecommandDoc1 = AttachFile_RecommandDoc1;
                    licenOnlys.AttachFile_RecommandDoc2 = AttachFile_RecommandDoc2;
                    licenOnlys.AttachFile_OperatorLicense = AttachFile_OperatorLicense;
                    licenOnlys.Address = changeLicenseOwnerAddressVM.Address;
                    licenOnlys.Township_Name = changeLicenseOwnerAddressVM.Township_Name;
                    licenOnlys.FormMode = "ChangeLicenseOwnerAddress";
                    licenOnlys.UpdatedDate = DateTime.Now;
                    _context.LicenseOnlys.Update(licenOnlys);
                }
                #endregion

                List<Vehicle> newVehicle = new List<Vehicle>();
                foreach (var item in changeLicenseOwnerAddressVM.vehicleIdList)
                {
                    // Vehicle Process
                    var vehicleToChange = await _context.Vehicles.FindAsync(item);
                    if (vehicleToChange == null)
                        continue;

                    var vehicle = new Vehicle();

                    vehicle.VehicleId = ConstantValue.Zero;
                    vehicle.Transaction_Id = TransactionIdN;
                    vehicle.ChalenNumber = ChalenNumberN;
                    vehicle.NRC_Number = vehicleToChange.NRC_Number;
                    vehicle.ApplicantId = vehicleToChange.ApplicantId;
                    vehicle.License_Number = vehicleToChange.License_Number;
                    vehicle.LicenseNumberLong = vehicleToChange.LicenseNumberLong;
                    vehicle.VehicleNumber = vehicleToChange.VehicleNumber;
                    vehicle.VehicleLineTitle = vehicleToChange.VehicleLineTitle;
                    vehicle.CarryLogisticType = vehicleToChange.CarryLogisticType;
                    vehicle.VehicleLocation = vehicleToChange.VehicleLocation;
                    vehicle.VehicleDesiredRoute = vehicleToChange.VehicleDesiredRoute;
                    vehicle.Notes = vehicleToChange.Notes;
                    vehicle.Status = "Pending";
                    vehicle.CertificatePrinted = vehicleToChange.CertificatePrinted;
                    vehicle.Part1Printed = vehicleToChange.Part1Printed;
                    vehicle.Part2Printed = vehicleToChange.Part2Printed;
                    vehicle.TrianglePrinted = vehicleToChange.TrianglePrinted;
                    vehicle.ApplyDate = vehicleToChange.ApplyDate;
                    vehicle.ExpiryDate = vehicleToChange.ExpiryDate;
                    vehicle.IsCurrent = vehicleToChange.IsCurrent;
                    vehicle.IsDeleted = vehicleToChange.IsDeleted;
                    vehicle.FormMode = "Change License Owner Address";
                    vehicle.RefTransactionId = vehicleToChange.RefTransactionId;
                    vehicle.Triangle = vehicleToChange.Triangle;
                    vehicle.OwnerBook = vehicleToChange.OwnerBook;
                    vehicle.AttachedFile1 = vehicleToChange.AttachedFile1;
                    vehicle.AttachedFile2 = vehicleToChange.AttachedFile2;
                    vehicle.LicenseTypeId = vehicleToChange.LicenseTypeId;
                    vehicle.LicenseType = vehicleToChange.LicenseType;
                    vehicle.VehicleWeightId = vehicleToChange.VehicleWeightId;
                    vehicle.CreatedDate = vehicleToChange.CreatedDate;
                    vehicle.CreateCarId = vehicleToChange.CreateCarId;
                    vehicle.LicenseOnlyId = vehicleToChange.LicenseOnlyId;
                    vehicle.OperatorId = vehicleToChange.OperatorId;

                    _context.Vehicles.Add(vehicle);
                    newVehicle.Add(vehicle);
                }




                //var vehicleObjs = _context.Vehicles
                //.Where(v => changeLicenseOwnerAddressVM.vehicleIdList.Contains(v.VehicleId))
                //.ToListAsync();



                await _context.SaveChangesAsync();
                return new { CarLists = newVehicle, LicenseOnly = licenOnlys };
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return (string.Empty, string.Empty, string.Empty, new List<int>());
            }
        }


        public async Task<object> ChangeVehicleOwnerAddress(ChangeVehicleOwnerAddressVM changeVehicleOwnerAddressVM)
        {

            try
            {
                var vehicleObj = _context.Vehicles.ToList();

                int tG = vehicleObj.OrderByDescending(x => x.Transaction_Id)
                                   .Select(x => x.Transaction_Id.Split('_').LastOrDefault())
                                   .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
                                   .FirstOrDefault(); //order by year and then other number

                int cG = vehicleObj.OrderByDescending(x => x.ChalenNumber)
                                   .Select(x => x.ChalenNumber.Split('_').LastOrDefault())
                                   .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
                                   .FirstOrDefault();

                string TransactionIdN = new CommonMethod().GenerateT_IdandC_Id("T", ++tG, 9); //generate Id
                string ChalenNumberN = new CommonMethod().GenerateT_IdandC_Id("C", ++cG, 7); //generate Id

                //int latestLicenseId = await _context.LicenseOnlys.Where(x => x.License_Number == decreaseCarVMList.LicenseNumberLong.Replace("**","/") && x.NRC_Number == decreaseCarVMList.NRC_Number)
                //                        .Select(x => x.LicenseOnlyId).FirstOrDefaultAsync();
                var licenOnlys = await _context.LicenseOnlys.Where(x => x.License_Number == changeVehicleOwnerAddressVM.LicenseNumberLong.Replace("**", "/") &&
                                                                        x.NRC_Number == changeVehicleOwnerAddressVM.NRC_Number)
                                                            .OrderByDescending(x => x.CreatedDate)
                                                            .FirstOrDefaultAsync();



                #region *** license update ***
                LicenseOnly newLicenseOnly = new LicenseOnly();
                string rootPath = _iConfig.GetSection("Upload_FolderPath").Value;
                string pathAttachFile_NRC = string.Empty;
                string AttachFile_M10 = string.Empty;
                string AttachFile_RecommandDoc1 = string.Empty;
                string AttachFile_RecommandDoc2 = string.Empty;
                string AttachFile_OperatorLicense = string.Empty;
                string AttachFile_Part1 = string.Empty;


                //create for license only folder
                string licenseOnlyFolderName = "VehicleId_" + licenOnlys.LicenseOnlyId;
                string licenseOnlyDateFolerName = Path.Combine("LicenseOnly_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), licenseOnlyFolderName);
                string licenseOnlySavePath = Path.Combine(rootPath, licenseOnlyDateFolerName);
                string licenseOnlyDateFolderNameR = licenseOnlyDateFolerName.Replace("\\", "/");

                try
                {
                    if (!Directory.Exists(licenseOnlySavePath))
                        Directory.CreateDirectory(licenseOnlySavePath);
                }
                catch (Exception e) { Console.WriteLine(changeVehicleOwnerAddressVM.ToString()); }


                // Save AttachFile_NRC
                if (changeVehicleOwnerAddressVM.AttachFile_NRC != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeVehicleOwnerAddressVM.AttachFile_NRC, licenseOnlySavePath + "\\AttachFile_NRC.pdf");
                    if (oky)
                        pathAttachFile_NRC = licenseOnlyDateFolderNameR + "/AttachFile_NRC.pdf";
                }

                // Save AttachFile_M10
                if (changeVehicleOwnerAddressVM.AttachFile_M10 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeVehicleOwnerAddressVM.AttachFile_M10, licenseOnlySavePath + "\\AttachFile_M10.pdf");
                    if (oky)
                        AttachFile_M10 = licenseOnlyDateFolderNameR + "/AttachFile_M10.pdf";
                }

                // Save AttachFile_RecommandDoc1
                if (changeVehicleOwnerAddressVM.AttachFile_RecommandDoc1 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeVehicleOwnerAddressVM.AttachFile_RecommandDoc1, licenseOnlySavePath + "\\AttachFile_RecommandDoc1.pdf");
                    if (oky)
                        AttachFile_RecommandDoc1 = licenseOnlyDateFolderNameR + "/AttachFile_RecommandDoc1.pdf";
                }

                // Save AttachFile_RecommandDoc2
                if (changeVehicleOwnerAddressVM.AttachFile_RecommandDoc2 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeVehicleOwnerAddressVM.AttachFile_RecommandDoc2, licenseOnlySavePath + "\\AttachFile_RecommandDoc2.pdf");
                    if (oky)
                        AttachFile_RecommandDoc2 = licenseOnlyDateFolderNameR + "/AttachFile_RecommandDoc2.pdf";
                }

                // Save AttachFile_OperatorLicense
                if (changeVehicleOwnerAddressVM.AttachFile_OperatorLicense != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeVehicleOwnerAddressVM.AttachFile_OperatorLicense, licenseOnlySavePath + "\\AttachFile_OperatorLicense.pdf");
                    if (oky)
                        AttachFile_OperatorLicense = licenseOnlyDateFolderNameR + "/AttachFile_OperatorLicense.pdf";
                }

                // Save AttachFile_Part1
                if (changeVehicleOwnerAddressVM.AttachFile_Part1 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeVehicleOwnerAddressVM.AttachFile_Part1, licenseOnlySavePath + "\\AttachFile_Part1.pdf");
                    if (oky)
                        AttachFile_Part1 = licenseOnlyDateFolderNameR + "/AttachFile_Part1.pdf";
                }

                if (licenOnlys != null)
                {
                    licenOnlys.Transaction_Id = TransactionIdN;
                    licenOnlys.AttachFile_NRC = pathAttachFile_NRC;
                    licenOnlys.AttachFile_M10 = AttachFile_M10;
                    licenOnlys.AttachFile_Part1 = AttachFile_Part1;
                    licenOnlys.AttachFile_RecommandDoc1 = AttachFile_RecommandDoc1;
                    licenOnlys.AttachFile_RecommandDoc2 = AttachFile_RecommandDoc2;
                    licenOnlys.AttachFile_OperatorLicense = AttachFile_OperatorLicense;
                    licenOnlys.FormMode = "ChangeVehicleOwnerAddresss";
                    licenOnlys.UpdatedDate = DateTime.Now;
                    _context.LicenseOnlys.Update(licenOnlys);
                }
                #endregion

                List<CreateCar> updateCreateCar = new List<CreateCar>();
                //foreach(var item in changeVehicleOwnerAddressVM.ChangeVOAs)
                //{
                //    var vehicleObjToUpdate = await _context.Vehicles.Include(x => x.CreateCar).SingleOrDefaultAsync(x => x.VehicleId == item.CreateCarId);


                //    if (vehicleObjToUpdate == null || vehicleObjToUpdate.CreateCar == null)
                //        continue;

                //    vehicleObjToUpdate.CreateCar.VehicleOwnerAddress = item.VehicleOwnerAddress;
                //    vehicleObjToUpdate.CreateCar.UpdatedDate = DateTime.Now;
                //    _context.CreateCars.Update(vehicleObjToUpdate.CreateCar);
                //    updateCreateCar.Add(vehicleObjToUpdate.CreateCar);
                //}


                List<Vehicle> newVehicle = new List<Vehicle>();
                foreach (var item in changeVehicleOwnerAddressVM.ChangeVOAs)
                {
                    // Vehicle Process
                    //var vehicleToChange = await _context.Vehicles.FindAsync(item.CreateCarId);
                    var vehicleToChange = await _context.Vehicles.Include(x => x.CreateCar).SingleOrDefaultAsync(x => x.VehicleId == item.CreateCarId);



                    if (vehicleToChange == null)
                        continue;

                    #region not use
                    //var vehicle = new Vehicle();

                    //vehicle.VehicleId = ConstantValue.Zero;
                    //vehicle.Transaction_Id = TransactionIdN;
                    //vehicle.ChalenNumber = ChalenNumberN;
                    //vehicle.NRC_Number = vehicleToChange.NRC_Number;
                    //vehicle.ApplicantId = vehicleToChange.ApplicantId;
                    //vehicle.License_Number = vehicleToChange.License_Number;
                    //vehicle.LicenseNumberLong = vehicleToChange.LicenseNumberLong;
                    //vehicle.VehicleNumber = vehicleToChange.VehicleNumber;
                    //vehicle.VehicleLineTitle = vehicleToChange.VehicleLineTitle;
                    //vehicle.CarryLogisticType = vehicleToChange.CarryLogisticType;
                    //vehicle.VehicleLocation = vehicleToChange.VehicleLocation;
                    //vehicle.VehicleDesiredRoute = vehicleToChange.VehicleDesiredRoute;
                    //vehicle.Notes = vehicleToChange.Notes;
                    //vehicle.Status = "Pending";
                    //vehicle.CertificatePrinted = false;
                    //vehicle.Part1Printed = false;
                    //vehicle.Part2Printed = false;
                    //vehicle.TrianglePrinted = false;
                    //vehicle.ApplyDate = DateTime.Now;
                    //vehicle.ExpiryDate = vehicleToChange.ExpiryDate;
                    //vehicle.IsCurrent = vehicleToChange.IsCurrent;
                    //vehicle.IsDeleted = vehicleToChange.IsDeleted;
                    //vehicle.FormMode = "ChangeVehicleOwnerAddress";
                    //vehicle.RefTransactionId = vehicleToChange.RefTransactionId;
                    //vehicle.Triangle = null;
                    //vehicle.OwnerBook = vehicleToChange.OwnerBook;
                    //vehicle.AttachedFile1 = vehicleToChange.AttachedFile1;
                    //vehicle.AttachedFile2 = vehicleToChange.AttachedFile2;
                    //vehicle.LicenseTypeId = vehicleToChange.LicenseTypeId;
                    //vehicle.LicenseType = vehicleToChange.LicenseType;
                    //vehicle.VehicleWeightId = vehicleToChange.VehicleWeightId;
                    //vehicle.CreatedDate = DateTime.Now;
                    //vehicle.CreateCarId = item.CreateCarId;
                    //vehicle.LicenseOnlyId = vehicleToChange.LicenseOnlyId;
                    //vehicle.OperatorId = null;
                    #endregion

                    vehicleToChange.VehicleId = ConstantValue.Zero;
                    vehicleToChange.Transaction_Id = TransactionIdN;
                    vehicleToChange.ChalenNumber = ChalenNumberN;
                    vehicleToChange.FormMode = ConstantValue.ChangeVOwnerAddress;
                    vehicleToChange.Status = ConstantValue.Status_Pending;
                    vehicleToChange.CreatedDate = DateTime.Now;
                    _context.Vehicles.Add(vehicleToChange);

                    vehicleToChange.CreateCar.VehicleOwnerAddress = item.VehicleOwnerAddress;
                    vehicleToChange.CreateCar.UpdatedDate = DateTime.Now;
                    _context.CreateCars.Update(vehicleToChange.CreateCar);
                    //newVehicle.Add(vehicle);
                }



                await _context.SaveChangesAsync();
                return new { VehicleLists = newVehicle, LicenseOnly = licenOnlys, UpdateCreateCarLists = updateCreateCar };
                /*return  licenOnlys ;*/
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return (string.Empty, string.Empty, string.Empty, new List<int>());
            }
        }


        #region not use VehicleOwnerNameChange
        //public async Task<object> VehicleOwnerChangeName(ChangeVehicleOwnerAddressVM changeVehicleOwnerAddressVM)
        //{

        //    try
        //    {
        //        var vehicleObj = _context.Vehicles.ToList();

        //        int tG = vehicleObj.OrderByDescending(x => x.Transaction_Id)
        //                           .Select(x => x.Transaction_Id.Split('_').LastOrDefault())
        //                           .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
        //                           .FirstOrDefault(); //order by year and then other number

        //        int cG = vehicleObj.OrderByDescending(x => x.ChalenNumber)
        //                           .Select(x => x.ChalenNumber.Split('_').LastOrDefault())
        //                           .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
        //                           .FirstOrDefault();

        //        string TransactionIdN = new CommonMethod().GenerateT_IdandC_Id("T", ++tG, 9); //generate Id
        //        string ChalenNumberN = new CommonMethod().GenerateT_IdandC_Id("C", ++cG, 7); //generate Id

        //        //int latestLicenseId = await _context.LicenseOnlys.Where(x => x.License_Number == decreaseCarVMList.LicenseNumberLong.Replace("**","/") && x.NRC_Number == decreaseCarVMList.NRC_Number)
        //        //                        .Select(x => x.LicenseOnlyId).FirstOrDefaultAsync();
        //        var licenOnlys = await _context.LicenseOnlys.Where(x => x.License_Number == changeVehicleOwnerAddressVM.LicenseNumberLong.Replace("**", "/") &&
        //                                                                x.NRC_Number == changeVehicleOwnerAddressVM.NRC_Number)
        //                                                    .OrderByDescending(x => x.CreatedDate)
        //                                                    .FirstOrDefaultAsync();



        //        #region license update
        //        LicenseOnly newLicenseOnly = new LicenseOnly();
        //        string rootPath = _iConfig.GetSection("Upload_FolderPath").Value;
        //        string pathAttachFile_NRC = string.Empty;
        //        string AttachFile_M10 = string.Empty;
        //        string AttachFile_RecommandDoc1 = string.Empty;
        //        string AttachFile_RecommandDoc2 = string.Empty;
        //        string AttachFile_OperatorLicense = string.Empty;
        //        string AttachFile_Part1 = string.Empty;


        //        //create for license only folder
        //        string licenseOnlyFolderName = "VehicleId_" + licenOnlys.LicenseOnlyId;
        //        string licenseOnlyDateFolerName = Path.Combine("LicenseOnly_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), licenseOnlyFolderName);
        //        string licenseOnlySavePath = Path.Combine(rootPath, licenseOnlyDateFolerName);
        //        string licenseOnlyDateFolderNameR = licenseOnlyDateFolerName.Replace("\\", "/");

        //        try
        //        {
        //            if (!Directory.Exists(licenseOnlySavePath))
        //                Directory.CreateDirectory(licenseOnlySavePath);
        //        }
        //        catch (Exception e) { Console.WriteLine(changeVehicleOwnerAddressVM.ToString()); }


        //        // Save AttachFile_NRC
        //        if (changeVehicleOwnerAddressVM.AttachFile_NRC != null)
        //        {
        //            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeVehicleOwnerAddressVM.AttachFile_NRC, licenseOnlySavePath + "\\AttachFile_NRC.pdf");
        //            if (oky)
        //                pathAttachFile_NRC = licenseOnlyDateFolderNameR + "/AttachFile_NRC.pdf";
        //        }

        //        // Save AttachFile_M10
        //        if (changeVehicleOwnerAddressVM.AttachFile_M10 != null)
        //        {
        //            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeVehicleOwnerAddressVM.AttachFile_M10, licenseOnlySavePath + "\\AttachFile_M10.pdf");
        //            if (oky)
        //                AttachFile_M10 = licenseOnlyDateFolderNameR + "/AttachFile_M10.pdf";
        //        }

        //        // Save AttachFile_RecommandDoc1
        //        if (changeVehicleOwnerAddressVM.AttachFile_RecommandDoc1 != null)
        //        {
        //            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeVehicleOwnerAddressVM.AttachFile_RecommandDoc1, licenseOnlySavePath + "\\AttachFile_RecommandDoc1.pdf");
        //            if (oky)
        //                AttachFile_RecommandDoc1 = licenseOnlyDateFolderNameR + "/AttachFile_RecommandDoc1.pdf";
        //        }

        //        // Save AttachFile_RecommandDoc2
        //        if (changeVehicleOwnerAddressVM.AttachFile_RecommandDoc2 != null)
        //        {
        //            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeVehicleOwnerAddressVM.AttachFile_RecommandDoc2, licenseOnlySavePath + "\\AttachFile_RecommandDoc2.pdf");
        //            if (oky)
        //                AttachFile_RecommandDoc2 = licenseOnlyDateFolderNameR + "/AttachFile_RecommandDoc2.pdf";
        //        }

        //        // Save AttachFile_OperatorLicense
        //        if (changeVehicleOwnerAddressVM.AttachFile_OperatorLicense != null)
        //        {
        //            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeVehicleOwnerAddressVM.AttachFile_OperatorLicense, licenseOnlySavePath + "\\AttachFile_OperatorLicense.pdf");
        //            if (oky)
        //                AttachFile_OperatorLicense = licenseOnlyDateFolderNameR + "/AttachFile_OperatorLicense.pdf";
        //        }

        //        // Save AttachFile_Part1
        //        if (changeVehicleOwnerAddressVM.AttachFile_Part1 != null)
        //        {
        //            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(changeVehicleOwnerAddressVM.AttachFile_Part1, licenseOnlySavePath + "\\AttachFile_Part1.pdf");
        //            if (oky)
        //                AttachFile_Part1 = licenseOnlyDateFolderNameR + "/AttachFile_Part1.pdf";
        //        }

        //        if (licenOnlys != null)
        //        {
        //            licenOnlys.Transaction_Id = TransactionIdN;
        //            licenOnlys.AttachFile_NRC = pathAttachFile_NRC;
        //            licenOnlys.AttachFile_M10 = AttachFile_M10;
        //            licenOnlys.AttachFile_Part1 = AttachFile_Part1;
        //            licenOnlys.AttachFile_RecommandDoc1 = AttachFile_RecommandDoc1;
        //            licenOnlys.AttachFile_RecommandDoc2 = AttachFile_RecommandDoc2;
        //            licenOnlys.AttachFile_OperatorLicense = AttachFile_OperatorLicense;
        //            licenOnlys.FormMode = "ChangeVehicleOwnerAddresss";
        //            licenOnlys.UpdatedDate = DateTime.Now;
        //            _context.LicenseOnlys.Update(licenOnlys);
        //        }
        //        #endregion


        //        List<Vehicle> newVehicle = new List<Vehicle>();
        //        foreach (var item in changeVehicleOwnerAddressVM.ChangeVOAs)
        //        {
        //            // Vehicle Process
        //            //var vehicleToChange = await _context.Vehicles.FindAsync(item.CreateCarId);
        //            var vehicleToChange = await _context.Vehicles.Include(x => x.CreateCar).SingleOrDefaultAsync(x => x.VehicleId == item.CreateCarId);



        //            if (vehicleToChange == null)
        //                continue;


        //            vehicleToChange.VehicleId = ConstantValue.Zero;
        //            vehicleToChange.Transaction_Id = TransactionIdN;
        //            vehicleToChange.ChalenNumber = ChalenNumberN;
        //            vehicleToChange.FormMode = "ChangeVehicleOwnerName";
        //            vehicleToChange.Status = ConstantValue.Status_Pending;
        //            vehicleToChange.CreatedDate = DateTime.Now;
        //            _context.Vehicles.Add(vehicleToChange);

        //            vehicleToChange.CreateCar.VehicleOwnerAddress = item.VehicleOwnerAddress;
        //            vehicleToChange.CreateCar.VehicleOwnerName = item.VehicleOwnerName;
        //            vehicleToChange.CreateCar.VehicleOwnerNRC = item.VehicleOwnerNRC;
        //            vehicleToChange.CreateCar.UpdatedDate = DateTime.Now;
        //            _context.CreateCars.Update(vehicleToChange.CreateCar);
        //            newVehicle.Add(vehicleToChange);
        //        }



        //        #region *** for Vehicle Type change ***
        //        if(dto.FormMode == ConstantValue.ChangeVType && dto.ChangeVehicleType != null)
        //        {
        //            foreach (var item in dto.ChangeVehicleType)
        //            {
        //                var vehicleObjN = await _context.Vehicles.Include(x => x.CreateCar)
        //                                                         .SingleOrDefaultAsync(x => x.VehicleId == item.VehicleId);
        //                if (vehicleObjN == null)
        //                    continue;

        //                #region *** Vehicle File Save ***
        //                //folder variable
        //                string vehicleFolderName = string.Empty;
        //                string vehicleDateFolerName = string.Empty;
        //                string vehicleSavePath = string.Empty;
        //                string vehicleLateFolderNameR = string.Empty;

        //                //file path variable
        //                string ownerBookFile = string.Empty;
        //                string triangelFile = string.Empty;
        //                string pathAttachedFile1 = string.Empty;
        //                string pathAttachedFile2 = string.Empty;

        //                if (item.NewTriangleFiles != null || item.NewOwnerBookFiles != null || item.NewAttachedFiles1 != null || item.NewAttachedFiles2 != null)
        //                {
        //                    //create folder
        //                    vehicleFolderName = "VehicleId_" + item.VehicleId;
        //                    vehicleDateFolerName = Path.Combine("Vehicle_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), vehicleFolderName);
        //                    vehicleSavePath = Path.Combine(rootPath, vehicleDateFolerName);
        //                    vehicleLateFolderNameR = vehicleDateFolerName.Replace("\\", "/");

        //                    try
        //                    {
        //                        if (!Directory.Exists(vehicleSavePath))
        //                            Directory.CreateDirectory(vehicleSavePath);
        //                    }
        //                    catch (Exception e) { Console.WriteLine(e.ToString()); }
        //                }

        //                // Save OwnerBook
        //                if (item.NewTriangleFiles != null)
        //                {
        //                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewTriangleFiles, vehicleSavePath + "\\Triangle.pdf");
        //                    if (oky)
        //                        triangelFile = vehicleLateFolderNameR + "/Triangle.pdf";
        //                }

        //                // Save Triangle 
        //                if (item.NewOwnerBookFiles != null)
        //                {
        //                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewOwnerBookFiles, vehicleSavePath + "\\Owner.pdf");
        //                    if (oky)
        //                        ownerBookFile = vehicleLateFolderNameR + "/Owner.pdf";
        //                }

        //                // Save AttachedFile2
        //                if (item.NewAttachedFiles1 != null)
        //                {
        //                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewAttachedFiles1, vehicleSavePath + "\\AttachedFile1.pdf");
        //                    if (oky)
        //                        pathAttachedFile1 = vehicleLateFolderNameR + "/AttachedFile1.pdf";
        //                }

        //                if (item.NewAttachedFiles2 != null)
        //                {
        //                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewAttachedFiles2, vehicleSavePath + "\\AttachedFile2.pdf");
        //                    if (oky)
        //                        pathAttachedFile2 = vehicleLateFolderNameR + "/AttachedFile2.pdf";
        //                }
        //                #endregion

        //                //for vehicle table
        //                vehicleObjN.VehicleId = ConstantValue.Zero;
        //                vehicleObjN.Transaction_Id = TransactionIdN;
        //                vehicleObjN.ChalenNumber = ChalenNumberN;
        //                vehicleObjN.Status = ConstantValue.Status_Pending;
        //                vehicleObjN.CertificatePrinted = false;
        //                vehicleObjN.Part1Printed = false;
        //                vehicleObjN.Part2Printed = false;
        //                vehicleObjN.TrianglePrinted = false;
        //                vehicleObjN.IsCurrent = false;
        //                vehicleObjN.IsDeleted = false;
        //                vehicleObjN.FormMode = dto.FormMode;
        //                vehicleObjN.CreatedDate = DateTime.Now;
        //                vehicleObjN.LicenseOnlyId = licenOnlys.LicenseOnlyId;

        //                await _context.Vehicles.AddAsync(vehicleObjN);

        //                //for create car table
        //                vehicleObjN.CreateCar.VehicleType = item.VehicleType;
        //                vehicleObjN.CreateCar.VehicleBrand = item.VehicleBrand;
        //                vehicleObjN.CreateCar.VehicleWeight = item.VehicleWeight;
        //                vehicleObjN.CreateCar.UpdatedDate = DateTime.Now;
        //                _context.CreateCars.Update(vehicleObjN.CreateCar);
        //            }
        //        }
        //        #endregion
        //        await _context.SaveChangesAsync();
        //        return new { VehicleLists = newVehicle, LicenseOnly = licenOnlys };
        //        /*return  licenOnlys ;*/
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.ToString());
        //        return (string.Empty, string.Empty, string.Empty, new List<int>());
        //    }



        //}
        #endregion
        #region Commented for to make same T&C from BE
        //public async Task<(string, string, string, DateTime)> CommonChangesProcess(CommonChangesVM dto)
        //{

        //    var licenOnlys = await _context.LicenseOnlys.Where(x => x.License_Number == dto.LicenseNumberLong.Replace("**", "/") &&
        //                                                                x.NRC_Number == dto.NRC_Number)
        //                                                    .OrderByDescending(x => x.CreatedDate)
        //                                                    .FirstOrDefaultAsync();

        //    if (licenOnlys != null)
        //    {
        //        #region *** generate new Transaction and Chalen ID ***
        //        string TransactionIdN = string.Empty;
        //        string ChalenNumberN = string.Empty;

        //        if (dto.Transaction_Id == null || dto.ChalenNumber == null)
        //        {
        //            var vehicleObj = _context.Vehicles.ToList();

        //            int tG = vehicleObj.OrderByDescending(x => x.Transaction_Id)
        //                               .Select(x => x.Transaction_Id.Split('_').LastOrDefault())
        //                               .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
        //                               .FirstOrDefault(); //order by year and then other number

        //            int cG = vehicleObj.OrderByDescending(x => x.ChalenNumber)
        //                               .Select(x => x.ChalenNumber.Split('_').LastOrDefault())
        //                               .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
        //                               .FirstOrDefault();

        //            TransactionIdN = new CommonMethod().GenerateT_IdandC_Id("T", ++tG, 9); //generate Id
        //            ChalenNumberN = new CommonMethod().GenerateT_IdandC_Id("C", ++cG, 7); //generate I
        //        }
        //        else
        //        {
        //            TransactionIdN = dto.Transaction_Id;
        //            ChalenNumberN = dto.ChalenNumber;
        //        }

        //        #endregion

        //        #region *** create for license only folder ***

        //        string rootPath = _iConfig.GetSection("Upload_FolderPath").Value;
        //        string licenseOnlyFolderName = "VehicleId_" + licenOnlys.LicenseOnlyId;
        //        string licenseOnlyDateFolerName = Path.Combine("LicenseOnly_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), licenseOnlyFolderName);
        //        string licenseOnlySavePath = Path.Combine(rootPath, licenseOnlyDateFolerName);
        //        string licenseOnlyDateFolderNameR = licenseOnlyDateFolerName.Replace("\\", "/");

        //        try
        //        {
        //            if (!Directory.Exists(licenseOnlySavePath))
        //                Directory.CreateDirectory(licenseOnlySavePath);
        //        }
        //        catch (Exception e) { Console.WriteLine(e.ToString()); }
        //        #endregion

        //        #region *** Save License Attached Files ***

        //        string pathAttachFile_NRC = string.Empty;
        //        string AttachFile_M10 = string.Empty;
        //        string AttachFile_RecommandDoc1 = string.Empty;
        //        string AttachFile_RecommandDoc2 = string.Empty;
        //        string AttachFile_OperatorLicense = string.Empty;
        //        string AttachFile_Part1 = string.Empty;

        //        // Save AttachFile_NRC
        //        if (dto.LicenseAttachedFiles.AttachFile_NRC != null)
        //        {
        //            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.LicenseAttachedFiles.AttachFile_NRC, licenseOnlySavePath + "\\AttachFile_NRC.pdf");
        //            if (oky)
        //                pathAttachFile_NRC = licenseOnlyDateFolderNameR + "/AttachFile_NRC.pdf";
        //        }

        //        // Save AttachFile_M10
        //        if (dto.LicenseAttachedFiles.AttachFile_M10 != null)
        //        {
        //            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.LicenseAttachedFiles.AttachFile_M10, licenseOnlySavePath + "\\AttachFile_M10.pdf");
        //            if (oky)
        //                AttachFile_M10 = licenseOnlyDateFolderNameR + "/AttachFile_M10.pdf";
        //        }

        //        // Save AttachFile_RecommandDoc1
        //        if (dto.LicenseAttachedFiles.AttachFile_RecommandDoc1 != null)
        //        {
        //            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.LicenseAttachedFiles.AttachFile_RecommandDoc1, licenseOnlySavePath + "\\AttachFile_RecommandDoc1.pdf");
        //            if (oky)
        //                AttachFile_RecommandDoc1 = licenseOnlyDateFolderNameR + "/AttachFile_RecommandDoc1.pdf";
        //        }

        //        // Save AttachFile_RecommandDoc2
        //        if (dto.LicenseAttachedFiles.AttachFile_RecommandDoc2 != null)
        //        {
        //            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.LicenseAttachedFiles.AttachFile_RecommandDoc2, licenseOnlySavePath + "\\AttachFile_RecommandDoc2.pdf");
        //            if (oky)
        //                AttachFile_RecommandDoc2 = licenseOnlyDateFolderNameR + "/AttachFile_RecommandDoc2.pdf";
        //        }

        //        // Save AttachFile_OperatorLicense
        //        if (dto.LicenseAttachedFiles.AttachFile_OperatorLicense != null)
        //        {
        //            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.LicenseAttachedFiles.AttachFile_OperatorLicense, licenseOnlySavePath + "\\AttachFile_OperatorLicense.pdf");
        //            if (oky)
        //                AttachFile_OperatorLicense = licenseOnlyDateFolderNameR + "/AttachFile_OperatorLicense.pdf";
        //        }

        //        // Save AttachFile_Part1
        //        if (dto.LicenseAttachedFiles.AttachFile_Part1 != null)
        //        {
        //            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.LicenseAttachedFiles.AttachFile_Part1, licenseOnlySavePath + "\\AttachFile_Part1.pdf");
        //            if (oky)
        //                AttachFile_Part1 = licenseOnlyDateFolderNameR + "/AttachFile_Part1.pdf";
        //        }
        //        #endregion

        //        #region *** LicenseOnly Process ***
        //        licenOnlys.Transaction_Id = TransactionIdN;
        //        licenOnlys.AttachFile_NRC = pathAttachFile_NRC;
        //        licenOnlys.AttachFile_M10 = AttachFile_M10;
        //        licenOnlys.AttachFile_Part1 = AttachFile_Part1;
        //        licenOnlys.AttachFile_RecommandDoc1 = AttachFile_RecommandDoc1;
        //        licenOnlys.AttachFile_RecommandDoc2 = AttachFile_RecommandDoc2;
        //        licenOnlys.AttachFile_OperatorLicense = AttachFile_OperatorLicense;
        //        licenOnlys.UpdatedDate = DateTime.Now;
        //        licenOnlys.FormMode = dto.FormMode;

        //        if (dto.FormMode == ConstantValue.ChangeLOwnerAddress && dto.ChangeLicenseAddress != null) //for license owner address change
        //        {
        //            licenOnlys.Address = dto.ChangeLicenseAddress.Address;
        //            licenOnlys.Township_Name = dto.ChangeLicenseAddress.Township_Name;
        //        }
        //        _context.LicenseOnlys.Update(licenOnlys);
        //        #endregion

        //        #region *** for License Address change ***
        //        if (dto.FormMode == ConstantValue.ChangeLOwnerAddress && dto.ChangeLicenseAddress != null)
        //        {
        //            foreach (var item in dto.ChangeLicenseAddress.vehicleIdList)
        //            {
        //                //var vehicleObjN = dto.FormMode == ConstantValue.ChangeLOwnerAddress? 
        //                //                                await _context.Vehicles.FindAsync(item) : 
        //                //                                await _context.Vehicles.Include(x => x.CreateCar).FirstAsync(12);

        //                var vehicleObjN = await _context.Vehicles.FindAsync(item);

        //                if (vehicleObjN == null)
        //                    continue;

        //                vehicleObjN.VehicleId = ConstantValue.Zero;
        //                vehicleObjN.Transaction_Id = TransactionIdN;
        //                vehicleObjN.ChalenNumber = ChalenNumberN;
        //                vehicleObjN.Status = ConstantValue.Status_Pending;
        //                vehicleObjN.CertificatePrinted = false;
        //                vehicleObjN.Part1Printed = false;
        //                vehicleObjN.Part2Printed = false;
        //                vehicleObjN.TrianglePrinted = false;
        //                vehicleObjN.IsCurrent = false;
        //                vehicleObjN.IsDeleted = false;
        //                vehicleObjN.FormMode = dto.FormMode;
        //                vehicleObjN.CreatedDate = DateTime.Now;
        //                vehicleObjN.LicenseOnlyId = licenOnlys.LicenseOnlyId;

        //                _context.Vehicles.Add(vehicleObjN);
        //            }
        //        }
        //        #endregion

        //        #region *** for Vehicle Address change ***
        //        if (dto.FormMode == ConstantValue.ChangeVOwnerAddress && dto.ChangeVehicleAddress != null)
        //        {
        //            foreach (var item in dto.ChangeVehicleAddress)
        //            {
        //                var vehicleObjN = await _context.Vehicles.Include(x => x.CreateCar)
        //                                                         .SingleOrDefaultAsync(x => x.VehicleId == item.CreateCarId);
        //                //var gCreateCar_id = await _context.CreateCars.OrderByDescending(x =>  x.CreateCarId).
        //                if (vehicleObjN == null)
        //                    continue;

        //                //var newCar = new CreateCar
        //                //{
        //                //    VehicleOwnerAddress = item.VehicleOwnerAddress,
        //                //    VehicleNumber = vehicleObjN.CreateCar.VehicleNumber,
        //                //    VehicleBrand = vehicleObjN.CreateCar.VehicleBrand,
        //                //    VehicleType = vehicleObjN.CreateCar.VehicleType,
        //                //    CreatedDate = DateTime.Now
        //                //};

        //                //for create car table
        //                //vehicleObjN.CreateCarId = ConstantValue.Zero; //when add new
        //                //vehicleObjN.CreateCar.VehicleOwnerAddress = item.VehicleOwnerAddress;
        //                //vehicleObjN.CreateCar.CreatedDate = DateTime.Now; //when add new
        //                //vehicleObjN.CreateCar.UpdatedDate = DateTime.Now;
        //                //var crObj = _context.CreateCars.Add(newCar);
        //                //await _context.CreateCars.AddAsync(vehicleObjN.CreateCar);
        //                //_context.CreateCars.Update(vehicleObjN.CreateCar);
        //                //await _context.SaveChangesAsync();

        //                //for vehicle table
        //                vehicleObjN.VehicleId = ConstantValue.Zero;
        //                vehicleObjN.Transaction_Id = TransactionIdN;
        //                vehicleObjN.ChalenNumber = ChalenNumberN;
        //                vehicleObjN.Status = ConstantValue.Status_Pending;
        //                vehicleObjN.CertificatePrinted = false;
        //                vehicleObjN.Part1Printed = false;
        //                vehicleObjN.Part2Printed = false;
        //                vehicleObjN.TrianglePrinted = false;
        //                vehicleObjN.IsCurrent = false;
        //                vehicleObjN.IsDeleted = false;
        //                vehicleObjN.FormMode = dto.FormMode;
        //                vehicleObjN.CreatedDate = DateTime.Now;
        //                vehicleObjN.LicenseOnlyId = licenOnlys.LicenseOnlyId;

        //                await _context.Vehicles.AddAsync(vehicleObjN);

        //                //for create car table
        //                vehicleObjN.CreateCar.VehicleOwnerAddress = item.VehicleOwnerAddress;
        //                vehicleObjN.CreateCar.UpdatedDate = DateTime.Now;
        //                _context.CreateCars.Update(vehicleObjN.CreateCar);

        //            }
        //        }
        //        #endregion

        //        #region *** for Vehicle Type change ***
        //        if (dto.FormMode == ConstantValue.ChangeVType && dto.ChangeVehicleType != null)
        //        {
        //            foreach (var item in dto.ChangeVehicleType)
        //            {
        //                var vehicleObjN = await _context.Vehicles.Include(x => x.CreateCar)
        //                                                         .SingleOrDefaultAsync(x => x.VehicleId == item.VehicleId);
        //                if (vehicleObjN == null)
        //                    continue;

        //                #region *** Vehicle File Save ***
        //                //folder variable
        //                string vehicleFolderName = string.Empty;
        //                string vehicleDateFolerName = string.Empty;
        //                string vehicleSavePath = string.Empty;
        //                string vehicleLateFolderNameR = string.Empty;

        //                //file path variable
        //                string ownerBookFile = string.Empty;
        //                string triangelFile = string.Empty;
        //                string pathAttachedFile1 = string.Empty;
        //                string pathAttachedFile2 = string.Empty;

        //                if (item.NewTriangleFiles != null || item.NewOwnerBookFiles != null || item.NewAttachedFiles1 != null || item.NewAttachedFiles2 != null)
        //                {
        //                    //create folder
        //                    vehicleFolderName = "VehicleId_" + item.VehicleId;
        //                    vehicleDateFolerName = Path.Combine("Vehicle_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), vehicleFolderName);
        //                    vehicleSavePath = Path.Combine(rootPath, vehicleDateFolerName);
        //                    vehicleLateFolderNameR = vehicleDateFolerName.Replace("\\", "/");

        //                    try
        //                    {
        //                        if (!Directory.Exists(vehicleSavePath))
        //                            Directory.CreateDirectory(vehicleSavePath);
        //                    }
        //                    catch (Exception e) { Console.WriteLine(e.ToString()); }
        //                }

        //                // Save OwnerBook
        //                if (item.NewTriangleFiles != null)
        //                {
        //                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewTriangleFiles, vehicleSavePath + "\\Triangle.pdf");
        //                    if (oky)
        //                        triangelFile = vehicleLateFolderNameR + "/Triangle.pdf";
        //                }

        //                // Save Triangle 
        //                if (item.NewOwnerBookFiles != null)
        //                {
        //                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewOwnerBookFiles, vehicleSavePath + "\\Owner.pdf");
        //                    if (oky)
        //                        ownerBookFile = vehicleLateFolderNameR + "/Owner.pdf";
        //                }

        //                // Save AttachedFile2
        //                if (item.NewAttachedFiles1 != null)
        //                {
        //                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewAttachedFiles1, vehicleSavePath + "\\AttachedFile1.pdf");
        //                    if (oky)
        //                        pathAttachedFile1 = vehicleLateFolderNameR + "/AttachedFile1.pdf";
        //                }

        //                if (item.NewAttachedFiles2 != null)
        //                {
        //                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewAttachedFiles2, vehicleSavePath + "\\AttachedFile2.pdf");
        //                    if (oky)
        //                        pathAttachedFile2 = vehicleLateFolderNameR + "/AttachedFile2.pdf";
        //                }
        //                #endregion

        //                //for vehicle table
        //                vehicleObjN.VehicleId = ConstantValue.Zero;
        //                vehicleObjN.Transaction_Id = TransactionIdN;
        //                vehicleObjN.ChalenNumber = ChalenNumberN;
        //                vehicleObjN.Status = ConstantValue.Status_Pending;
        //                vehicleObjN.CertificatePrinted = false;
        //                vehicleObjN.Part1Printed = false;
        //                vehicleObjN.Part2Printed = false;
        //                vehicleObjN.TrianglePrinted = false;
        //                vehicleObjN.IsCurrent = false;
        //                vehicleObjN.IsDeleted = false;
        //                vehicleObjN.FormMode = dto.FormMode;
        //                vehicleObjN.CreatedDate = DateTime.Now;
        //                vehicleObjN.LicenseOnlyId = licenOnlys.LicenseOnlyId;
        //                await _context.Vehicles.AddAsync(vehicleObjN);

        //                //for create car table
        //                //vehicleObjN.CreateCarId = ConstantValue.Zero; //when add new
        //                vehicleObjN.CreateCar.VehicleType = item.VehicleType;
        //                vehicleObjN.CreateCar.VehicleBrand = item.VehicleBrand;
        //                vehicleObjN.CreateCar.VehicleWeight = item.VehicleWeight;
        //                vehicleObjN.CreateCar.CreatedDate = DateTime.Now; //when add new
        //                vehicleObjN.CreateCar.UpdatedDate = DateTime.Now;
        //                //_context.CreateCars.Add(vehicleObjN.CreateCar);
        //                _context.CreateCars.Update(vehicleObjN.CreateCar);
        //            }
        //        }
        //        #endregion

        //        #region *** for Vehicle Owner Name change ***
        //        if (dto.FormMode == ConstantValue.ChangeVOwnerName && dto.ChangeVehicleOwnerName != null)
        //        {
        //            foreach (var item in dto.ChangeVehicleOwnerName)
        //            {
        //                var vehicleObjN = await _context.Vehicles.Include(x => x.CreateCar)
        //                                                         .SingleOrDefaultAsync(x => x.VehicleId == item.VehicleId);
        //                if (vehicleObjN == null)
        //                    continue;

        //                //for vehicle table
        //                vehicleObjN.VehicleId = ConstantValue.Zero;
        //                vehicleObjN.Transaction_Id = TransactionIdN;
        //                vehicleObjN.ChalenNumber = ChalenNumberN;
        //                vehicleObjN.Status = ConstantValue.Status_Pending;
        //                vehicleObjN.CertificatePrinted = false;
        //                vehicleObjN.Part1Printed = false;
        //                vehicleObjN.Part2Printed = false;
        //                vehicleObjN.TrianglePrinted = false;
        //                vehicleObjN.IsCurrent = false;
        //                vehicleObjN.IsDeleted = false;
        //                vehicleObjN.FormMode = dto.FormMode;
        //                vehicleObjN.CreatedDate = DateTime.Now;
        //                vehicleObjN.LicenseOnlyId = licenOnlys.LicenseOnlyId;
        //                await _context.Vehicles.AddAsync(vehicleObjN);

        //                //for create car table
        //                vehicleObjN.CreateCar.CreateCarId = ConstantValue.Zero; // for add
        //                vehicleObjN.CreateCar.VehicleOwnerAddress = item.VehicleOwnerAddress;
        //                vehicleObjN.CreateCar.VehicleOwnerName = item.VehicleOwnerName;
        //                vehicleObjN.CreateCar.VehicleOwnerNRC = item.VehicleOwnerNRC;
        //                vehicleObjN.CreateCar.CreatedDate = DateTime.Now;
        //                vehicleObjN.CreateCar.UpdatedDate = DateTime.Now;
        //                //_context.CreateCars.Add(vehicleObjN.CreateCar);
        //                _context.CreateCars.Update(vehicleObjN.CreateCar);
        //            }
        //        }
        //        #endregion

        //        await _context.SaveChangesAsync();
        //        return (TransactionIdN, ChalenNumberN, dto.LicenseNumberLong, DateTime.Now);
        //    }

        //    return (string.Empty, string.Empty, string.Empty, DateTime.Now);
        //}

        #endregion

        public async Task<(bool, bool, string?)> CommonChangesProcess(CommonChangesVM dto)
        {
            if (dto.TakeNewRecord != null && dto.TakeNewRecord == true)
            {
                var dataToDelete = await _context.Vehicles.AsNoTracking()
                    .Where(x => x.LicenseNumberLong == dto.LicenseNumberLong &&
                                x.FormMode == dto.FormMode &&
                                (x.Status == ConstantValue.Status_Pending || x.Status == ConstantValue.Status_OperationPending) &&
                                x.CreatedDate.Date == DateTime.Now.Date)
                    .ToListAsync();
                _context.Vehicles.RemoveRange(dataToDelete);
                //await _context.SaveChangesAsync(); // if with T_id only have one fromMode and delete it cause duplicate then will change new T_id and prevent if something wrong not delete it
            }
            else
            {
                bool checkFormModeDuplicate = await _context.Vehicles.AsNoTracking()
                           .AnyAsync(x => x.LicenseNumberLong == dto.LicenseNumberLong &&
                                          x.FormMode == dto.FormMode &&
                                          //x.Status == ConstantValue.Status_Pending &&
                                          x.CreatedDate.Date == DateTime.Now.Date);

                if (checkFormModeDuplicate)
                    return (false, true, null); //duplicate formMode in one single day (not done, duplicate)
            }


            var licenOnlys = await _context.LicenseOnlys.AsNoTracking()
                    .Where(x => x.License_Number == dto.LicenseNumberLong.Replace("**", "/") &&
                                x.NRC_Number == dto.NRC_Number)
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefaultAsync();

            if (licenOnlys != null)
            {
                #region *** generate new Transaction and Chalen ID ***

                var checkTandC = await _context.Vehicles.AsNoTracking()
                        .Where(x => x.LicenseNumberLong == dto.LicenseNumberLong &&
                                    x.CreatedDate.Date == DateTime.Now.Date 
                                    //&& x.Status == ConstantValue.Status_Pending // only for after paid and wanna two transaction_id (no need at the moment)
                                    ) 
                        .Select(x => new { x.Transaction_Id, x.ChalenNumber })
                        .FirstOrDefaultAsync();

                string TransactionIdN = string.Empty;
                string ChalenNumberN = string.Empty;

                if (checkTandC == null)
                {
                    //var vehicleObj = await _context.Vehicles.AsNoTracking().ToListAsync();

                    //ရှိတမျ data အကုန်ဆွဲ ထုတ်တာထက် လက်ရှိရောက်နေတဲ့ ခုနှစ်ထက် တစ်လလောက် နောက်ဆုတ်ပီး ဆွဲထုတ်တာ ပိုပေါ့ (if there is no operation duing last month it would wrong)
                    var vehicleObj = await _context.Vehicles.AsNoTracking()
                        .Where(x => x.CreatedDate >= DateTime.Now.Date.AddMonths(-1))
                        .ToListAsync();

                    int tG = vehicleObj.OrderByDescending(x => x.Transaction_Id)
                                       .Select(x => x.Transaction_Id.Split('_').LastOrDefault())
                                       .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
                                       .FirstOrDefault(); //order by year and then other number

                    int cG = vehicleObj.OrderByDescending(x => x.ChalenNumber)
                                       .Select(x => x.ChalenNumber.Split('_').LastOrDefault())
                                       .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
                                       .FirstOrDefault();

                    TransactionIdN = new CommonMethod().GenerateT_IdandC_Id("T", ++tG, 9); //generate Id
                    ChalenNumberN = new CommonMethod().GenerateT_IdandC_Id("C", ++cG, 7); //generate I
                }
                else
                {
                    TransactionIdN = checkTandC.Transaction_Id;
                    ChalenNumberN = checkTandC.ChalenNumber;
                }
                #endregion

                #region *** create for license only folder ***

                string rootPath = _iConfig.GetSection("Upload_FolderPath").Value;
                string licenseOnlyFolderName = "VehicleId_" + licenOnlys.LicenseOnlyId;
                string licenseOnlyDateFolerName = Path.Combine("LicenseOnly_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), licenseOnlyFolderName);
                string licenseOnlySavePath = Path.Combine(rootPath, licenseOnlyDateFolerName);
                string licenseOnlyDateFolderNameR = licenseOnlyDateFolerName.Replace("\\", "/");

                try
                {
                    if (!Directory.Exists(licenseOnlySavePath))
                        Directory.CreateDirectory(licenseOnlySavePath);
                }
                catch (Exception e) { Console.WriteLine(e.ToString()); }
                #endregion

                #region *** Save License Attached Files ***

                string pathAttachFile_NRC = string.Empty;
                string pathAttachFile_M10 = string.Empty;
                string pathAttachFile_RecommandDoc1 = string.Empty;
                string pathAttachFile_RecommandDoc2 = string.Empty;
                string pathAttachFile_OperatorLicense = string.Empty;
                string pathAttachFile_Part1 = string.Empty;

                // Save AttachFile_NRC
                if (dto.LicenseAttachedFiles.AttachFile_NRC != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.LicenseAttachedFiles.AttachFile_NRC, licenseOnlySavePath + "\\AttachFile_NRC.pdf");
                    if (oky)
                        pathAttachFile_NRC = licenseOnlyDateFolderNameR + "/AttachFile_NRC.pdf";
                }

                // Save AttachFile_M10
                if (dto.LicenseAttachedFiles.AttachFile_M10 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.LicenseAttachedFiles.AttachFile_M10, licenseOnlySavePath + "\\AttachFile_M10.pdf");
                    if (oky)
                        pathAttachFile_M10 = licenseOnlyDateFolderNameR + "/AttachFile_M10.pdf";
                }

                // Save AttachFile_RecommandDoc1
                if (dto.LicenseAttachedFiles.AttachFile_RecommandDoc1 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.LicenseAttachedFiles.AttachFile_RecommandDoc1, licenseOnlySavePath + "\\AttachFile_RecommandDoc1.pdf");
                    if (oky)
                        pathAttachFile_RecommandDoc1 = licenseOnlyDateFolderNameR + "/AttachFile_RecommandDoc1.pdf";
                }

                // Save AttachFile_RecommandDoc2
                if (dto.LicenseAttachedFiles.AttachFile_RecommandDoc2 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.LicenseAttachedFiles.AttachFile_RecommandDoc2, licenseOnlySavePath + "\\AttachFile_RecommandDoc2.pdf");
                    if (oky)
                        pathAttachFile_RecommandDoc2 = licenseOnlyDateFolderNameR + "/AttachFile_RecommandDoc2.pdf";
                }

                // Save AttachFile_OperatorLicense
                if (dto.LicenseAttachedFiles.AttachFile_OperatorLicense != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.LicenseAttachedFiles.AttachFile_OperatorLicense, licenseOnlySavePath + "\\AttachFile_OperatorLicense.pdf");
                    if (oky)
                        pathAttachFile_OperatorLicense = licenseOnlyDateFolderNameR + "/AttachFile_OperatorLicense.pdf";
                }

                // Save AttachFile_Part1
                if (dto.LicenseAttachedFiles.AttachFile_Part1 != null)
                {
                    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(dto.LicenseAttachedFiles.AttachFile_Part1, licenseOnlySavePath + "\\AttachFile_Part1.pdf");
                    if (oky)
                        pathAttachFile_Part1 = licenseOnlyDateFolderNameR + "/AttachFile_Part1.pdf";
                }
                #endregion

                #region *** LicenseOnly Process ***

                licenOnlys.Transaction_Id = TransactionIdN;
                licenOnlys.AttachFile_NRC = pathAttachFile_NRC;
                licenOnlys.Temp_AttachFile_M10 = pathAttachFile_M10;
                licenOnlys.Temp_AttachFile_Part1 = pathAttachFile_Part1;
                licenOnlys.Temp_AttachFile_RecommandDoc1 = pathAttachFile_RecommandDoc1;
                licenOnlys.Temp_AttachFile_RecommandDoc2 = pathAttachFile_RecommandDoc2;
                licenOnlys.Temp_AttachFile_OperatorLicense = pathAttachFile_OperatorLicense;
                licenOnlys.UpdatedDate = DateTime.Now;
                licenOnlys.FormMode = dto.FormMode;

                if (dto.FormMode == ConstantValue.ChangeLOwnerAddress && dto.ChangeLicenseAddress != null) //for license owner address change
                {
                    licenOnlys.Temp_Address = dto.ChangeLicenseAddress.Address;
                    licenOnlys.Temp_Township_Name = dto.ChangeLicenseAddress.Township_Name;
                }
                _context.LicenseOnlys.Update(licenOnlys);
                #endregion


                #region *** for License Address change ***
                if (dto.FormMode == ConstantValue.ChangeLOwnerAddress && dto.ChangeLicenseAddress != null)
                {
                    foreach (var vehicleId in dto.ChangeLicenseAddress.vehicleIdList)
                    {
                        // each vehicleId of data can't be same so you should find for each
                        var vehicleObjN = await _context.Vehicles
                            .SingleOrDefaultAsync(x => x.VehicleId == vehicleId);

                        if (vehicleObjN == null)
                            continue;

                        vehicleObjN.VehicleId = ConstantValue.Zero;
                        vehicleObjN.Transaction_Id = TransactionIdN;
                        vehicleObjN.ChalenNumber = ChalenNumberN;
                        vehicleObjN.Status = ConstantValue.Status_OperationPending;
                        vehicleObjN.CertificatePrinted = false;
                        vehicleObjN.Part1Printed = false;
                        vehicleObjN.Part2Printed = false;
                        vehicleObjN.TrianglePrinted = false;
                        vehicleObjN.IsCurrent = false;
                        vehicleObjN.IsDeleted = false;
                        vehicleObjN.FormMode = dto.FormMode;
                        vehicleObjN.CreatedDate = DateTime.Now;
                        vehicleObjN.LicenseOnlyId = licenOnlys.LicenseOnlyId;

                        await _context.Vehicles.AddAsync(vehicleObjN);
                    }
                }
                #endregion

                #region *** for Vehicle Address change ***
                if (dto.FormMode == ConstantValue.ChangeVOwnerAddress && dto.ChangeVehicleAddress != null)
                {
                    foreach (var item in dto.ChangeVehicleAddress)
                    {
                        var vehicleObjN = await _context.Vehicles
                            .SingleOrDefaultAsync(x => x.VehicleId == item.CreateCarId);
                        
                        if (vehicleObjN == null)
                            continue;

                        //for vehicle table
                        vehicleObjN.VehicleId = ConstantValue.Zero;
                        vehicleObjN.Transaction_Id = TransactionIdN;
                        vehicleObjN.ChalenNumber = ChalenNumberN;
                        vehicleObjN.Status = ConstantValue.Status_OperationPending;
                        vehicleObjN.CertificatePrinted = false;
                        vehicleObjN.Part1Printed = false;
                        vehicleObjN.Part2Printed = false;
                        vehicleObjN.TrianglePrinted = false;
                        vehicleObjN.IsCurrent = false;
                        vehicleObjN.IsDeleted = false;
                        vehicleObjN.FormMode = dto.FormMode;
                        vehicleObjN.CreatedDate = DateTime.Now;
                        vehicleObjN.LicenseOnlyId = licenOnlys.LicenseOnlyId;

                        vehicleObjN.Temp_VehicleOwnerAddress = item.VehicleOwnerAddress; //temp
                        vehicleObjN.Temp_Township_Name = item.Township_Name; //temp

                        await _context.Vehicles.AddAsync(vehicleObjN);
                    }
                }
                #endregion

                #region *** for Vehicle Type change ***
                if (dto.FormMode == ConstantValue.ChangeVType && dto.ChangeVehicleType != null)
                {
                    foreach (var item in dto.ChangeVehicleType)
                    {
                        var vehicleObjN = await _context.Vehicles
                            .SingleOrDefaultAsync(x => x.VehicleId == item.VehicleId);

                        if (vehicleObjN == null)
                            continue;

                        #region *** Vehicle File Save ***
                        //folder variable
                        string vehicleFolderName = string.Empty;
                        string vehicleDateFolerName = string.Empty;
                        string vehicleSavePath = string.Empty;
                        string vehicleLateFolderNameR = string.Empty;

                        //file path variable
                        string pathOwnerBookFile = string.Empty;
                        string pathTriangelFile = string.Empty;
                        string pathAttachedFile1 = string.Empty;
                        string pathAttachedFile2 = string.Empty;

                        if (item.NewTriangleFiles != null || item.NewOwnerBookFiles != null || item.NewAttachedFiles1 != null || item.NewAttachedFiles2 != null)
                        {
                            //create folder
                            vehicleFolderName = "VehicleId_" + item.VehicleId;
                            vehicleDateFolerName = Path.Combine("Vehicle_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), vehicleFolderName);
                            vehicleSavePath = Path.Combine(rootPath, vehicleDateFolerName);
                            vehicleLateFolderNameR = vehicleDateFolerName.Replace("\\", "/");

                            try
                            {
                                if (!Directory.Exists(vehicleSavePath))
                                    Directory.CreateDirectory(vehicleSavePath);
                            }
                            catch (Exception e) { Console.WriteLine(e.ToString()); }
                        }

                        // Save OwnerBook
                        if (item.NewTriangleFiles != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewTriangleFiles, vehicleSavePath + "\\Triangle.pdf");
                            if (oky)
                                pathTriangelFile = vehicleLateFolderNameR + "/Triangle.pdf";
                        }

                        // Save Triangle 
                        if (item.NewOwnerBookFiles != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewOwnerBookFiles, vehicleSavePath + "\\Owner.pdf");
                            if (oky)
                                pathOwnerBookFile = vehicleLateFolderNameR + "/Owner.pdf";
                        }

                        // Save AttachedFile2
                        if (item.NewAttachedFiles1 != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewAttachedFiles1, vehicleSavePath + "\\AttachedFile1.pdf");
                            if (oky)
                                pathAttachedFile1 = vehicleLateFolderNameR + "/AttachedFile1.pdf";
                        }

                        if (item.NewAttachedFiles2 != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewAttachedFiles2, vehicleSavePath + "\\AttachedFile2.pdf");
                            if (oky)
                                pathAttachedFile2 = vehicleLateFolderNameR + "/AttachedFile2.pdf";
                        }
                        #endregion

                        //for vehicle table
                        vehicleObjN.VehicleId = ConstantValue.Zero;
                        vehicleObjN.Transaction_Id = TransactionIdN;
                        vehicleObjN.ChalenNumber = ChalenNumberN;
                        vehicleObjN.Status = ConstantValue.Status_OperationPending;
                        vehicleObjN.CertificatePrinted = false;
                        vehicleObjN.Part1Printed = false;
                        vehicleObjN.Part2Printed = false;
                        vehicleObjN.TrianglePrinted = false;
                        vehicleObjN.IsCurrent = false;
                        vehicleObjN.IsDeleted = false;
                        vehicleObjN.FormMode = dto.FormMode;
                        vehicleObjN.CreatedDate = DateTime.Now;
                        vehicleObjN.LicenseOnlyId = licenOnlys.LicenseOnlyId;

                        vehicleObjN.Temp_VehicleType = item.VehicleType;
                        vehicleObjN.Temp_VehicleBrand = item.VehicleBrand;
                        vehicleObjN.Temp_VehicleWeight = item.VehicleWeight;
                        vehicleObjN.Temp_Triangle = pathTriangelFile;
                        vehicleObjN.Temp_OwnerBook = pathOwnerBookFile;
                        vehicleObjN.Temp_AttachedFile1 = pathAttachedFile1;
                        vehicleObjN.Temp_AttachedFile2 = pathAttachedFile2;

                        await _context.Vehicles.AddAsync(vehicleObjN);
                    }
                }
                #endregion

                #region *** for Vehicle Owner Name change ***
                if (dto.FormMode == ConstantValue.ChangeVOwnerName && dto.ChangeVehicleOwnerName != null)
                {
                    foreach (var item in dto.ChangeVehicleOwnerName)
                    {
                        var vehicleObjN = await _context.Vehicles
                            .SingleOrDefaultAsync(x => x.VehicleId == item.VehicleId);

                        if (vehicleObjN == null)
                            continue;

                        //for vehicle table
                        vehicleObjN.VehicleId = ConstantValue.Zero;
                        vehicleObjN.Transaction_Id = TransactionIdN;
                        vehicleObjN.ChalenNumber = ChalenNumberN;
                        vehicleObjN.Status = ConstantValue.Status_OperationPending;
                        vehicleObjN.CertificatePrinted = false;
                        vehicleObjN.Part1Printed = false;
                        vehicleObjN.Part2Printed = false;
                        vehicleObjN.TrianglePrinted = false;
                        vehicleObjN.IsCurrent = false;
                        vehicleObjN.IsDeleted = false;
                        vehicleObjN.FormMode = dto.FormMode;
                        vehicleObjN.CreatedDate = DateTime.Now;
                        vehicleObjN.LicenseOnlyId = licenOnlys.LicenseOnlyId;

                        vehicleObjN.Temp_VehicleOwnerAddress = item.VehicleOwnerAddress;
                        vehicleObjN.Temp_VehicleOwnerName = item.VehicleOwnerName;
                        vehicleObjN.Temp_VehicleOwnerNRC = item.VehicleOwnerNRC;

                        await _context.Vehicles.AddAsync(vehicleObjN);
                    }
                }
                #endregion

                #region *** for AddNewCar ***
                if (dto.FormMode == ConstantValue.AddNewCar_FM && dto.AddNewCars != null)
                {

                    int applicantId = await _context.Vehicles.AsNoTracking()
                        .Where(x => x.LicenseNumberLong == dto.LicenseNumberLong && x.NRC_Number == dto.NRC_Number)
                        .Select(x => x.ApplicantId)
                        .FirstOrDefaultAsync();

                    int licenseType_Id = 0;

                    if (dto.LicenseNumberLong.Contains('က'))
                        licenseType_Id = 2;
                    else if (dto.LicenseNumberLong.Contains('ခ'))
                        licenseType_Id = 4;
                    else if (dto.LicenseNumberLong.Contains('ဂ'))
                        licenseType_Id = 5;
                    else if (dto.LicenseNumberLong.Contains('ဃ'))
                        licenseType_Id = 6;
                    else if (dto.LicenseNumberLong.Contains('င'))
                        licenseType_Id = 8;

                    foreach (var item in dto.AddNewCars)
                    {
                        #region *** save attached file for CreateNew ***
                        string pathOwnerBookFiles = string.Empty;
                        string pathAttachedFiles1 = string.Empty;

                        string vehicleFolderName = "VehicleId_" + item.vehicleNumber;
                        string vehicleDateFolerName = Path.Combine("Vehicle_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), vehicleFolderName);
                        string vehicleSavePath = Path.Combine(rootPath, vehicleDateFolerName);
                        string vehicleLateFolderNameR = vehicleDateFolerName.Replace("\\", "/");

                        try
                        {
                            if (!Directory.Exists(vehicleSavePath))
                                Directory.CreateDirectory(vehicleSavePath);
                        }
                        catch (Exception e) { Console.WriteLine(e.ToString()); }

                        // Save OwnerBook
                        if (item.OwnerBookFiles != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.OwnerBookFiles, vehicleSavePath + "\\OwnerBook.pdf");
                            if (oky)
                                pathOwnerBookFiles = vehicleLateFolderNameR + "/OwnerBook.pdf";
                        }

                        // Save AttachedFile1
                        if (item.AttachedFiles1 != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.AttachedFiles1, vehicleSavePath + "\\CarAttached1.pdf");
                            if (oky)
                                pathAttachedFiles1 = vehicleLateFolderNameR + "/CarAttached1.pdf";
                        }
                        #endregion

                        var newCar = new CreateCar()
                        {
                            VehicleNumber = item.vehicleNumber,
                            VehicleBrand = item.vehicleBrand,
                            VehicleType = item.vehicleType,
                            VehicleLocation = item.vehicleLocation,
                            VehicleOwnerName = item.vehicleOwnerName,
                            VehicleOwnerNRC = item.vehicleOwnerNRC,
                            VehicleOwnerAddress = item.vehicleOwnerAddress,
                            IsDeleted = false,
                            CreatedDate = DateTime.Now,
                            VehicleWeight = item.vehicleWeight,
                            Status = ConstantValue.Status_Pending
                        };

                        await _context.CreateCars.AddAsync(newCar);
                        await _context.SaveChangesAsync();

                        Vehicle newVehicle = new Vehicle
                        {
                            Transaction_Id = TransactionIdN,
                            ChalenNumber = ChalenNumberN,
                            NRC_Number = dto.NRC_Number,
                            ApplicantId = applicantId,
                            License_Number = dto.LicenseNumberLong.Substring(2, dto.LicenseNumberLong.IndexOf("(") - 3),
                            LicenseNumberLong = dto.LicenseNumberLong,
                            VehicleNumber = item.vehicleNumber,
                            VehicleLineTitle = "-",
                            CarryLogisticType = "_",
                            VehicleLocation = item.vehicleLocation,
                            VehicleDesiredRoute = "_",
                            Notes = "_",
                            Status = ConstantValue.Status_OperationPending,
                            CertificatePrinted = false,
                            Part1Printed = false,
                            Part2Printed = false,
                            TrianglePrinted = false,
                            ApplyDate = DateTime.Now,
                            ExpiryDate = DateTime.Now,
                            FormMode = dto.FormMode,
                            RefTransactionId = 0,
                            LicenseTypeId = licenseType_Id,
                            VehicleWeightId = 2, //set deault at admin side could be edit able
                            Temp_OwnerBook = pathOwnerBookFiles,
                            Temp_AttachedFile1 = pathAttachedFiles1,
                            LicenseOnlyId = licenOnlys.LicenseOnlyId,
                            CreatedDate = DateTime.Now,
                            CreateCarId = newCar.CreateCarId
                        };
                        await _context.Vehicles.AddAsync(newVehicle);
                    }
                }
                #endregion

                #region *** for DecreaseCar ***
                if (dto.FormMode == ConstantValue.DecreaseCar_FM && dto.DecreaseCars != null)
                {
                    foreach (var item in dto.DecreaseCars)
                    {
                        var vehicleObjN = await _context.Vehicles
                            .SingleOrDefaultAsync(x => x.VehicleId == item.VehicleID);

                        if (vehicleObjN == null)
                            continue;

                        //folder variable
                        string vehicleFolderName = string.Empty;
                        string vehicleDateFolerName = string.Empty;
                        string vehicleSavePath = string.Empty;
                        string vehicleLateFolderNameR = string.Empty;

                        //file path variable
                        string pathOwnerBookFile = string.Empty;
                        string pathTriangelFile = string.Empty;
                        string pathAttachedFile2 = string.Empty;

                        if (item.NewOwnerBook != null || item.NewTriangle != null || item.NewAttachedFile2 != null)
                        {
                            //create folder
                            vehicleFolderName = "VehicleId_" + item.VehicleID;
                            vehicleDateFolerName = Path.Combine("Vehicle_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), vehicleFolderName);
                            vehicleSavePath = Path.Combine(rootPath, vehicleDateFolerName);
                            vehicleLateFolderNameR = vehicleDateFolerName.Replace("\\", "/");

                            try
                            {
                                if (!Directory.Exists(vehicleSavePath))
                                    Directory.CreateDirectory(vehicleSavePath);
                            }
                            catch (Exception e) { Console.WriteLine(e.ToString()); }
                        }

                        #region ** save DecreaseCar files **
                        // Save OwnerBook
                        if (item.NewOwnerBook != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewOwnerBook, vehicleSavePath + "\\Owner.pdf");
                            if (oky)
                                pathOwnerBookFile = vehicleLateFolderNameR + "/Owner.pdf";
                        }

                        // Save Triangle 
                        if (item.NewTriangle != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewTriangle, vehicleSavePath + "\\Triangle.pdf");
                            if (oky)
                                pathTriangelFile = vehicleLateFolderNameR + "/Triangle.pdf";
                        }

                        // Save AttachedFile2
                        if (item.NewAttachedFile2 != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewAttachedFile2, vehicleSavePath + "\\AttachedFile2.pdf");
                            if (oky)
                                pathAttachedFile2 = vehicleLateFolderNameR + "/AttachedFile2.pdf";
                        }
                        #endregion

                        //for vehicle table
                        vehicleObjN.VehicleId = ConstantValue.Zero;
                        vehicleObjN.Transaction_Id = TransactionIdN;
                        vehicleObjN.ChalenNumber = ChalenNumberN;
                        vehicleObjN.Status = ConstantValue.Status_OperationPending;
                        vehicleObjN.CertificatePrinted = false;
                        vehicleObjN.Part1Printed = false;
                        vehicleObjN.Part2Printed = false;
                        vehicleObjN.TrianglePrinted = false;
                        vehicleObjN.IsCurrent = false;
                        vehicleObjN.IsDeleted = false;
                        vehicleObjN.FormMode = dto.FormMode;
                        vehicleObjN.CreatedDate = DateTime.Now;
                        vehicleObjN.LicenseOnlyId = licenOnlys.LicenseOnlyId;
                        vehicleObjN.Temp_OwnerBook = pathOwnerBookFile;
                        vehicleObjN.Temp_Triangle = pathTriangelFile;
                        vehicleObjN.Temp_AttachedFile2 = pathAttachedFile2;
                    }
                }
                #endregion

                #region *** for DecreaseCar Over 2 ton ***
                if (dto.FormMode == ConstantValue.AddNewCar_FM && dto.DecreaseCarsOver2ton != null)
                {
                    foreach (var item in dto.DecreaseCarsOver2ton)
                    {
                        var vehicleObjN = await _context.Vehicles
                            .SingleOrDefaultAsync(x => x.VehicleId == item.VehicleID);

                        if (vehicleObjN == null)
                            continue;

                        //folder variable
                        string vehicleFolderName = string.Empty;
                        string vehicleDateFolerName = string.Empty;
                        string vehicleSavePath = string.Empty;
                        string vehicleLateFolderNameR = string.Empty;

                        //file path variable
                        string pathOwnerBookFile = string.Empty;
                        string pathTriangelFile = string.Empty;
                        string pathAttachedFile2 = string.Empty;

                        if (item.NewOwnerBook != null || item.NewTriangle != null || item.NewAttachedFile2 != null)
                        {
                            //create folder
                            vehicleFolderName = "VehicleId_" + item.VehicleID;
                            vehicleDateFolerName = Path.Combine("Vehicle_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), vehicleFolderName);
                            vehicleSavePath = Path.Combine(rootPath, vehicleDateFolerName);
                            vehicleLateFolderNameR = vehicleDateFolerName.Replace("\\", "/");

                            try
                            {
                                if (!Directory.Exists(vehicleSavePath))
                                    Directory.CreateDirectory(vehicleSavePath);
                            }
                            catch (Exception e) { Console.WriteLine(e.ToString()); }
                        }

                        #region ** save DecreaseCar files **
                        // Save OwnerBook
                        if (item.NewOwnerBook != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewOwnerBook, vehicleSavePath + "\\Owner.pdf");
                            if (oky)
                                pathOwnerBookFile = vehicleLateFolderNameR + "/Owner.pdf";
                        }

                        // Save Triangle 
                        if (item.NewTriangle != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewTriangle, vehicleSavePath + "\\Triangle.pdf");
                            if (oky)
                                pathTriangelFile = vehicleLateFolderNameR + "/Triangle.pdf";
                        }

                        // Save AttachedFile2
                        if (item.NewAttachedFile2 != null)
                        {
                            bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.NewAttachedFile2, vehicleSavePath + "\\AttachedFile2.pdf");
                            if (oky)
                                pathAttachedFile2 = vehicleLateFolderNameR + "/AttachedFile2.pdf";
                        }
                        #endregion

                        //for vehicle table
                        vehicleObjN.VehicleId = ConstantValue.Zero;
                        vehicleObjN.Transaction_Id = TransactionIdN;
                        vehicleObjN.ChalenNumber = ChalenNumberN;
                        vehicleObjN.Status = ConstantValue.Status_OperationPending;
                        vehicleObjN.CertificatePrinted = false;
                        vehicleObjN.Part1Printed = false;
                        vehicleObjN.Part2Printed = false;
                        vehicleObjN.TrianglePrinted = false;
                        vehicleObjN.IsCurrent = false;
                        vehicleObjN.IsDeleted = false;
                        vehicleObjN.FormMode = dto.FormMode;
                        vehicleObjN.CreatedDate = DateTime.Now;
                        vehicleObjN.LicenseOnlyId = licenOnlys.LicenseOnlyId;
                        vehicleObjN.Temp_OwnerBook = pathOwnerBookFile;
                        vehicleObjN.Temp_Triangle = pathTriangelFile;
                        vehicleObjN.Temp_AttachedFile2 = pathAttachedFile2;

                        await _context.Vehicles.AddAsync(vehicleObjN);
                    }
                }
                #endregion

                #region *** for ExtendOperator License ***
                if(dto.FormMode == ConstantValue.EOPL_FM && dto.ExtendOperatorLicense != null)
                {
                    foreach (var item in dto.ExtendOperatorLicense)
                    {
                        var vehicleObjN = await _context.Vehicles
                            .SingleOrDefaultAsync(x => x.VehicleId == item.VehicleId);
                        if (vehicleObjN != null)
                        {
                            #region *** Save Vehicle Attached Files ***
                            ////vehicleWeightIdObj = vehicle.VehicleWeightId; //each license number long of weight are same
                            ////create folder
                            //string vehicleFolderName = "VehicleId_" + item.VehicleId;
                            //string dateFolderName = Path.Combine("Vehicle_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), vehicleFolderName);
                            //string vehicleSavePath = Path.Combine(rootPath, dateFolderName);
                            //string dateFolderNameR = dateFolderName.Replace("\\", "/");

                            //try
                            //{
                            //    if (!Directory.Exists(vehicleSavePath))
                            //        Directory.CreateDirectory(vehicleSavePath);
                            //}
                            //catch (Exception e) { Console.WriteLine(e.ToString()); }

                            //// save TriangleFiles
                            //string pathTriangleFiles = string.Empty;
                            //if (item.TriangleFiles != null)
                            //{
                            //    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.TriangleFiles, vehicleSavePath + "\\Triangle.pdf");
                            //    if (oky)
                            //        pathTriangleFiles = dateFolderNameR + "/Triangle.pdf";
                            //}

                            //// save OwnerBookFiles
                            //string pathOwnerBookFiles = string.Empty;
                            //if (item.OwnerBookFiles != null)
                            //{
                            //    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.OwnerBookFiles, vehicleSavePath + "\\OwnerBook.pdf");
                            //    if (oky)
                            //        pathOwnerBookFiles = dateFolderNameR + "/OwnerBook.pdf";
                            //}

                            //// save AttachedFiles1
                            //string pathAttachedFiles1 = string.Empty;
                            //if (item.AttachedFiles1 != null)
                            //{
                            //    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.AttachedFiles1, vehicleSavePath + "\\CarAttached1.pdf");
                            //    if (oky)
                            //        pathAttachedFiles1 = dateFolderNameR + "/CarAttached1.pdf";
                            //}

                            //// save AttachedFiles2
                            //string pathAttachedFiles2 = string.Empty;
                            //if (item.AttachedFiles2 != null)
                            //{
                            //    bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.AttachedFiles2, vehicleSavePath + "\\CarAttached2.pdf");
                            //    if (oky)
                            //        pathAttachedFiles2 = dateFolderNameR + "/CarAttached2.pdf";
                            //}
                            #endregion

                            //for vehicle table
                            vehicleObjN.VehicleId = ConstantValue.Zero;
                            vehicleObjN.Transaction_Id = TransactionIdN;
                            vehicleObjN.ChalenNumber = ChalenNumberN;
                            vehicleObjN.Status = ConstantValue.Status_OperationPending;
                            vehicleObjN.CertificatePrinted = false;
                            vehicleObjN.Part1Printed = false;
                            vehicleObjN.Part2Printed = false;
                            vehicleObjN.TrianglePrinted = false;
                            vehicleObjN.IsCurrent = false;
                            vehicleObjN.IsDeleted = false;
                            vehicleObjN.FormMode = dto.FormMode;
                            vehicleObjN.CreatedDate = DateTime.Now;
                            vehicleObjN.LicenseOnlyId = licenOnlys.LicenseOnlyId;

                            //vehicleObjN.Temp_Triangle = pathTriangleFiles;
                            //vehicleObjN.Temp_OwnerBook = pathOwnerBookFiles;
                            //vehicleObjN.Temp_AttachedFile1 = pathAttachedFiles1;
                            //vehicleObjN.Temp_AttachedFile2 = pathAttachedFiles2;

                            await _context.Vehicles.AddAsync(vehicleObjN);
                        }
                    }
                }
                #endregion

                #region *** for ExtendVehicleLicense ***
                if(dto.FormMode == ConstantValue.EVL_FM && dto.ExtendVehicleLicense != null)
                {
                    foreach (var item in dto.ExtendVehicleLicense)
                    {
                        var vehicleObjN = await _context.Vehicles
                            .SingleOrDefaultAsync(x => x.VehicleId == item.VehicleId);
                        if (vehicleObjN != null)
                        {
                            #region *** Save Vehicle Attached Files ***
                            //vehicleWeightIdObj = vehicle.VehicleWeightId; //each license number long of weight are same
                            //create folder
                            string vehicleFolderName = "VehicleId_" + item.VehicleId;
                            string dateFolderName = Path.Combine("Vehicle_AttachedFiles", DateTime.Now.ToString("yyyyMMdd"), vehicleFolderName);
                            string vehicleSavePath = Path.Combine(rootPath, dateFolderName);
                            string dateFolderNameR = dateFolderName.Replace("\\", "/");

                            try
                            {
                                if (!Directory.Exists(vehicleSavePath))
                                    Directory.CreateDirectory(vehicleSavePath);
                            }
                            catch (Exception e) { Console.WriteLine(e.ToString()); }

                            // save TriangleFiles
                            string pathTriangleFiles = string.Empty;
                            if (item.TriangleFiles != null)
                            {
                                bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.TriangleFiles, vehicleSavePath + "\\Triangle.pdf");
                                if (oky)
                                    pathTriangleFiles = dateFolderNameR + "/Triangle.pdf";
                            }

                            // save OwnerBookFiles
                            string pathOwnerBookFiles = string.Empty;
                            if (item.OwnerBookFiles != null)
                            {
                                bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.OwnerBookFiles, vehicleSavePath + "\\OwnerBook.pdf");
                                if (oky)
                                    pathOwnerBookFiles = dateFolderNameR + "/OwnerBook.pdf";
                            }

                            // save AttachedFiles1
                            string pathAttachedFiles1 = string.Empty;
                            if (item.AttachedFiles1 != null)
                            {
                                bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.AttachedFiles1, vehicleSavePath + "\\CarAttached1.pdf");
                                if (oky)
                                    pathAttachedFiles1 = dateFolderNameR + "/CarAttached1.pdf";
                            }

                            // save AttachedFiles2
                            string pathAttachedFiles2 = string.Empty;
                            if (item.AttachedFiles2 != null)
                            {
                                bool oky = await CommonMethod.AddOperatorLicenseAttachPDFAsync(item.AttachedFiles2, vehicleSavePath + "\\CarAttached2.pdf");
                                if (oky)
                                    pathAttachedFiles2 = dateFolderNameR + "/CarAttached2.pdf";
                            }
                            #endregion

                            //for vehicle table
                            vehicleObjN.VehicleId = ConstantValue.Zero;
                            vehicleObjN.Transaction_Id = TransactionIdN;
                            vehicleObjN.ChalenNumber = ChalenNumberN;
                            vehicleObjN.Status = ConstantValue.Status_OperationPending;
                            vehicleObjN.CertificatePrinted = false;
                            vehicleObjN.Part1Printed = false;
                            vehicleObjN.Part2Printed = false;
                            vehicleObjN.TrianglePrinted = false;
                            vehicleObjN.IsCurrent = false;
                            vehicleObjN.IsDeleted = false;
                            vehicleObjN.FormMode = dto.FormMode;
                            vehicleObjN.CreatedDate = DateTime.Now;
                            vehicleObjN.LicenseOnlyId = licenOnlys.LicenseOnlyId;

                            vehicleObjN.Temp_Triangle = pathTriangleFiles;
                            vehicleObjN.Temp_OwnerBook = pathOwnerBookFiles;
                            vehicleObjN.Temp_AttachedFile1 = pathAttachedFiles1;
                            vehicleObjN.Temp_AttachedFile2 = pathAttachedFiles2;
                            vehicleObjN.ExpiryDate = vehicleObjN.ExpiryDate != null? vehicleObjN.ExpiryDate.Value.AddYears(1) : DateTime.Now.AddYears(1);

                            await _context.Vehicles.AddAsync(vehicleObjN);
                        }
                    }

                }
                #endregion

                await _context.SaveChangesAsync();
                return (true, false, TransactionIdN); // (done, not duplicate)
            }

            return (false, false, null); //(not done, not duplicate)
        }

        public async Task<bool> AllOperationDoneProcess(List<AllOperationDoneVM> dto)
        {

            #region *** Old Method ***
            //try
            //{
            //    var tempAllOprDoneList = await _context.Temp_Tables
            //        .Where(x => x.Transaction_Id == dto.TransactionId &&
            //                    x.LicenseNumberLong == dto.LicenseNumberLong &&
            //                    dto.FormModes.Contains(x.FormMode))
            //        .ToListAsync();

            //    foreach (var item in tempAllOprDoneList)
            //    {
            //        item.Status = ConstantValue.Status_Pending;
            //    }

            //    ////method 1
            //    //_context.Temp_Tables.AttachRange(tempAllOprDoneList);
            //    //_context.Entry(tempAllOprDoneList).State = EntityState.Modified;
            //    //await _context.SaveChangesAsync();

            //    //method 2
            //    _context.Temp_Tables.UpdateRange(tempAllOprDoneList);
            //    await _context.SaveChangesAsync();
            //    return true;
            //}catch(Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //    return false;
            //}
            #endregion

            try
            {
                foreach(var item in dto)
                {
                    var vehicleOperationStatus = await _context.Vehicles
                        .Where(x => x.CreatedDate.Date == item.CreatedDate.Date &&
                                    x.Transaction_Id == item.TransactionId &&
                                    x.LicenseNumberLong == item.LicenseNumberLong &&
                                    x.FormMode == item.FormModes)
                        .ToListAsync();
                    foreach (var vehicle in vehicleOperationStatus)
                    {
                        vehicle.Status = ConstantValue.Status_Pending;
                    }
                    _context.Vehicles.UpdateRange(vehicleOperationStatus);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public async Task<(LicenseOnly?, string?)> LicenseDetailForOver2ton(string licenseNumberLong)
        {
            var vehicleObj = await _context.Vehicles
                   .AsNoTracking()
                   .Where(x => x.LicenseNumberLong == licenseNumberLong)
                   .Include(x => x.LicenseOnly).ThenInclude(x => x.RegistrationOffice)
                   .Include(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
                  .OrderByDescending(x => x.ApplyDate)
                   .Select(x => new { x.NRC_Number, x.LicenseOnly, x.ExpiryDate})
                   .FirstOrDefaultAsync();
                   //.ToListAsync();

            if (vehicleObj == null)
                return (null, null);

            if (vehicleObj.ExpiryDate.HasValue && vehicleObj.ExpiryDate.Value.Date < DateTime.Now.Date)
                return (null, "လွဲယူမည့်သူ၏ လုပ်ငန်းလိုင်စင်သည် သက်တမ်းကုန်နေပါသည်။");
            if (!vehicleObj.ExpiryDate.HasValue)
                return (null, "လွဲယူမည့်သူ၏ လုပ်ငန်းလိုင်စင် သည် သက်တမ်းကုန်ဆုံးရက် မှားရွှင်းနေပါသည်။");

            if (vehicleObj.LicenseOnly == null)
            {
                var licenseOnlyObj = await _context.LicenseOnlys.AsNoTracking()
                    .Where(x => x.License_Number == licenseNumberLong && x.NRC_Number == vehicleObj.NRC_Number)
                    .OrderByDescending(x => x.CreatedDate)
                    .FirstOrDefaultAsync();

                if (licenseOnlyObj != null)
                    return (null, null);
                return (licenseOnlyObj, null);
            }
            return (vehicleObj.LicenseOnly, null);

        }

        //public Temp_Table DeepCopy(Temp_Table dto)
        //{
        //    return new Temp_Table
        //    {
        //        LicenseOnlyId = dto.LicenseOnlyId,
        //        Transaction_Id = dto.Transaction_Id,
        //        LicenseNumberLong = dto.LicenseNumberLong,
        //        NRC_Number = dto.NRC_Number,
        //        AttachFile_NRC = dto.AttachFile_NRC,
        //        AttachFile_M10 = dto.AttachFile_M10,
        //        AttachFile_Part1 = dto.AttachFile_Part1,
        //        AttachFile_RecommandDoc1 = dto.AttachFile_RecommandDoc1,
        //        AttachFile_RecommandDoc2 = dto.AttachFile_RecommandDoc2,
        //        FormMode = dto.FormMode,
        //        CreatedDate = dto.CreatedDate
        //    };
        //}
    }
}
