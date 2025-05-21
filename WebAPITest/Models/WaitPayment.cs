using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPITest.Models
{
    [Table("WAIT_PAYMENT")]
    public class WaitPayment
    {
        [Key]
        [Column("TRANSACTION_UUID")]
        [StringLength(200)]
        public string? TransactionUuid { get; set; }

        [Column("BILL_NO")]
        public int BillNo { get; set; }

        [Column("DEPOSIT_AMT")]
        [DataType(DataType.Currency)]
        public decimal? DepositAmt { get; set; }

        [Column("MOTHER_ACCNT_NO")]
        [StringLength(255)]
        public string? MotherAccntNo { get; set; }

        [Column("MOTHER_ACCNT_OWNER")]
        [StringLength(255)]
        public string? MotherAccntOwner { get; set; }

        [Column("ECOLLECTION_CD")]
        [StringLength(30)]
        public string? EcollectionCd { get; set; }

        [Column("SALE_DATE")]
        public DateTime? SaleDate { get; set; }

        [Column("STATUS")]
        [StringLength(10)]
        public string? Status { get; set; }

        [Column("BILL_SEQ")]
        public int? BillSeq { get; set; }

        [Column("CD_SHOP")]
        [StringLength(10)]
        public string? CdShop { get; set; }

        [Column("POS_NO")]
        [StringLength(10)]
        public string? PosNo { get; set; }

        [Column("POS_CREATE")]
        public int? PosCreate { get; set; }

        [Column("MODIFILE_DATE")]
        public DateTime? ModifileDate { get; set; }

        [Column("INSERT_DATE")]
        public DateTime? InsertDate { get; set; }

        [Column("STATUS_OLD")]
        [StringLength(10)]
        public string? StatusOld { get; set; }

        [Column("QR_DATA")]
        public string? QrData { get; set; }
    }
}
