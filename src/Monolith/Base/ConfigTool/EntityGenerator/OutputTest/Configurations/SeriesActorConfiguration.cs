using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for SeriesActor
    /// </summary>
    public class SeriesActorConfiguration : IEntityTypeConfiguration<SeriesActor>
    {
        public void Configure(EntityTypeBuilder<SeriesActor> builder)
        {
            // Table name
            builder.ToTable("SeriesActor");


            // Properties Configuration

            builder.Property(s => s.SeriesId)
                .HasColumnName("SeriesId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            builder.Property(s => s.ActorId)
                .HasColumnName("ActorId")
                .HasColumnType("INT")
                .IsRequired(false)
;
            builder.Property(s => s.CharacterName)
                .HasColumnName("CharacterName")
                .HasColumnType("NVARCHAR(100)")
                .IsRequired(false)
;
            builder.Property(s => s.IsMainCharacter)
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
