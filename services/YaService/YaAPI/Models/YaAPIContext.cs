using Microsoft.EntityFrameworkCore;

namespace YaAPI.Models
{
    public class YaAPIContext : DbContext
    {
        public YaAPIContext(DbContextOptions<YaAPIContext> options) : base(options) { } 

        public DbSet<Company> Companies { get; set; }
    }
}