using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for Notification
    /// </summary>
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            // Table name
            builder.ToTable("Notification");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(n => n.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(n => n.UserId)
                .HasColumnName("UserId")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(n => n.Title)
                .HasColumnName("Title")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(true)
;
            builder.Property(n => n.Message)
                .HasColumnName("Message")
                .HasColumnType("NTEXT")
                .IsRequired(true)
;
            builder.Property(n => n.Type)
                .HasColumnName("Type")
                .HasColumnType("NOTIFICATIONS_TYPE_ENUM")
                .IsRequired(false)
                .HasDefaultValue(system)
;
            builder.Property(n => n.IsRead)
                .HasColumnName("IsRead")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(false)
;
            builder.Property(n => n.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME2")
                .IsRequired(false)
                .HasDefaultValueSql("GETDATE()")
;
            // Indexes
            // TODO: Add indexes based on your requirements
        }
    }
}
