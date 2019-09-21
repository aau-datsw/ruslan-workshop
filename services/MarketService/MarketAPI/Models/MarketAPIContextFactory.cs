using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MarketAPI.Models
{
    public class MarketAPIContextFactory : IDesignTimeDbContextFactory<MarketAPIContext>
    {
        public MarketAPIContext CreateDbContext(string[] args)
        {
            return new MarketAPIContext(new DbContextOptionsBuilder<MarketAPIContext>()
                .UseNpgsql(Startup.Configuration.GetConnectionString("MigrationConnection"))
                .Options);
        }
    }
}