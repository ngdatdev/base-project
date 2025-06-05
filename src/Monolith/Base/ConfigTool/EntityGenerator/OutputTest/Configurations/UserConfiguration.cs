using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YourProject.Entities;

namespace YourProject.Data.Configurations
{
    /// <summary>
    /// Entity Configuration for User
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table name
            builder.ToTable("User");

            // Primary Key
            builder.HasKey(e => e.Id);

            // Properties Configuration

            builder.Property(u => u.Id)
                .HasColumnName("Id")
                .HasColumnType("INT")
                .IsRequired(true)
;
            builder.Property(u => u.Username)
                .HasColumnName("Username")
                .HasColumnType("NVARCHAR(50)")
                .IsRequired(true)
;
            builder.Property(u => u.Email)
                .HasColumnName("Email")
                .HasColumnType("NVARCHAR(100)")
                .IsRequired(true)
;
            builder.Property(u => u.PasswordHash)
                .HasColumnName("PasswordHash")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(true)
;
            builder.Property(u => u.FirstName)
                .HasColumnName("FirstName")
                .HasColumnType("NVARCHAR(50)")
                .IsRequired(false)
;
            builder.Property(u => u.LastName)
                .HasColumnName("LastName")
                .HasColumnType("NVARCHAR(50)")
                .IsRequired(false)
;
            builder.Property(u => u.DateOfBirth)
                .HasColumnName("DateOfBirth")
                .HasColumnType("DATE")
                .IsRequired(false)
;
            builder.Property(u => u.Phone)
                .HasColumnName("Phone")
                .HasColumnType("NVARCHAR(15)")
                .IsRequired(false)
;
            builder.Property(u => u.AvatarUrl)
                .HasColumnName("AvatarUrl")
                .HasColumnType("NVARCHAR(255)")
                .IsRequired(false)
;
            builder.Property(u => u.SubscriptionType)
                .HasColumnName("SubscriptionType")
                .HasColumnType("USERS_SUBSCRIPTION_TYPE_ENUM")
                .IsRequired(false)
                .HasDefaultValue(free)
;
            builder.Property(u => u.SubscriptionStartDate)
                .HasColumnName("SubscriptionStartDate")
                .HasColumnType("DATE")
                .IsRequired(false)
;
            builder.Property(u => u.SubscriptionEndDate)
                .HasColumnName("SubscriptionEndDate")
                .HasColumnType("DATE")
                .IsRequired(false)
;
            builder.Property(u => u.IsActive)
                .HasColumnName("IsActive")
                .HasColumnType("BIT")
                .IsRequired(false)
                .HasDefaultValue(true)
;
            builder.Property(u => u.CreatedAt)
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME2")
                .IsRequired(false)
                .HasDefaultValueSql("GETDATE()")
;
            builder.Property(u => u.UpdatedAt)
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
