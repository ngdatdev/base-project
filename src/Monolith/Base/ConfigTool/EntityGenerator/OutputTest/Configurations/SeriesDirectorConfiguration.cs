using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for SeriesDirector
    /// </summary>
    public class SeriesDirectorConfiguration : IEntityTypeConfiguration<SeriesDirector>
    {
        public void Configure(EntityTypeBuilder<SeriesDirector> builder)
        {
            // Table name
            builder.ToTable("SeriesDirector");


            // Properties Configuration

            builder.Property(s => s.SeriesId)
                .HasColumnName("SeriesId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            builder.Property(s => s.DirectorId)
                .HasColumnName("DirectorId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
