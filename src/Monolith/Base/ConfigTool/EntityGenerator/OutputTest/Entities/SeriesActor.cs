using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourProject.Entities
{
    /// <summary>
    /// Entity class for series_actor table
    /// </summary>
    public class SeriesActor
    {

        public int? SeriesId { get; set; }

        public int? ActorId { get; set; }

        [MaxLength(100)]
        public string? CharacterName { get; set; }

        public bool? IsMainCharacter { get; set; } = false;

        // Navigation Properties
        public Series Series { get; set; }
        public Actor Actor { get; set; }
