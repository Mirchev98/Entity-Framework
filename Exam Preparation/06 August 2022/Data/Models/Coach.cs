using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footballers.Data.Models
{
    public class Coach
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(CommonRestrictions.footballerNameMinLen)]
        [MaxLength(CommonRestrictions.footballerNameMaxLen)]
        public string Name { get; set; }

        [Required]
        [MinLength(CommonRestrictions.nationalityMinLen)]
        [MaxLength(CommonRestrictions.nationalityMaxLen)]
        public string Nationality { get; set; }

        public ICollection<Footballer> Footballers { get; set; } = new List<Footballer>();
    }
}
