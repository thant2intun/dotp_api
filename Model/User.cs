namespace DOTP_BE.Model
{
    public class User
    {
            public int UserId { get; set; }
            public string Name { get; set; }
            public string Password { get; set; }
            public string NRC_Number { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            
            //public string? Role { get; set; }

            public int? NRCId { get; set; }
            public NRC NRC { get; set; }

            public bool? IsActive { get; set; }
            public bool IsConfirm { get; set; }
            public DateTime CreatedAt { get; set; }

            public int? PersonInformationId { get; set; }
            public PersonInformation PersonInformation { get; set; }
    }
}
