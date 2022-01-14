
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.DataProcessor.ExportDto;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            List<Shell> shells = context
                .Shells
                .Include(s => s.Guns)
                .Where(s => s.ShellWeight > shellWeight)
                .ToList();

            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<ArtilleryProfile>()));

            List<ShellJsonExportDto> exports = mapper.Map<List<ShellJsonExportDto>>(shells).OrderBy(x => x.ShellWeight).ToList();

            JsonSerializerSettings set = new JsonSerializerSettings { Formatting = Formatting.Indented };

            string json = JsonConvert.SerializeObject(exports, set);

            return json.Trim();
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            List<Gun> guns = context
                .Guns
                .Include(g => g.Manufacturer)
                .Include(g => g.CountriesGuns)
                .ThenInclude(cg => cg.Country)
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .ToList();

            //guns by BarrelLength (ascending).

            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<ArtilleryProfile>()));

            List<XGunExportXmlDto> exports = mapper.Map<List<XGunExportXmlDto>>(guns).OrderBy(g => g.BarrelLengthV).ToList();

            XmlSerializer serializer = new XmlSerializer(typeof(List<XGunExportXmlDto>), new XmlRootAttribute("Guns"));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            StringBuilder sb = new StringBuilder();

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, exports, ns);
            }
            return sb.ToString().Trim();
        }
    }
}
