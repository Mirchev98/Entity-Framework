using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boardgames.DataProcessor.ImportDto
{
    public class ImportSellersDto
    {
        [Required]
        [MaxLength(ValidationConstraints.SellerNameMax)]
        [MinLength(ValidationConstraints.SellerNameMin)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(ValidationConstraints.SellerAddressMax)]
        [MinLength(ValidationConstraints.SellerAddressMin)]
        public string Address { get; set; } = null!;

        [Required]
        public string Country { get; set; } = null!;

        [Required]
        [RegularExpression(ValidationConstraints.WebRegex)]
        public string Website { get; set; } = null!;

        public int[] Boardgames { get; set; }

    }
}
