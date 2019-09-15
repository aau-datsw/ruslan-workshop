using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MarketAPI.Models
{
  public class MarketAPIContext : DbContext
  {
    public DbSet<Person> Persons { get; set; }

    public MarketAPIContext(DbContextOptions<MarketAPIContext> options) 
    : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);        
        MapPersons(modelBuilder.Entity<Person>());
    }

    // Since table names are lowercase but C# properties use PascalCasing, we
    // need to map every property of the Person class to its corresponding column name
    // in the persons table.
    private void MapPersons(EntityTypeBuilder<Person> entityBuilder)
    {
        entityBuilder.HasKey(x => x.Id);
        entityBuilder.ToTable("person");

        entityBuilder.Property(x => x.Id).HasColumnName("id");
        entityBuilder.Property(x => x.Name).HasColumnName("name");
        entityBuilder.Property(x => x.OtherName).HasColumnName("other_name");
    }
  }
}