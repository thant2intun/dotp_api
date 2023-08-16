using System.ComponentModel.DataAnnotations;

namespace DOTP_BE.Model
{
    public class Years
    {
        [Key]
        public int YearId { get; set; }
        public int EngYear { get; set; }
        public string? MyanYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
