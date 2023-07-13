using AutoMapper;
using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using DOTP_BE.ViewModel.AdminResponses;
using DOTP_BE.ViewModel.ReportResponses;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DOTP_BE.Repositories
{
    public class ReportOutRepo : IReportOut
    {
        private readonly ApplicationDbContext _context;
        private IMapper _mapper;

        public ReportOutRepo(ApplicationDbContext context, IMapper map)
        {
            _context = context;
            _mapper = map;
        }
               

        public ReportListData GetReportData(ReportListData data)
        {
            var veLst = _context.Vehicles.AsNoTracking();
            var opDLst = _context.OperatorDetails.AsNoTracking();
            var jourLst = _context.JourneyTypes.AsNoTracking();
            var lType = _context.LicenseTypes.AsNoTracking();

            var obj1 = new { vehicleId = 1, formMode = "ExtendOperatorLicense" };
            var obj2 = new { vehicleId = 2, formMode = "ExtendCarLicense" };

            var licenList = new[] { obj1, obj2 }.ToList();

            ReportListData lst = new ReportListData();

            var query = from v in veLst
                        join op in opDLst on v.VehicleId equals op.VehicleId
                        join jou in jourLst on op.JourneyType_Id equals jou.JourneyTypeId
                        join lt in lType on v.LicenseTypeId equals lt.LicenseTypeId
                        where v.Status == "Pending"
                        select new
                        {
                            VehicleId = v.VehicleId,
                            FormMode = v.FormMode,
                            LicenseTypeId = lt.LicenseTypeId,
                            LicenType = lt.LicenseTypeShort,
                            Licen_Number = v.License_Number,
                            JourneyTypeId = jou.JourneyTypeId,
                            JourneyTypeLong = jou.JourneyTypeLong,
                            JourneyTypeShort = jou.JourneyTypeShort,
                            TotalCars = op.TotalCar,
                            ApplyDate = v.ApplyDate,
                            PermitDate = v.CreatedDate,
                            ExpireDate = v.ExpiryDate,
                        };
            if(data != null)
            {
                if (data.filter.fromDate != null && data.filter.toDate != null)
                {
                    query = query.Where(x => x.ApplyDate.Date >= data.filter.fromDate.Date && x.ApplyDate.Date <= data.filter.toDate.Date);
                }

                if (data.filter.vehicleId != 0)
                {
                    if(data.filter.vehicleId == 1)
                    {
                        query = query.Where(x => x.FormMode == "ExtendOperatorLicense");
                    }
                    else if(data.filter.vehicleId == 2)
                    {
                        query = query.Where(x => x.FormMode == "ExtendCarLicense");
                    }
                    
                }

                if (data.filter.journeyTypeId != 0)
                {  
                    if(data.filter.journeyTypeId == 1)
                    {
                        query = query.Where(x => x.JourneyTypeId == data.filter.journeyTypeId);
                    }
                    else if(data.filter.journeyTypeId == 2)
                    {
                        query = query.Where(x => x.JourneyTypeId >= data.filter.journeyTypeId);
                    }
                    else
                    {
                        query = query.Where(x => x.JourneyTypeId <= data.filter.journeyTypeId);
                    }
                    
                }

                if (data.filter.licenseTypeId != 0)
                {
                    query = query.Where(x => x.LicenseTypeId == data.filter.licenseTypeId);
                }
            }

            var no = 0;
           var vmFilter = query.Select(x => new reportList
                {
                    no = no + 1,
                    formMode = x.FormMode,
                    licenTypeLong = x.LicenType,
                    licenNumber = x.Licen_Number,
                    journeyTypeLong = x.JourneyTypeLong,
                    totalCar = x.TotalCars,
                    applyDate = x.ApplyDate,
                    permitDate = x.PermitDate,
                    expireDate = (DateTime)x.ExpireDate,  // Type Expilist --- (**)
                }).ToList();


            var jouLst = jourLst.Select(x => new journeyTypeList
            {
                journeyTypeId = x.JourneyTypeId,
                journeyTypeShort = x.JourneyTypeShort,
            }).ToList();

            var licenLst = lType.Select(x => new licenseTypeList
            {
                licenseTypeId = x.LicenseTypeId,
                licenseTypeLong = x.LicenseTypeLong,
            }).ToList();

            var vehicleLst = licenList.Select(x => new vehicleList
            {
                vehicleId = x.vehicleId,
                formMode = x.formMode,
            }).ToList();

            lst.reportList = vmFilter;
            lst.journeyType = jouLst;
            lst.licenseType = licenLst;
            lst.vehicleType = vehicleLst;
            
            return lst;
        }

        public Task<DashboardData> ReportData(reportFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
