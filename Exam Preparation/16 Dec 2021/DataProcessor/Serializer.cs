
using System.Text;
using System.Xml.Serialization;
using Artillery.DataProcessor.ExportDto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Artillery.DataProcessor
{
    using Artillery.Data;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var result = context.Shells
                .Where(x => x.ShellWeight > shellWeight)
                .Include(x => x.Guns)
                .ToArray()
                .Select(x => new ExportShellsDto
                {
                    ShellWeight = x.ShellWeight,
                    Caliber = x.Caliber,
                    Guns = x.Guns
                        .Where(g => g.GunType.ToString() == "AntiAircraftGun")
                        .Select(x => new ExportShellGuns
                        {
                            GunType = x.GunType.ToString(),
                            GunWeight = x.GunWeight,
                            BarrelLength = x.BarrelLength,
                            Range = x.Range > 3000 ? "Long-range" : "Regular range"
                        })
                        .OrderByDescending(x => x.GunWeight)
                        .ToArray()
                })
                .OrderBy(x => x.ShellWeight)
                .ToArray();

            return JsonConvert.SerializeObject(result, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            StringBuilder sb = new StringBuilder();

            StringWriter writer = new StringWriter(sb);

            XmlSerializer serializer = new XmlSerializer(typeof(ExportGunsDto[]), new XmlRootAttribute("Guns"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var result = context.Guns
                .Where(m => m.Manufacturer.ManufacturerName == manufacturer)
                .Select(m => new ExportGunsDto
                {
                    Manufacturer = m.Manufacturer.ManufacturerName,
                    GunType = m.GunType.ToString(),
                    GunWeight = m.GunWeight,
                    BarrelLength = m.BarrelLength,
                    Range = m.Range,
                    Countries = m.CountriesGuns
                        .Where(x => x.Country.ArmySize > 4500000)
                        .Select(c => new ExportGunsCountriesDto
                        {
                            CountryName = c.Country.CountryName,
                            ArmySize = c.Country.ArmySize
                        })
                        .OrderBy(x => x.ArmySize)
                        .ToArray()

                })
                .OrderBy(x => x.BarrelLength)
                .ToArray();

            serializer.Serialize(writer, result, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
