using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Theatre.Data.Common;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTheatreTicketsDto
    {
        [Required]
        [MaxLength(Validations.TheatreMaxLenName)]
        [MinLength(Validations.TheatreMinLenName)]
        public string Name { get; set; } = null!;

        [Required]
        [Range(1, 10)]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [MaxLength(Validations.TheatreMaxDirector)]
        [MinLength(Validations.TheatreMinDirector)]
        public string Director { get; set; } = null!;

        public ImportTicketsDto[] Tickets { get; set; }
    }
}
