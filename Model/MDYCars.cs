using System.ComponentModel.DataAnnotations;

namespace DOTP_BE.Model
{
    public class MDYCars
    {
        [Key]
        public int CarId { get; set; }
        public DateTime L_IRG { get; set; }
        public DateTime V_DATE { get; set; }
        public string CANCEL { get; set; }
        public string AIR { get; set; }
        public DateTime I_RG { get; set; }
        public string MAKE_MODEL { get; set; }
        public string WHEEL { get; set; }
        public int M_YEAR { get; set; }
        public string TYPE { get; set; }
        public string TYPE_8 { get; set; }
        public int VEH_WT { get; set; }
        public string PAYLOAD { get; set; }
        public int EP { get; set; }
        public string ENGINE_NO { get; set; }
        public string COLOUR { get; set; }
        public string FUEL { get; set; }
        public string DR { get; set; }
        public string OWNER { get; set; }
        public DateTime D_E { get; set; }
        public string LOCATION { get; set; }
        public string NAME { get; set; }
        public string NRC_NO { get; set; }
        public string HOUSE_NO { get; set; }
        public string RD_ST { get; set; }
        public string QTR { get; set; }
        public string TSP { get; set; }
        public string LXWXH { get; set; }
        public int WB { get; set; }
        public int OH { get; set; }
        public string ENGINE { get; set; }
        public string GEAR { get; set; }
        public string FRAMEH { get; set; }
        public string F_AXLE { get; set; }
        public string B_AXLE { get; set; }
        public string SERVICE { get; set; }
        public string STATUS { get; set; }
        public string Millage { get; set; }
        public string VIC_no { get; set; }
        public string VIC_DE { get; set; }
        public int CYL { get; set; }
        public string m_axle { get; set; }
        public string f_rta { get; set; }
        public string b_rta { get; set; }
        public string d_rta { get; set; }
        public string CypherNo { get; set; }
        public string imgFileLoc { get; set; }
        public string REG_NO { get; set; }
        public string Address { get; set; }
    }
}
