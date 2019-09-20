using Microsoft.EntityFrameworkCore;

namespace SkeetAPI.Models
{
    public class SkeetAPIContext : DbContext
    {
        public SkeetAPIContext(DbContextOptions<SkeetAPIContext> options) : base(options) { } 

        public DbSet<Company> Companies { get; set; }
    }
}