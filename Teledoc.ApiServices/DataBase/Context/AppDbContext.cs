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


        }

        public DbSet<Founder> Founders { get; set; }
    }
}
