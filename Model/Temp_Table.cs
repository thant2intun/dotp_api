using DOTP_BE.Models;
using System.ComponentModel.DataAnnotations;

namespace DOTP_BE.Model
{
    public class Temp_Table
    {
        [Key]
        public int Temp_Id { get; set; }

        #region *** LicenseOnly ***
        public int LicenseOnlyId { get; set; }
        public LicenseOnly LicenseOnly { get; set; }
        public string Transaction_Id { get; set; } //string before 07/02/2023
        public string ChalenNumber { get; set; }
        public string Status { get;set; }
        public string LicenseNumberLong { get; set; }
        public string NRC_Number { get; set; } // all NRC Number is the same
        public string? L_O_Address { get; set; }
        public string? L_N_Address { get; set; }
        public string? L_O_Township_Name { get; set; }
        public string? L_N_Township_Name { get; set; }
        public string? L_Phone { get; set; }
        public string? L_Fax { get; set; }
        public string? L_AllowBusinessTitle { get; set; }
        public int? L_OtherRegistrationOffice_Id { get; set; }
        public bool? L_IsClosed { get; set; }
        public string FormMode { get; set; }
        public bool? L_IsDeleted { get; set; }
        public string AttachFile_NRC { get; set; }
        public string AttachFile_M10 { get; set; }
        public string? AttachFile_OperatorLicense { get; set; }
        public string AttachFile_Part1 { get; set; }
        public string AttachFile_RecommandDoc1 { get; set; }
        public string AttachFile_RecommandDoc2 { get; set; }
        public string? AttachFile_RecommandDoc3 { get; set; }
        public string? AttachFile_RecommandDoc4 { get; set; }
        public string? AttachFile_RecommandDoc5 { get; set; }
        //public int RegistrationOfficeId { get; set; }
        //public RegistrationOffice RegistrationOffice { get; set; }
        #endregion

        #region *** OperatorDetail ***
        public int? OperatorId { get; set; }
        //public OperatorDetail OperatorDetail { get; set; }
        public string? OperatorName { get; set; }
        public string? O_AllowBusinessTitle { get; set; }
        public string? O_Address { get; set; }
        public DateTime? O_ApplyDate { get; set; }
        public int? O_RegistrationOffice_Id { get; set; }
        public string? O_NRC { get; set; }
        //public int? Applicant_Id { get; set; } // -> for what
        public string? O_Township { get; set; }
        public string? O_Phone { get; set; }
        public string? O_Fax { get; set; }
        public string? O_Email { get; set; }
        public DateTime? O_ExpiredDate { get; set; }
        public int? O_JourneyType_Id { get; set; }
        public int? O_TotalCar { get; set; }
        public string? O_DesiredRoute { get; set; }
        public string? O_Notes { get; set; }
        public int? O_ApplyLicenseType { get; set; }
        public bool? O_IsClosed { get; set; }
        public bool? O_IsDeleted { get; set; }
        #endregion

        #region *** Vehicle ***
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }
        public int ApplicantId { get; set; }
        public string? V_VehicleNumber { get; set; } //From CreateCar
        public string? V_VehicleLineTitle { get; set; }
        public string? V_CarryLogisticType { get; set; }
        public string? V_VehicleLocation { get; set; }
        public string? V_VehicleDesiredRoute { get; set; }
        public string? V_Notes { get; set; }
        public int RefTransactionId { get; set; }
        public string? Triangle { get; set; }
        public string? OwnerBook { get; set; }
        public string? AttachedFile1 { get; set; }
        public string? AttachedFile2 { get; set; }
        public int VehicleWeightId { get; set; }
        public string? V_VehicleType { get; set; }

        public int LicenseTypeId { get; set; }
        public DateTime? ExpiryDate { get; set; }

        #endregion

        #region *** CreateCar ***
        public int? CreateCarId { get; set; }
        public CreateCar CreateCar { get; set; }
        public string? VehicleNumber { get; set; }
        public string? Old_VehicleBrand { get; set; }
        public string? VehicleBrand { get; set; }
        public string? Old_VehicleType { get; set; }
        public string? VehicleType { get; set; }
        public string? Old_VehicleWeight { get; set; }
        public string? VehicleWeight { get; set; }
        public string? Old_VehicleLocation { get; set; }

        public string? VehicleLocation { get; set; }
        public string? Old_VehicleOwnerName { get; set; }
        public string? VehicleOwnerName { get; set; }
        public string? VehicleOwnerNRC { get; set; }
        public string? Old_VehicleOwnerNRC { get; set; }

        public string? Old_VehicleOwnerAddress { get; set; }
        public string? VehicleOwnerAddress { get; set; }
        public bool? C_IsDeleted { get; set; }
        public int? C_PersonInformationId { get; set; }

        #endregion

        public DateTime CreatedDate { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public Temp_Table DeepCopy()
        {
            return new Temp_Table
            {
                LicenseOnlyId = this.LicenseOnlyId,
                Transaction_Id = this.Transaction_Id,
                ChalenNumber = this.ChalenNumber,
                LicenseNumberLong = this.LicenseNumberLong,
                NRC_Number = this.NRC_Number,
                AttachFile_NRC = this.AttachFile_NRC,
                AttachFile_M10 = this.AttachFile_M10,
                AttachFile_Part1 = this.AttachFile_Part1,
                AttachFile_RecommandDoc1 = this.AttachFile_RecommandDoc1,
                AttachFile_RecommandDoc2 = this.AttachFile_RecommandDoc2,
                FormMode = this.FormMode,
                CreatedDate = this.CreatedDate,
                L_O_Address = this.L_O_Address,
                L_N_Address = this.L_N_Address,
                L_O_Township_Name= this.L_O_Township_Name,
                L_N_Township_Name  = this.L_N_Township_Name,
                ExpiryDate = this.ExpiryDate,
                Status = this.Status,
            };
        }
    }
}
