namespace DOTP_BE.ViewModel.AdminResponses
{
    public class DashboardData
    {
        public Filter filter { get; set; } = new Filter();
        public totalLicense Card_1Value { get; set; } = new totalLicense();
        public List<totalLicense> Card1Value { get; set; }
        public List<totalRCLicense> Card_2Lst { get; set; } = new List<totalRCLicense>();
        public List<totalVal> Card_3Lst_1 { get; set; } = new List<totalVal>();
        public List<totalVal> Card_3Lst_2 { get; set; } = new List<totalVal>();
    }
    public class totalLicense //maw maw
    {
        public int kaOptLicense { get; set; }
        public int kaVehNumber { get; set; }
        public int kaTotalCars { get; set; }
        public int chaOptLicense { get; set; }
        public int chaVehNumber { get; set; }
        public int chaTotalCars { get; set; }
        public int gaOptLicense { get; set; }
        public int gaVehNumber { get; set; }
        public int gaTotalCars { get; set; }
        public int ghaOptLicense { get; set; }
        public int ghaVehNumber { get; set; }
        public int ghaTotalCars { get; set; }
        public int ngaOptLicense { get; set; }
        public int ngaVehNumber { get; set; }
        public int ngaTotalCars { get; set; }
    }

    public class totalRCLicense
    {
        public string name { get; set; }
        public int kaReg { get; set; }
        public int kaRemain { get; set; }
        public int chaReg { get; set; }
        public int chaRemain { get; set; }
        public int gaReg { get; set; }
        public int gaRemain { get; set; }
        public int ghaReg { get; set; }
        public int ghaRemain { get; set; }
        public int ngaReg { get; set; }
        public int ngaRemain { get; set; }
    }

    public class totalVal
    {
        public string name { get; set; }
        public int kaVal { get; set; }
        public int kaVehVal { get; set; }
        public int chaVal { get; set; }
        public int chaVehVal { get; set; }
        public int gaVal { get; set; }
        public int gaVehVal { get; set; }
        public int gaaVal { get; set; }
        public int gaaVehVal { get; set; }
        public int ngaVal { get; set; }
        public int ngaVehVal { get; set; }
        public int conAmt { get; set; }
    }

    public class Prepare
    {
        public string name { get; set; }
        public string kaVal { get; set; }
        public string chaVal { get; set; }
        public string gaVal { get; set; }
        public string gaaVal { get; set; }
        public string ngaVal { get; set; }
        public string conAmt { get; set; }
    }

    public class Filter
    {
        //public int? tId { get; set; } //tzt 300523
        public int officeId { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
    }

    #region tZT update code to reduce code line and complexity 290523
    public class TotalLicense
    {
        public TotalLicense(int totlLicnseKa, int totlLicnseKha, int totlLicnseGa, int totlLicnseGha, int totlLicnseNga, int totalCarKa, int totalCarKha, int totalCarGa, int totalCarGha, int totalCarNga)
        {
            this.totlLicnseKa = totlLicnseKa;
            this.totlLicnseKha = totlLicnseKha;
            this.totlLicnseGa = totlLicnseGa;
            this.totlLicnseGha = totlLicnseGha;
            this.totlLicnseNga = totlLicnseNga;
            this.totalCarKa = totalCarKa;
            this.totalCarKha = totalCarKha;
            this.totalCarGa = totalCarGa;
            this.totalCarGha = totalCarGha;
            this.totalCarNga = totalCarNga;
        }

        public int totlLicnseKa { get; set; }
        public int totlLicnseKha { get; set; }
        public int totlLicnseGa { get; set; }
        public int totlLicnseGha { get; set; }
        public int totlLicnseNga { get; set; }
        public int totalCarKa { get; set; }
        public int totalCarKha { get; set; }
        public int totalCarGa { get; set; }
        public int totalCarGha { get; set; }
        public int totalCarNga { get; set; }

    }

    public class RestLicenseToCheck
    {
        public string name { get; set; }
        public int kaReg { get; set; }
        public int kaRemain { get; set; }
        public int khaReg { get; set; }
        public int khaRemain { get; set; }
        public int gaReg { get; set; }
        public int gaRemain { get; set; }
        public int ghaReg { get; set; }
        public int ghaRemain { get; set; }
        public int ngaReg { get; set; }
        public int ngaRemain { get; set; }
    }

    #endregion

    #region TZT 070623 For 3rd Card of Dashboard
    public class ThirdCardData
    {
        public List<totalVal> Card_3Lst_1 { get; set; } = new List<totalVal>();
        public List<totalVal> Card_3Lst_2 { get; set; } = new List<totalVal>();
        public List<totalVal> Card_3Lst_3 { get; set; } = new List<totalVal>();
        public List<totalVal> Card_3Lst_4 { get; set; } = new List<totalVal>();

    }
    #endregion
}