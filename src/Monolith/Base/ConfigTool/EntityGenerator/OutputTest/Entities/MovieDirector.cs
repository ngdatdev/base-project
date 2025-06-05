using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YourProject.Entities
{
    /// <summary>
    /// Entity class for movie_director table
    /// </summary>
    public class MovieDirector
    {

        public int? MovieId { get; set; }

        public int? DirectorId { get; set; }

        // Navigation Properties
        public Movie Movie { get; set; }
        public Director Director { get; set; }
