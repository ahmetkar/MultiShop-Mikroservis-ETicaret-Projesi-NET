using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MultiShop.Payment.DAL.Context
{
    public class PaymentContextFactory : IDesignTimeDbContextFactory<PaymentContext>
    {
        public PaymentContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PaymentContext>();

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            optionsBuilder.UseSqlServer(
               "Server=localhost,1448;Database=MultiShopPaymentDb;User Id=sa;Password=12345Aa*;TrustServerCertificate=True;"
            );

            return new PaymentContext(optionsBuilder.Options);
        }
    }
}