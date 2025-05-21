namespace WebAPITest.Models
{
    public class ZaloPayQueryResponse
    {
        public int return_code { get; set; }
        public string? return_message { get; set; }
        public int sub_return_code { get; set; }
        public string? sub_return_message { get; set; }
        public bool is_processing { get; set; }
        public long amount { get; set; }
        public long discount_amount { get; set; }
        public long zp_trans_id { get; set; }

    }
}
