using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourProject.Entities
{
    /// <summary>
    /// Entity class for movie table
    /// </summary>
    public class Movie
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? OriginalTitle { get; set; }

        [Required]
        [MaxLength(255)]
        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        [MaxLength(255)]
        public string? PosterUrl { get; set; }

        [MaxLength(255)]
        public string? BannerUrl { get; set; }

        [MaxLength(255)]
        public string? TrailerUrl { get; set; }

        public int? Duration { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public object? ImdbRating { get; set; }

        public object? OurRating { get; set; }

        public int? ViewCount { get; set; } = 0;

        public object? Status { get; set; }

        public bool? IsPremium { get; set; } = false;

        public int? CountryId { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public Country Country { get; set; }
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
