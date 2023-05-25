using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.DataProcessor.ImportDto
{
    public class ImportTeamsDto
    {
        [Required]
        [MinLength(CommonRestrictions.teamNameMinLen)]
        [MaxLength(CommonRestrictions.teamNameMaxLen)]
        [RegularExpression(CommonRestrictions.teamNameRegex)]
        public string Name { get; set; }

        [Required]
        [MinLength(CommonRestrictions.nationalityMinLen)]
        [MaxLength(CommonRestrictions.nationalityMaxLen)]
        public string Nationality { get; set; }

        [Required]
        public int Trophies { get; set; }

        [Required]
        public int[] Footballers { get; set; }
    }
}
