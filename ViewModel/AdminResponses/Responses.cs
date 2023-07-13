namespace DOTP_BE.ViewModel.AdminResponses
{
    public class Responses
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public string Token { get; set; }
        public List<MenuVM>  MenuList { get; set; } = new List<MenuVM>();
        public AdminUserVM vmAdminUser { get; set; } = new AdminUserVM();
    }
}
