using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourProject.Entities
{
    /// <summary>
    /// Entity class for rating table
    /// </summary>
    public class Rating
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        public int? MovieId { get; set; }

        public int? SeriesId { get; set; }

        [Required]
        public int Rating { get; set; }

        public string? Review { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public User User { get; set; }
        public Movie Movie { get; set; }
        public Series Series { get; set; }
