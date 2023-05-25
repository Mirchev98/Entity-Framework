using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.Data.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(CommonRestrictions.teamNameMinLen)]
        [MaxLength(CommonRestrictions.teamNameMaxLen)]
        [RegularExpression(CommonRestrictions.teamNameRegex)]
        public string Name { get; set; }

        [Required]
        [MinLength(CommonRestrictions.nationalityMinLen)]
        [MaxLength(CommonRestrictions.nationalityMaxLen)]
        public string Nationality  { get; set; }

        [Required]
        public int Trophies { get; set; }

        public ICollection<TeamFootballer> TeamsFootballers { get; set; } = new List<TeamFootballer>();
    }
}
