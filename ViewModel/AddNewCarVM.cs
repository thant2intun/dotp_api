namespace DOTP_BE.ViewModel
{
    public class AddNewCarVM
    {

        //For Create Car DB
        public List<ExtenseCarVM> extenseCarVMs { get; set; }




        // For Vehicles DB
        public List<VehicleVM> vehicleVMs { get; set; }



        // For LicenseOnly
        public List<IFormFile> AttachFile_NRC { get; set; }
        public List<IFormFile> AttachFile_M10 { get; set; }
        public List<IFormFile> AttachFile_RecommandDoc1 { get; set; }
        public List<IFormFile> AttachFile_RecommandDoc2 { get; set; }
        public List<IFormFile> AttachFile_OperatorLicense { get; set; }
        public List<IFormFile> AttachFile_Part1 { get; set; }
        public List<LicenseOnlyVM> licenseOnlyVMs { get; set; }


    }

}
