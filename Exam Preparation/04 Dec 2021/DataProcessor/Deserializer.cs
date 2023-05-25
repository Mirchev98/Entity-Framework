using System.Globalization;
using System.Text;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Newtonsoft.Json;
using Theatre.Data.Models.Enums;
using Theatre.DataProcessor.ImportDto;

namespace Theatre.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models;
    using Theatre.Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";



        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();
            
            XmlSerializer serializer = new XmlSerializer(typeof(ImportPlayDto[]), new XmlRootAttribute("Plays"));

            StringReader reader = new StringReader(xmlString);

            ImportPlayDto[] imported = (ImportPlayDto[])serializer.Deserialize(reader);

            List<Play> valid = new List<Play>();

            foreach (var playDto in imported)
            {
                string[] validGenre = new string[] { "Drama", "Comedy", "Romance", "Musical" };

                var minimumTime = new TimeSpan(1, 0, 0);

                var currentTime = TimeSpan.ParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture);

                if (!IsValid(playDto) || (currentTime < minimumTime) || !validGenre.Contains(playDto.Genre))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Play newPlay = new Play()
                {
                    Title = playDto.Title,
                    Duration = TimeSpan.ParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture),
                    Rating = playDto.Rating,
                    Genre = (Genre)Enum.Parse(typeof(Genre), playDto.Genre),
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter
                };

                valid.Add(newPlay);

                sb.AppendLine(string.Format(SuccessfulImportPlay, newPlay.Title, newPlay.Genre.ToString(), newPlay.Rating));
            }

            context.Plays.AddRange(valid);

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(ImportCastDto[]), new XmlRootAttribute("Casts"));

            ImportCastDto[] imported = (ImportCastDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Cast> valid = new List<Cast>();

            foreach (var castDto in imported)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast newCast = new Cast()
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId
                };

                valid.Add(newCast);

                sb.AppendLine(string.Format(SuccessfulImportActor, newCast.FullName,
                    newCast.IsMainCharacter ? "main" : "lesser"));
            }

            context.Casts.AddRange(valid);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            ImportTheatreTicketsDto[] imported = JsonConvert.DeserializeObject<ImportTheatreTicketsDto[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Theatre> valid = new List<Theatre>();

            foreach (var theatreDto in imported)
            {
                if (!IsValid(theatreDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre newTheatre = new Theatre()
                {
                    Name = theatreDto.Name,
                    Director = theatreDto.Director,
                    NumberOfHalls = theatreDto.NumberOfHalls
                };

                valid.Add(newTheatre);

                foreach (var ticketDto in theatreDto.Tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Ticket newTicket = new Ticket()
                    {
                        Price = ticketDto.Price,
                        RowNumber = ticketDto.RowNumber,
                        PlayId = ticketDto.PlayId
                    };

                    newTheatre.Tickets.Add(newTicket);
                }

                sb.AppendLine(string.Format(SuccessfulImportTheatre, newTheatre.Name, newTheatre.Tickets.Count));
            }

            context.Theatres.AddRange(valid);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
