using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPITest.Models
{
    [Table("MERCHANT_ACCOUNTS_ZALOPAY")]
    public class MerchantAccountsZalopay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required]
        [StringLength(50)]
        public string? CD_SHOP { get; set; }

        [Required]
        [StringLength(50)]
        public string? APP_ID { get; set; }

        [Required]
        [StringLength(100)]
        public string? KEY1 { get; set; }

        [Required]
        [StringLength(100)]
        public string? KEY2 { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
