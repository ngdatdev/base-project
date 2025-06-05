using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourProject.Entities
{
    /// <summary>
    /// Entity class for movie_category table
    /// </summary>
    public class MovieCategory
    {

        public int? MovieId { get; set; }

        public int? CategoryId { get; set; }

        // Navigation Properties
        public Movie Movie { get; set; }
        public Category Category { get; set; }
