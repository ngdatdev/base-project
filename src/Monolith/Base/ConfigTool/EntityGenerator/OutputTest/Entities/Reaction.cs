using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourProject.Entities
{
    /// <summary>
    /// Entity class for reaction table
    /// </summary>
    public class Reaction
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CommentId { get; set; }

        [Required]
        public object ReactionType { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public User User { get; set; }
        public Comment Comment { get; set; }
