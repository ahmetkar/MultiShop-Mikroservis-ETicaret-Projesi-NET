using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using MultiShop.Payment.DAL.Entities;

namespace MultiShop.Payment.DAL.Context
{
    public class PaymentContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private string _connectionString;
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options)
        {
          
        }
       
        

        public DbSet<PaymentInfo> PaymentInfos { get; set; }
        public DbSet<CardInfo> CardInfos { get; set; }

        public DbSet<PaymentOrderSnapshot> PaymentOrderSnapshots { get; set; }

    }
}
