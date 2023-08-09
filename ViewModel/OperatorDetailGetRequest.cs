namespace DOTP_BE.ViewModel
{
    public class OperatorDetailGetRequest
    {
        public int userId { get; set; }
        public int operatorId { get; set; }
        public string licenseNumlong { get; set; }
        public int page { get; set; }
        public int countPerPage { get; set; }
    }
}
