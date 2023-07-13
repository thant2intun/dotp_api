using DOTP_BE.Model;

namespace DOTP_BE.ViewModel
{
    public class AdminUserVM
    {
        public int AdminId { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
        public int OfficeId { get; set; }
        public string OfficeName { get; set; }
        public string RoleName { get; set; }
    }
}
