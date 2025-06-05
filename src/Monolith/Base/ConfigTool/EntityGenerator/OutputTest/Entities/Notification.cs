using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourProject.Entities
{
    /// <summary>
    /// Entity class for notification table
    /// </summary>
    public class Notification
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        public object? Type { get; set; }

        public bool? IsRead { get; set; } = false;

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        public User User { get; set; }
