using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
    [XmlType("Country")]
    public class XCountryExportXmlDto
    {
        [XmlAttribute("Country")]
        public string Name { get; set; }


        [XmlAttribute("ArmySize")]
        public string ArmySize { get; set; }

        [XmlIgnore]
        public int ArmySizeV { get; set; }
    }
}
