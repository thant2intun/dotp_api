using DOTP_BE.Model;

namespace DOTP_BE.ViewModel
{
    public class VehicleVM
    {
        public string Transaction_Id { get; set; } //string before 07/02/2023
        public string ChalenNumber { get; set; }
        public string NRC_Number { get; set; } //From LicenseOnly
        public int ApplicantId { get; set; }
        public string License_Number { get; set; } //From LicenseOnly
        public string LicenseNumberLong { get; set; }
        public string VehicleNumber { get; set; } //From CreateCar
        public string VehicleLineTitle { get; set; }
        public string CarryLogisticType { get; set; }
        public string VehicleLocation { get; set; }
        public string VehicleDesiredRoute { get; set; }
        public string? Notes { get; set; }
        public string Status { get; set; }
        public bool CertificatePrinted { get; set; }
        public bool Part1Printed { get; set; }
        public bool Part2Printed { get; set; }
        public bool TrianglePrinted { get; set; }
        public DateTime ApplyDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string? FormMode { get; set; }
        public int RefTransactionId { get; set; }
        public string? Triangle { get; set; }
        public string? OwnerBook { get; set; }
        public string? AttachedFile1 { get; set; }
        public string? AttachedFile2 { get; set; }
        public int LicenseTypeId { get; set; }
        public int VehicleWeightId { get; set; }
        public int CreateCarId { get; set; }
        public int LicenseOnlyId { get; set; }
        public string CreatedBy { get; set; }
    }
}
