using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketAPI.Models
{
  public class MarketAPIContext : DbContext
  {
    public DbSet<Company> Companies { get; set; }

    public MarketAPIContext(DbContextOptions<MarketAPIContext> options) 
    : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);        
        MapCompanies(modelBuilder.Entity<Company>());
    }

    private void MapCompanies(EntityTypeBuilder<Company> entityBuilder)
    {
        entityBuilder.HasKey(x => x.Id);
        entityBuilder.ToTable("company");

        entityBuilder.Property(x => x.Id).HasColumnName("id");
        entityBuilder.Property(x => x.Name).HasColumnName("name");
        entityBuilder.Property(x => x.Price).HasColumnName("price");
        entityBuilder.Property(x => x.Volatility).HasColumnName("volatility");
    }
  }
}