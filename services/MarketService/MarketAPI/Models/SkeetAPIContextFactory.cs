using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace SkeetAPI.Models
{
    public class SkeetAPIContextFactory : IDesignTimeDbContextFactory<SkeetAPIContext>
    {
        public SkeetAPIContext CreateDbContext(string[] args)
        {
            return new SkeetAPIContext(new DbContextOptionsBuilder<SkeetAPIContext>()
                .UseNpgsql(Startup.Configuration.GetConnectionString("MigrationConnection"))
                .Options);
        }
    }
}