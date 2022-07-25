using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CenimaApp.Models
{
    public partial class Rate
    {
        public int MovieId { get; set; }
        public int PersonId { get; set; }
        [StringLength(1000, MinimumLength = 10)]
        [Required]
        public string Comment { get; set; }
        [Range(1, 10)]
        [Required]
        public double? NumericRating { get; set; }
        public DateTime Time { get; set; }

        public virtual Movie Movie { get; set; }
        public virtual Person Person { get; set; }
    }
}
