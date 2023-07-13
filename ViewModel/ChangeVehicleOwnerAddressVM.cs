namespace DOTP_BE.ViewModel
{
    public class ChangeVehicleOwnerAddressVM
    {
        public List<IFormFile> AttachFile_NRC { get; set; }
        public List<IFormFile> AttachFile_M10 { get; set; }
        public List<IFormFile>? AttachFile_RecommandDoc1 { get; set; }
        public List<IFormFile>? AttachFile_RecommandDoc2 { get; set; }
        public List<IFormFile> AttachFile_OperatorLicense { get; set; }
        public List<IFormFile> AttachFile_Part1 { get; set; }

        public string LicenseNumberLong { get; set; }
        public string NRC_Number { get; set; }

        public List<ChangeVOA> ChangeVOAs { get; set; }
    }
}

public class ChangeVOA
{
    public int CreateCarId { get; set; }       
    public string VehicleOwnerAddress { get; set; }
    public string? VehicleOwnerName { get; set; }
    public string? VehicleOwnerNRC { get; set; }
}
