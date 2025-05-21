namespace WebAPITest.Models
{
    public class BankListResponse
    {
        public string? returncode { get; set; }
        public string? returnmessage { get; set; }
        public Dictionary<string, List<BankDTO>>? banks { get; set; }
    }
}
