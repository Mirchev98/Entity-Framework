using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trucks.Shared;

namespace Trucks.DataProcessor.ImportDto
{
    [XmlType("Despatcher")]
    public class ImportDespatchersDto
    {
        [XmlElement("Name")]
        [MinLength(GlobalConstraints.NameMinDespach)]
        [MaxLength(GlobalConstraints.NameMaxDespach)]
        public string Name { get; set; } = null!;

        [XmlElement("Position")]
        public string Position { get; set; }

        [XmlArray("Trucks")]
        public ImportTrucksDto[] Trucks { get; set; }
    }
}
