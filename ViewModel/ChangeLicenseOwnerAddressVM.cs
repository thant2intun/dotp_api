namespace DOTP_BE.ViewModel
{
    public class ChangeLicenseOwnerAddressVM
    {
        public List<IFormFile> AttachFile_NRC { get; set; }
        public List<IFormFile> AttachFile_M10 { get; set; }
        public List<IFormFile>? AttachFile_RecommandDoc1 { get; set; }
        public List<IFormFile>? AttachFile_RecommandDoc2 { get; set; }
        public List<IFormFile> AttachFile_OperatorLicense { get; set; }
        public List<IFormFile> AttachFile_Part1 { get; set; }

        public string LicenseNumberLong { get; set; }
        public string NRC_Number { get; set; }
        public string Address { get; set; }
        public string Township_Name { get; set; }


        public List<int> vehicleIdList { get; set; }
    }
}
