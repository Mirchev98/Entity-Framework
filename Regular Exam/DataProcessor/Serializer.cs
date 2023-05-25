using System.Text;
using System.Xml.Serialization;
using Boardgames.DataProcessor.ExportDto;
using Newtonsoft.Json;

namespace Boardgames.DataProcessor
{
    using Boardgames.Data;

    public class Serializer
    {
        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var result = context.Creators
                .Where(x => x.Boardgames.Any())
                .ToArray()
                .Select(c => new ExportCreators
                {
                    BoardGamesCount = c.Boardgames.Count,
                    Name = $"{c.FirstName} {c.LastName}",
                    Boardgames = c.Boardgames
                        .Select(b => new ExportCreatorsBoardgames
                        {
                            Name = b.Name,
                            Year = b.YearPublished
                        }).OrderBy(b => b.Name).ToArray()
                })
                .ToArray()
                .OrderByDescending(c => c.BoardGamesCount)
                .ThenBy(c => c.Name)
                .ToArray();

            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Creators");

            StringWriter writer = new StringWriter(sb);

            XmlSerializer serializer = new XmlSerializer(typeof(ExportCreators[]), root);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            serializer.Serialize(writer, result, namespaces);

            return sb.ToString().TrimEnd();
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellers = context.Sellers
                .Where(s => s.BoardgamesSellers.Any(b => b.Boardgame.YearPublished >= year &&
                                                         b.Boardgame.Rating <= rating))
                .ToArray()
                .Select(x => new
                {
                    Name = x.Name,
                    Website = x.Website,
                    Boardgames = x.BoardgamesSellers.Where(b => b.Boardgame.YearPublished >= year &&
                                                                b.Boardgame.Rating <= rating)
                        .Select(b => new
                        {
                            Name = b.Boardgame.Name,
                            Rating = b.Boardgame.Rating,
                            Mechanics = b.Boardgame.Mechanics,
                            Category = b.Boardgame.CategoryType.ToString()
                        })
                        .OrderByDescending(x => x.Rating)
                        .ThenBy(x => x.Name)
                        .ToArray()

                })
                .OrderByDescending(x => x.Boardgames.Length)
                .ThenBy(x => x.Name)
                .ToArray()
                .Take(5);

            return JsonConvert.SerializeObject(sellers, Formatting.Indented);
        
        }
    }
    
}