using Microsoft.EntityFrameworkCore;

namespace {api_name}.Models
{
    public class {api_name}Context : DbContext
    {
        public {api_name}Context(DbContextOptions<{api_name}Context> options) : base(options) { } 

        public DbSet<Company> Companies { get; set; }
    }
}