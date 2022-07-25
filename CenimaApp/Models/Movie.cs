using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CenimaApp.Models
{
    public partial class Movie
    {
        public Movie()
        {
            Rates = new HashSet<Rate>();
        }

        public int MovieId { get; set; }
        [StringLength(1000, MinimumLength = 1)]
        [Required]
        public string Title { get; set; }
        public int? Year { get; set; }
        public int? GenreId { get; set; }
        [StringLength(1000, MinimumLength = 5)]
        [Required]
        public string Image { get; set; }
        [StringLength(1000, MinimumLength = 10)]
        [Required]
        public string Description { get; set; }

        public virtual Genre Genre { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
    }
}
