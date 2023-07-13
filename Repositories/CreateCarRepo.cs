using DOTP_BE.Data;
using DOTP_BE.Helpers;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace DOTP_BE.Repositories
{
    public class CreateCarRepo : ICreateCar
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _iConfig;

        public CreateCarRepo(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _iConfig = config;
        }
        public async Task<List<CreateCar>> getCreateCarList()
        {
            var result = await _context.CreateCars.ToListAsync();
            return result;
        }
        public async Task<CreateCar> getCreateCarById(int id)
        {
            var createCar = await _context.CreateCars.Where(s => s.CreateCarId == id).FirstOrDefaultAsync();
            return createCar;
        }
        public bool Create(CreateCarVM createCarVM)
        {
            var createCar = new CreateCar()
            {
                VehicleNumber = createCarVM.VehicleNumber,
                VehicleBrand = createCarVM.VehicleBrand,
                VehicleType = createCarVM.VehicleType,
                //VehicleWeight = createCarVM.VehicleWeight,
                VehicleLocation = createCarVM.VehicleLocation,
                VehicleOwnerName = createCarVM.VehicleOwnerName,
                VehicleOwnerNRC = createCarVM.VehicleOwnerNRC,
                VehicleOwnerAddress = createCarVM.VehicleOwnerAddress,
                IsDeleted = false,
                CreatedBy = createCarVM.CreatedBy,
                CreatedDate = DateTime.Now,
                //PersonInformationId = createCarVM.PersonInformationId
            };
            _context.CreateCars.Add(createCar);
            _context.SaveChanges();
            return true;
        }

        public bool Update(int id, CreateCarVM createCarVM)
        {
            var createCar = _context.CreateCars.Find(id);
            if (createCar != null)
            {
                createCar.VehicleNumber = createCarVM.VehicleNumber;
                createCar.VehicleBrand = createCarVM.VehicleBrand;
                createCar.VehicleType = createCarVM.VehicleType;
                //createCar.VehicleWeight = createCarVM.VehicleWeight;
                createCar.VehicleLocation = createCarVM.VehicleLocation;
                createCar.VehicleOwnerName = createCarVM.VehicleOwnerName;
                createCar.VehicleOwnerNRC = createCarVM.VehicleOwnerNRC;
                createCar.VehicleOwnerAddress = createCarVM.VehicleOwnerAddress;
                createCar.UpdatedDate = DateTime.Now;
                //createCar.PersonInformationId = createCarVM.PersonInformationId;
                _context.CreateCars.Update(createCar);
                _context.SaveChanges();
                return true;
            };
            return false;
        }

        public bool UpdateCar(List<UpdateCreateCar> createCarVM)
        {
            //createCarVM.ForEach(data => _context.CreateCars.Find(data.createCarId));
            
            if (createCarVM != null)
            {
                foreach (var item in createCarVM)
                {
                    var createCar = _context.CreateCars.Find(item.createCarId);
                    createCar.VehicleBrand = item.VehicleBrand;
                    createCar.VehicleType = item.VehicleType;
                    createCar.VehicleWeight = item.VehicleWeight;
                    _context.CreateCars.Update(createCar);
                    _context.SaveChanges();                    
                }
                return true;
            };
            return false;
        }

        public void Delete(int id)
        {
            var createCar = _context.CreateCars.Find(id);
            if (createCar != null)
            {
                _context.CreateCars.Remove(createCar);
                _context.SaveChanges();
            }

        }

        //public async Task<string> NewCarAttach(List<ExtenseCarVM> cars)
        //{
        //    var vehicleObj = await _context.Vehicles.ToListAsync();

        //    int tG = vehicleObj.OrderByDescending(x=>x.Transaction_Id)
        //            .Select(x=>x.Transaction_Id.Split('_').LastOrDefault())
        //            .Select(x=>int.TryParse(x,out int val)?val: int.MinValue)
        //            .FirstOrDefault();


        //    int cG = vehicleObj.OrderByDescending(x => x.ChalenNumber)
        //             .Select(x => x.ChalenNumber.Split('_').LastOrDefault())
        //             .Select(x => int.TryParse(x, out int val) ? val : int.MinValue)
        //             .FirstOrDefault();



        //    string TransactionIdN = new CommonMethod().GenerateT_IdandC_Id("T", ++tG, 9);
        //    string ChalenNumberN = new CommonMethod().GenerateT_IdandC_Id("C", ++cG, 6);




        //}

    }
}
