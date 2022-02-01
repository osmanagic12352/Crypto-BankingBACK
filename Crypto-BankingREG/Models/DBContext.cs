using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto_BankingREG.Models
{
    public class DBContext : IdentityDbContext<ApplicationUser>

    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TransakcijaModel>()
                .HasOne(b => b.Uplata)
                .WithMany(ba => ba.Transakcije)
                .HasForeignKey(bi => bi.UplataId);

            builder.Entity<TransakcijaModel>()
                .HasOne(b => b.User)
                .WithMany(ba => ba.Transakcije)
                .HasForeignKey(bi => bi.UserId);

            builder.Entity<PaymentDetail>()
                .HasOne(b => b.User)
                .WithOne(ba => ba.PaymentDetail)
                .HasForeignKey<PaymentDetail>(b => b.UserId);
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<TransakcijaModel> Transakcija { get; set; }
    }
}
