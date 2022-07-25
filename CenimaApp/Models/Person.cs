using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CenimaApp.Models
{
    public partial class Person
    {
        public Person()
        {
            Rates = new HashSet<Rate>();
        }

        public int PersonId { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required]
        public string Fullname { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Range(1, 2)]
        public int? Type { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<Rate> Rates { get; set; }
    }
}
