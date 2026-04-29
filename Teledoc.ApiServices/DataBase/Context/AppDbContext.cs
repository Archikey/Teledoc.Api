using Microsoft.EntityFrameworkCore;
using Teledoc.ApiServices.DataBase.Entities;

namespace Teledoc.ApiServices.DataBase.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.TaxpayerIndividualNumber)
                    .IsRequired()
                    .HasColumnType("varchar(12)");

                entity.Property(x => x.TypeRole)
                    .IsRequired()
                    .HasColumnType("varchar(20)");

                entity.Property(x => x.CreatedAt)
                    .IsRequired();

                entity.Property(x => x.UpdatedAt)
                    .IsRequired();

                entity.HasMany(x => x.Founders)
                    .WithOne(x => x.Client)
                    .HasForeignKey(x => x.ClientId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(x => x.TaxpayerIndividualNumber)
                    .IsUnique();
            });

            modelBuilder.Entity<Founder>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.ClientId)
                    .IsRequired();

                entity.Property(x => x.TaxpayerIndividualNumber)
                    .IsRequired()
                    .HasColumnType("varchar(12)");

                entity.Property(x => x.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.MiddleName)
                    .HasMaxLength(50);

                entity.Property(x => x.CreatedAt)
                    .IsRequired();

                entity.Property(x => x.UpdatedAt)
                    .IsRequired();

                entity.HasIndex(x => new { x.ClientId, x.TaxpayerIndividualNumber })
                    .IsUnique();
            });
        }

        public DbSet<Founder> Founders { get; set; }
        public DbSet<Client> Clients { get; set; }
    }
}
