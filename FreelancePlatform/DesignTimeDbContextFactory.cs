using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FreelancePlatform
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FreelancePlatformDbContext>
    {
        public FreelancePlatformDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FreelancePlatformDbContext>();
            optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FreelancePlatformDB;Integrated Security=True;");

            return new FreelancePlatformDbContext(optionsBuilder.Options);
        }
    }
}
