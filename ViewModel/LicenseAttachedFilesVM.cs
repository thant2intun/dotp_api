namespace DOTP_BE.ViewModel
{
    public class LicenseAttachedFilesVM
    {
        public List<IFormFile> AttachFile_NRC { get; set; }
        public List<IFormFile>? AttachFile_M10 { get; set; }
        public List<IFormFile>? AttachFile_RecommandDoc1 { get; set; }
        public List<IFormFile>? AttachFile_RecommandDoc2 { get; set; }
        public List<IFormFile>? AttachFile_OperatorLicense { get; set; }
        public List<IFormFile>? AttachFile_Part1 { get; set; }
    }
}
