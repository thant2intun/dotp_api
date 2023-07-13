using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOTP_BE.Model
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        //[ForeignKey("Menus")]               //very important
        //public int MenuId { get; set; }
        //public virtual Menu Menu { get; set; }   //very important
        public string RoleName { get; set; }
        public virtual List<Menu> Menus { get; set; } = new List<Menu>();
    }
}
