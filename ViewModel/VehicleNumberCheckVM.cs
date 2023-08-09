using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System;

namespace DOTP_BE.ViewModel
{
    public class VehicleNumberCheckVM
    {
        public string? VehicleNumber { get; set; }
        public string? VehicleOwnerName { get; set; }
        public string? VehicleType { get; set; }
        public string? VehicleBrand { get; set; }
        public string NRC_Number { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? VehicleWeight { get; set; }
        public string? VehicleOwnerAddress { get; set; }
        public string? OwnerBook { get; set; }
        public string? Triangle { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
