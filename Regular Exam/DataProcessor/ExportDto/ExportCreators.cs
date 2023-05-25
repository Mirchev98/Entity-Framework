using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Boardgames.DataProcessor.ImportDto;

namespace Boardgames.DataProcessor.ExportDto
{
    [XmlType("Creator")]
    public class ExportCreators
    {
        [XmlAttribute("BoardgamesCount")]
        public int BoardGamesCount { get; set; }

        [XmlElement("CreatorName")]
        public string Name { get; set; } = null!;

        [XmlArray("Boardgames")]
        public ExportCreatorsBoardgames[] Boardgames { get; set; }
    }
}
