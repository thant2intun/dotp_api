using DOTP_BE.Models;
using System.ComponentModel.DataAnnotations;
namespace DOTP_BE.Model
{
    public class Vehicle : BaseModel
    {
        [Key]
        public int VehicleId { get; set; }
        public string Transaction_Id { get; set; } //string before 07/02/2023
        public string ChalenNumber { get; set; }
        public string NRC_Number { get; set; } //From LicenseOnly
        public int ApplicantId { get; set; }
        public string License_Number { get; set; } //From LicenseOnly
        public string LicenseNumberLong { get; set; }
        public string? VehicleNumber { get; set; } //From CreateCar
        public string? VehicleLineTitle { get; set; }
        public string? CarryLogisticType { get; set; }
        public string? VehicleLocation { get; set; }
        public string VehicleDesiredRoute { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; }
        public bool CertificatePrinted { get; set; }
        public bool Part1Printed { get; set; }
        public bool Part2Printed { get; set; }
        public bool TrianglePrinted { get; set; }
        public DateTime ApplyDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool? IsCurrent { get; set; }
        public bool? IsDeleted { get; set; }
        public string? FormMode { get; set; }
        public int RefTransactionId { get; set; }
        public string? Triangle { get; set; }
        public string? OwnerBook { get; set; }
        public string? AttachedFile1 { get; set; }
        public string? AttachedFile2 { get; set; }
        public int LicenseTypeId { get; set; }
        public LicenseType LicenseType { get; set; }
        public int VehicleWeightId { get; set; }
        public VehicleWeight VehicleWeight { get; set; }
        public int CreateCarId { get; set; }
        public CreateCar CreateCar { get; set; }
        public int? LicenseOnlyId { get; set; }
        public LicenseOnly LicenseOnly { get; set; }
        public int? OperatorId { get; set; }
        public OperatorDetail OperatorDetail { get; set; }


        //Change Vehicle Owner Address
        public string? Temp_VehicleOwnerAddress { get; set; } //temp
        public string? Temp_Township_Name { get; set; } //temp
        public string Temp_VehicleLocation { get; set; }


        //Change Vehicle Type
        public string? Temp_VehicleType { get; set; }
        public string? Temp_VehicleBrand { get; set; }
        public string? Temp_VehicleWeight { get; set; }
        public string? Temp_Triangle { get; set; }
        public string? Temp_OwnerBook { get; set; }
        public string? Temp_AttachedFile1 { get; set; }
        public string? Temp_AttachedFile2 { get; set; }

        //Change Vehicle Owner Name Change
        public string? Temp_VehicleOwnerName { get; set; }
        public string? Temp_VehicleOwnerNRC { get; set; }
    }
}
