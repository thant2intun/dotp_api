using BitMiracle.LibTiff.Classic;
using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.Model;
using DOTP_BE.ViewModel;
using Microsoft.EntityFrameworkCore;

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

        //public async Task<CreateCar?> CheckVehicleNumber(string vehicleNumber)
        //{
        //    //bool found = await _context.Vehicles.AnyAsync(x => x.VehicleNumber == vehicleNumber.Replace('*', '/'));
        //    vehicleNumber = vehicleNumber.Replace("*", "/");
        //    var createCar = await _context.CreateCars.FirstOrDefaultAsync(x => x.VehicleNumber == vehicleNumber);
        //    if (createCar != null && createCar.IsDeleted == false)
        //        return null;
        //    else if (createCar != null && createCar.IsDeleted == true)
        //        return createCar;
        //    return null;
        //}

        public async Task<(bool, VehicleNumberCheckVM?)> CheckVehicleNumber(string vehicleNumber)
        {
            //bool found = await _context.Vehicles.AnyAsync(x => x.VehicleNumber == vehicleNumber.Replace('*', '/'));
            vehicleNumber = vehicleNumber.Replace("*", "/");
            var vehicleObj = await _context.Vehicles.AsNoTracking()
                .Where(x => x.VehicleNumber == vehicleNumber)
                .OrderByDescending(x => x.CreatedDate)
                .Include(x => x.CreateCar)
                .Select(x => new VehicleNumberCheckVM
                {
                    VehicleNumber = x.VehicleNumber,
                    VehicleOwnerName = x.CreateCar.VehicleOwnerName,
                    VehicleBrand = x.CreateCar.VehicleBrand,
                    VehicleType = x.CreateCar.VehicleType,
                    NRC_Number = x.NRC_Number,
                    ExpiryDate = x.ExpiryDate,
                    VehicleWeight = x.CreateCar.VehicleWeight,
                    VehicleOwnerAddress = x.CreateCar.VehicleOwnerAddress,
                    OwnerBook = x.OwnerBook,
                    Triangle = x.Triangle,
                    IsDeleted = x.IsDeleted
                })
                .FirstOrDefaultAsync(x => x.VehicleNumber == vehicleNumber);

            if (vehicleObj != null && vehicleObj.IsDeleted == false)
                return (true, null);
            else if (vehicleObj != null && vehicleObj.IsDeleted == true)
                return (true, vehicleObj);
            return (false, null);
        }

        public async Task<bool> CheckVehicleNoGoodToSave(string vehicleNumber)
        {
            return await _context.CreateCars.AsNoTracking()
                .AnyAsync(x => x.VehicleNumber == vehicleNumber && x.IsDeleted == true);
        }
    }
}
