using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for MovieCategory
    /// </summary>
    public class MovieCategoryConfiguration : IEntityTypeConfiguration<MovieCategory>
    {
        public void Configure(EntityTypeBuilder<MovieCategory> builder)
        {
            // Table name
            builder.ToTable("MovieCategory");


            // Properties Configuration

            builder.Property(m => m.MovieId)
                .HasColumnName("MovieId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            builder.Property(m => m.CategoryId)
                .HasColumnName("CategoryId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
