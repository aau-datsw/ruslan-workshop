using Microsoft.EntityFrameworkCore;

namespace MarketAPI.Models
{
    public class MarketAPIContext : DbContext
    {
        public MarketAPIContext(DbContextOptions<MarketAPIContext> options) : base(options) { } 

        public DbSet<Company> Companies { get; set; }
    }
}