using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourProject.Entities
{
    /// <summary>
    /// Entity class for episode_video table
    /// </summary>
    public class EpisodeVideo
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int EpisodeId { get; set; }

        [Required]
        public int ServerId { get; set; }

        [Required]
        public int QualityId { get; set; }

        [Required]
        [MaxLength(500)]
        public string VideoUrl { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? SubtitleUrl { get; set; }

        [MaxLength(10)]
        public string? Language { get; set; }

        public bool? IsActive { get; set; } = true;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public Episode Episode { get; set; }
        public Server Server { get; set; }
        public Quality Quality { get; set; }
