using System.ComponentModel.DataAnnotations;

namespace DOTP_BE.Model
{
    public class Transaction :BaseModel
    {
        public int TransactionId { get; set; }
        public string Transaction_Id { get; set; }
        public string ChalenNumber { get; set; }
        public string NRC_Number { get; set; }
        public decimal RegistrationFees { get; set; }
        public decimal RegistrationCharges { get; set; }
        public decimal CertificateFees { get; set; }
        public decimal PartOneFees { get; set; }
        public decimal PartTwoFees { get; set; }
        public decimal TriangleFees { get; set; }
        public decimal ModifiedCharges { get; set; }
        public int TotalCars { get; set; }
        public decimal Total_WithoutCertificate { get; set; }
        public decimal Total { get; set; }
        //public bool? IsDeleted { get; set; }
        //public bool? IsAccpected { get; set; }
        //public bool? IsRejected { get; set; }
        //public bool? IsPaid   { get; set; }
        //public bool? IsPrinted  { get; set; }
        public string? Status { get; set; }
        public int? AccpectedBy { get; set; }
        public DateTime? AccpectedAt { get; set; }
        public DateTime? PrintedAt { get; set; }

    }
}
