using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Artillery.Data.Models;
using Artillery.Data.Models.Enums;
using Artillery.DataProcessor.ImportDto;
using Castle.Core.Internal;
using Newtonsoft.Json;

namespace Artillery.DataProcessor
{
    using Artillery.Data;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            StringReader reader = new StringReader(xmlString);

            XmlRootAttribute root = new XmlRootAttribute("Countries");

            XmlSerializer serializer = new XmlSerializer(typeof(ImportCountriesDto[]), root);

            ImportCountriesDto[] imported = (ImportCountriesDto[])serializer.Deserialize(reader);

            List<Country> valid = new List<Country>();

            foreach (var cDto in imported)
            {
                if (!IsValid(cDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (cDto.CountryName.IsNullOrEmpty())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = new Country()
                {

                    CountryName = cDto.CountryName,
                    ArmySize = cDto.ArmySize
                };

                valid.Add(country);

                sb.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }

            context.Countries.AddRange(valid);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            StringReader reader = new StringReader(xmlString);

            XmlRootAttribute root = new XmlRootAttribute("Manufacturers");

            XmlSerializer serializer = new XmlSerializer(typeof(ImportManufacturerDto[]), root);

            ImportManufacturerDto[] imported = (ImportManufacturerDto[])serializer.Deserialize(reader);

            List<Manufacturer> valid = new List<Manufacturer>();

            foreach (var mDto in imported)
            {
                if (!IsValid(mDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (mDto.ManufacturerName.IsNullOrEmpty())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (mDto.Founded.IsNullOrEmpty())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Manufacturer manu = new Manufacturer()
                {
                    ManufacturerName = mDto.ManufacturerName,
                    Founded = mDto.Founded
                };

                if (valid.FirstOrDefault(x => x.ManufacturerName == manu.ManufacturerName) != null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                valid.Add(manu);

                string[] output = manu.Founded.Split(", ");
                string result = output[output.Length - 2] + ", " + output[output.Length - 1];

                sb.AppendLine(string.Format(SuccessfulImportManufacturer, manu.ManufacturerName, result));
            }

            context.Manufacturers.AddRange(valid);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlSerializer serializer = new XmlSerializer(typeof(ImportShellDto[]), new XmlRootAttribute("Shells"));

            ImportShellDto[] imported = (ImportShellDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Shell> validShells = new List<Shell>();

            foreach (var shellDto in imported)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell newShell = new Shell()
                {
                    ShellWeight = shellDto.ShellWeight,
                    Caliber = shellDto.Caliber
                };

                validShells.Add(newShell);

                sb.AppendLine(string.Format(SuccessfulImportShell, newShell.Caliber, newShell.ShellWeight));
            }

            context.Shells.AddRange(validShells);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();
            
            ImportGunDto[] imported = JsonConvert.DeserializeObject<ImportGunDto[]>(jsonString);

            List<Gun> validGuns = new List<Gun>();

            foreach (var gunDto in imported)
            {
                if (!IsValid(gunDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                
                List<string> validation = new List<string>()
                {
                    "Howitzer",
                    "Mortar",
                    "FieldGun",
                    "AntiAircraftGun",
                    "MountainGun",
                    "AntiTankGun"
                };

                if (!validation.Any(x => x.Equals(gunDto.GunType)))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Gun newGun = new Gun()
                {
                    ManufacturerId = gunDto.ManufacturerId,
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    Range = gunDto.Range,
                    GunType = (GunType)Enum.Parse(typeof(GunType), gunDto.GunType),
                    ShellId = gunDto.ShellId
                };

                if (gunDto.NumberBuild != null)
                {
                    newGun.NumberBuild = gunDto.NumberBuild;
                }

                foreach (var countryId in gunDto.Countries)
                {

                    newGun.CountriesGuns.Add(new CountryGun()
                    {
                        Gun = newGun,
                        CountryId = countryId.Id
                    });
                }

                validGuns.Add(newGun);
                sb.AppendLine(string.Format(SuccessfulImportGun, newGun.GunType.ToString(), newGun.GunWeight,
                    newGun.BarrelLength));
            }

            context.Guns.AddRange(validGuns);
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