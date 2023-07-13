namespace DOTP_BE.ViewModel.AdminResponses
{
    public class ExtendLicenseDashBoardVMAdmin
    {
        public List<ExtendLicenseVMAdmin> ExtendLicenseVMAdmins { get; set; }
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int PaidCount { get; set; }
        public int RejectedCount { get; set; }
    }

    public class ExtendLicenseVMAdmin
    {
        public List<string?> FormMode { get; set; }
        public string LicenseNumberLong { get; set; }
        public string JourneyTypeLong { get; set; }
        public int TotalCar { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string TransactionId { get; set; }
        public int PendingCount { get; set; }
        public int LicenseTypeId { get; set; }
        //public int JourneyTypeId { get; set; }

    }

    //public class ExtendLicenseVMAdmin
    //{
    //    public string? FormMode { get; set; }
    //    public string LicenseNumberLong { get; set; }
    //    public string JourneyTypeLong { get; set; }
    //    public int TotalCar { get; set; }
    //    public string Status { get; set; }
    //    public DateTime CreatedDate { get; set; }
    //    public DateTime? UpdatedDate { get; set; }
    //    public DateTime? ExpireDate { get; set; }
    //    public string TransactionId { get; set; }
    //    public int PendingCount { get; set; }
    //    public int LicenseTypeId { get; set; }
    //    //public int JourneyTypeId { get; set; }

    //}
}
