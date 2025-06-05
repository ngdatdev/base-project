using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourProject.Entities
{
    /// <summary>
    /// Entity class for comment table
    /// </summary>
    public class Comment
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public int? MovieId { get; set; }

        public int? SeriesId { get; set; }

        public int? EpisodeId { get; set; }

        public int? ParentId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public int? LikeCount { get; set; } = 0;

        public int? DislikeCount { get; set; } = 0;

        public bool? IsApproved { get; set; } = true;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public User User { get; set; }
        public Movie Movie { get; set; }
        public Series Series { get; set; }
        public Episode Episode { get; set; }
        public Parent Parent { get; set; }
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
