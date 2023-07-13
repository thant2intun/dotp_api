using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DOTP_BE.Migrations
{
    public partial class TT_140623 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminUsers",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminUsers", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Deliveries",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Township_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deliveries", x => x.DeliveryId);
                });

            migrationBuilder.CreateTable(
                name: "JourneyTypes",
                columns: table => new
                {
                    JourneyTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JourneyTypeLong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JourneyTypeShort = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyTypes", x => x.JourneyTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Kala_YgnCars",
                columns: table => new
                {
                    Carid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    REG_NO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MAKE_MODEL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TYPE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VEH_WT = table.Column<int>(type: "int", nullable: false),
                    PAYLOAD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OWNER = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    D_E = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOCATION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NRC_NO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HOUSE_NO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RD_ST = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QTR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TSP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kala_YgnCars", x => x.Carid);
                });

            migrationBuilder.CreateTable(
                name: "LicenseTypes",
                columns: table => new
                {
                    LicenseTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicenseTypeLong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseTypeShort = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseTypes", x => x.LicenseTypeId);
                });

            migrationBuilder.CreateTable(
                name: "MdyCars",
                columns: table => new
                {
                    CarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    L_IRG = table.Column<DateTime>(type: "datetime2", nullable: false),
                    V_DATE = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CANCEL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AIR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    I_RG = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MAKE_MODEL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WHEEL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    M_YEAR = table.Column<int>(type: "int", nullable: false),
                    TYPE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TYPE_8 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VEH_WT = table.Column<int>(type: "int", nullable: false),
                    PAYLOAD = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EP = table.Column<int>(type: "int", nullable: false),
                    ENGINE_NO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    COLOUR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FUEL = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OWNER = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    D_E = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LOCATION = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NRC_NO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HOUSE_NO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RD_ST = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QTR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TSP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LXWXH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WB = table.Column<int>(type: "int", nullable: false),
                    OH = table.Column<int>(type: "int", nullable: false),
                    ENGINE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GEAR = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FRAMEH = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    F_AXLE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    B_AXLE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SERVICE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    STATUS = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Millage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VIC_no = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VIC_DE = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CYL = table.Column<int>(type: "int", nullable: false),
                    m_axle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    f_rta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    b_rta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    d_rta = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CypherNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imgFileLoc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    REG_NO = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MdyCars", x => x.CarId);
                });

            migrationBuilder.CreateTable(
                name: "NRCs",
                columns: table => new
                {
                    NRCId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NRCCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NRCEnglishCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NRCMyanmarCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NRCNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NRCs", x => x.NRCId);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationOffices",
                columns: table => new
                {
                    OfficeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OfficeLongName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OfficeShortName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationOffices", x => x.OfficeId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Townships",
                columns: table => new
                {
                    TownshipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TownshipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TownshipNameEnglish = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TownshipNameMyanmar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Townships", x => x.TownshipId);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Transaction_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChalenNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NRC_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RegistrationCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CertificateFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PartOneFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PartTwoFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TriangleFees = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ModifiedCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCars = table.Column<int>(type: "int", nullable: false),
                    Total_WithoutCertificate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccpectedBy = table.Column<int>(type: "int", nullable: true),
                    AccpectedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PrintedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                });

            migrationBuilder.CreateTable(
                name: "VehicleWeights",
                columns: table => new
                {
                    VehicleWeightId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleWeights", x => x.VehicleWeightId);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    MenuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.MenuId);
                    table.ForeignKey(
                        name: "FK_Menus_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonInformations",
                columns: table => new
                {
                    PersonInformationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NRC_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tsp_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TownshipId = table.Column<int>(type: "int", nullable: true),
                    NRCId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonInformations", x => x.PersonInformationId);
                    table.ForeignKey(
                        name: "FK_PersonInformations_NRCs_NRCId",
                        column: x => x.NRCId,
                        principalTable: "NRCs",
                        principalColumn: "NRCId");
                    table.ForeignKey(
                        name: "FK_PersonInformations_Townships_TownshipId",
                        column: x => x.TownshipId,
                        principalTable: "Townships",
                        principalColumn: "TownshipId");
                });

            migrationBuilder.CreateTable(
                name: "Fees",
                columns: table => new
                {
                    FeesId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationFees = table.Column<int>(type: "int", nullable: false),
                    RegistrationCharges = table.Column<int>(type: "int", nullable: false),
                    CertificateFees = table.Column<int>(type: "int", nullable: false),
                    PartOneFees = table.Column<int>(type: "int", nullable: false),
                    PartTwoFees = table.Column<int>(type: "int", nullable: false),
                    TriangleFees = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MinCars = table.Column<int>(type: "int", nullable: false),
                    MaxCars = table.Column<int>(type: "int", nullable: false),
                    JourneyTypeId = table.Column<int>(type: "int", nullable: false),
                    VehicleWeightId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fees", x => x.FeesId);
                    table.ForeignKey(
                        name: "FK_Fees_JourneyTypes_JourneyTypeId",
                        column: x => x.JourneyTypeId,
                        principalTable: "JourneyTypes",
                        principalColumn: "JourneyTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Fees_VehicleWeights_VehicleWeightId",
                        column: x => x.VehicleWeightId,
                        principalTable: "VehicleWeights",
                        principalColumn: "VehicleWeightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleWeightFees",
                columns: table => new
                {
                    VehicleWeightFeeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegistrationTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OneToFive = table.Column<int>(type: "int", nullable: false),
                    SixToTen = table.Column<int>(type: "int", nullable: false),
                    ElevenToTwenty = table.Column<int>(type: "int", nullable: false),
                    TwentyOneToThirty = table.Column<int>(type: "int", nullable: false),
                    ThirtyOneToFourty = table.Column<int>(type: "int", nullable: false),
                    FourtyOneToHundred = table.Column<int>(type: "int", nullable: false),
                    HundredOneToFiveHundred = table.Column<int>(type: "int", nullable: false),
                    FiveHundredOneToThousand = table.Column<int>(type: "int", nullable: false),
                    ThousandOneAndAbove = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleWeightId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleWeightFees", x => x.VehicleWeightFeeId);
                    table.ForeignKey(
                        name: "FK_VehicleWeightFees_VehicleWeights_VehicleWeightId",
                        column: x => x.VehicleWeightId,
                        principalTable: "VehicleWeights",
                        principalColumn: "VehicleWeightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreateCars",
                columns: table => new
                {
                    CreateCarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleBrand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleWeight = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleOwnerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleOwnerNRC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleOwnerAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    PersonInformationId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreateCars", x => x.CreateCarId);
                    table.ForeignKey(
                        name: "FK_CreateCars_PersonInformations_PersonInformationId",
                        column: x => x.PersonInformationId,
                        principalTable: "PersonInformations",
                        principalColumn: "PersonInformationId");
                });

            migrationBuilder.CreateTable(
                name: "LicenseOnlys",
                columns: table => new
                {
                    LicenseOnlyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Transaction_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    License_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseOwner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NRC_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Township_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowBusinessTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtherRegistrationOffice_Id = table.Column<int>(type: "int", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: true),
                    FormMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    AttachFile_NRC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachFile_M10 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachFile_OperatorLicense = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachFile_Part1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachFile_RecommandDoc1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachFile_RecommandDoc2 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachFile_RecommandDoc3 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachFile_RecommandDoc4 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AttachFile_RecommandDoc5 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationOfficeId = table.Column<int>(type: "int", nullable: false),
                    JourneyTypeId = table.Column<int>(type: "int", nullable: false),
                    DeliveryId = table.Column<int>(type: "int", nullable: true),
                    PersonInformationId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicenseOnlys", x => x.LicenseOnlyId);
                    table.ForeignKey(
                        name: "FK_LicenseOnlys_Deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "Deliveries",
                        principalColumn: "DeliveryId");
                    table.ForeignKey(
                        name: "FK_LicenseOnlys_JourneyTypes_JourneyTypeId",
                        column: x => x.JourneyTypeId,
                        principalTable: "JourneyTypes",
                        principalColumn: "JourneyTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseOnlys_PersonInformations_PersonInformationId",
                        column: x => x.PersonInformationId,
                        principalTable: "PersonInformations",
                        principalColumn: "PersonInformationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LicenseOnlys_RegistrationOffices_RegistrationOfficeId",
                        column: x => x.RegistrationOfficeId,
                        principalTable: "RegistrationOffices",
                        principalColumn: "OfficeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NRC_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NRCId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsConfirm = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PersonInformationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_NRCs_NRCId",
                        column: x => x.NRCId,
                        principalTable: "NRCs",
                        principalColumn: "NRCId");
                    table.ForeignKey(
                        name: "FK_Users_PersonInformations_PersonInformationId",
                        column: x => x.PersonInformationId,
                        principalTable: "PersonInformations",
                        principalColumn: "PersonInformationId");
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Transaction_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChalenNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NRC_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicantId = table.Column<int>(type: "int", nullable: false),
                    License_Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseNumberLong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleLineTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarryLogisticType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleLocation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleDesiredRoute = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertificatePrinted = table.Column<bool>(type: "bit", nullable: false),
                    Part1Printed = table.Column<bool>(type: "bit", nullable: false),
                    Part2Printed = table.Column<bool>(type: "bit", nullable: false),
                    TrianglePrinted = table.Column<bool>(type: "bit", nullable: false),
                    ApplyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    FormMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefTransactionId = table.Column<int>(type: "int", nullable: false),
                    Triangle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerBook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachedFile1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttachedFile2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LicenseTypeId = table.Column<int>(type: "int", nullable: false),
                    VehicleWeightId = table.Column<int>(type: "int", nullable: false),
                    CreateCarId = table.Column<int>(type: "int", nullable: false),
                    LicenseOnlyId = table.Column<int>(type: "int", nullable: true),
                    OperatorId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_Vehicles_CreateCars_CreateCarId",
                        column: x => x.CreateCarId,
                        principalTable: "CreateCars",
                        principalColumn: "CreateCarId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_LicenseOnlys_LicenseOnlyId",
                        column: x => x.LicenseOnlyId,
                        principalTable: "LicenseOnlys",
                        principalColumn: "LicenseOnlyId");
                    table.ForeignKey(
                        name: "FK_Vehicles_LicenseTypes_LicenseTypeId",
                        column: x => x.LicenseTypeId,
                        principalTable: "LicenseTypes",
                        principalColumn: "LicenseTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleWeights_VehicleWeightId",
                        column: x => x.VehicleWeightId,
                        principalTable: "VehicleWeights",
                        principalColumn: "VehicleWeightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperatorDetails",
                columns: table => new
                {
                    OperatorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Transaction_Id = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseHolderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AllowBusinessTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplyDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LicenseOwner = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegistrationOffice_Id = table.Column<int>(type: "int", nullable: false),
                    NRC = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    applicant_Id = table.Column<int>(type: "int", nullable: false),
                    Township = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    JourneyType_Id = table.Column<int>(type: "int", nullable: false),
                    TotalCar = table.Column<int>(type: "int", nullable: false),
                    DesiredRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplyLicenseType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsClosed = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    FormMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PersonInformationId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperatorDetails", x => x.OperatorId);
                    table.ForeignKey(
                        name: "FK_OperatorDetails_PersonInformations_PersonInformationId",
                        column: x => x.PersonInformationId,
                        principalTable: "PersonInformations",
                        principalColumn: "PersonInformationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperatorDetails_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CreateCars_PersonInformationId",
                table: "CreateCars",
                column: "PersonInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_JourneyTypeId",
                table: "Fees",
                column: "JourneyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_VehicleWeightId",
                table: "Fees",
                column: "VehicleWeightId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseOnlys_DeliveryId",
                table: "LicenseOnlys",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseOnlys_JourneyTypeId",
                table: "LicenseOnlys",
                column: "JourneyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseOnlys_PersonInformationId",
                table: "LicenseOnlys",
                column: "PersonInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_LicenseOnlys_RegistrationOfficeId",
                table: "LicenseOnlys",
                column: "RegistrationOfficeId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_RoleId",
                table: "Menus",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_OperatorDetails_PersonInformationId",
                table: "OperatorDetails",
                column: "PersonInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_OperatorDetails_VehicleId",
                table: "OperatorDetails",
                column: "VehicleId",
                unique: true,
                filter: "[VehicleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PersonInformations_NRCId",
                table: "PersonInformations",
                column: "NRCId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonInformations_TownshipId",
                table: "PersonInformations",
                column: "TownshipId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NRCId",
                table: "Users",
                column: "NRCId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PersonInformationId",
                table: "Users",
                column: "PersonInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CreateCarId",
                table: "Vehicles",
                column: "CreateCarId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_LicenseOnlyId",
                table: "Vehicles",
                column: "LicenseOnlyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_LicenseTypeId",
                table: "Vehicles",
                column: "LicenseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleWeightId",
                table: "Vehicles",
                column: "VehicleWeightId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleWeightFees_VehicleWeightId",
                table: "VehicleWeightFees",
                column: "VehicleWeightId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminUsers");

            migrationBuilder.DropTable(
                name: "Fees");

            migrationBuilder.DropTable(
                name: "Kala_YgnCars");

            migrationBuilder.DropTable(
                name: "MdyCars");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "OperatorDetails");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VehicleWeightFees");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "CreateCars");

            migrationBuilder.DropTable(
                name: "LicenseOnlys");

            migrationBuilder.DropTable(
                name: "LicenseTypes");

            migrationBuilder.DropTable(
                name: "VehicleWeights");

            migrationBuilder.DropTable(
                name: "Deliveries");

            migrationBuilder.DropTable(
                name: "JourneyTypes");

            migrationBuilder.DropTable(
                name: "PersonInformations");

            migrationBuilder.DropTable(
                name: "RegistrationOffices");

            migrationBuilder.DropTable(
                name: "NRCs");

            migrationBuilder.DropTable(
                name: "Townships");
        }
    }
}
