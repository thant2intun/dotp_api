using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOTP_BE.Model
{
    public class Menu
    {
        [Key]
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        [ForeignKey("Role")]               //very important
        public int RoleId { get; set; }
        public virtual Role Role { get; set; } = new Role();  //very important
        //   public virtual List<Role> Roles { get; set; }  = new List<Role>();  //detail very important
    }
}
