using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for SeriesCategory
    /// </summary>
    public class SeriesCategoryConfiguration : IEntityTypeConfiguration<SeriesCategory>
    {
        public void Configure(EntityTypeBuilder<SeriesCategory> builder)
        {
            // Table name
            builder.ToTable("SeriesCategory");


            // Properties Configuration

            builder.Property(s => s.SeriesId)
                .HasColumnName("SeriesId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            builder.Property(s => s.CategoryId)
                .HasColumnName("CategoryId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
