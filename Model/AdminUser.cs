using System.ComponentModel.DataAnnotations;

namespace DOTP_BE.Model
{
    public class AdminUser
    {
        [Key]
        public int AdminId { get; set; }
        public string Name { get; set; }
        public int RoleId { get; set; }
        public string Password { get; set; }
        public int OfficeId { get; set; }
    }
}
