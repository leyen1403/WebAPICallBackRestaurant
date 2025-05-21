using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WebAPITest.Models
{
    [Table("ZaloPay_Callback_Detail")]
    public class ZaloPayCallbackDetail
    {
        [Key]
        [Column("APP_TRANS_ID")]
        [StringLength(256)]
        [JsonProperty("app_trans_id")]
        public string? AppTransId { get; set; }

        [Column("APP_ID")]
        [JsonProperty("app_id")]
        public int AppId { get; set; }

        [Column("APP_TIME")]
        [JsonProperty("app_time")]
        public long AppTime { get; set; }

        [Column("APP_USER")]
        [JsonProperty("app_user")]
        public string? AppUser { get; set; }

        [Column("AMOUNT")]
        [JsonProperty("amount")]
        public long Amount { get; set; }

        [Column("EMBED_DATA")]
        [JsonProperty("embed_data")]
        public string? EmbedData { get; set; }

        [Column("ITEM")]
        [JsonProperty("item")]
        public string? Item { get; set; }

        [Column("ZP_TRANS_ID")]
        [JsonProperty("zp_trans_id")]
        public string? ZpTransId { get; set; }

        [Column("SERVER_TIME")]
        [JsonProperty("server_time")]
        public long ServerTime { get; set; }

        [Column("CHANNEL")]
        [JsonProperty("channel")]
        public int Channel { get; set; }

        [Column("MERCHANT_USER_ID")]
        [JsonProperty("merchant_user_id")]
        public string? MerchantUserId { get; set; }

        [Column("USER_FEE_AMOUNT")]
        [JsonProperty("user_fee_amount")]
        public long UserFeeAmount { get; set; }

        [Column("DISCOUNT_AMOUNT")]
        [JsonProperty("discount_amount")]
        public long DiscountAmount { get; set; }
    }
}
