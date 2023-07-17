using DOTP_BE.Model;
using DOTP_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace DOTP_BE.Data
{
    public class ApplicationDbContext:DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options) {}
        public DbSet<RegistrationOffice> RegistrationOffices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VehicleWeight> VehicleWeights { get; set; }
        public DbSet<NRC> NRCs { get; set; }

        //al
        public DbSet<LicenseType> LicenseTypes { get; set; }

        public DbSet<Township> Townships { get; set; }

        public DbSet<JourneyType> JourneyTypes { get; set; }

        public DbSet<MDYCars> MdyCars { get; set; }
        public DbSet<KALA_YGNCars> Kala_YgnCars { get; set; }
        public DbSet<OperatorDetail> OperatorDetails { get; set; }


        //tzt
        public DbSet<PersonInformation> PersonInformations { get; set; }
        public DbSet<VehicleWeightFee> VehicleWeightFees { get; set; }
        public DbSet<Fee> Fees { get; set; }

        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<CreateCar> CreateCars { get; set; }
        public DbSet<LicenseOnly> LicenseOnlys { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        // Adding by Mm
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            base.OnModelCreating(modelBuilder);
        }
        //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
