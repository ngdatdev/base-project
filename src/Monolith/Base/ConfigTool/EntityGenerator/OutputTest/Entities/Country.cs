using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourProject.Entities
{
    /// <summary>
    /// Entity class for country table
    /// </summary>
    public class Country
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(5)]
        public string Code { get; set; } = string.Empty;

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
