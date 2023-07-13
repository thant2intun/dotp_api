using DOTP_BE.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DOTP_BE.Model
{
    public class PersonInformation : BaseModel
    {
        [Key]
        public int PersonInformationId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string NRC_Number { get; set; }
        [Required]
        public string Tsp_Name { get; set; }
        public string Address { get; set; }      
        [Required]
        public string Phone { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public string? Notes { get; set; }
        public string? Status { get; set; }
        [Required]
        public DateTime RegisterDate { get; set; }

        public int? TownshipId { get; set; }
        public Township Township { get; set; }
        public int? NRCId { get; set; }
        public NRC NRC { get; set; }

        public List<CreateCar> CreateCars { get; set; }
        public List<LicenseOnly> LicenseOnlys { get; set; }
        public List<User> Users { get; set; }
        public List<OperatorDetail> OperatorDetails { get; set; }

    }
}
