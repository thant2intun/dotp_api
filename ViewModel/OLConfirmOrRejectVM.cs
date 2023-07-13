namespace DOTP_BE.ViewModel
{
    public class OLConfirmOrRejectVM
    {
        public string Remark { get; set; }
        public string TransactionId { get; set; }
        public string ApprovedOrRejected { get; set; }
        public string FormMode { get; set; }
        //public List<VehicleInfo> VehiclesDto { get; set; }

    }

    public class VehicleInfo
    {
        public int VehicleID { get; set; }
        public string Status { get; set; }
    }
}
