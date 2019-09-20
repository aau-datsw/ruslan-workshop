using Microsoft.EntityFrameworkCore;

namespace YeetAPI.Models
{
    public class YeetAPIContext : DbContext
    {
        public YeetAPIContext(DbContextOptions<YeetAPIContext> options) : base(options) { } 

        public DbSet<Company> Companies { get; set; }
    }
}