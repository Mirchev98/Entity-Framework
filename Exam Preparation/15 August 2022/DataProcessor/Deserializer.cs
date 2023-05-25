using System.Text;
using System.Xml.Serialization;
using Castle.Core.Internal;
using Newtonsoft.Json;
using Trucks.Data.Models;
using Trucks.Data.Models.Enums;
using Trucks.DataProcessor.ImportDto;

namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using Data;


    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            StringBuilder sb  = new StringBuilder();
            
            XmlRootAttribute root = new XmlRootAttribute("Despatchers");

            XmlSerializer serializer = new XmlSerializer(typeof(ImportDespatchersDto[]), root);

            using StringReader reader = new StringReader(xmlString);

            ImportDespatchersDto[] imported = (ImportDespatchersDto[])serializer.Deserialize(reader);

            List<Despatcher> valid = new List<Despatcher>();

            foreach (var des in imported)
            {
                if (!IsValid(des))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (des.Position.IsNullOrEmpty())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Despatcher d = new Despatcher
                {
                    Name = des.Name,
                    Position = des.Position
                };

                foreach (var truck in des.Trucks)
                {
                    if (!IsValid(truck))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (truck.VinNumber.IsNullOrEmpty() || truck.RegistrationNumber.IsNullOrEmpty())
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Truck tr = new Truck
                    {
                        RegistrationNumber = truck.RegistrationNumber,
                        VinNumber = truck.VinNumber,
                        TankCapacity = truck.TankCapacity,
                        CargoCapacity = truck.CargoCapacity,
                        CategoryType = (CategoryType)truck.CategoryType,
                        MakeType = (MakeType)truck.MakeType
                    };

                    d.Trucks.Add(tr);
                }

                valid.Add(d);
                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, d.Name, d.Trucks.Count));
            }

            context.Despatchers.AddRange(valid);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            
            var imported = JsonConvert.DeserializeObject<ImportClientsDto[]>(jsonString);

            ICollection<Client> valid = new List<Client>();

            foreach (var t in imported)
            {
                if (!IsValid(t))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client newClient = new Client()
                {
                    Name = t.Name,
                    Nationality = t.Nationality,
                    Type = t.Type
                };

                if (newClient.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                foreach (var id in t.Trucks.Distinct())
                {
                    Truck truck = context.Trucks.Find(id);

                    if (truck == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    newClient.ClientsTrucks.Add(new ClientTruck()
                    {
                        Truck = truck
                    });
                }
                
                valid.Add(newClient);
                sb.AppendLine(string.Format(SuccessfullyImportedClient, newClient.Name, newClient.ClientsTrucks.Count));
            }

            context.Clients.AddRange(valid);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}