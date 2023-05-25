using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theatre.Data.Common;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models
{
    public class Play
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(Validations.PlayTitleMax)]
        public string Title { get; set; } = null!;

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public float Rating { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        [MaxLength(Validations.PlayDescriptionMax)]
        public string Description { get; set; } = null!;

        [Required]
        [MaxLength(Validations.PlayScreenwriterMax)]
        public string Screenwriter { get; set; } = null!;

        public ICollection<Cast> Casts { get; set; } = new List<Cast>();

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
