using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
    [XmlType("Country")]
    public class ImportCountryXMLDto
    {

        [Required]
        [StringLength(60), MinLength(4)]
        [XmlElement("CountryName")]
        public string CountryName { get; set; }

        [Range(50000, 10000000)]
        [XmlElement("ArmySize")]
        public int ArmySize { get; set; }
    }
}
