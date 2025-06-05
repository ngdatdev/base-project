using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourProject.Entities
{
    /// <summary>
    /// Entity class for series_director table
    /// </summary>
    public class SeriesDirector
    {

        public int? SeriesId { get; set; }

        public int? DirectorId { get; set; }

        // Navigation Properties
        public Series Series { get; set; }
        public Director Director { get; set; }
