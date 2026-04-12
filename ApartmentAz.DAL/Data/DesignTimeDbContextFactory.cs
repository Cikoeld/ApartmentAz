using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ApartmentAz.DAL.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;Database=ApartmentAzDb;Trusted_Connection=True;TrustServerCertificate=True;",
                sql => sql.MigrationsAssembly("ApartmentAz.DAL"));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
