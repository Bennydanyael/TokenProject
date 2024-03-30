using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataProvider.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options) { }

        public DbSet<UserAccount> UserAccounts { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    builder.Entity<UserAccount>()
        //        .HasData(new UserAccount { Id=0, Username = "Admin", Password = "Admin@12345", Role = "SuperAdmin" });
        //}
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string _connectionString = "Server=HALELUYAH\\SQLEXPRESS;Database=TokenProject;Trusted_Connection=False;User ID=sa;Password=Sabeso76;MultipleActiveResultSets=true;TrustServerCertificate=true";
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }

    public class BaseDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer("Server=HALELUYAH\\SQLEXPRESS;Database=BaseProject;Trusted_Connection=False;User ID=sa;Password=Sabeso76;MultipleActiveResultSets=true;TrustServerCertificate=true");

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
