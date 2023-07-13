namespace DOTP_BE.ViewModel
{
    public class MasterCardRes
    {
        public string merchant { get; set; }
        public string result { get; set; }
        public Session session { get; set; }
        public string successIndicator { get; set; }
    }
    public class Session
    {
        public string id { get; set; }
        public string updateStatus { get; set; }
        public string version { get; set; }
    }
}
