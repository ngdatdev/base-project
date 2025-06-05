using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for Director
    /// </summary>
    public class DirectorConfiguration : IEntityTypeConfiguration<Director>
    {
        public void Configure(EntityTypeBuilder<Director> builder)
        {
            // Table name
            builder.ToTable("Director");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(d => d.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(d => d.Name)
                .HasColumnName("Name")
                .HasColumnType("NVARCHAR(100)")
                .IsRequired(true)
;
            builder.Property(d => d.Biography)
                .HasColumnName("Biography")
                .HasColumnType("NTEXT")
                .IsRequired(false)
;
            builder.Property(d => d.DateOfBirth)
                .HasColumnName("DateOfBirth")
                .HasColumnType("DATE")
                .IsRequired(false)
;
            builder.Property(d => d.Nationality)
                .HasColumnName("Nationality")
                .HasColumnType("NVARCHAR(50)")
                .IsRequired(false)
;
            builder.Property(d => d.AvatarUrl)
                .HasColumnName("AvatarUrl")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(false)
;
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
