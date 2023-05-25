using System.Globalization;
using System.Text;
using System.Xml.Serialization;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Footballers.DataProcessor.ExportDto;
using Newtonsoft.Json;

namespace Footballers.DataProcessor
{
    using Data;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile<FootballersProfile>();
            });

            StringBuilder sb = new StringBuilder();

            var mapper = new Mapper(config);

            XmlRootAttribute root = new XmlRootAttribute("Coaches");

            StringWriter writer = new StringWriter(sb);

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCoachDto[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();

            namespaces.Add(string.Empty, string.Empty);

            ExportCoachDto[] coaches = context.Coaches
                .Where(c => c.Footballers.Count >= 1)
                .ProjectTo<ExportCoachDto>(config)
                .OrderByDescending(x => x.FootballersCount)
                .ThenBy(x => x.Name)
                .ToArray();

            serializer.Serialize(writer, coaches, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var teams = context.Teams
                .Where(x => x.TeamsFootballers.Count > 0
                            && x.TeamsFootballers.Any(f => f.Footballer.ContractStartDate >= date))
                .ToArray()
                .Select(x => new
                {
                    Name = x.Name,
                    Footballers = x.TeamsFootballers
                        .Where(f => f.Footballer.ContractStartDate >= date)
                        .OrderByDescending(x => x.Footballer.ContractEndDate)
                        .ThenBy(x => x.Footballer.Name)
                        .Select(f => new
                    {
                        FootballerName = f.Footballer.Name,
                        ContractStartDate = f.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                        ContractEndDate = f.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                        BestSkillType = f.Footballer.BestSkillType.ToString(),
                        PositionType = f.Footballer.PositionType.ToString()
                    }).ToArray()
                })
                .OrderByDescending(x => x.Footballers.Count())
                .ThenBy(x => x.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
    }
}
