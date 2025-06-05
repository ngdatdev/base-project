using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for Reaction
    /// </summary>
    public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            // Table name
            builder.ToTable("Reaction");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(r => r.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(r => r.UserId)
                .HasColumnName("UserId")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(r => r.CommentId)
                .HasColumnName("CommentId")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(r => r.ReactionType)
                .HasColumnName("ReactionType")
                .HasColumnType("REACTIONS_REACTION_TYPE_ENUM")
                .IsRequired(true)
;
            builder.Property(r => r.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME2")
                .IsRequired(false)
                .HasDefaultValueSql("GETDATE()")
;
            builder.Property(r => r.UpdatedAt)
                .HasColumnName("UpdatedAt")
                .HasColumnType("DATETIME2")
                .IsRequired(false)
                .HasDefaultValueSql("GETDATE()")
;
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
