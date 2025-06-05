using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for Country
    /// </summary>
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            // Table name
            builder.ToTable("Country");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(c => c.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(c => c.Name)
                .HasColumnName("Name")
                .HasColumnType("NVARCHAR(100)")
                .IsRequired(true)
;
            builder.Property(c => c.Code)
                .HasColumnName("Code")
                .HasColumnType("NVARCHAR(5)")
                .IsRequired(true)
;
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
