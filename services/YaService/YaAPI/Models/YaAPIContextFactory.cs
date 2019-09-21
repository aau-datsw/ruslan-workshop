using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace YaAPI.Models
{
    public class YaAPIContextFactory : IDesignTimeDbContextFactory<YaAPIContext>
    {
        public YaAPIContext CreateDbContext(string[] args)
        {
            return new YaAPIContext(new DbContextOptionsBuilder<YaAPIContext>()
                .UseNpgsql(Startup.Configuration.GetConnectionString("MigrationConnection"))
                .Options);
        }
    }
}