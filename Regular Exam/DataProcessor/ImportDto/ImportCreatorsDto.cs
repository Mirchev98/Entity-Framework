using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Boardgames.DataProcessor.ImportDto
{
    [XmlType("Creator")]
    public class ImportCreatorsDto
    {
        [XmlElement("FirstName")]
        [Required]
        [MaxLength(ValidationConstraints.CreatorFirstNameMax)]
        [MinLength(ValidationConstraints.CreatorFirstNameMin)]
        public string FirstName { get; set; } = null!;

        [XmlElement("LastName")]
        [Required]
        [MaxLength(ValidationConstraints.CreatorLastNameMax)]
        [MinLength(ValidationConstraints.CreatorLastNameMin)]
        public string LastName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public ImportCreatorBoardgamesDto[] Boardgames { get; set; }
    }
}
