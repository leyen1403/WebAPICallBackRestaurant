using Microsoft.EntityFrameworkCore;

namespace WebAPITest.Models
{
    public class RestaurantGLBContext : DbContext
    {
        // Constructor nhận vào DbContextOptions để cấu hình
        public RestaurantGLBContext(DbContextOptions<RestaurantGLBContext> options) : base(options)
        {
        }
        public DbSet<ZaloPayCallbackDetail> ZaloPayCallbackDetails { get; set; }
        public DbSet<WaitPayment> WaitPayments { get; set; }
        public DbSet<MerchantAccountsZalopay> MerchantAccountsZalopays { get; set; }
    }
}
