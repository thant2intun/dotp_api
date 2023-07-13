namespace DOTP_BE.ViewModel.ReportResponses
{
    public class ReportListData
    {
        public reportFilter filter { get; set; } = new reportFilter();
        public List<reportList> reportList { get; set; } = new List<reportList>();
        public List<journeyTypeList> journeyType { get; set; } = new List<journeyTypeList>();
        public List<licenseTypeList> licenseType { get; set; } = new List<licenseTypeList>();
        public List<vehicleList> vehicleType { get; set; } = new List<vehicleList>();
    }
    public class reportList
    {
        public int no { get; set; }
        public string licenTypeLong { get; set; }
        public string licenNumber { get; set; }
        public string formMode { get; set; }
        public string journeyTypeLong { get; set; }
        public int totalCar { get; set; }
        public DateTime applyDate { get; set; }
        public DateTime permitDate { get; set; }
        public DateTime? expireDate { get; set; }
    }

    public class journeyTypeList
    {
        public int journeyTypeId { get; set; }
        public string journeyTypeShort { get; set; }
    }
    public class licenseTypeList
    {
        public int licenseTypeId { get; set; }
        public string licenseTypeLong { get; set; }
    }

    public class vehicleList
    {
        public int vehicleId { get; set; }
        public string formMode { get; set; }
    }

    public class reportFilter
    {
        public int vehicleId { get; set; }
        public int journeyTypeId { get; set; }
        public int licenseTypeId { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
    }
}
