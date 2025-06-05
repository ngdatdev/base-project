using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourProject.Entities
{
    /// <summary>
    /// Entity class for user table
    /// </summary>
    public class User
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [MaxLength(15)]
        public string? Phone { get; set; }

        [MaxLength(255)]
        public string? AvatarUrl { get; set; }

        public object? SubscriptionType { get; set; }

        public DateTime? SubscriptionStartDate { get; set; }

        public DateTime? SubscriptionEndDate { get; set; }

        public bool? IsActive { get; set; } = true;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<Series> Series { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Director> Directors { get; set; }
        public IEnumerable<Actor> Actors { get; set; }
        public IEnumerable<Server> Servers { get; set; }
        public IEnumerable<Quality> Qualities { get; set; }
        public IEnumerable<Episode> Episodes { get; set; }
        public IEnumerable<User> Users { get; set; }
        public IEnumerable<Parent> Parents { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}
