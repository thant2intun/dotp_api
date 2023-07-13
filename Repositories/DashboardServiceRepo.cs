using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.ViewModel.AdminResponses;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Repositories
{

    //Code By Maw MAw 
    public class DashboardServiceRepo : IDashboard
    {
        private readonly ApplicationDbContext _dbcontext;
        public DashboardServiceRepo(ApplicationDbContext context)
        {
            _dbcontext = context;
        }
        #region With Office Id mawmaw
        public async Task<DashboardData> GetDataByOffId(int offId)
        {

            DashboardData res = new DashboardData();
            var OptQuery = _dbcontext.OperatorDetails.Where(x => x.ApplyLicenseType != null).AsNoTracking().Select(r => new Models.OperatorDetail
            {
                RegistrationOffice_Id = r.RegistrationOffice_Id,
                VehicleId = r.VehicleId,
                TotalCar = r.TotalCar,
                ApplyLicenseType = r.ApplyLicenseType,
                JourneyType_Id = r.JourneyType_Id,
            });
            var VehQuery = _dbcontext.Vehicles.Where(x => x.LicenseTypeId != 0).AsNoTracking().Select(x => new Model.Vehicle
            {
                VehicleId = x.VehicleId,
                FormMode = x.FormMode,
                Status = x.Status,
                LicenseTypeId = x.LicenseTypeId,
                VehicleWeightId = x.VehicleWeightId,
            });

            var upgradeQuery = from opt in OptQuery
                               join v in VehQuery on opt.VehicleId equals v.VehicleId
                               //select new
                               //{
                               //    opt = opt,
                               //    v = v
                               //};
                               select new Model.Vehicle
                               {
                                   VehicleId = v.VehicleId,
                                   FormMode = v.FormMode,
                                   Status = v.Status,
                                   LicenseTypeId = v.LicenseTypeId,
                                   VehicleWeightId = v.VehicleWeightId,
                                   // Value of operationDetail
                                   ApplicantId = opt.JourneyType_Id,
                                   CreateCarId = opt.TotalCar,
                                   RefTransactionId = opt.RegistrationOffice_Id
                               };

            var liceOnlys = _dbcontext.LicenseOnlys.ToList(); //tzt 290523
            if (offId != 54)
            {
                List<int?> vehidlst = OptQuery.Where(x => x.RegistrationOffice_Id == offId).Select(x => x.VehicleId).ToList();
                OptQuery = OptQuery.Where(x => x.RegistrationOffice_Id == offId);
                VehQuery = VehQuery.Where(x => vehidlst.Contains(x.VehicleId));
                upgradeQuery = upgradeQuery.Where(x => x.RefTransactionId == offId);

                //  upgradeQuery = upgradeQuery.Where(x => x.opt.RegistrationOffice_Id == offId);
            }
            var data = OptQuery.ToList();
            res.Card_1Value = data.Count() == 0 ? new totalLicense() : new totalLicense()
            {
                //kaOptLicense = data.Where(x => x.ApplyLicenseType == 1).Count(),
                kaOptLicense = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("က")).Count(),
                kaVehNumber = data.Where(x => x.ApplyLicenseType == 1 || x.ApplyLicenseType == 2 || x.ApplyLicenseType == 3).Select(r => r.TotalCar).Sum(),
                //chaOptLicense = data.Where(x => x.ApplyLicenseType == 2).Count(),
                chaOptLicense = liceOnlys.Where(x => x.License_Number.StartsWith("ခ")).Count(),
                chaVehNumber = data.Where(x => x.ApplyLicenseType == 2).Select(r => r.TotalCar).Sum(),
                //gaOptLicense = data.Where(x => x.ApplyLicenseType == 3).Count(),
                gaOptLicense = liceOnlys.Where(x => x.License_Number.StartsWith("ဂ")).Count(),
                gaVehNumber = data.Where(x => x.ApplyLicenseType == 3).Select(r => r.TotalCar).Sum(),
                //ghaOptLicense = data.Where(x => x.ApplyLicenseType == 4).Count(),
                ghaOptLicense = liceOnlys.Where(x => x.License_Number.StartsWith("ဃ")).Count(),
                ghaVehNumber = data.Where(x => x.ApplyLicenseType == 4).Select(r => r.TotalCar).Sum(),
                //ngaOptLicense = data.Where(x => x.ApplyLicenseType == 5).Count(),
                ngaOptLicense = liceOnlys.Where(x => x.License_Number.StartsWith("င")).Count(),
                ngaVehNumber = data.Where(x => x.ApplyLicenseType == 5).Select(r => r.TotalCar).Sum(),
            };
            var vehdata = VehQuery.ToList();
            res.Card_2Lst = vehdata.Count() == 0 ? new List<totalRCLicense>() : new List<totalRCLicense>()
            {
                new totalRCLicense()
                {
                    name =  "လုပ်ငန်းလိုင်စင်အရေအတွက်",
                    kaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 1).Count(),
                    chaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 2).Count(),
                    gaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 3).Count(),
                    ghaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 4).Count(),
                    ngaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 5).Count(),
                },
                new totalRCLicense()
                {
                    name =  "ယာဉ်အရေအတွက်",
                    kaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 1).Count(),
                    chaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 2).Count(),
                    gaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 3).Count(),
                    ghaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 4).Count(),
                    ngaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 5).Count(),
                }
            };
            var upgradedata = upgradeQuery.Where(x => x.FormMode == "ExtendOperatorLicense").ToList();
            //var upgradedata = await upgradeQuery.GroupBy(x => x.opt.FormMode).Select(r => new Model.Vehicle
            //{
            //    FormMode = r.Key,
            //    Status = r.Max(x => x.v.Status),
            //    LicenseTypeId = r.Max(x => x.v.LicenseTypeId),
            //    VehicleWeightId = r.Max(x => x.v.VehicleWeightId),
            //    ApplicantId = r.Max(x => x.opt.JourneyType_Id),
            //    CreateCarId = r.Sum(x => x.opt.TotalCar)
            //}).ToListAsync();          

            res.Card_3Lst_1 = upgradedata.Count() == 0 ? new List<totalVal>() : new List<totalVal>()
            {
                new totalVal()
                {
                    name = "၂ တန် (သို့မဟုတ်) ၁၇ ဉီးနှင့် အထက်",
                    kaVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    kaVehVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     chaVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    chaVehVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    gaVehVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaaVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    gaaVehVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     ngaVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    ngaVehVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                },
                new totalVal()
                {
                    name = "၂ တန် အထက်မှ ၉တန် (သို့မဟုတ်) ၄၀ ဉီး",
                    kaVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    kaVehVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     chaVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    chaVehVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    gaVehVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaaVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    gaaVehVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     ngaVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    ngaVehVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                },
                new totalVal()
                {
                    name = "၉ တန် အထက်မှ ၄၁ဉီး အထက်",
                    kaVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    kaVehVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     chaVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    chaVehVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    gaVehVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaaVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    gaaVehVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     ngaVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    ngaVehVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                }
            };
            res.Card_3Lst_2 = upgradedata.Count() == 0 ? new List<totalVal>() : new List<totalVal>()
            {
                new totalVal()
                {
                    name = "၂ တန် (သို့မဟုတ်) ၁၇ ဉီးနှင့် အထက်",
                    kaVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    kaVehVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     chaVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    chaVehVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    gaVehVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaaVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    gaaVehVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     ngaVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    ngaVehVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                },
                new totalVal()
                {
                    name = "၂ တန် အထက်မှ ၉တန် (သို့မဟုတ်) ၄၀ ဉီး",
                    kaVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    kaVehVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     chaVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    chaVehVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    gaVehVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaaVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    gaaVehVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     ngaVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    ngaVehVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                },
                new totalVal()
                {
                    name = "၉ တန် အထက်မှ ၄၁ဉီး အထက်",
                    kaVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    kaVehVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     chaVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    chaVehVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    gaVehVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaaVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    gaaVehVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     ngaVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    ngaVehVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                }
            };
            var regfee = _dbcontext.Fees.Where(x => x.JourneyTypeId == 1 || x.JourneyTypeId == 2 || x.JourneyTypeId == 3).ToList();
            if (res.Card_3Lst_1.Count() > 0)
            {
                int amt_1 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 1).Select(x => x.RegistrationFees).FirstOrDefault();
                int amt_2 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 3).Select(x => x.RegistrationFees).FirstOrDefault();
                int amt_3 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 4).Select(x => x.RegistrationFees).FirstOrDefault();
                res.Card_3Lst_1.ForEach(x =>
                {
                    x.conAmt = x.name == "၂ တန် (သို့မဟုတ်) ၁၇ ဉီးနှင့် အထက်" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_1) :
                                x.name == "၂ တန် အထက်မှ ၉တန် (သို့မဟုတ်) ၄၀ ဉီး" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_2) :
                                ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_3);
                });
            }
            if (res.Card_3Lst_2.Count() > 0)
            {
                int amt_1 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 1).Select(x => x.RegistrationFees).FirstOrDefault();
                int amt_2 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 3).Select(x => x.RegistrationFees).FirstOrDefault();
                int amt_3 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 4).Select(x => x.RegistrationFees).FirstOrDefault();
                res.Card_3Lst_2.ForEach(x =>
                {
                    x.conAmt = x.name == "၂ တန် (သို့မဟုတ်) ၁၇ ဉီးနှင့် အထက်" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_1) :
                                x.name == "၂ တန် အထက်မှ ၉တန် (သို့မဟုတ်) ၄၀ ဉီး" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_2) :
                                ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_3);
                });
            }
            return res;
        }
        #endregion

        #region TZT 290523
        //GetDashboardData by OfficeId
        public async Task<TotalLicense> GetLicenseDataByOffId(int offId)
        {

            int totlLicnseKa = 0;
            int totlLicnseKha = 0;
            int totlLicnseGa = 0;
            int totlLicnseGha = 0;
            int totlLicnseNga = 0;
            //DashboardData res = new DashboardData();List<TotalLicense> totalLicenses = new List<TotalLicense>();
            if (offId != 54)
            {
                totlLicnseKa = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("က") && x.RegistrationOfficeId == offId).Count();
                totlLicnseKha = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("ခ") && x.RegistrationOfficeId == offId).Count();
                totlLicnseGa = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("ဂ") && x.RegistrationOfficeId == offId).Count();
                totlLicnseGha = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("ဃ") && x.RegistrationOfficeId == offId).Count();
                totlLicnseNga = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("င") && x.RegistrationOfficeId == offId).Count();
            }
            else
            {
                totlLicnseKa = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("က")).Count();
                totlLicnseKha = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("ခ")).Count();
                totlLicnseGa = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("ဂ")).Count();
                totlLicnseGha = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("ဃ")).Count();
                totlLicnseNga = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("င")).Count();
            }
            //opdetail
            //var op_details = _dbcontext.OperatorDetails.Where(x => x.RegistrationOffice_Id == offId);
            int[] totalCars = new int[8];
            //int [] addCars = new int[8];
            //int [] decCars = new int[8];
            for (int i = 1; i < 9; i++)
            {

                //for extend operator linc and create new
                if (offId == 54 || offId == 0)
                {
                    var opdetail = _dbcontext.OperatorDetails.
                                Where(x => (x.ApplyLicenseType == i) ||
                                (x.FormMode == "CreateNew" && x.ApplyLicenseType == i)).ToList();
                    int TotCar = 0;
                    int DCar = 0;
                    int AddCar = 0;
                    foreach (var a in opdetail)
                    {
                        TotCar = opdetail.GroupBy(a => a.NRC).
                                       Select(c => c.OrderByDescending(x => x.ApplyDate).First()).Sum(x => x.TotalCar);
                        //for Dec New
                        DCar = _dbcontext.OperatorDetails.Where(x => x.ApplyDate >= a.ApplyDate && x.FormMode == "Decrease Car").ToList().Sum(x => x.TotalCar);

                        //for Add New Car

                        AddCar = _dbcontext.OperatorDetails.Where(x => x.ApplyDate >= a.ApplyDate && x.FormMode == "Add New Car").ToList().Sum(x => x.TotalCar);
                    }
                    totalCars[i - 1] = TotCar + AddCar - DCar;

                }
                else
                {

                    var opdetail = _dbcontext.OperatorDetails.
                                Where(x => (x.ApplyLicenseType == i && x.RegistrationOffice_Id == offId) ||
                                (x.FormMode == "CreateNew" && x.ApplyLicenseType == i && x.RegistrationOffice_Id == offId)).ToList();
                    int TotCar = 0;
                    int DCar = 0;
                    int AddCar = 0;
                    foreach (var a in opdetail)
                    {
                        TotCar = opdetail.GroupBy(a => a.NRC).
                                       Select(c => c.OrderByDescending(x => x.ApplyDate).First()).Sum(x => x.TotalCar);
                        //for Dec New
                        DCar = _dbcontext.OperatorDetails.Where(x => x.ApplyDate >= a.ApplyDate && x.FormMode == "Decrease Car" && x.RegistrationOffice_Id == offId).ToList().Sum(x => x.TotalCar);

                        //for Add New Car

                        AddCar = _dbcontext.OperatorDetails.Where(x => x.ApplyDate >= a.ApplyDate && x.FormMode == "Add New Car" && x.RegistrationOffice_Id == offId).ToList().Sum(x => x.TotalCar);
                    }
                    totalCars[i - 1] = TotCar + AddCar - DCar;
                }

            }
            int totalCarKa = (totalCars[0] + totalCars[1] + totalCars[2]);//+ (addCars[0] + addCars[1] + addCars[2]) - (decCars[0] + decCars[1] + decCars[2]);
            int totalCarKha = totalCars[3]; //+ addCars[3] - decCars[3];
            int totalCarGa = totalCars[4];//+ addCars[4] - decCars[3];
            int totalCarGha = (totalCars[5] + totalCars[6]);//+ (addCars[5]+addCars[6]) - (decCars[5]+decCars[6]);
            int totalCarNga = totalCars[7]; //+ addCars[7] - decCars[7];  

            TotalLicense totalLinc = new TotalLicense(totlLicnseKa, totlLicnseKha, totlLicnseGa, totlLicnseGha, totlLicnseNga, totalCarKa, totalCarKha, totalCarGa,
                                        totalCarGha, totalCarNga);
            return totalLinc;
        }

        public Task<TotalLicense> GetLicenseData(Filter filter)
        {
            int totlLicnseKa = 0;
            int totlLicnseKha = 0;
            int totlLicnseGa = 0;
            int totlLicnseGha = 0;
            int totlLicnseNga = 0;
            //DashboardData res = new DashboardData();List<TotalLicense> totalLicenses = new List<TotalLicense>();
            totlLicnseKa = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("က") && x.RegistrationOfficeId == filter.officeId && x.IssueDate >= filter.fromDate && x.IssueDate <= filter.toDate).Count();
            totlLicnseKha = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("ခ") && x.RegistrationOfficeId == filter.officeId && x.IssueDate >= filter.fromDate && x.IssueDate <= filter.toDate).Count();
            totlLicnseGa = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("ဂ") && x.RegistrationOfficeId == filter.officeId && x.IssueDate >= filter.fromDate && x.IssueDate <= filter.toDate).Count();
            totlLicnseGha = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("ဃ") && x.RegistrationOfficeId == filter.officeId && x.IssueDate >= filter.fromDate && x.IssueDate <= filter.toDate).Count();
            totlLicnseNga = _dbcontext.LicenseOnlys.Where(x => x.License_Number.StartsWith("င") && x.RegistrationOfficeId == filter.officeId && x.IssueDate >= filter.fromDate && x.IssueDate <= filter.toDate).Count();

            //opdetail
            //var op_details = _dbcontext.OperatorDetails.Where(x => x.RegistrationOffice_Id == offId);
            int[] totalCars = new int[8];
            int[] addCars = new int[8];
            int[] decCars = new int[8];
            for (int i = 1; i < 9; i++)
            {
                var opdetail = _dbcontext.OperatorDetails.
                                Where(x => (x.ApplyLicenseType == i && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate) ||
                                (x.FormMode == "CreateNew" && x.ApplyLicenseType == i && x.RegistrationOffice_Id == filter.officeId || x.RegistrationOffice_Id == 54 && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate)).ToList();
                if (filter.officeId != 54)
                    opdetail = opdetail.Where(op => op.RegistrationOffice_Id == filter.officeId).ToList();

                int TotCar = 0;
                int DCar = 0;
                int AddCar = 0;
                foreach (var a in opdetail)
                {
                    TotCar = opdetail.GroupBy(a => a.NRC).
                                   Select(c => c.OrderByDescending(x => x.ApplyDate).First()).Sum(x => x.TotalCar);
                    //for Dec New
                    var DCarLst = _dbcontext.OperatorDetails.Where(x => x.ApplyDate >= a.ApplyDate && x.FormMode == "Decrease Car" && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).ToList();
                    if (filter.officeId != 54)
                        DCar = DCarLst.Where(dc => dc.RegistrationOffice_Id == filter.officeId).ToList().Sum(x => x.TotalCar);
                    //for Add New Car
                    var AddCarlst = _dbcontext.OperatorDetails.Where(x => x.ApplyDate >= a.ApplyDate && x.FormMode == "Add New Car"
                    && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate
                    ).ToList();
                    if (filter.officeId != 54)
                        AddCar = AddCarlst.Where(ac => ac.RegistrationOffice_Id == filter.officeId).ToList().Sum(x => x.TotalCar);
                }
                totalCars[i - 1] = TotCar + AddCar - DCar;
            }
            int totalCarKa = (totalCars[0] + totalCars[1] + totalCars[2]) + (addCars[0] + addCars[1] + addCars[2]) - (decCars[0] + decCars[1] + decCars[2]);
            int totalCarKha = totalCars[3] + addCars[3] - decCars[3];
            int totalCarGa = totalCars[4] + addCars[4] - decCars[3];
            int totalCarGha = (totalCars[5] + totalCars[6]) + (addCars[5] + addCars[6]) - (decCars[5] + decCars[6]);
            int totalCarNga = totalCars[7] + addCars[7] - decCars[7];

            TotalLicense totalLinc = new TotalLicense(totlLicnseKa, totlLicnseKha, totlLicnseGa, totlLicnseGha, totlLicnseNga, totalCarKa, totalCarKha, totalCarGa,
                                        totalCarGha, totalCarNga);
            return Task.FromResult(totalLinc);
        }

        public List<RestLicenseToCheck> GetRestLicenseDataToCheckByOffId(int offId)
        {
            var VehQuery = (from v in _dbcontext.Vehicles
                            join lo in _dbcontext.LicenseOnlys on v.LicenseNumberLong equals lo.License_Number
                            select new
                            {
                                v.Transaction_Id,
                                v.License_Number,
                                v.LicenseTypeId,
                                v.FormMode,
                                v.Status,
                                v.ApplyDate,
                                v.ChalenNumber,
                                v.VehicleNumber,
                                lo.RegistrationOfficeId
                            }).ToList();


            if (offId != 54)
            {
                VehQuery = VehQuery.Where(x => x.RegistrationOfficeId == offId).ToList();
            }

            var myInClause = new int[] { 1, 2, 3 };
            List<RestLicenseToCheck> lstResLincToChk = VehQuery.Count() == 0 ? new List<RestLicenseToCheck>() : new List<RestLicenseToCheck>()
            {
                new RestLicenseToCheck()
                {
                    name = "လုပ်ငန်းလိုင်စင်အရေအတွက်",
                    kaReg = VehQuery.Where(x => myInClause.Contains(x.LicenseTypeId)).GroupBy(y => y.Transaction_Id).ToList().Count(),
                    kaRemain = VehQuery.Where(x => myInClause.Contains(x.LicenseTypeId) && x.Status == "Pending")
                    .GroupBy(y => y.Transaction_Id).Count(),
                    khaReg = VehQuery.Where(x => x.LicenseTypeId == 4).GroupBy(y => y.Transaction_Id).ToList().Count(),
                    khaRemain = VehQuery.Where(x => x.LicenseTypeId == 4 && x.Status == "Pending").GroupBy(y => y.Transaction_Id).ToList().Count(),
                    gaReg = VehQuery.Where(x => x.LicenseTypeId == 5).GroupBy(y => y.Transaction_Id).ToList().Count(),
                    gaRemain = VehQuery.Where(x => x.LicenseTypeId == 5 && x.Status == "Pending").GroupBy(y => y.Transaction_Id).ToList().Count(),
                    ghaReg = VehQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7)).GroupBy(y => y.Transaction_Id).ToList().Count(),
                    ghaRemain = VehQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.Status == "Pending").GroupBy(y => y.Transaction_Id).ToList().Count(),
                    ngaReg = VehQuery.Where(x => x.LicenseTypeId == 8).GroupBy(y => y.Transaction_Id).ToList().Count(),
                    ngaRemain = VehQuery.Where(x => x.LicenseTypeId == 8 && x.Status == "Pending").GroupBy(y => y.Transaction_Id).ToList().Count()
                },
                new RestLicenseToCheck()
                {
                    name = "ယာဉ်အရေအတွက်",
                    kaReg = VehQuery.Where(x => myInClause.Contains(x.LicenseTypeId)).GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    kaRemain = VehQuery.Where(x => myInClause.Contains(x.LicenseTypeId) && x.Status == "Pending").GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    khaReg = VehQuery.Where(x => x.LicenseTypeId == 4).GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    khaRemain = VehQuery.Where(x => x.LicenseTypeId == 4 && x.Status == "Pending").GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    gaReg = VehQuery.Where(x => x.LicenseTypeId == 5).GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    gaRemain = VehQuery.Where(x => x.LicenseTypeId == 5 && x.Status == "Pending").GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    ghaReg = VehQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7)).GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    ghaRemain = VehQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.Status == "Pending").GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    ngaReg = VehQuery.Where(x => x.LicenseTypeId == 8).GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    ngaRemain = VehQuery.Where(x => x.LicenseTypeId == 8 && x.Status == "Pending").GroupBy(y=>y.VehicleNumber).ToList().Count(),
                },
            };
            return lstResLincToChk;

        }

        public List<RestLicenseToCheck> GetRestLicenseDataToCheckByFilter(Filter filter)
        {
            var VehQuery = (from v in _dbcontext.Vehicles
                            join lo in _dbcontext.LicenseOnlys on v.LicenseNumberLong equals lo.License_Number
                            select new
                            {
                                v.Transaction_Id,
                                v.License_Number,
                                v.LicenseTypeId,
                                v.FormMode,
                                v.Status,
                                v.ApplyDate,
                                v.ChalenNumber,
                                v.VehicleNumber,
                                lo.RegistrationOfficeId
                            }).ToList();

            if (filter.officeId != 54)
                VehQuery = VehQuery.Where(x => x.RegistrationOfficeId == filter.officeId).ToList();
            var myInClause = new int[] { 1, 2, 3 };
            List<RestLicenseToCheck> lstResLincToChk = VehQuery.Count() == 0 ? new List<RestLicenseToCheck>() : new List<RestLicenseToCheck>()
            {
                new RestLicenseToCheck()
                {
                    name = "လုပ်ငန်းလိုင်စင်အရေအတွက်",
                    kaReg = VehQuery.Where(x => myInClause.Contains(x.LicenseTypeId) && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y => y.Transaction_Id).ToList().Count(),
                    kaRemain = VehQuery.Where(x => myInClause.Contains(x.LicenseTypeId) && x.Status == "Pending" && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate)
                    .GroupBy(y => y.Transaction_Id).Count(),
                    khaReg = VehQuery.Where(x => x.LicenseTypeId == 4 && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y => y.Transaction_Id).ToList().Count(),
                    khaRemain = VehQuery.Where(x => x.LicenseTypeId == 4 && x.Status == "Pending" && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y => y.Transaction_Id).ToList().Count(),
                    gaReg = VehQuery.Where(x => x.LicenseTypeId == 5 && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y => y.Transaction_Id).ToList().Count(),
                    gaRemain = VehQuery.Where(x => x.LicenseTypeId == 5 && x.Status == "Pending" && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y => y.Transaction_Id).ToList().Count(),
                    ghaReg = VehQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y => y.Transaction_Id).ToList().Count(),
                    ghaRemain = VehQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.Status == "Pending" && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y => y.Transaction_Id).ToList().Count(),
                    ngaReg = VehQuery.Where(x => x.LicenseTypeId == 8 && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y => y.Transaction_Id).ToList().Count(),
                    ngaRemain = VehQuery.Where(x => x.LicenseTypeId == 8 && x.Status == "Pending" && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y => y.Transaction_Id).ToList().Count()
                },
                new RestLicenseToCheck()
                {
                    name = "ယာဉ်အရေအတွက်",
                    kaReg = VehQuery.Where(x => myInClause.Contains(x.LicenseTypeId) && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    kaRemain = VehQuery.Where(x => myInClause.Contains(x.LicenseTypeId) && x.Status == "Pending" && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    khaReg = VehQuery.Where(x => x.LicenseTypeId == 4 && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y=>y.VehicleNumber ).ToList().Count(),
                    khaRemain = VehQuery.Where(x => x.LicenseTypeId == 4 && x.Status == "Pending" && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    gaReg = VehQuery.Where(x => x.LicenseTypeId == 5 && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    gaRemain = VehQuery.Where(x => x.LicenseTypeId == 5 && x.Status == "Pending" && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    ghaReg = VehQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    ghaRemain = VehQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.Status == "Pending" && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    ngaReg = VehQuery.Where(x => x.LicenseTypeId == 8 && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y=>y.VehicleNumber).ToList().Count(),
                    ngaRemain = VehQuery.Where(x => x.LicenseTypeId == 8 && x.Status == "Pending" && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).GroupBy(y=>y.VehicleNumber).ToList().Count(),
                },
            };
            return lstResLincToChk;

        }

        #region tzt_070623 For Third Card of Dashboard
        public ThirdCardData GetDataForTotalExtendValuesById(int offId)
        {
            ThirdCardData res = new ThirdCardData();
            var myVehWeigh = new int[] { 2, 3, 4 };
            var VehQuery = (from v in _dbcontext.Vehicles
                            join lo in _dbcontext.LicenseOnlys on v.LicenseNumberLong equals lo.License_Number
                            join vw in _dbcontext.VehicleWeights on v.VehicleWeightId equals vw.VehicleWeightId
                            join op in _dbcontext.OperatorDetails on v.Transaction_Id equals op.Transaction_Id
                            join jt in _dbcontext.JourneyTypes on op.JourneyType_Id equals jt.JourneyTypeId
                            select new
                            {
                                v.Transaction_Id,
                                v.License_Number,
                                v.LicenseTypeId,
                                vw.VehicleType,
                                vw.VehicleWeightId,
                                jt.JourneyTypeId,
                                v.FormMode,
                                v.Status,
                                v.ApplyDate,
                                v.ChalenNumber,
                                v.VehicleNumber,
                                lo.RegistrationOfficeId,

                            }).Where(x => myVehWeigh.Contains(x.VehicleWeightId)).ToList();
            if (offId != 54)
            {
                VehQuery = VehQuery.Where(x => x.RegistrationOfficeId == offId).ToList();
            }
            var VehExtOpQuery = VehQuery.Where(x => x.FormMode == "ExtendOperatorLicense").ToList();
            var VehExtCarQuery = VehQuery.Where(x => x.FormMode == "ExtendCarLicense").ToList();
            var myInClause = new int[] { 1, 2, 3 };
            var fees = _dbcontext.Fees.ToList();
            List<totalVal> lstTotalVal = new List<totalVal>();

            res.Card_3Lst_1 = VehExtOpQuery.Count() == 0 ? new List<totalVal>() : new List<totalVal>()
            {
                new totalVal()
                {
                    name = "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ",
                    kaVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId) && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId)  && x.VehicleWeightId == 2 && x.JourneyTypeId == 1 ).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1 ).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtOpQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtOpQuery.Where(x =>(x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ",
                    kaVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId) && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId)  && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtOpQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7 ) && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtOpQuery.Where(x =>(x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၉ တန်အထက် (သို့မဟုတ်) ၄၁ ဦး အထက်",
                    kaVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId) && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId)  && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtOpQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7 && x.JourneyTypeId == 1) && x.VehicleWeightId == 4 ).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtOpQuery.Where(x =>(x.LicenseTypeId == 6 || x.LicenseTypeId == 7 && x.JourneyTypeId == 1) && x.VehicleWeightId == 4 ).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                }
            };
            res.Card_3Lst_2 = VehExtOpQuery.Count() == 0 ? new List<totalVal>() : new List<totalVal>()
            {
                new totalVal()
                {
                    name = "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ",
                    kaVal = VehExtOpQuery.Where(x =>myInClause.Contains(x.LicenseTypeId) && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId)  && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtOpQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)) && x.VehicleWeightId == 2 ).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtOpQuery.Where(x =>(x.LicenseTypeId == 6 || x.LicenseTypeId == 7 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)) && x.VehicleWeightId == 2 ).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ",
                    kaVal = VehExtOpQuery.Where(x =>myInClause.Contains(x.LicenseTypeId) && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId)  && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    chaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtOpQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7 )&& (x.JourneyTypeId == 2 || x.JourneyTypeId == 3) && x.VehicleWeightId == 3 ).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtOpQuery.Where(x =>(x.LicenseTypeId == 6 || x.LicenseTypeId == 7 ) && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3) && x.VehicleWeightId == 3 ).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၉ တန်အထက် (သို့မဟုတ်) ၄၁ ဦး အထက်",
                    kaVal = VehExtOpQuery.Where(x =>myInClause.Contains(x.LicenseTypeId) && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId)  && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    chaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    gaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    gaaVal = VehExtOpQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtOpQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    ngaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 &&  x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                }
            };


            res.Card_3Lst_3 = VehExtCarQuery.Count() == 0 ? new List<totalVal>() : new List<totalVal>()
            {
                new totalVal()
                {
                    name = "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ",
                    kaVal = VehExtCarQuery.Where(x =>  myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtCarQuery.Where(x =>myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtCarQuery.Where(x =>(x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 1 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ",
                    kaVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId )  && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၉ တန်အထက် (သို့မဟုတ်) ၄၁ ဦး အထက်",
                    kaVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId )  && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                }
            };
            res.Card_3Lst_4 = VehExtCarQuery.Count() == 0 ? new List<totalVal>() : new List<totalVal>()
            {
                new totalVal()
                {
                    name = "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ",
                    kaVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(g=>g.Transaction_Id).ToList().Count(),
                    kaVehVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    chaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    gaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    gaaVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    ngaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ",
                    kaVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId )  && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၉ တန်အထက် (သို့မဟုတ်) ၄၁ ဦး အထက်",
                    kaVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId )  && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                }
            };
            var regfee = _dbcontext.Fees.Where(x => x.JourneyTypeId == 1 || x.JourneyTypeId == 2 || x.JourneyTypeId == 3).ToList();
            if (res.Card_3Lst_1.Count() > 0)
            {
                int amt_1 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 2).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_2 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 3).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_3 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 4).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                res.Card_3Lst_1.ForEach(x =>
                {
                    x.conAmt = x.name == "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_1) :
                                x.name == "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_2) :
                                ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_3);
                });
            }
            if (res.Card_3Lst_2.Count() > 0)
            {
                int amt_1 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 2).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_2 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 3).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_3 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 4).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                res.Card_3Lst_2.ForEach(x =>
                {
                    x.conAmt = x.name == "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_1) :
                                x.name == "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_2) :
                                ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_3);
                });
            }
            if (res.Card_3Lst_3.Count() > 0)
            {
                int amt_1 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 2).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_2 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 3).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_3 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 4).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                res.Card_3Lst_3.ForEach(x =>
                {
                    x.conAmt = x.name == "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ" ? ((Convert.ToInt32(x.kaVehVal) + Convert.ToInt32(x.chaVehVal) + Convert.ToInt32(x.gaVehVal) + Convert.ToInt32(x.gaaVehVal) + Convert.ToInt32(x.ngaVehVal)) * amt_1) :
                                x.name == "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVehVal) + Convert.ToInt32(x.gaVehVal) + Convert.ToInt32(x.gaaVehVal) + Convert.ToInt32(x.ngaVehVal)) * amt_2) :
                                ((Convert.ToInt32(x.kaVehVal) + Convert.ToInt32(x.chaVehVal) + Convert.ToInt32(x.gaVehVal) + Convert.ToInt32(x.gaaVehVal) + Convert.ToInt32(x.ngaVehVal)) * amt_3);
                });
            }
            if (res.Card_3Lst_4.Count() > 0)
            {
                int amt_1 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 2).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_2 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 3).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_3 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 4).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                res.Card_3Lst_4.ForEach(x =>
                {
                    x.conAmt = x.name == "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ" ? ((Convert.ToInt32(x.kaVehVal) + Convert.ToInt32(x.chaVehVal) + Convert.ToInt32(x.gaVehVal) + Convert.ToInt32(x.gaaVehVal) + Convert.ToInt32(x.ngaVehVal)) * amt_1) :
                                x.name == "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ" ? ((Convert.ToInt32(x.kaVehVal) + Convert.ToInt32(x.chaVehVal) + Convert.ToInt32(x.gaVehVal) + Convert.ToInt32(x.gaaVehVal) + Convert.ToInt32(x.ngaVehVal)) * amt_2) :
                                ((Convert.ToInt32(x.kaVehVal) + Convert.ToInt32(x.chaVehVal) + Convert.ToInt32(x.gaVehVal) + Convert.ToInt32(x.gaaVehVal) + Convert.ToInt32(x.ngaVehVal)) * amt_3);
                });
            }

            return res;
        }

        public ThirdCardData GetDataForTotalExtendValuesByFilter(Filter filter)
        {
            ThirdCardData res = new ThirdCardData();
            var myVehWeigh = new int[] { 2, 3, 4 };
            var VehQuery = (from v in _dbcontext.Vehicles
                            join lo in _dbcontext.LicenseOnlys on v.LicenseNumberLong equals lo.License_Number
                            join vw in _dbcontext.VehicleWeights on v.VehicleWeightId equals vw.VehicleWeightId
                            join op in _dbcontext.OperatorDetails on v.Transaction_Id equals op.Transaction_Id
                            join jt in _dbcontext.JourneyTypes on op.JourneyType_Id equals jt.JourneyTypeId
                            select new
                            {
                                v.Transaction_Id,
                                v.License_Number,
                                v.LicenseTypeId,
                                vw.VehicleType,
                                vw.VehicleWeightId,
                                jt.JourneyTypeId,
                                v.FormMode,
                                v.Status,
                                v.ApplyDate,
                                v.ChalenNumber,
                                v.VehicleNumber,
                                lo.RegistrationOfficeId,

                            }).Where(x => myVehWeigh.Contains(x.VehicleWeightId) && x.ApplyDate >= filter.fromDate && x.ApplyDate <= filter.toDate).ToList();
            if (filter.officeId != 54)
            {
                VehQuery = VehQuery.Where(x => x.RegistrationOfficeId == filter.officeId).ToList();
            }
            var VehExtOpQuery = VehQuery.Where(x => x.FormMode == "ExtendOperatorLicense").ToList();
            var VehExtCarQuery = VehQuery.Where(x => x.FormMode == "ExtendCarLicense").ToList();
            var myInClause = new int[] { 1, 2, 3 };
            var fees = _dbcontext.Fees.ToList();
            List<totalVal> lstTotalVal = new List<totalVal>();

            res.Card_3Lst_1 = VehExtOpQuery.Count() == 0 ? new List<totalVal>() : new List<totalVal>()
            {
                new totalVal()
                {
                    name = "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ",
                    kaVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId) && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId)  && x.VehicleWeightId == 2 && x.JourneyTypeId == 1 ).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1 ).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtOpQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtOpQuery.Where(x =>(x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ",
                    kaVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId) && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId)  && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtOpQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7 ) && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtOpQuery.Where(x =>(x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၉ တန်အထက် (သို့မဟုတ်) ၄၁ ဦး အထက်",
                    kaVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId) && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId)  && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtOpQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7 && x.JourneyTypeId == 1) && x.VehicleWeightId == 4 ).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtOpQuery.Where(x =>(x.LicenseTypeId == 6 || x.LicenseTypeId == 7 && x.JourneyTypeId == 1) && x.VehicleWeightId == 4 ).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                }
            };
            res.Card_3Lst_2 = VehExtOpQuery.Count() == 0 ? new List<totalVal>() : new List<totalVal>()
            {
                new totalVal()
                {
                    name = "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ",
                    kaVal = VehExtOpQuery.Where(x =>myInClause.Contains(x.LicenseTypeId) && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId)  && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtOpQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)) && x.VehicleWeightId == 2 ).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtOpQuery.Where(x =>(x.LicenseTypeId == 6 || x.LicenseTypeId == 7 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)) && x.VehicleWeightId == 2 ).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ",
                    kaVal = VehExtOpQuery.Where(x =>myInClause.Contains(x.LicenseTypeId) && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId)  && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    chaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtOpQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7 )&& (x.JourneyTypeId == 2 || x.JourneyTypeId == 3) && x.VehicleWeightId == 3 ).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtOpQuery.Where(x =>(x.LicenseTypeId == 6 || x.LicenseTypeId == 7 ) && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3) && x.VehicleWeightId == 3 ).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၉ တန်အထက် (သို့မဟုတ်) ၄၁ ဦး အထက်",
                    kaVal = VehExtOpQuery.Where(x =>myInClause.Contains(x.LicenseTypeId) && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtOpQuery.Where(x => myInClause.Contains(x.LicenseTypeId)  && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    chaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    gaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    gaaVal = VehExtOpQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtOpQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    ngaVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtOpQuery.Where(x => x.LicenseTypeId == 5 &&  x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                }
            };


            res.Card_3Lst_3 = VehExtCarQuery.Count() == 0 ? new List<totalVal>() : new List<totalVal>()
            {
                new totalVal()
                {
                    name = "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ",
                    kaVal = VehExtCarQuery.Where(x =>  myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtCarQuery.Where(x =>myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtCarQuery.Where(x =>(x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 1 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 2 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ",
                    kaVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId )  && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 3 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၉ တန်အထက် (သို့မဟုတ်) ၄၁ ဦး အထက်",
                    kaVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId )  && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 4 && x.JourneyTypeId == 1).GroupBy(x => x.VehicleNumber).Count(),
                }
            };
            res.Card_3Lst_4 = VehExtCarQuery.Count() == 0 ? new List<totalVal>() : new List<totalVal>()
            {
                new totalVal()
                {
                    name = "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ",
                    kaVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(g=>g.Transaction_Id).ToList().Count(),
                    kaVehVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    chaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    gaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    gaaVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                    ngaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 2 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ",
                    kaVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId )  && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 3 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                },
                new totalVal()
                {
                    name = "၉ တန်အထက် (သို့မဟုတ်) ၄၁ ဦး အထက်",
                    kaVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId ) && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    kaVehVal = VehExtCarQuery.Where(x => myInClause.Contains(x.LicenseTypeId )  && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     chaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    chaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 4 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 5 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     gaaVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    gaaVehVal = VehExtCarQuery.Where(x => (x.LicenseTypeId == 6 || x.LicenseTypeId == 7) && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                     ngaVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 && x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.Transaction_Id).Count(),
                    ngaVehVal = VehExtCarQuery.Where(x => x.LicenseTypeId == 8 &&  x.VehicleWeightId == 4 && (x.JourneyTypeId == 2 || x.JourneyTypeId == 3)).GroupBy(x => x.VehicleNumber).Count(),
                }
            };
            var regfee = _dbcontext.Fees.Where(x => x.JourneyTypeId == 1 || x.JourneyTypeId == 2 || x.JourneyTypeId == 3).ToList();
            if (res.Card_3Lst_1.Count() > 0)
            {
                int amt_1 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 2).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_2 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 3).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_3 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 4).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                res.Card_3Lst_1.ForEach(x =>
                {
                    x.conAmt = x.name == "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_1) :
                                x.name == "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_2) :
                                ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_3);
                });
            }
            if (res.Card_3Lst_2.Count() > 0)
            {
                int amt_1 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 2).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_2 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 3).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_3 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 4).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                res.Card_3Lst_2.ForEach(x =>
                {
                    x.conAmt = x.name == "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_1) :
                                x.name == "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_2) :
                                ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_3);
                });
            }
            if (res.Card_3Lst_3.Count() > 0)
            {
                int amt_1 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 2).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_2 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 3).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_3 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 4).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                res.Card_3Lst_3.ForEach(x =>
                {
                    x.conAmt = x.name == "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ" ? ((Convert.ToInt32(x.kaVehVal) + Convert.ToInt32(x.chaVehVal) + Convert.ToInt32(x.gaVehVal) + Convert.ToInt32(x.gaaVehVal) + Convert.ToInt32(x.ngaVehVal)) * amt_1) :
                                x.name == "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVehVal) + Convert.ToInt32(x.gaVehVal) + Convert.ToInt32(x.gaaVehVal) + Convert.ToInt32(x.ngaVehVal)) * amt_2) :
                                ((Convert.ToInt32(x.kaVehVal) + Convert.ToInt32(x.chaVehVal) + Convert.ToInt32(x.gaVehVal) + Convert.ToInt32(x.gaaVehVal) + Convert.ToInt32(x.ngaVehVal)) * amt_3);
                });
            }
            if (res.Card_3Lst_4.Count() > 0)
            {
                int amt_1 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 2).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_2 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 3).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                int amt_3 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 4).Select(x => x.RegistrationFees + x.RegistrationCharges).FirstOrDefault();
                res.Card_3Lst_4.ForEach(x =>
                {
                    x.conAmt = x.name == "၂ တန်ထိ (သို့မဟုတ်) ၁၇ ဦးထိ" ? ((Convert.ToInt32(x.kaVehVal) + Convert.ToInt32(x.chaVehVal) + Convert.ToInt32(x.gaVehVal) + Convert.ToInt32(x.gaaVehVal) + Convert.ToInt32(x.ngaVehVal)) * amt_1) :
                                x.name == "၂ တန်အထက်မှ ၉ တန်အထိ (သို့မဟုတ်) ၄၀ ဦး အထိ" ? ((Convert.ToInt32(x.kaVehVal) + Convert.ToInt32(x.chaVehVal) + Convert.ToInt32(x.gaVehVal) + Convert.ToInt32(x.gaaVehVal) + Convert.ToInt32(x.ngaVehVal)) * amt_2) :
                                ((Convert.ToInt32(x.kaVehVal) + Convert.ToInt32(x.chaVehVal) + Convert.ToInt32(x.gaVehVal) + Convert.ToInt32(x.gaaVehVal) + Convert.ToInt32(x.ngaVehVal)) * amt_3);
                });
            }
            return res;
        }
        #endregion tzt_070623 For Third Card of Dashboard

        #endregion

        // With Search Filter
        public async Task<DashboardData> GetDataByOffId(DashboardData m)
        {
            int offId = m.filter.officeId;
            DashboardData res = new DashboardData();
            var OptQuery = _dbcontext.OperatorDetails.Where(x => x.ApplyLicenseType != null).AsNoTracking().Select(r => new Models.OperatorDetail
            {
                RegistrationOffice_Id = r.RegistrationOffice_Id,
                VehicleId = r.VehicleId,
                TotalCar = r.TotalCar,
                ApplyLicenseType = r.ApplyLicenseType,
                JourneyType_Id = r.JourneyType_Id,
            });
            var VehQuery = _dbcontext.Vehicles.Where(x => x.LicenseTypeId != 0).AsNoTracking().Select(x => new Model.Vehicle
            {
                VehicleId = x.VehicleId,
                FormMode = x.FormMode,
                Status = x.Status,
                LicenseTypeId = x.LicenseTypeId,
                VehicleWeightId = x.VehicleWeightId,
            });
            var upgradeQuery = from opt in OptQuery
                               join v in VehQuery on opt.VehicleId equals v.VehicleId
                               //select new
                               //{
                               //    opt = opt,
                               //    v = v
                               //};
                               select new Model.Vehicle
                               {
                                   VehicleId = v.VehicleId,
                                   FormMode = v.FormMode,
                                   Status = v.Status,
                                   LicenseTypeId = v.LicenseTypeId,
                                   VehicleWeightId = v.VehicleWeightId,
                                   // Value of operationDetail
                                   ApplicantId = opt.JourneyType_Id,
                                   CreateCarId = opt.TotalCar,
                                   RefTransactionId = opt.RegistrationOffice_Id
                               };
            if (offId != 54)
            {
                List<int?> vehidlst = OptQuery.Where(x => x.RegistrationOffice_Id == offId).Select(x => x.VehicleId).ToList();
                OptQuery = OptQuery.Where(x => x.RegistrationOffice_Id == offId);
                VehQuery = VehQuery.Where(x => vehidlst.Contains(x.VehicleId));
                upgradeQuery = upgradeQuery.Where(x => x.RefTransactionId == offId);
                //  upgradeQuery = upgradeQuery.Where(x => x.opt.RegistrationOffice_Id == offId);
            }
            var data = OptQuery.ToList();
            res.Card_1Value = data.Count() == 0 ? new totalLicense() : new totalLicense()
            {
                kaOptLicense = data.Where(x => x.ApplyLicenseType == 1).Count(),
                kaVehNumber = data.Where(x => x.ApplyLicenseType == 1).Select(r => r.TotalCar).Sum(),
                chaOptLicense = data.Where(x => x.ApplyLicenseType == 2).Count(),
                chaVehNumber = data.Where(x => x.ApplyLicenseType == 2).Select(r => r.TotalCar).Sum(),
                gaOptLicense = data.Where(x => x.ApplyLicenseType == 3).Count(),
                gaVehNumber = data.Where(x => x.ApplyLicenseType == 3).Select(r => r.TotalCar).Sum(),
                ghaOptLicense = data.Where(x => x.ApplyLicenseType == 4).Count(),
                ghaVehNumber = data.Where(x => x.ApplyLicenseType == 4).Select(r => r.TotalCar).Sum(),
                ngaOptLicense = data.Where(x => x.ApplyLicenseType == 5).Count(),
                ngaVehNumber = data.Where(x => x.ApplyLicenseType == 5).Select(r => r.TotalCar).Sum(),
            };
            var vehdata = VehQuery.ToList();
            res.Card_2Lst = new List<totalRCLicense>()
            {
                new totalRCLicense()
                {
                    name =  "လုပ်ငန်းလိုင်စင်အရေအတွက်",
                    kaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 1).Count(),
                    chaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 2).Count(),
                    gaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 3).Count(),
                    ghaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 4).Count(),
                    ngaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 5).Count(),
                },
                new totalRCLicense()
                {
                    name =  "ယာဉ်အရေအတွက်",
                    kaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 1).Count(),
                    chaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 2).Count(),
                    gaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 3).Count(),
                    ghaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 4).Count(),
                    ngaReg = vehdata.Where(x => x.Status == "Pending" && x.LicenseTypeId == 5).Count(),
                }
            };
            var upgradedata = upgradeQuery.Where(x => x.FormMode == "ExtendOperatorLicense").ToList();
            //var upgradedata = await upgradeQuery.GroupBy(x => x.opt.FormMode).Select(r => new Model.Vehicle
            //{
            //    FormMode = r.Key,
            //    Status = r.Max(x => x.v.Status),
            //    LicenseTypeId = r.Max(x => x.v.LicenseTypeId),
            //    VehicleWeightId = r.Max(x => x.v.VehicleWeightId),
            //    ApplicantId = r.Max(x => x.opt.JourneyType_Id),
            //    CreateCarId = r.Sum(x => x.opt.TotalCar)
            //}).ToListAsync();          

            //res.Card_3Lst_1 = upgradedata.Count() == 0 ? new List<totalVal>() : new List<totalVal>()
            res.Card_3Lst_1 = new List<totalVal>()
            {
                new totalVal()
                {
                    name = "၂ တန် (သို့မဟုတ်) ၁၇ ဉီးနှင့် အထက်",
                    kaVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    kaVehVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     chaVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    chaVehVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    gaVehVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaaVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    gaaVehVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     ngaVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    ngaVehVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 1 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                },
                new totalVal()
                {
                    name = "၂ တန် အထက်မှ ၉တန် (သို့မဟုတ်) ၄၀ ဉီး",
                    kaVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    kaVehVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     chaVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    chaVehVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    gaVehVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaaVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    gaaVehVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     ngaVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    ngaVehVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 1 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                },
                new totalVal()
                {
                    name = "၉ တန် အထက်မှ ၄၁ဉီး အထက်",
                    kaVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    kaVehVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     chaVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    chaVehVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    gaVehVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaaVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    gaaVehVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     ngaVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    ngaVehVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 1 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                }
            };
            //res.Card_3Lst_2 = upgradedata.Count() == 0 ? new List<totalVal>() : new List<totalVal>()
            res.Card_3Lst_2 = new List<totalVal>()
            {
                new totalVal()
                {
                    name = "၂ တန် (သို့မဟုတ်) ၁၇ ဉီးနှင့် အထက်",
                    kaVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    kaVehVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     chaVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    chaVehVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    gaVehVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaaVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    gaaVehVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     ngaVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Count(),
                    ngaVehVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 2 && x.VehicleWeightId == 1 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                },
                new totalVal()
                {
                    name = "၂ တန် အထက်မှ ၉တန် (သို့မဟုတ်) ၄၀ ဉီး",
                    kaVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    kaVehVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     chaVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    chaVehVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    gaVehVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaaVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    gaaVehVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     ngaVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Count(),
                    ngaVehVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 2 && x.VehicleWeightId == 3 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                },
                new totalVal()
                {
                    name = "၉ တန် အထက်မှ ၄၁ဉီး အထက်",
                    kaVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    kaVehVal = upgradedata.Where(x => x.LicenseTypeId == 1 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     chaVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    chaVehVal = upgradedata.Where(x => x.LicenseTypeId == 2 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    gaVehVal = upgradedata.Where(x => x.LicenseTypeId == 3 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     gaaVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    gaaVehVal = upgradedata.Where(x => x.LicenseTypeId == 4 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                     ngaVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Count(),
                    ngaVehVal = upgradedata.Where(x => x.LicenseTypeId == 5 && x.ApplicantId == 2 && x.VehicleWeightId == 4 && x.Status == "Pending").Select(x => x.CreateCarId).Sum(),
                }
            };
            var regfee = _dbcontext.Fees.Where(x => x.JourneyTypeId == 1 || x.JourneyTypeId == 2 || x.JourneyTypeId == 3).ToList();
            if (res.Card_3Lst_1.Count() > 0)
            {
                int amt_1 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 1).Select(x => x.RegistrationFees).FirstOrDefault();
                int amt_2 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 3).Select(x => x.RegistrationFees).FirstOrDefault();
                int amt_3 = regfee.Where(x => x.JourneyTypeId == 1 && x.VehicleWeightId == 4).Select(x => x.RegistrationFees).FirstOrDefault();
                res.Card_3Lst_1.ForEach(x =>
                {
                    x.conAmt = x.name == "၂ တန် (သို့မဟုတ်) ၁၇ ဉီးနှင့် အထက်" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_1) :
                                x.name == "၂ တန် အထက်မှ ၉တန် (သို့မဟုတ်) ၄၀ ဉီး" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_2) :
                                ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_3);
                });
            }
            if (res.Card_3Lst_2.Count() > 0)
            {
                int amt_1 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 1).Select(x => x.RegistrationFees).FirstOrDefault();
                int amt_2 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 3).Select(x => x.RegistrationFees).FirstOrDefault();
                int amt_3 = regfee.Where(x => x.JourneyTypeId == 2 && x.VehicleWeightId == 4).Select(x => x.RegistrationFees).FirstOrDefault();
                res.Card_3Lst_2.ForEach(x =>
                {
                    x.conAmt = x.name == "၂ တန် (သို့မဟုတ်) ၁၇ ဉီးနှင့် အထက်" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_1) :
                                x.name == "၂ တန် အထက်မှ ၉တန် (သို့မဟုတ်) ၄၀ ဉီး" ? ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_2) :
                                ((Convert.ToInt32(x.kaVal) + Convert.ToInt32(x.chaVal) + Convert.ToInt32(x.gaVal) + Convert.ToInt32(x.gaaVal) + Convert.ToInt32(x.ngaVal)) * amt_3);
                });
            }
            res.filter = m.filter;
            return res;
        }
    }
}
