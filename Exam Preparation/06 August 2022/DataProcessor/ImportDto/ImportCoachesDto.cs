using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Footballers.DataProcessor.ImportDto
{
    [XmlType("Coach")]
    public class ImportCoachesDto
    {
        [XmlElement("Name")]
        [Required]
        [MinLength(CommonRestrictions.footballerNameMinLen)]
        [MaxLength(CommonRestrictions.footballerNameMaxLen)]
        public string Name { get; set; }

        [XmlElement("Nationality")]
        [Required]
        [MinLength(CommonRestrictions.nationalityMinLen)]
        [MaxLength(CommonRestrictions.nationalityMaxLen)]
        public string Nationality { get; set; }

        [XmlArray("Footballers")]
        [XmlArrayItem("Footballer")]
        public ImportFootballersDto[] Footballers { get; set; }
    }
}
