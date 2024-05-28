using BankApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApplication.Data
{
    public class BankDbContext : DbContext
    {

        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
            
        }


        public DbSet<Account> Accounts { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<LegalClientInfo> LegalClientInfos { get; set; }

        public DbSet<PhysClientInfo> PhysClientInfo { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API configuration for Account
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Accounts");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Number).HasColumnName("Number");
                entity.Property(e => e.OpenDate).HasColumnName("OpenDate");
                entity.Property(e => e.OwnerId).HasColumnName("OwnerId");
                entity.Property(e => e.Balance).HasColumnName("Balance");

                entity.HasOne(e => e.Client)
                    .WithMany(c => c.Accounts)
                    .HasForeignKey(e => e.OwnerId);
            });

            // Fluent API configuration for Client
            modelBuilder.Entity<Client>(entity =>
            {
                entity.ToTable("Clients");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Phone).HasColumnName("Phone");
                entity.Property(e => e.Address).HasColumnName("Address");
                entity.Property(e => e.ClientFullName).HasColumnName("ClientFullName");
                entity.Property(e => e.Sex).HasColumnName("Sex");
                entity.Property(e => e.BirthDate).HasColumnName("BirthDate");
                entity.Property(e => e.IsDebtor).HasColumnName("IsDebtor");
            });
        }
    }
}
