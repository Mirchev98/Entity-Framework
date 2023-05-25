using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trucks.Shared;

namespace Trucks.DataProcessor.ImportDto
{
    public class ImportClientsDto
    {
        [Required]
        [MinLength(GlobalConstraints.NameMin)]
        [MaxLength(GlobalConstraints.NameMax)]
        public string Name { get; set; }

        [Required]
        [MinLength(GlobalConstraints.NationalityMin)]
        [MaxLength(GlobalConstraints.NationalityMax)]
        public string Nationality { get; set; }

        [Required]
        public string Type { get; set; }
        public int[] Trucks { get; set; }
    }
}
