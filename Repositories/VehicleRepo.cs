using DOTP_BE.Common;
using DOTP_BE.Data;
using DOTP_BE.Helpers;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.Models;
using DOTP_BE.ViewModel;
using DOTP_BE.ViewModel.AdminResponses;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DOTP_BE.Repositories
{
    public class VehicleRepo : IVehicle
    {
        private readonly ApplicationDbContext _context;
        public VehicleRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Vehicle>> getVehicleList()
        {
            var result = await _context.Vehicles.ToListAsync();
            return result;
        }

        public async Task<List<Vehicle>> getVehicleById(string formMode, string transactionId, string status)
        {
            var vehicles = await _context.Vehicles.AsNoTracking()
                                        .Include(x => x.OperatorDetail)
                                        .Include(x => x.CreateCar)
                                        .Include(x => x.VehicleWeight)
                                        .Include(x => x.LicenseOnly).ThenInclude(x => x.RegistrationOffice)
                                        .Include(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
                                        .Where(s => s.Transaction_Id == transactionId &&
                                                    s.Status == status &&
                                                    s.FormMode == formMode)
                                        .ToListAsync();
            if (vehicles.Count > 0 && vehicles[0].LicenseOnly == null) //for licenseOnly include
            {
                vehicles[0].LicenseOnly = await _context.LicenseOnlys.AsNoTracking()
                                                        .Where(x => x.License_Number == vehicles[0].LicenseNumberLong)
                                                        .FirstAsync();
            }

            //can't filter by Transaction_Id cause after admin approved new operator record with that transaction will add,
            //here operator will not have and data yet so with only NRC Number
            ////if can use include then don't need this one
            //if (vehicles.Count > 0 && vehicles[0].OperatorDetail == null)
            //{
            //    //operatorDetail is use for only (email in extendOperatorLicense) currently found
            //    var opObj = await _context.OperatorDetails.AsNoTracking()
            //                                              .Where(x => x.NRC == vehicles[0].NRC_Number)
            //                                              //.Select(x => x.Email)
            //                                              .FirstAsync();
            //    vehicles.ForEach(x => x.OperatorDetail = opObj); // include operator to every(OperatorDetail)
            //}
            return vehicles;
        }

        public async Task<List<Vehicle>> getVehicleById(string transactionId, string status)
        {
            var vehicles = await _context.Vehicles.AsNoTracking()
                                        .Include(x => x.OperatorDetail)
                                        .Include(x => x.CreateCar)
                                        .Include(x => x.VehicleWeight)
                                        .Include(x => x.LicenseOnly).ThenInclude(x => x.RegistrationOffice)
                                        .Include(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
                                        .Where(s => s.Transaction_Id == transactionId &&
                                                    s.Status == status &&
                                                    s.FormMode == ConstantValue.EOPL_FM)
                                        .ToListAsync();
            if (vehicles.Count > 0 && vehicles[0].LicenseOnly == null) //for licenseOnly include
            {
                var licenseOnlyObj = await _context.LicenseOnlys.AsNoTracking()
                                                        .Where(x => x.License_Number == vehicles[0].LicenseNumberLong)
                                                        .FirstOrDefaultAsync();
                vehicles[0].LicenseOnly = licenseOnlyObj;
            }

            //can't filter by Transaction_Id cause after admin approved new operator record with that transaction will add,
            //here operator will not have and data yet so with only NRC Number
            //if can use include then don't need this one
            if (vehicles.Count > 0 && vehicles[0].OperatorDetail == null)
            {
                //operatorDetail is use for only (email in extendOperatorLicense) currently found
                var opObj = await _context.OperatorDetails.AsNoTracking()
                                                          .Where(x => x.NRC == vehicles[0].NRC_Number)
                                                          //.Select(x => x.Email)
                                                          //.FirstAsync();
                                                          .FirstOrDefaultAsync();
                vehicles[0].OperatorDetail = opObj;
                //vehicles.ForEach(x => x.OperatorDetail = opObj); // include operator to every(OperatorDetail)
            }
            return vehicles;
        }

        //public async Task<(Vehicle, List<Vehicle>)> getVehicleById(int id)
        //{
        //    var vehicle = await _context.Vehicles
        //                                .Include(x => x.OperatorDetail)
        //                                .Include(x => x.LicenseOnly).ThenInclude(x => x.RegistrationOffice)
        //                                .Include(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
        //                                .Where(s => s.VehicleId == id && s.Status == ConstantValue.Status_Pending)
        //                                .FirstOrDefaultAsync();

        //    var totalCarList = await _context.Vehicles
        //                                     .Include(x => x.CreateCar)
        //                                     .Include(x => x.VehicleWeight)
        //                                     //.Include(x => x.LicenseOnly)
        //                                     .Where(x => x.NRC_Number == (vehicle == null ? "" : vehicle.NRC_Number) &&
        //                                                 x.LicenseNumberLong == (vehicle == null ? "" : vehicle.LicenseNumberLong) &&
        //                                                 x.Status == ConstantValue.Status_Pending) //pending status
        //                                     .ToListAsync();
        //    return (vehicle != null ? vehicle : new Vehicle(), totalCarList);
        //}
        public async Task<bool> Create(VehicleVM vehicleVM)
        {
            var createCar = new CreateCar();
            var licenseOnly = new LicenseOnly();
            var vehicleWeight = new VehicleWeight();

            if (vehicleVM.CreateCarId != null)
            {
                createCar = _context.CreateCars.Find(vehicleVM.CreateCarId);
            }
            if (vehicleVM.LicenseOnlyId != null)
            {
                licenseOnly = _context.LicenseOnlys.Find(vehicleVM.LicenseOnlyId);
            }
            if (vehicleVM.VehicleWeightId != null)
            {
                vehicleWeight = _context.VehicleWeights.Find(vehicleVM.VehicleWeightId);
            }

            var vehicle = new Vehicle()
            {
                Transaction_Id = vehicleVM.Transaction_Id,
                ChalenNumber = vehicleVM.ChalenNumber,
                NRC_Number = licenseOnly.NRC_Number ?? "", //From LicenseOnly
                ApplicantId = vehicleVM.ApplicantId,
                License_Number = licenseOnly.License_Number ?? "",//From LicenseOnly
                LicenseNumberLong = vehicleVM.LicenseNumberLong,
                VehicleNumber = createCar.VehicleNumber ?? "", //From CreateCar
                VehicleLineTitle = vehicleVM.VehicleLineTitle,
                CarryLogisticType = vehicleVM.CarryLogisticType,
                VehicleLocation = vehicleVM.VehicleLocation,
                VehicleDesiredRoute = vehicleVM.VehicleDesiredRoute,
                Notes = vehicleVM.Notes,
                Status = vehicleVM.Status,
                CertificatePrinted = vehicleVM.CertificatePrinted,
                Part1Printed = vehicleVM.Part1Printed,
                Part2Printed = vehicleVM.Part2Printed,
                TrianglePrinted = vehicleVM.TrianglePrinted,
                ApplyDate = vehicleVM.ApplyDate,
                ExpiryDate = vehicleVM.ExpiryDate,
                IsDeleted = false,
                FormMode = vehicleVM.FormMode,
                RefTransactionId = vehicleVM.RefTransactionId,
                Triangle = vehicleVM.Triangle,
                OwnerBook = vehicleVM.OwnerBook,
                AttachedFile1 = vehicleVM.AttachedFile1,
                AttachedFile2 = vehicleVM.AttachedFile2,
                LicenseTypeId = vehicleVM.LicenseTypeId,
                VehicleWeightId = vehicleVM.VehicleWeightId,
                CreateCarId = vehicleVM.CreateCarId,
                LicenseOnlyId = vehicleVM.LicenseOnlyId,
                CreatedDate = DateTime.Now,
                CreatedBy = vehicleVM.CreatedBy
            };
            await _context.Vehicles.AddAsync(vehicle);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Update(int id, VehicleVM vehicleVM)
        {
            var vehicle = _context.Vehicles.Find(id);
            var createCar = new CreateCar();
            var licenseOnly = new LicenseOnly();
            var vehicleWeight = new VehicleWeight();

            if (vehicleVM.CreateCarId != null)
            {
                createCar = _context.CreateCars.Find(vehicleVM.CreateCarId);
            }
            if (vehicleVM.LicenseOnlyId != null)
            {
                licenseOnly = _context.LicenseOnlys.Find(vehicleVM.LicenseOnlyId);
            }

            if (vehicle != null)
            {
                vehicle.Transaction_Id = vehicleVM.Transaction_Id;
                vehicle.ChalenNumber = vehicleVM.ChalenNumber;
                vehicle.NRC_Number = licenseOnly.NRC_Number ?? "";//From LicenseOnl
                vehicle.ApplicantId = vehicleVM.ApplicantId;
                vehicle.License_Number = licenseOnly.License_Number ?? "";//From LicenseOnly
                vehicle.LicenseNumberLong = vehicleVM.LicenseNumberLong;
                vehicle.VehicleNumber = createCar.VehicleNumber ?? ""; //From CreateCar
                vehicle.VehicleLineTitle = vehicleVM.VehicleLineTitle;
                vehicle.CarryLogisticType = vehicleVM.CarryLogisticType;
                vehicle.VehicleLocation = vehicleVM.VehicleLocation;
                vehicle.VehicleDesiredRoute = vehicleVM.VehicleDesiredRoute;
                vehicle.Notes = vehicleVM.Notes;
                vehicle.Status = vehicleVM.Status;
                vehicle.CertificatePrinted = vehicleVM.CertificatePrinted;
                vehicle.Part1Printed = vehicleVM.Part1Printed;
                vehicle.Part2Printed = vehicleVM.Part2Printed;
                vehicle.TrianglePrinted = vehicleVM.TrianglePrinted;
                vehicle.ApplyDate = vehicleVM.ApplyDate;
                vehicle.ExpiryDate = vehicleVM.ExpiryDate;
                vehicle.IsDeleted = false;
                vehicle.FormMode = vehicleVM.FormMode;
                vehicle.RefTransactionId = vehicleVM.RefTransactionId;
                vehicle.Triangle = vehicleVM.Triangle;
                vehicle.OwnerBook = vehicleVM.OwnerBook;
                vehicle.AttachedFile1 = vehicleVM.AttachedFile1;
                vehicle.AttachedFile2 = vehicleVM.AttachedFile2;
                vehicle.LicenseTypeId = vehicleVM.LicenseTypeId;
                vehicle.VehicleWeightId = vehicleWeight.VehicleWeightId;
                vehicle.CreateCarId = vehicleVM.CreateCarId;
                vehicle.LicenseOnlyId = vehicleVM.LicenseOnlyId;
                vehicle.UpdatedDate = DateTime.Now;

                _context.Vehicles.Update(vehicle);
                await _context.SaveChangesAsync();
                return true;
            };
            return false;
        }

        public void Delete(int id)
        {
            var vehicle = _context.Vehicles.Find(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                _context.SaveChangesAsync();
            }

        }

        public async Task<ExtendLicenseDashBoardVMAdmin> getVehicleListByStatusTesting(ExtenLicenseDbSearchVM dto)
        {
            var fd = DateTime.Parse(dto.FromDate).Date;
            var td = DateTime.Parse(dto.ToDate).Date;

            var filteredByDate = await _context.Vehicles.AsNoTracking()
                .Where(x => x.CreatedDate >= fd && x.CreatedDate <= td)
                .Include(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
                .ToListAsync();

            var extendLicenseVMs = filteredByDate
                                 .Where(x => x.Status.Equals(dto.Status, StringComparison.OrdinalIgnoreCase))
                                 .GroupBy(x => x.Transaction_Id)
                                 //.GroupBy(x => new { x.Transaction_Id, x.FormMode })
                                 .Select(x => new ExtendLicenseVMAdmin
                                 {
                                     FormMode = x.Select(g => g.FormMode).Distinct().ToList(),
                                     LicenseNumberLong = x.First().LicenseNumberLong,
                                     JourneyTypeLong = x.First().LicenseOnly.JourneyType.JourneyTypeLong,
                                     TotalCar = x.Count(),
                                     CreatedDate = x.First().CreatedDate,
                                     UpdatedDate = x.First().UpdatedDate,
                                     ExpireDate = x.First().ExpiryDate,
                                     Status = x.First().Status,
                                     //Status = x.Select(g => g.Status).Distinct().ToList(),
                                     TransactionId = x.First().Transaction_Id,
                                     LicenseTypeId = x.First().LicenseTypeId
                                 })
                                 .ToList();

            ExtendLicenseDashBoardVMAdmin result = new ExtendLicenseDashBoardVMAdmin();
            result.ExtendLicenseVMAdmins = extendLicenseVMs;


            #region *** old method ***
            //ExtendLicenseDashBoardVMAdmin result = new ExtendLicenseDashBoardVMAdmin
            //{
            //    PendingCount = extendLicenseVMs.Count(x => x.Status == ConstantValue.Status_Pending),
            //    ApprovedCount = extendLicenseVMss.Count(c => c.Status == ConstantValue.Status_Approved),
            //    PaidCount = extendLicenseVMss.Count(c => c.Status == ConstantValue.Status_Paid),
            //    RejectedCount = extendLicenseVMss.Count(c => c.Status == ConstantValue.Status_Rejected)
            //};

            //result.ExtendLicenseVMAdmins = extendLicenseVMs.Where(x => x.Status == dto.Status).ToList();
            ////if (dto.FormMode != null)
            ////    result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.FormMode == dto.FormMode).ToList();
            //if (dto.JourneyType != 0)
            //{
            //    if (dto.JourneyType == 1)
            //        result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseNumberLong.Contains(ConstantValue.Twin)).ToList();
            //    else if (dto.JourneyType == 2)
            //        result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseNumberLong.Contains(ConstantValue.Kyaw)).ToList();
            //}
            //if (dto.LicenseType != 0)
            //{
            //    if (dto.LicenseType == 1)
            //        result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 1 ||
            //                                                                               x.LicenseTypeId == 2 ||
            //                                                                               x.LicenseTypeId == 3)
            //                                                                   .ToList();
            //    else if (dto.LicenseType == 4)
            //        result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 4)
            //                                                                   .ToList();
            //    else if (dto.LicenseType == 5)
            //        result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 5)
            //                                                                   .ToList();
            //    else if (dto.LicenseType == 6)
            //        result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 6 ||
            //                                                                               x.LicenseTypeId == 7)
            //                                                                   .ToList();
            //    else if (dto.LicenseType == 8)
            //        result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 8)
            //                                                                   .ToList();
            //}
            #endregion

            return result;
        }


        public async Task<(int, ExtendLicenseDashBoardVMAdmin)> getVehicleListByStatus(ExtenLicenseDbSearchVM dto)
        {
            //var fd = DateTime.Parse(dto.FromDate).Date;
            //var td = DateTime.Parse(dto.ToDate).Date;
            if (!DateTime.TryParse(dto.FromDate, out var fd) || !DateTime.TryParse(dto.ToDate, out var td))
            {
                // Handle invalid date format
                // For example, return an error or default values
                return (0, new ExtendLicenseDashBoardVMAdmin());
            }
            fd = fd.Date; //no need for web but for mobile
            td = td.Date; //no need for web but for mobile

            var filteredByDate = await _context.Vehicles.AsNoTracking()
                .Where(x => x.CreatedDate.Date >= fd && 
                            x.CreatedDate.Date <= td &&
                            x.IsDeleted != true)
                .Include(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
                .ToListAsync();

            if (filteredByDate.Count == 0)
                return (0, new ExtendLicenseDashBoardVMAdmin());

            #region *** Filter by search parameters ***
            if (!string.IsNullOrWhiteSpace(dto.FormMode))
                filteredByDate = filteredByDate.Where(x => x.FormMode == dto.FormMode).ToList();
            if (dto.JourneyType != 0)
            {
                if (dto.JourneyType == 1)
                    filteredByDate = filteredByDate.Where(x => x.LicenseNumberLong.Contains(ConstantValue.Twin)).ToList();
                else if (dto.JourneyType == 2)
                    filteredByDate = filteredByDate.Where(x => x.LicenseNumberLong.Contains(ConstantValue.Kyaw)).ToList();
            }
            if (dto.LicenseType != 0)
            {
                if (dto.LicenseType == 1)
                    filteredByDate = filteredByDate.Where(x => x.LicenseTypeId == 1 ||
                                                               x.LicenseTypeId == 2 ||
                                                               x.LicenseTypeId == 3)
                                                    .ToList();
                else if (dto.LicenseType == 4)
                    filteredByDate = filteredByDate.Where(x => x.LicenseTypeId == 4)
                                                   .ToList();
                else if (dto.LicenseType == 5)
                    filteredByDate = filteredByDate.Where(x => x.LicenseTypeId == 5)
                                                   .ToList();
                else if (dto.LicenseType == 6)
                    filteredByDate = filteredByDate.Where(x => x.LicenseTypeId == 6 ||
                                                               x.LicenseTypeId == 7)
                                                   .ToList();
                else if (dto.LicenseType == 8)
                    filteredByDate = filteredByDate.Where(x => x.LicenseTypeId == 8)
                                                   .ToList();
            }
            #endregion

            #region *** SQL query ***
            //select t.Transaction_Id,count(Transaction_Id) T from (SELECT Status,CreatedDate,Transaction_Id
            //FROM[Dotp_Phase4].[dbo].[Vehicles] where Status = 'Approved') T
            //group by Transaction_Id
            #endregion

            var extendLicenseVMs = filteredByDate
                                 .Where(x => x.Status == dto.Status)
                                 .OrderByDescending(x => x.CreatedDate)
                                 .GroupBy(x => x.Transaction_Id)
                                 .Select(x => new ExtendLicenseVMAdmin
                                 {
                                     FormMode = x.Select(g => g.FormMode).Distinct().ToList(),
                                     LicenseNumberLong = x.First().LicenseNumberLong,
                                     JourneyTypeLong = x.First().LicenseOnly.JourneyType.JourneyTypeLong,
                                     TotalCar = x.Count(),
                                     CreatedDate = x.First().CreatedDate,
                                     UpdatedDate = x.First().UpdatedDate,
                                     ExpireDate = x.First().ExpiryDate,
                                     Status = x.First().Status,
                                     TransactionId = x.First().Transaction_Id,
                                     LicenseTypeId = x.First().LicenseTypeId
                                 })
                                 .Skip((dto.PageNumber - 1) * dto.PageSize)
                                 .Take(dto.PageSize)
                                 .ToList();

            int pendingCount = CountByStatus(filteredByDate, ConstantValue.Status_Pending);
            int approvedCount = CountByStatus(filteredByDate, ConstantValue.Status_Approved);
            int rejectedCount = CountByStatus(filteredByDate, ConstantValue.Status_Rejected);
            int paidCount = CountByStatus(filteredByDate, ConstantValue.Status_Paid);

            #region *** Not Use ***
            int totalCount = 0;
            switch (dto.Status)
            {
                case ConstantValue.Status_Pending:
                    totalCount = CountByStatus(filteredByDate, ConstantValue.Status_Pending);
                    break;
                case ConstantValue.Status_Approved:
                    totalCount = CountByStatus(filteredByDate, ConstantValue.Status_Approved);
                    break;
                case ConstantValue.Status_Rejected:
                    totalCount = CountByStatus(filteredByDate, ConstantValue.Status_Rejected);
                    break;
                case ConstantValue.Status_Paid:
                    totalCount = CountByStatus(filteredByDate, ConstantValue.Status_Paid);
                    break;
            }
            #endregion

            ExtendLicenseDashBoardVMAdmin result = new ExtendLicenseDashBoardVMAdmin
            {
                PendingCount = pendingCount,
                ApprovedCount = approvedCount,
                PaidCount = paidCount,
                RejectedCount = rejectedCount,
                ExtendLicenseVMAdmins = extendLicenseVMs
            };
            return (totalCount, result);
        }

        #region Currently using 2
        //public async Task<ExtendLicenseDashBoardVMAdmin> getVehicleListByStatus(ExtenLicenseDbSearchVM dto)
        //{
        //    var fd = DateTime.Parse(dto.FromDate).Date;
        //    var td = DateTime.Parse(dto.ToDate).Date;
        //    //var ddate = _context.Vehicles.Select(x => x.CreatedDate.Date).First();
        //    //var fdd = DateTime.Parse(ddate);
        //    var extendLicenseVMs = await _context.Vehicles
        //                         .Include(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
        //                         .GroupBy(x => new { x.Transaction_Id, x.FormMode})
        //                         .Select(x => new ExtendLicenseVMAdmin
        //                         {
        //                             FormMode = x.First().FormMode,
        //                             LicenseNumberLong = x.First().LicenseNumberLong,
        //                             JourneyTypeLong = x.First().LicenseOnly.JourneyType.JourneyTypeLong,
        //                             TotalCar = x.Count(),
        //                             CreatedDate = x.First().CreatedDate.Date,
        //                             UpdatedDate = x.First().UpdatedDate,
        //                             ExpireDate = x.First().ExpiryDate,
        //                             Status = x.First().Status,
        //                             TransactionId = x.First().Transaction_Id,
        //                             LicenseTypeId = x.First().LicenseTypeId
        //                         })
        //                         .Where(x => x.CreatedDate.Date >= fd && x.CreatedDate.Date <=td)
        //                         //.Where(x =>
        //                         //            x.CreatedDate >= fd &&
        //                         //            x.CreatedDate <= td &&
        //                         //            x.FormMode == ConstantValue.EOPL_FM ||
        //                         //            x.FormMode == ConstantValue.ECL_FM)
        //                         .ToListAsync();


        //    ExtendLicenseDashBoardVMAdmin result = new ExtendLicenseDashBoardVMAdmin();
        //    result.PendingCount = extendLicenseVMs.Count(x => x.Status == ConstantValue.Status_Pending);
        //    result.ApprovedCount = extendLicenseVMs.Count(c => c.Status == ConstantValue.Status_Approved);
        //    result.PaidCount = extendLicenseVMs.Count(c => c.Status == ConstantValue.Status_Paid);
        //    result.RejectedCount = extendLicenseVMs.Count(c => c.Status == ConstantValue.Status_Rejected);

        //    result.ExtendLicenseVMAdmins = extendLicenseVMs.Where(x => x.Status == dto.Status).ToList();
        //    if (dto.FormMode != null)
        //        result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.FormMode == dto.FormMode).ToList();
        //    if (dto.JourneyType != 0)
        //    {
        //        if (dto.JourneyType == 1)
        //            result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseNumberLong.Contains(ConstantValue.Twin)).ToList();
        //        else if (dto.JourneyType == 2)
        //            result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseNumberLong.Contains(ConstantValue.Kyaw)).ToList();
        //    }
        //    if (dto.LicenseType != 0)
        //    {
        //        if (dto.LicenseType == 1)
        //            result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 1 ||
        //                                                                                   x.LicenseTypeId == 2 ||
        //                                                                                   x.LicenseTypeId == 3)
        //                                                                       .ToList();
        //        else if (dto.LicenseType == 4)
        //            result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 4)
        //                                                                       .ToList();
        //        else if (dto.LicenseType == 5)
        //            result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 5)
        //                                                                       .ToList();
        //        else if (dto.LicenseType == 6)
        //            result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 6 ||
        //                                                                                   x.LicenseTypeId == 7)
        //                                                                       .ToList();
        //        else if (dto.LicenseType == 8)
        //            result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 8)
        //                                                                       .ToList();
        //    }

        //    return result;
        //}
        #endregion

        #region Currently using 1
        //public async Task<ExtendLicenseDashBoardVMAdmin> getVehicleListByStatus(ExtenLicenseDbSearchVM dto)
        //{
        //    var fd = DateTime.Parse(dto.FromDate).Date;
        //    var td = DateTime.Parse(dto.ToDate).Date;
        //    //var ddate = _context.Vehicles.Select(x => x.CreatedDate.Date).First();
        //    //var fdd = DateTime.Parse(ddate);
        //    var extendLicenseVMs = await _context.Vehicles
        //                         .Include(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
        //                         .GroupBy(x => x.Transaction_Id)
        //                         .Select(x => new ExtendLicenseVMAdmin
        //                         {
        //                             FormMode = x.First().FormMode,
        //                             LicenseNumberLong = x.First().LicenseNumberLong,
        //                             JourneyTypeLong = x.First().LicenseOnly.JourneyType.JourneyTypeLong,
        //                             TotalCar = x.Count(),
        //                             CreatedDate = x.First().CreatedDate.Date,
        //                             UpdatedDate = x.First().UpdatedDate,
        //                             ExpireDate = x.First().ExpiryDate,
        //                             Status = x.First().Status,
        //                             TransactionId = x.First().Transaction_Id,
        //                             LicenseTypeId = x.First().LicenseTypeId
        //                         })
        //                         //.Where(x => x.Status == status &&
        //                         //           (x.FormMode == ConstantValue.EOPL_FM ||z
        //                         //            x.FormMode == ConstantValue.ECL_FM))
        //                         .Where(x => 
        //                                     x.CreatedDate >= fd &&
        //                                     x.CreatedDate <= td &&
        //                                     x.FormMode == ConstantValue.EOPL_FM ||
        //                                     x.FormMode == ConstantValue.ECL_FM)
        //                         .ToListAsync();


        //    ExtendLicenseDashBoardVMAdmin result = new ExtendLicenseDashBoardVMAdmin();
        //    result.PendingCount =  extendLicenseVMs.Count(x => x.Status == ConstantValue.Status_Pending);
        //    result.ApprovedCount = extendLicenseVMs.Count(c => c.Status == ConstantValue.Status_Approved);
        //    result.PaidCount = extendLicenseVMs.Count(c => c.Status == ConstantValue.Status_Paid);
        //    result.RejectedCount = extendLicenseVMs.Count(c => c.Status == ConstantValue.Status_Rejected);

        //    result.ExtendLicenseVMAdmins = extendLicenseVMs.Where(x => x.Status == dto.Status).ToList();
        //    if(dto.FormMode !=null)
        //        result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.FormMode ==  dto.FormMode).ToList();
        //    if (dto.JourneyType != 0)
        //    {
        //        if (dto.JourneyType == 1)
        //            result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseNumberLong.Contains(ConstantValue.Twin)).ToList();
        //        else if(dto.JourneyType == 2)
        //            result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseNumberLong.Contains(ConstantValue.Kyaw)).ToList();
        //    }
        //    if(dto.LicenseType != 0)
        //    {
        //        if (dto.LicenseType == 1)
        //            result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 1 ||
        //                                                                                   x.LicenseTypeId == 2 || 
        //                                                                                   x.LicenseTypeId == 3)
        //                                                                       .ToList();
        //        else if (dto.LicenseType == 4)
        //            result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 4)
        //                                                                       .ToList();
        //        else if (dto.LicenseType == 5)
        //            result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 5)
        //                                                                       .ToList();
        //        else if (dto.LicenseType == 6)
        //            result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 6 ||
        //                                                                                   x.LicenseTypeId == 7)
        //                                                                       .ToList();
        //        else if (dto.LicenseType == 8)
        //            result.ExtendLicenseVMAdmins = result.ExtendLicenseVMAdmins.Where(x => x.LicenseTypeId == 8)
        //                                                                       .ToList();
        //    }

        //    return result;
        //}
        #endregion


        #region ****** search by Status, From and To Date ******
        //public async Task<ExtendLicenseDashBoardVMAdmin> getVehicleListByStatusAndDate(string status, string fromDate, string toDate)
        //{
        //    var fd = DateTime.Parse(fromDate).Date;
        //    var td = DateTime.Parse(toDate).Date;
        //    //var ddate = _context.Vehicles.Select(x => x.CreatedDate.Date).First();
        //    //var fdd = DateTime.Parse(ddate);
        //    var extendLicenseVMs = await _context.Vehicles
        //                         .Include(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
        //                         .GroupBy(x => x.Transaction_Id)
        //                         .Select(x => new ExtendLicenseVMAdmin
        //                         {
        //                             //FormMode = x.First().FormMode,
        //                             LicenseNumberLong = x.First().LicenseNumberLong,
        //                             JourneyTypeLong = x.First().LicenseOnly.JourneyType.JourneyTypeLong,
        //                             TotalCar = x.Count(),
        //                             CreatedDate = x.First().CreatedDate.Date,
        //                             UpdatedDate = x.First().UpdatedDate,
        //                             ExpireDate = x.First().ExpiryDate,
        //                             Status = x.First().Status,
        //                             TransactionId = x.First().Transaction_Id
        //                         })
        //                         //.Where(x => x.Status == status &&
        //                         //           (x.FormMode == ConstantValue.EOPL_FM ||z
        //                         //            x.FormMode == ConstantValue.ECL_FM))
        //                         .Where(x => x.CreatedDate >= fd &&
        //                                     x.CreatedDate <= td 
        //                                     //&&
        //                                     //x.FormMode == ConstantValue.EOPL_FM ||
        //                                     //x.FormMode == ConstantValue.ECL_FM
        //                                     )
        //                         .ToListAsync();


        //    ExtendLicenseDashBoardVMAdmin result = new ExtendLicenseDashBoardVMAdmin();
        //    result.PendingCount = extendLicenseVMs.Count(x => x.Status == ConstantValue.Status_Pending);
        //    result.ApprovedCount = extendLicenseVMs.Count(c => c.Status == ConstantValue.Status_Approved);
        //    result.PaidCount = extendLicenseVMs.Count(c => c.Status == ConstantValue.Status_Paid);
        //    result.RejectedCount = extendLicenseVMs.Count(c => c.Status == ConstantValue.Status_Rejected);

        //    result.ExtendLicenseVMAdmins = extendLicenseVMs.Where(x => x.Status == status).ToList();

        //    return result;
        //}


        #endregion

        //public async Task<List<ExtendLicenseVMAdmin>> getVehicleListByStatus(string status)
        //{

        //    var a=  await _context.Vehicles
        //                         .Include(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
        //                         .GroupBy(x => x.Transaction_Id)
        //                         .Select(x => new ExtendLicenseVMAdmin
        //                         {
        //                             FormMode = x.First().FormMode,
        //                             LicenseNumberLong = x.First().LicenseNumberLong,
        //                             JourneyTypeLong = x.First().LicenseOnly.JourneyType.JourneyTypeLong,
        //                             TotalCar = x.Count(),
        //                             CreatedDate = x.First().CreatedDate,
        //                             UpdatedDate = x.First().UpdatedDate,
        //                             ExpireDate = x.First().ExpiryDate,
        //                             Status =x.First().Status,
        //                             TransactionId = x.First().Transaction_Id
        //                         })
        //                         //.Where(x => x.Status == status &&
        //                         //           (x.FormMode == ConstantValue.EOPL_FM ||
        //                         //            x.FormMode == ConstantValue.ECL_FM))
        //                         .Where(x => x.FormMode == ConstantValue.EOPL_FM ||
        //                                     x.FormMode == ConstantValue.ECL_FM)
        //                         .ToListAsync();
        //    return a;
        //}

        public async Task<Vehicle> VehicleDetailToCheckById(int id)
        {
            var vehicleObj = await _context.Vehicles
                                           .Include(x => x.CreateCar)
                                           .Include(x => x.OperatorDetail)
                                           .Include(x => x.VehicleWeight)
                                           .Where(x => x.VehicleId == id)
                                           .FirstOrDefaultAsync();
            return vehicleObj != null ? vehicleObj : new Vehicle();
            //return await _context.Vehicles.Include(x => x.CreateCar).FirstAsync(id);
        }

        public async Task<bool> UpdateStatusById(int id, string statusDto)
        {
            var vehicleObj = await _context.Vehicles.FindAsync(id);
            if (vehicleObj == null)
                return false;
            vehicleObj.Status = statusDto;
            _context.Vehicles.Update(vehicleObj);
            await _context.SaveChangesAsync();
            return true;
        }

        #region *** not Used included Commond Changes ***
        //public async Task<(bool, string?)> OperatorLicenseConfirmReject(OLConfirmOrRejectVM oLConfirmOrRejectVM)
        //{
        //    var vehicleObj = await _context.Vehicles.AsNoTracking()
        //                                            .Where(x => x.Transaction_Id == oLConfirmOrRejectVM.TransactionId &&
        //                                                        x.FormMode == oLConfirmOrRejectVM.FormMode)
        //                                            .ToListAsync();
        //    if (vehicleObj != null)
        //    {
        //        //if addmin approved
        //        if (oLConfirmOrRejectVM.ApprovedOrRejected == ConstantValue.Status_Approved)
        //        {
        //            // to check formmod(all changes are the same)
        //            string[] changesGroupd = { "Decrease Car", "ChangeVehicleOwnerAddress", "ChangeLicenseOwnerAddress", "ChangeVehicleType", "ChangeVehicleOwnerName" };


        //            //to prevent 'ExtendOperatorLicense' formMode more than one time in single day 
        //            bool doOperatorLicense = false;
        //            if (oLConfirmOrRejectVM.FormMode == ConstantValue.EOPL_FM)
        //            {
        //                var existingEntry = await _context.OperatorDetails.AsNoTracking()
        //                                                  .FirstOrDefaultAsync(x => x.FormMode == ConstantValue.EOPL_FM &&
        //                                                                            x.ApplyDate.Date == DateTime.Now.Date); //check double ExtendOperatorLicense (we wrok with applydate so it can be wrong)
        //                if (existingEntry != null)
        //                    return (false, "လုပ်ငန်းလိုင်စင်သက်တမ်းတိုးခြင်းသည် တစ်ရက်လျင် တစ်ကြိမ်ထက်ပို၍ လုပ်ခွင့်မပေးပါ။");
        //                else
        //                    doOperatorLicense = true;
        //            }

        //            #region ****** Handle Operator ******
        //            //add new operator
        //            var operatorDetailDto = await _context.OperatorDetails
        //                                                  .Where(x => x.NRC == vehicleObj[0].NRC_Number &&
        //                                                              x.ApplyLicenseType == vehicleObj[0].LicenseTypeId &&
        //                                                              x.FormMode == ConstantValue.CreateNew_FM || x.FormMode == ConstantValue.EOPL_FM)
        //                                                  .OrderByDescending(x => x.ApplyDate) //to get the update one
        //                                                  .FirstOrDefaultAsync();
        //            if (operatorDetailDto != null)
        //            {
        //                operatorDetailDto.OperatorId = ConstantValue.Zero;
        //                operatorDetailDto.Transaction_Id = vehicleObj[0].Transaction_Id;
        //                operatorDetailDto.ApplyDate = DateTime.Now;
        //                //operatorDetailDto.ExpiredDate = operatorDetailDto.ExpiredDate.Value.AddYears(1);
        //                operatorDetailDto.ExpiredDate = operatorDetailDto.ExpiredDate.HasValue ? operatorDetailDto.ExpiredDate.Value.AddYears(1) : DateTime.Now.AddYears(1);
        //                operatorDetailDto.TotalCar = vehicleObj.Count;
        //                operatorDetailDto.Notes = FormModeHelper.formModeIndexMap.ContainsKey(oLConfirmOrRejectVM.FormMode) ? FormModeHelper.formModeIndexMap[oLConfirmOrRejectVM.FormMode] : "";
        //                operatorDetailDto.IsClosed = true;
        //                operatorDetailDto.FormMode = oLConfirmOrRejectVM.FormMode;
        //                operatorDetailDto.VehicleId = vehicleObj[0].VehicleId;
        //                operatorDetailDto.UpdatedDate = DateTime.Now;
        //                operatorDetailDto.CreatedBy = "Admin Name";

        //                _context.OperatorDetails.Add(operatorDetailDto);
        //            }
        //            #endregion

        //            #region ****** Handle Transaction ******
        //            var feesObj = await _context.Fees.AsNoTracking()
        //                                       .Where(x => x.JourneyTypeId == (vehicleObj[0].LicenseNumberLong.Contains(ConstantValue.Twin) ? ConstantValue.One : ConstantValue.Two) &&
        //                                                   x.VehicleWeightId == vehicleObj[0].VehicleWeightId)
        //                                       .FirstOrDefaultAsync();

        //            //check transaction_id already exit or not
        //            bool alreadyExit = await _context.Transactions.AsNoTracking().AnyAsync(x => x.Transaction_Id == vehicleObj[0].Transaction_Id);

        //            //checking form Mode
        //            #region *** for 'ExtendOperatorLicense' FormMode ***
        //            if (oLConfirmOrRejectVM.FormMode == ConstantValue.EOPL_FM && feesObj != null && doOperatorLicense)
        //            {
        //                //check transaction_id already exit or not
        //                //bool alreadyExit = await _context.Transactions.AsNoTracking().AnyAsync(x => x.Transaction_Id == vehicleObj[0].Transaction_Id);
        //                if (alreadyExit) //transaction_id exit (Update Function)
        //                {
        //                    var tObj = await _context.Transactions.FirstOrDefaultAsync(x => x.Transaction_Id == oLConfirmOrRejectVM.TransactionId);
        //                    if (tObj != null)
        //                    {
        //                        tObj.RegistrationFees += vehicleObj.Count * feesObj.RegistrationFees;
        //                        tObj.RegistrationCharges += feesObj.RegistrationCharges;
        //                        tObj.CertificateFees += feesObj.CertificateFees;
        //                        tObj.PartOneFees += feesObj.PartOneFees;
        //                        tObj.PartTwoFees += feesObj.PartTwoFees; //null able (to check again)
        //                        tObj.TriangleFees += feesObj.TriangleFees; //null able (to check again)
        //                        tObj.ModifiedCharges += ConstantValue.Zero;
        //                        tObj.TotalCars += vehicleObj.Count;
        //                        tObj.Total_WithoutCertificate += feesObj.RegistrationCharges + ConstantValue.twoThousand;
        //                        tObj.Total += tObj.RegistrationFees + tObj.RegistrationCharges + tObj.CertificateFees + tObj.PartOneFees +
        //                                                        tObj.PartTwoFees + tObj.TriangleFees;
        //                        tObj.UpdatedDate = DateTime.Now;
        //                        _context.Transactions.Update(tObj);
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine("Transaction not found.");
        //                    }
        //                }
        //                else //transaction_id doesn't exit (Add Function)
        //                {
        //                    var tObj = new Transaction()
        //                    {
        //                        Transaction_Id = vehicleObj[0].Transaction_Id,
        //                        ChalenNumber = vehicleObj[0].ChalenNumber,
        //                        NRC_Number = vehicleObj[0].NRC_Number,
        //                        RegistrationFees = vehicleObj.Count * feesObj.RegistrationFees,
        //                        RegistrationCharges = feesObj.RegistrationCharges,
        //                        CertificateFees = feesObj.CertificateFees,
        //                        PartOneFees = feesObj.PartOneFees,
        //                        PartTwoFees = feesObj.PartTwoFees,
        //                        TriangleFees = feesObj.TriangleFees,
        //                        ModifiedCharges = ConstantValue.Zero,
        //                        TotalCars = vehicleObj.Count,
        //                        Total_WithoutCertificate = feesObj.RegistrationCharges + ConstantValue.twoThousand,
        //                        Total = (vehicleObj.Count * feesObj.RegistrationFees) + feesObj.RegistrationCharges + feesObj.CertificateFees + feesObj.PartOneFees +
        //                                                        feesObj.PartTwoFees + feesObj.TriangleFees,
        //                        Status = ConstantValue.Status_Pending,
        //                        CreatedDate = DateTime.Now
        //                    };
        //                    _context.Transactions.Add(tObj);
        //                }

        //            }
        //            #endregion

        //            #region *** for 'Common Changes' FormMode ***
        //            else if (changesGroupd.Contains(oLConfirmOrRejectVM.FormMode) && feesObj != null)
        //            {
        //                //check transaction_id already exit or not
        //                //bool alreadyExit = await _context.Transactions.AsNoTracking().AnyAsync(x => x.Transaction_Id == vehicleObj[0].Transaction_Id);
        //                if (alreadyExit) //transaction_id exit (Update Function)
        //                {
        //                    var tObj = await _context.Transactions.FirstOrDefaultAsync(x => x.Transaction_Id == oLConfirmOrRejectVM.TransactionId);
        //                    if (tObj != null)
        //                    {
        //                        //already check vehicleObj != null so count star from 1
        //                        int mofifiedCharges = vehicleObj.Count >= ConstantValue.Two ? ConstantValue.thirtyThousand : ConstantValue.tenThousand;
        //                        tObj.RegistrationCharges += ConstantValue.oneThousand;
        //                        tObj.ModifiedCharges += mofifiedCharges;
        //                        tObj.TotalCars += vehicleObj.Count;
        //                        tObj.Total_WithoutCertificate += ConstantValue.oneThousand + ConstantValue.twoThousand; // oneThousand is for RegistrationCharges
        //                        tObj.Total += ConstantValue.oneThousand + mofifiedCharges; // oneThousand is for RegistrationCharges
        //                        tObj.UpdatedDate = DateTime.Now;
        //                        _context.Transactions.Update(tObj);
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine("Transaction not found.");
        //                    }
        //                }
        //                else //transaction_id doesn't exit (Add Function)
        //                {
        //                    //already check vehicleObj != null so count star from 1
        //                    int mofifiedCharges = vehicleObj.Count >= ConstantValue.Two ? ConstantValue.thirtyThousand : ConstantValue.tenThousand;
        //                    var tObj = new Transaction()
        //                    {
        //                        Transaction_Id = vehicleObj[0].Transaction_Id,
        //                        ChalenNumber = vehicleObj[0].ChalenNumber,
        //                        NRC_Number = vehicleObj[0].NRC_Number,
        //                        RegistrationFees = ConstantValue.Zero,
        //                        RegistrationCharges = ConstantValue.oneThousand,
        //                        CertificateFees = ConstantValue.Zero,
        //                        PartOneFees = ConstantValue.Zero,
        //                        PartTwoFees = ConstantValue.Zero,
        //                        TriangleFees = ConstantValue.Zero,
        //                        ModifiedCharges = mofifiedCharges,
        //                        TotalCars = vehicleObj.Count,
        //                        Total_WithoutCertificate = ConstantValue.oneThousand + ConstantValue.twoThousand,
        //                        Total = ConstantValue.oneThousand + mofifiedCharges,
        //                        Status = ConstantValue.Status_Pending,
        //                        CreatedDate = DateTime.Now
        //                    };
        //                    _context.Transactions.Add(tObj);
        //                }
        //            }
        //            #endregion

        //            #region *** for 'Add New Car' FormMode ***
        //            else if ((oLConfirmOrRejectVM.FormMode == ConstantValue.AddNewCar_FM || oLConfirmOrRejectVM.FormMode == ConstantValue.ECL_FM) && feesObj != null)
        //            {
        //                //check transaction_id already exit or not
        //                //bool alreadyExit = await _context.Transactions.AsNoTracking().AnyAsync(x => x.Transaction_Id == vehicleObj[0].Transaction_Id);
        //                if (alreadyExit) //transaction_id exit (Update Function)
        //                {
        //                    var tObj = await _context.Transactions.FirstOrDefaultAsync(x => x.Transaction_Id == oLConfirmOrRejectVM.TransactionId);
        //                    if (tObj != null)
        //                    {
        //                        int mofifiedCharges = vehicleObj.Count >= ConstantValue.Two ? ConstantValue.thirtyThousand : ConstantValue.tenThousand;
        //                        int partTwoFeesDto = vehicleObj.Count > feesObj.MaxCars ? (feesObj.PartTwoFees * vehicleObj.Count) : feesObj.PartTwoFees;
        //                        int triangleDto = vehicleObj.Count > feesObj.MaxCars ? (feesObj.TriangleFees * vehicleObj.Count) : feesObj.TriangleFees;

        //                        tObj.RegistrationFees += feesObj.RegistrationFees * vehicleObj.Count;
        //                        tObj.RegistrationCharges += feesObj.RegistrationCharges;
        //                        tObj.PartTwoFees += partTwoFeesDto;
        //                        tObj.TriangleFees += triangleDto;
        //                        tObj.ModifiedCharges += mofifiedCharges;
        //                        tObj.TotalCars += vehicleObj.Count;
        //                        tObj.Total_WithoutCertificate += feesObj.RegistrationCharges + ConstantValue.twoThousand;
        //                        tObj.Total += (feesObj.RegistrationFees * vehicleObj.Count) + feesObj.RegistrationCharges + partTwoFeesDto + triangleDto + mofifiedCharges;
        //                        tObj.UpdatedDate = DateTime.Now;
        //                        _context.Transactions.Update(tObj);
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine("Transaction not found.");
        //                    }
        //                }
        //                else //transaction_id doesn't exit (Add Function)
        //                {
        //                    int mofifiedCharges = vehicleObj.Count >= ConstantValue.Two ? ConstantValue.thirtyThousand : ConstantValue.tenThousand;
        //                    int partTwoFeesDto = vehicleObj.Count > feesObj.MaxCars ? (feesObj.PartTwoFees * vehicleObj.Count) : feesObj.PartTwoFees;
        //                    int triangleDto = vehicleObj.Count > feesObj.MaxCars ? (feesObj.TriangleFees * vehicleObj.Count) : feesObj.TriangleFees;

        //                    var tObj = new Transaction()
        //                    {
        //                        Transaction_Id = vehicleObj[0].Transaction_Id,
        //                        ChalenNumber = vehicleObj[0].ChalenNumber,
        //                        NRC_Number = vehicleObj[0].NRC_Number,
        //                        RegistrationFees = feesObj.RegistrationFees * vehicleObj.Count,
        //                        RegistrationCharges = feesObj.RegistrationCharges,
        //                        CertificateFees = ConstantValue.Zero,
        //                        PartOneFees = ConstantValue.Zero,
        //                        PartTwoFees = partTwoFeesDto,
        //                        TriangleFees = triangleDto,
        //                        ModifiedCharges = mofifiedCharges,
        //                        TotalCars = vehicleObj.Count,
        //                        Total_WithoutCertificate = feesObj.RegistrationCharges + ConstantValue.twoThousand,
        //                        Total = (feesObj.RegistrationFees * vehicleObj.Count) + feesObj.RegistrationCharges + partTwoFeesDto + triangleDto + mofifiedCharges,
        //                        Status = ConstantValue.Status_Pending,
        //                        CreatedDate = DateTime.Now
        //                    };
        //                    _context.Transactions.Add(tObj);
        //                }
        //            }
        //            else
        //            {
        //                //will add other operation
        //            }
        //            #endregion

        //            #region *** for 'ExtendCarLicense' FormMode (not use) ***
        //            //if (oLConfirmOrRejectVM.FormMode == ConstantValue.ECL_FM && feesObj != null)
        //            //{
        //            //    //check transaction_id already exit or not
        //            //    bool alreadyExit = await _context.Transactions.AsNoTracking().AnyAsync(x => x.Transaction_Id == vehicleObj[0].Transaction_Id);
        //            //    if (alreadyExit) //transaction_id exit (Update Function)
        //            //    {
        //            //        var tObj = await _context.Transactions.FirstOrDefaultAsync(x => x.Transaction_Id == oLConfirmOrRejectVM.TransactionId);
        //            //        if (tObj != null)
        //            //        {
        //            //            int mofifiedCharges = vehicleObj.Count >= ConstantValue.Two ? ConstantValue.thirtyThousand : ConstantValue.tenThousand;
        //            //            int partTwoFeesDto = vehicleObj.Count > feesObj.MaxCars ? (feesObj.PartTwoFees * vehicleObj.Count) : feesObj.PartTwoFees;
        //            //            int triangleDto = vehicleObj.Count > feesObj.MaxCars ? (feesObj.TriangleFees * vehicleObj.Count) : feesObj.TriangleFees;

        //            //            tObj.RegistrationFees += feesObj.RegistrationFees * vehicleObj.Count;
        //            //            tObj.RegistrationCharges += feesObj.RegistrationCharges;
        //            //            tObj.PartTwoFees += partTwoFeesDto;
        //            //            tObj.TriangleFees += triangleDto;
        //            //            tObj.ModifiedCharges += mofifiedCharges;
        //            //            tObj.TotalCars += vehicleObj.Count;
        //            //            tObj.Total_WithoutCertificate += feesObj.RegistrationCharges + ConstantValue.twoThousand;
        //            //            tObj.Total += (feesObj.RegistrationFees * vehicleObj.Count) + feesObj.RegistrationCharges + partTwoFeesDto + triangleDto + mofifiedCharges;
        //            //            tObj.UpdatedDate = DateTime.Now;
        //            //            _context.Transactions.Update(tObj);
        //            //        }
        //            //        else
        //            //        {
        //            //            Console.WriteLine("Transaction not found.");
        //            //        }
        //            //    }
        //            //    else //transaction_id doesn't exit (Add Function)
        //            //    {
        //            //        int mofifiedCharges = vehicleObj.Count >= ConstantValue.Two ? ConstantValue.thirtyThousand : ConstantValue.tenThousand;
        //            //        int partTwoFeesDto = vehicleObj.Count > feesObj.MaxCars ? (feesObj.PartTwoFees * vehicleObj.Count) : feesObj.PartTwoFees;
        //            //        int triangleDto = vehicleObj.Count > feesObj.MaxCars ? (feesObj.TriangleFees * vehicleObj.Count) : feesObj.TriangleFees;

        //            //        var tObj = new Transaction()
        //            //        {
        //            //            Transaction_Id = vehicleObj[0].Transaction_Id,
        //            //            ChalenNumber = vehicleObj[0].ChalenNumber,
        //            //            NRC_Number = vehicleObj[0].NRC_Number,
        //            //            RegistrationFees = feesObj.RegistrationFees * vehicleObj.Count,
        //            //            RegistrationCharges = feesObj.RegistrationCharges,
        //            //            CertificateFees = ConstantValue.Zero,
        //            //            PartOneFees = ConstantValue.Zero,
        //            //            PartTwoFees = partTwoFeesDto,
        //            //            TriangleFees = triangleDto,
        //            //            ModifiedCharges = mofifiedCharges,
        //            //            TotalCars = vehicleObj.Count,
        //            //            Total_WithoutCertificate = feesObj.RegistrationCharges + ConstantValue.twoThousand,
        //            //            Total = (feesObj.RegistrationFees * vehicleObj.Count) + feesObj.RegistrationCharges + partTwoFeesDto + triangleDto + mofifiedCharges,
        //            //            Status = ConstantValue.Status_Pending,
        //            //            CreatedDate = DateTime.Now
        //            //        };
        //            //        _context.Transactions.Add(tObj);
        //            //    }
        //            //}
        //            #endregion

        //            #region *** for 'Decrease Car' FormMode (not use) ***
        //            //else if (oLConfirmOrRejectVM.FormMode == ConstantValue.DecreaseCar_FM && feesObj != null)
        //            //{
        //            //    //check transaction_id already exit or not
        //            //    //bool alreadyExit = await _context.Transactions.AsNoTracking().AnyAsync(x => x.Transaction_Id == vehicleObj[0].Transaction_Id);
        //            //    if (alreadyExit) //transaction_id exit (Update Function)
        //            //    {
        //            //        var tObj = await _context.Transactions.FirstOrDefaultAsync(x => x.Transaction_Id == oLConfirmOrRejectVM.TransactionId);
        //            //        if (tObj != null)
        //            //        {
        //            //            //already check vehicleObj != null so count star from 1
        //            //            int mofifiedCharges = vehicleObj.Count >= ConstantValue.Two ? ConstantValue.thirtyThousand : ConstantValue.tenThousand;
        //            //            tObj.RegistrationCharges += ConstantValue.oneThousand;
        //            //            tObj.ModifiedCharges += mofifiedCharges;
        //            //            tObj.TotalCars += vehicleObj.Count;
        //            //            tObj.Total_WithoutCertificate += ConstantValue.oneThousand + ConstantValue.twoThousand; // oneThousand is for RegistrationCharges
        //            //            tObj.Total += ConstantValue.oneThousand + mofifiedCharges; // oneThousand is for RegistrationCharges
        //            //            tObj.UpdatedDate = DateTime.Now;
        //            //            _context.Transactions.Update(tObj);
        //            //        }
        //            //        else
        //            //        {
        //            //            Console.WriteLine("Transaction not found.");
        //            //        }
        //            //    }
        //            //    else //transaction_id doesn't exit (Add Function)
        //            //    {
        //            //        //already check vehicleObj != null so count star from 1
        //            //        int mofifiedCharges = vehicleObj.Count >= ConstantValue.Two ? ConstantValue.thirtyThousand : ConstantValue.tenThousand;
        //            //        var tObj = new Transaction()
        //            //        {
        //            //            Transaction_Id = vehicleObj[0].Transaction_Id,
        //            //            ChalenNumber = vehicleObj[0].ChalenNumber,
        //            //            NRC_Number = vehicleObj[0].NRC_Number,
        //            //            RegistrationFees = ConstantValue.Zero,
        //            //            RegistrationCharges = ConstantValue.oneThousand,
        //            //            CertificateFees = ConstantValue.Zero,
        //            //            PartOneFees = ConstantValue.Zero,
        //            //            PartTwoFees = ConstantValue.Zero,
        //            //            TriangleFees = ConstantValue.Zero,
        //            //            ModifiedCharges = mofifiedCharges,
        //            //            TotalCars = vehicleObj.Count,
        //            //            Total_WithoutCertificate = ConstantValue.oneThousand + ConstantValue.twoThousand,
        //            //            Total = ConstantValue.oneThousand + mofifiedCharges,
        //            //            Status = ConstantValue.Status_Pending,
        //            //            CreatedDate = DateTime.Now
        //            //        };
        //            //        _context.Transactions.Add(tObj);
        //            //    }
        //            //}
        //            #endregion

        //            #endregion

        //            #region ****** Handle vehilce Status (not use) *****
        //            ////vehicle's status update
        //            //foreach (var item in vehicleObj)
        //            //{
        //            //    item.Status = ConstantValue.Status_Approved;
        //            //    item.Notes = oLConfirmOrRejectVM.Remark;
        //            //    item.ApplyDate = DateTime.Now;
        //            //    item.UpdatedDate = DateTime.Now;
        //            //}
        //            //_context.Vehicles.UpdateRange(vehicleObj);
        //            #endregion
        //        }
        //        //else
        //        //{
        //        #region ****** Handle vehilce Status (not use) *****
        //        ////vehicle's status update
        //        //foreach (var item in vehicleObj)
        //        //{
        //        //    item.Status = ConstantValue.Status_Rejected;
        //        //    item.Notes = oLConfirmOrRejectVM.Remark;
        //        //    item.ApplyDate = DateTime.Now;
        //        //    item.UpdatedDate = DateTime.Now;
        //        //}
        //        //_context.Vehicles.UpdateRange(vehicleObj);
        //        #endregion
        //        //}

        //        //vehicle's status update
        //        foreach (var item in vehicleObj)
        //        {
        //            item.Status = oLConfirmOrRejectVM.ApprovedOrRejected;
        //            item.Notes = oLConfirmOrRejectVM.Remark;
        //            item.ApplyDate = DateTime.Now;
        //            item.UpdatedDate = DateTime.Now;
        //        }
        //        _context.Vehicles.UpdateRange(vehicleObj);

        //        await _context.SaveChangesAsync();
        //        return (true, null);
        //    }
        //    return (false, "Vehilce Object not found");
        //}

        #endregion
        public async Task<(bool, string?)> OperatorLicenseConfirmReject(OLConfirmOrRejectVM oLConfirmOrRejectVM)
        {
            var vehicleObj = await _context.Vehicles.AsNoTracking()
                                                    .Where(x => x.Transaction_Id == oLConfirmOrRejectVM.TransactionId &&
                                                                x.FormMode == oLConfirmOrRejectVM.FormMode)
                                                    .ToListAsync();
            if (vehicleObj != null)
            {
                //if addmin approved
                if (oLConfirmOrRejectVM.ApprovedOrRejected == ConstantValue.Status_Approved)
                {
                    // to check formmod(all changes are the same)
                    string[] changesGroupd = { "Decrease Car", "ChangeVehicleOwnerAddress", "ChangeLicenseOwnerAddress", "ChangeVehicleType", "ChangeVehicleOwnerName" };


                    //to prevent 'ExtendOperatorLicense' formMode more than one time in single day 
                    if (oLConfirmOrRejectVM.FormMode == ConstantValue.EOPL_FM)
                    {
                        var existingEntry = await _context.OperatorDetails.AsNoTracking()
                                                          .FirstOrDefaultAsync(x => x.FormMode == ConstantValue.EOPL_FM &&
                                                                                    x.ApplyDate.Date == DateTime.Now.Date); //check double ExtendOperatorLicense (we wrok with applydate so it can be wrong)
                        if (existingEntry != null)
                            return (false, "လုပ်ငန်းလိုင်စင်သက်တမ်းတိုးခြင်းသည် တစ်ရက်လျင် တစ်ကြိမ်ထက်ပို၍ လုပ်ခွင့်မပေးပါ။");
                    }

                    #region ****** Handle Operator ******
                    //add new operator
                    var operatorDetailDto = await _context.OperatorDetails
                                                          .Where(x => x.NRC == vehicleObj[0].NRC_Number &&
                                                                      x.ApplyLicenseType == vehicleObj[0].LicenseTypeId &&
                                                                      x.FormMode == ConstantValue.CreateNew_FM || x.FormMode == ConstantValue.EOPL_FM)
                                                          .OrderByDescending(x => x.ApplyDate) //to get the update one
                                                          .FirstOrDefaultAsync();
                    if (operatorDetailDto != null)
                    {
                        operatorDetailDto.OperatorId = ConstantValue.Zero;
                        operatorDetailDto.Transaction_Id = vehicleObj[0].Transaction_Id;
                        operatorDetailDto.ApplyDate = DateTime.Now;
                        //operatorDetailDto.ExpiredDate = operatorDetailDto.ExpiredDate.Value.AddYears(1);
                        operatorDetailDto.ExpiredDate = operatorDetailDto.ExpiredDate.HasValue ? operatorDetailDto.ExpiredDate.Value.AddYears(1) : DateTime.Now.AddYears(1);
                        operatorDetailDto.TotalCar = vehicleObj.Count;
                        operatorDetailDto.Notes = FormModeHelper.formModeIndexMap.ContainsKey(oLConfirmOrRejectVM.FormMode) ? FormModeHelper.formModeIndexMap[oLConfirmOrRejectVM.FormMode] : "";
                        operatorDetailDto.IsClosed = true;
                        operatorDetailDto.FormMode = oLConfirmOrRejectVM.FormMode;
                        operatorDetailDto.VehicleId = vehicleObj[0].VehicleId;
                        operatorDetailDto.UpdatedDate = DateTime.Now;
                        operatorDetailDto.CreatedBy = "Admin Name";

                        _context.OperatorDetails.Add(operatorDetailDto);
                    }
                    #endregion

                    #region ****** Handle Transaction ******
                    var feesObj = await _context.Fees.AsNoTracking()
                                               .Where(x => x.JourneyTypeId == (vehicleObj[0].LicenseNumberLong.Contains(ConstantValue.Twin) ? ConstantValue.One : ConstantValue.Two) &&
                                                           x.VehicleWeightId == vehicleObj[0].VehicleWeightId)
                                               .FirstOrDefaultAsync();

                    //check transaction_id already exit or not
                    //bool alreadyExit = await _context.Transactions.AsNoTracking().AnyAsync(x => x.Transaction_Id == vehicleObj[0].Transaction_Id);

                    //checking form Mode
                    #region *** for 'ExtendOperatorLicense' FormMode ***
                    if (oLConfirmOrRejectVM.FormMode == ConstantValue.EOPL_FM && feesObj != null)
                    {
                        //check transaction_id already exit or not
                        var tObj = await _context.Transactions.FirstOrDefaultAsync(x => x.Transaction_Id == oLConfirmOrRejectVM.TransactionId);
                        if (tObj != null)
                        {
                            tObj.RegistrationFees += vehicleObj.Count * feesObj.RegistrationFees;
                            tObj.RegistrationCharges += feesObj.RegistrationCharges;
                            tObj.CertificateFees += feesObj.CertificateFees;
                            tObj.PartOneFees += feesObj.PartOneFees;
                            tObj.PartTwoFees += feesObj.PartTwoFees; //null able (to check again)
                            tObj.TriangleFees += feesObj.TriangleFees; //null able (to check again)
                            tObj.ModifiedCharges += ConstantValue.Zero;
                            tObj.TotalCars += vehicleObj.Count;
                            tObj.Total_WithoutCertificate += feesObj.RegistrationCharges + ConstantValue.twoThousand;
                            tObj.Total += tObj.RegistrationFees + tObj.RegistrationCharges + tObj.CertificateFees + tObj.PartOneFees +
                                                            tObj.PartTwoFees + tObj.TriangleFees;
                            tObj.UpdatedDate = DateTime.Now;
                            _context.Transactions.Update(tObj);
                        }
                        else
                        {
                            var tObjN = new Transaction()
                            {
                                Transaction_Id = vehicleObj[0].Transaction_Id,
                                ChalenNumber = vehicleObj[0].ChalenNumber,
                                NRC_Number = vehicleObj[0].NRC_Number,
                                RegistrationFees = vehicleObj.Count * feesObj.RegistrationFees,
                                RegistrationCharges = feesObj.RegistrationCharges,
                                CertificateFees = feesObj.CertificateFees,
                                PartOneFees = feesObj.PartOneFees,
                                PartTwoFees = feesObj.PartTwoFees,
                                TriangleFees = feesObj.TriangleFees,
                                ModifiedCharges = ConstantValue.Zero,
                                TotalCars = vehicleObj.Count,
                                Total_WithoutCertificate = feesObj.RegistrationCharges + ConstantValue.twoThousand,
                                Total = (vehicleObj.Count * feesObj.RegistrationFees) + feesObj.RegistrationCharges + feesObj.CertificateFees + feesObj.PartOneFees +
                                                            feesObj.PartTwoFees + feesObj.TriangleFees,
                                Status = ConstantValue.Status_Pending,
                                CreatedDate = DateTime.Now
                            };
                            _context.Transactions.Add(tObjN);
                        }

                    }
                    #endregion

                    #region *** for 'Common Changes' FormMode ***
                    else if (changesGroupd.Contains(oLConfirmOrRejectVM.FormMode) && feesObj != null)
                    {
                        //check transaction_id already exit or not
                       
                        var tObj = await _context.Transactions.FirstOrDefaultAsync(x => x.Transaction_Id == oLConfirmOrRejectVM.TransactionId);
                        if (tObj != null)
                        {
                            //already check vehicleObj != null so count star from 1
                            int mofifiedCharges = vehicleObj.Count >= ConstantValue.Two ? ConstantValue.thirtyThousand : ConstantValue.tenThousand;
                            tObj.RegistrationCharges += ConstantValue.oneThousand;
                            tObj.ModifiedCharges += mofifiedCharges;
                            tObj.TotalCars += vehicleObj.Count;
                            tObj.Total_WithoutCertificate += ConstantValue.oneThousand + ConstantValue.twoThousand; // oneThousand is for RegistrationCharges
                            tObj.Total += ConstantValue.oneThousand + mofifiedCharges; // oneThousand is for RegistrationCharges
                            tObj.UpdatedDate = DateTime.Now;
                            _context.Transactions.Update(tObj);
                        }
                        else
                        {
                            //already check vehicleObj != null so count star from 1
                            int mofifiedCharges = vehicleObj.Count >= ConstantValue.Two ? ConstantValue.thirtyThousand : ConstantValue.tenThousand;
                            var tObjN = new Transaction()
                            {
                                Transaction_Id = vehicleObj[0].Transaction_Id,
                                ChalenNumber = vehicleObj[0].ChalenNumber,
                                NRC_Number = vehicleObj[0].NRC_Number,
                                RegistrationFees = ConstantValue.Zero,
                                RegistrationCharges = ConstantValue.oneThousand,
                                CertificateFees = ConstantValue.Zero,
                                PartOneFees = ConstantValue.Zero,
                                PartTwoFees = ConstantValue.Zero,
                                TriangleFees = ConstantValue.Zero,
                                ModifiedCharges = mofifiedCharges,
                                TotalCars = vehicleObj.Count,
                                Total_WithoutCertificate = ConstantValue.oneThousand + ConstantValue.twoThousand,
                                Total = ConstantValue.oneThousand + mofifiedCharges,
                                Status = ConstantValue.Status_Pending,
                                CreatedDate = DateTime.Now
                            };
                            _context.Transactions.Add(tObjN);
                        }
                    }
                    #endregion

                    #region *** for 'Add New Car' FormMode ***
                    else if ((oLConfirmOrRejectVM.FormMode == ConstantValue.AddNewCar_FM || oLConfirmOrRejectVM.FormMode == ConstantValue.ECL_FM) && feesObj != null)
                    {
                        //check transaction_id already exit or not
                        //bool alreadyExit = await _context.Transactions.AsNoTracking().AnyAsync(x => x.Transaction_Id == vehicleObj[0].Transaction_Id);
                        var tObj = await _context.Transactions.FirstOrDefaultAsync(x => x.Transaction_Id == oLConfirmOrRejectVM.TransactionId);
                        if (tObj != null)
                        {
                            int mofifiedCharges = vehicleObj.Count >= ConstantValue.Two ? ConstantValue.thirtyThousand : ConstantValue.tenThousand;
                            int partTwoFeesDto = vehicleObj.Count > feesObj.MaxCars ? (feesObj.PartTwoFees * vehicleObj.Count) : feesObj.PartTwoFees;
                            int triangleDto = vehicleObj.Count > feesObj.MaxCars ? (feesObj.TriangleFees * vehicleObj.Count) : feesObj.TriangleFees;

                            tObj.RegistrationFees += feesObj.RegistrationFees * vehicleObj.Count;
                            tObj.RegistrationCharges += feesObj.RegistrationCharges;
                            tObj.PartTwoFees += partTwoFeesDto;
                            tObj.TriangleFees += triangleDto;
                            tObj.ModifiedCharges += mofifiedCharges;
                            tObj.TotalCars += vehicleObj.Count;
                            tObj.Total_WithoutCertificate += feesObj.RegistrationCharges + ConstantValue.twoThousand;
                            tObj.Total += (feesObj.RegistrationFees * vehicleObj.Count) + feesObj.RegistrationCharges + partTwoFeesDto + triangleDto + mofifiedCharges;
                            tObj.UpdatedDate = DateTime.Now;
                            _context.Transactions.Update(tObj);
                        }
                        else
                        {
                            int mofifiedCharges = vehicleObj.Count >= ConstantValue.Two ? ConstantValue.thirtyThousand : ConstantValue.tenThousand;
                            int partTwoFeesDto = vehicleObj.Count > feesObj.MaxCars ? (feesObj.PartTwoFees * vehicleObj.Count) : feesObj.PartTwoFees;
                            int triangleDto = vehicleObj.Count > feesObj.MaxCars ? (feesObj.TriangleFees * vehicleObj.Count) : feesObj.TriangleFees;

                            var tObjN = new Transaction()
                            {
                                Transaction_Id = vehicleObj[0].Transaction_Id,
                                ChalenNumber = vehicleObj[0].ChalenNumber,
                                NRC_Number = vehicleObj[0].NRC_Number,
                                RegistrationFees = feesObj.RegistrationFees * vehicleObj.Count,
                                RegistrationCharges = feesObj.RegistrationCharges,
                                CertificateFees = ConstantValue.Zero,
                                PartOneFees = ConstantValue.Zero,
                                PartTwoFees = partTwoFeesDto,
                                TriangleFees = triangleDto,
                                ModifiedCharges = mofifiedCharges,
                                TotalCars = vehicleObj.Count,
                                Total_WithoutCertificate = feesObj.RegistrationCharges + ConstantValue.twoThousand,
                                Total = (feesObj.RegistrationFees * vehicleObj.Count) + feesObj.RegistrationCharges + partTwoFeesDto + triangleDto + mofifiedCharges,
                                Status = ConstantValue.Status_Pending,
                                CreatedDate = DateTime.Now
                            };
                            _context.Transactions.Add(tObjN);
                        }
                    }
                    else
                    {
                        //will add other operation
                    }
                    #endregion

                    #endregion
                }
                //vehicle's status update
                foreach (var item in vehicleObj)
                {
                    item.Status = oLConfirmOrRejectVM.ApprovedOrRejected;
                    item.Notes = oLConfirmOrRejectVM.Remark;
                    item.ApplyDate = DateTime.Now;
                    item.UpdatedDate = DateTime.Now;
                }
                _context.Vehicles.UpdateRange(vehicleObj);

                await _context.SaveChangesAsync();
                return (true, null);
            }
            return (false, "Vehilce Object not found");
        }

        #region without fromMode (not use)
        //public async Task<bool> OperatorLicenseConfirmReject(OLConfirmOrRejectVM oLConfirmOrRejectVM)
        //{

        //    var vehicleObj = await _context.Vehicles.Where(x => x.Transaction_Id == oLConfirmOrRejectVM.TransactionId).ToListAsync();
        //    if (vehicleObj != null)
        //    {
        //        //vehicle's status update
        //        foreach (var item in vehicleObj)
        //        {
        //            item.Status = oLConfirmOrRejectVM.ApprovedOrRejected;
        //            item.Notes = oLConfirmOrRejectVM.Remark;
        //            item.ApplyDate = DateTime.Now.Date;
        //            item.UpdatedDate = DateTime.Now.Date;
        //        }
        //        _context.Vehicles.UpdateRange(vehicleObj);
        //        await _context.SaveChangesAsync();

        //        //if addmin approved
        //        if (oLConfirmOrRejectVM.ApprovedOrRejected == ConstantValue.Status_Approved)
        //        {
        //            //add new operator
        //            var operatorDetailDto = await _context.OperatorDetails
        //                                                  .Where(x => x.NRC == vehicleObj[0].NRC_Number &&
        //                                                              x.ApplyLicenseType == vehicleObj[0].LicenseTypeId &&
        //                                                              x.FormMode == ConstantValue.CreateNew_FM || x.FormMode == ConstantValue.EOPL_FM)
        //                                                  .OrderByDescending(x => x.ApplyDate)
        //                                                  .FirstOrDefaultAsync();
        //            if (operatorDetailDto != null)
        //            {
        //                operatorDetailDto.OperatorId = ConstantValue.Zero;
        //                operatorDetailDto.Transaction_Id = vehicleObj[0].Transaction_Id;
        //                operatorDetailDto.ApplyDate = DateTime.Now.Date;
        //                operatorDetailDto.ExpiredDate = operatorDetailDto.ExpiredDate.HasValue ? operatorDetailDto.ExpiredDate.Value.AddYears(1) : DateTime.Now.AddYears(1);
        //                //operatorDetailDto.ExpiredDate = DateTime.Now.Date.AddYears(1);
        //                operatorDetailDto.TotalCar = vehicleObj.Count;
        //                operatorDetailDto.Notes = "လိုင်စင်သက်တမ်းတိုး";
        //                operatorDetailDto.IsClosed = true;
        //                operatorDetailDto.FormMode = ConstantValue.EOPL_FM;
        //                operatorDetailDto.VehicleId = vehicleObj[0].VehicleId;
        //                operatorDetailDto.UpdatedDate = DateTime.Now.Date;
        //                operatorDetailDto.CreatedBy = "Admin Name";

        //                _context.OperatorDetails.Add(operatorDetailDto);
        //                await _context.SaveChangesAsync();
        //            }

        //            //add new transaction
        //            var feesObj = await _context.Fees
        //                                       .Where(x => x.JourneyTypeId == (vehicleObj[0].LicenseNumberLong.Contains(ConstantValue.Twin) ? ConstantValue.One : ConstantValue.Two) &&
        //                                                   x.VehicleWeightId == vehicleObj[0].VehicleWeightId)
        //                                       .FirstOrDefaultAsync();

        //            var transactionDto = new Transaction()
        //            {
        //                Transaction_Id = vehicleObj[0].Transaction_Id,
        //                ChalenNumber = vehicleObj[0].ChalenNumber,
        //                NRC_Number = vehicleObj[0].NRC_Number,
        //                RegistrationFees = feesObj != null ? feesObj.RegistrationFees : ConstantValue.Zero,
        //                RegistrationCharges = feesObj != null ? feesObj.RegistrationCharges : ConstantValue.Zero,
        //                TriangleFees = feesObj != null ? feesObj.TriangleFees : ConstantValue.Zero,
        //                CertificateFees = ConstantValue.twoThousand,
        //                PartOneFees = ConstantValue.twoThousand * vehicleObj.Count,
        //                PartTwoFees = ConstantValue.oneThousand * vehicleObj.Count,
        //                ModifiedCharges = ConstantValue.Zero,
        //                TotalCars = vehicleObj.Count,
        //                Total_WithoutCertificate = (feesObj != null ? feesObj.RegistrationCharges : ConstantValue.Zero) + ConstantValue.twoThousand,
        //                Total = (ConstantValue.twoThousand * vehicleObj.Count) + (ConstantValue.oneThousand * vehicleObj.Count) + (feesObj != null ? feesObj.TriangleFees : ConstantValue.Zero),
        //                Status = ConstantValue.Status_Pending
        //            };
        //            _context.Transactions.Add(transactionDto);
        //            await _context.SaveChangesAsync();
        //        }
        //        //else
        //        //{
        //        //    //admin not approved
        //        //}                                                  

        //        return true;
        //    }
        //    return false;
        //}
        #endregion

        //public async Task<bool> OperatorLicenseConfirmReject(OLConfirmOrRejectVM oLConfirmOrRejectVM)
        //{
        //    int approvedVCount = 1;
        //    int rejectedVCount = 0;
        //    //int index = 0;
        //    //string Transaction_Id = "";
        //    //if we use like that then transcation total amount could wrong sometime
        //    //confirmedVCount = oLConfirmOrRejectVM.VehiclesDto.Count(x => x.Status == ConstantValue.Status_Confirmed);
        //    //rejectedVCount = oLConfirmOrRejectVM.VehiclesDto.Count(x => x.Status == ConstantValue.Status_Rejected);

        //    //int toUpdateTransactionLNL = -1;
        //    //int toUpdateTransactionVW_Id = -1;
        //    //string toUpdateTransactionT_Id = "";
        //    var toUpdateTransactionObj = new Vehicle();

        //    foreach (var item in oLConfirmOrRejectVM.VehiclesDto)
        //    {
        //        var vehicle = _context.Vehicles.Find(item.VehicleID);
        //        if (vehicle == null) continue;

        //        if (item.Status == ConstantValue.Status_Approved)
        //        {
        //            //toUpdateTransactionLNL = toUpdateTransactionLNL == -1 ? (vehicle.LicenseNumberLong.Contains(ConstantValue.Twin) ? ConstantValue.One : ConstantValue.Two) : toUpdateTransactionLNL;
        //            //toUpdateTransactionVW_Id = toUpdateTransactionVW_Id == -1 ? vehicle.VehicleWeightId : toUpdateTransactionVW_Id;
        //            //toUpdateTransactionT_Id = vehicle.Transaction_Id;
        //            if (approvedVCount == 0) toUpdateTransactionObj = vehicle; // wanna get first car obj (said by TL)
        //            approvedVCount++;
        //        }
        //        else if (item.Status == ConstantValue.Status_Rejected) rejectedVCount++;
        //        #region for transaction (logic changed  not use)
        //        //var trnsaction =await _context.Transactions.Where(x => x.Transaction_Id == (vehicle==null? "": vehicle.Transaction_Id)).FirstOrDefaultAsync();
        //        //if (vehicle == null || trnsaction == null) continue;

        //        //if (item.Status == ConstantValue.Status_Confirmed)
        //        //{
        //        //    vehicle.ExpiryDate = vehicle.CreatedDate.AddYears(1);
        //        //    //confirmedVCount++;
        //        //    //Transaction_Id = vehicle.Transaction_Id; //maybe expire date's Trnasaction_id can have so here ...
        //        //    if (confirmedVCount == 1)
        //        //    {
        //        //        trnsaction.PartTwoFees = ConstantValue.oneThousand;
        //        //        trnsaction.TriangleFees = ConstantValue.oneThousand;
        //        //        trnsaction.Total = ConstantValue.twoThousand + ConstantValue.twoThousand;
        //        //        trnsaction.TotalCars = confirmedVCount;
        //        //        trnsaction.AccpectedAt = DateTime.Now;
        //        //        trnsaction.Status = ConstantValue.Status_Confirmed;
        //        //    }
        //        //    else
        //        //    {
        //        //        trnsaction.PartTwoFees += ConstantValue.oneThousand;
        //        //        trnsaction.TriangleFees += ConstantValue.oneThousand;
        //        //        trnsaction.Total += ConstantValue.twoThousand + ConstantValue.twoThousand;
        //        //        trnsaction.TotalCars += confirmedVCount;
        //        //        trnsaction.AccpectedAt = DateTime.Now;
        //        //        trnsaction.Status = ConstantValue.Status_Confirmed;
        //        //    }
        //        //    _context.Transactions.Update(trnsaction);
        //        //    await _context.SaveChangesAsync();
        //        //    confirmedVCount++;
        //        //}
        //        ////else rejectedVCount++;
        //        ///
        //        #endregion
        //        vehicle.Status = item.Status;
        //        _context.Vehicles.Update(vehicle);
        //        await _context.SaveChangesAsync();
        //        //index++;
        //    }

        //    //Insert new transaction
        //    var feesObj = await _context.Fees.Where(x => x.JourneyTypeId == (toUpdateTransactionObj.LicenseNumberLong.Contains(ConstantValue.Twin) ? ConstantValue.One : ConstantValue.Two)
        //                                             && x.VehicleWeightId == toUpdateTransactionObj.VehicleWeightId)
        //                                    .FirstOrDefaultAsync();

        //    var transactionDto = new Transaction()
        //    {
        //        Transaction_Id = toUpdateTransactionObj.Transaction_Id,
        //        ChalenNumber = toUpdateTransactionObj.ChalenNumber,
        //        NRC_Number = toUpdateTransactionObj.NRC_Number,
        //        RegistrationFees = feesObj != null ? feesObj.RegistrationFees : ConstantValue.Nero,
        //        RegistrationCharges = feesObj != null ? feesObj.RegistrationCharges : ConstantValue.Nero,
        //        TriangleFees = feesObj != null ? feesObj.TriangleFees : ConstantValue.Nero,
        //        CertificateFees = ConstantValue.twoThousand,
        //        PartOneFees = ConstantValue.twoThousand * approvedVCount,
        //        PartTwoFees = ConstantValue.oneThousand * approvedVCount,
        //        ModifiedCharges = ConstantValue.Nero,
        //        TotalCars = approvedVCount,
        //        Total_WithoutCertificate = (feesObj != null ? feesObj.RegistrationCharges : ConstantValue.Nero) + ConstantValue.twoThousand,
        //        Total = ConstantValue.twoThousand * approvedVCount + ConstantValue.oneThousand * approvedVCount + (feesObj != null ? feesObj.TriangleFees : ConstantValue.Nero),
        //        Status = ConstantValue.Status_Pending,
        //    };
        //    _context.Transactions.Add(transactionDto);
        //    await _context.SaveChangesAsync();


        //    //Insert new operatorDetail
        //    var operatorDetails = await _context.OperatorDetails
        //                                    //.Include(x => x.Vehicle).ThenInclude(x => x.LicenseOnly).ThenInclude(x => x.RegistrationOffice)
        //                                    //.Include(x => x.Vehicle).ThenInclude(x => x.LicenseOnly).ThenInclude(x => x.JourneyType)
        //                                    .Where(x => x.NRC == toUpdateTransactionObj.NRC_Number &&
        //                                                x.Vehicle.LicenseNumberLong == toUpdateTransactionObj.LicenseNumberLong &&
        //                                                x.FormMode == ConstantValue.EOPL_FM)
        //                                    .ToListAsync();
        //    operatorDetails = operatorDetails.OrderByDescending(x => x.ExpiredDate).ToList();

        //    var operatorDot = operatorDetails[0];
        //    if (operatorDot != null)
        //    {
        //        operatorDot.OperatorId = ConstantValue.Nero;
        //        operatorDot.ApplyDate = DateTime.Now;
        //        operatorDot.ExpiredDate = DateTime.Now.AddYears(1);
        //        operatorDot.TotalCar = approvedVCount;
        //        operatorDot.VehicleId = toUpdateTransactionObj.VehicleId;
        //        operatorDot.UpdatedDate = DateTime.Now;
        //        operatorDot.CreatedBy = "Will Update"; //needed to update

        //        _context.OperatorDetails.Add(operatorDot);
        //        await _context.SaveChangesAsync();
        //    }
        //    return true;
        //}        

        private int CountByStatus(IEnumerable<Vehicle> vehicles, string status)
        {
            return vehicles
                .Where(x => x.Status == status)
                .GroupBy(x => x.Transaction_Id)
                .Count();
        }

        public async Task<(int, int, int, List<Vehicle>)> GetVehiclListByPagination(int page, int pageSize)
        {
            var vehicles = await _context.Vehicles.AsNoTracking().ToListAsync();
            int totalCount = vehicles.Count;
            vehicles = vehicles.OrderBy(x => x.VehicleId)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .ToList();

            return (page, pageSize, totalCount, vehicles);
        }
    }
}
