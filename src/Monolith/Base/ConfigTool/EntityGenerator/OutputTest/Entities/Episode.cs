using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourProject.Entities
{
    /// <summary>
    /// Entity class for episode table
    /// </summary>
    public class Episode
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int SeriesId { get; set; }

        [Required]
        public int SeasonNumber { get; set; }

        [Required]
        public int EpisodeNumber { get; set; }

        [MaxLength(255)]
        public string? Title { get; set; }

        public string? Description { get; set; }

        public int? Duration { get; set; }

        public DateTime? AirDate { get; set; }

        public int? ViewCount { get; set; } = 0;

        public bool? IsPremium { get; set; } = false;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public Series Series { get; set; }
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
