using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MultiShop.Order.Persistance.Context
{
    public class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
    {
        public OrderContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrderContext>();

            optionsBuilder.UseSqlServer(
                "Server=localhost,1440;Database=MultiShopOrderDb;User Id=sa;Password=12345Aa*;TrustServerCertificate=True;"
            );

            return new OrderContext(optionsBuilder.Options);
        }
    }
}