using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace YeetAPI.Models
{
    public class YeetAPIContextFactory : IDesignTimeDbContextFactory<YeetAPIContext>
    {
        public YeetAPIContext CreateDbContext(string[] args)
        {
            return new YeetAPIContext(new DbContextOptionsBuilder<YeetAPIContext>()
                .UseNpgsql(Startup.Configuration.GetConnectionString("MigrationConnection"))
                .Options);
        }
    }
}