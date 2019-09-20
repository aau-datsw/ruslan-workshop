using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace {api_name}.Models
{
    public class {api_name}ContextFactory : IDesignTimeDbContextFactory<{api_name}Context>
    {
        public {api_name}Context CreateDbContext(string[] args)
        {
            return new {api_name}Context(new DbContextOptionsBuilder<{api_name}Context>()
                .UseNpgsql(Startup.Configuration.GetConnectionString("MigrationConnection"))
                .Options);
        }
    }
}