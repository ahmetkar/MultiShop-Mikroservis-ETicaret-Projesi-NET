using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MultiShop.Payment.DAL.Entities;

namespace MultiShop.Payment.DAL.Context
{
    public class PaymentContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private string _connectionString;
        public PaymentContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<PaymentInfo> PaymentInfos { get; set; }
        public DbSet<CardInfo> CardInfos { get; set; }

    }
}
