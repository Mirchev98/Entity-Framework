﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Shell")]
    public class ImportShellDto
    {
        [XmlElement("ShellWeight")]
        [Range(2, 1_680)]
        [Required]
        public double ShellWeight { get; set; } 

        [XmlElement("Caliber")]
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Caliber { get; set; } 
    }
}