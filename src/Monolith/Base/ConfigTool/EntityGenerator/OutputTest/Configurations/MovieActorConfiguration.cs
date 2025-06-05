using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for MovieActor
    /// </summary>
    public class MovieActorConfiguration : IEntityTypeConfiguration<MovieActor>
    {
        public void Configure(EntityTypeBuilder<MovieActor> builder)
        {
            // Table name
            builder.ToTable("MovieActor");


            // Properties Configuration

            builder.Property(m => m.MovieId)
                .HasColumnName("MovieId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            builder.Property(m => m.ActorId)
                .HasColumnName("ActorId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            builder.Property(m => m.CharacterName)
                .HasColumnName("CharacterName")
                .HasColumnType("NVARCHAR(100)")
                .IsRequired(false)
;
            builder.Property(m => m.IsMainCharacter)
                .HasColumnName("IsMainCharacter")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(false)
;
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
