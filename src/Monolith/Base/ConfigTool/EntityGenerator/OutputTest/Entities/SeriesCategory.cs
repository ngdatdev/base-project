using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourProject.Entities
{
    /// <summary>
    /// Entity class for series_category table
    /// </summary>
    public class SeriesCategory
    {

        public int? SeriesId { get; set; }

        public int? CategoryId { get; set; }

        // Navigation Properties
        public Series Series { get; set; }
        public Category Category { get; set; }
