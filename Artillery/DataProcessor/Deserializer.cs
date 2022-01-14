namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;

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
            //Invalid data.
            //Successfully import {countryName} with {armySize} army personnel.

            XmlSerializer serializer = new XmlSerializer(typeof(List<ImportCountryXMLDto>), new XmlRootAttribute("Countries"));
            List<ImportCountryXMLDto> imports = new List<ImportCountryXMLDto>();
            StringBuilder sb = new StringBuilder();

            using (var reader = new StringReader(xmlString))
            {
                imports = (List<ImportCountryXMLDto>)serializer.Deserialize(reader);
            }

            List<Country> countries = new List<Country>();

            foreach (var country in imports)
            {
                if (!IsValid(country))
                {
                    sb.AppendLine("Invalid data.");
                    continue;
                }

                countries.Add(new Country { CountryName = country.CountryName, ArmySize = country.ArmySize });
                sb.AppendLine($"Successfully import {country.CountryName} with {country.ArmySize} army personnel.");
            }

            context.Countries.AddRange(countries);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            //"Invalid data." 
            //Successfully import manufacturer {manufacturerName} founded in {townName, countryName}.

            XmlSerializer serializer = new XmlSerializer(typeof(List<ImportManufacturerXMLDto>), new XmlRootAttribute("Manufacturers"));
            List<ImportManufacturerXMLDto> imports = new List<ImportManufacturerXMLDto>();
            StringBuilder sb = new StringBuilder();

            using (var reader = new StringReader(xmlString))
            {
                imports = (List<ImportManufacturerXMLDto>)serializer.Deserialize(reader);
            }

            List<Manufacturer> manufacturers = new List<Manufacturer>();

            foreach (var manufacturer in imports)
            {
                if (!IsValid(manufacturer))
                {
                    sb.AppendLine("Invalid data.");
                    continue;
                }

                Manufacturer checker = manufacturers.FirstOrDefault(m => m.ManufacturerName == manufacturer.ManufacturerName);
                if (checker != null)
                {
                    sb.AppendLine("Invalid data.");
                    continue;
                }

                string[] countryTown = manufacturer.Founded.Split(", ");
                manufacturers.Add(new Manufacturer { ManufacturerName = manufacturer.ManufacturerName, Founded = manufacturer.Founded });
                sb.AppendLine($"Successfully import manufacturer {manufacturer.ManufacturerName} founded in {countryTown[countryTown.Length - 2]}, {countryTown[countryTown.Length - 1]}.");
            }

            context.Manufacturers.AddRange(manufacturers);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            //Invalid data." 
            //Successfully import shell caliber #{caliber} weight {shellWeigh} kg.
            XmlSerializer serializer = new XmlSerializer(typeof(List<ImportShellDto>), new XmlRootAttribute("Shells"));
            List<ImportShellDto> imports = new List<ImportShellDto>();
            StringBuilder sb = new StringBuilder();

            using (var reader = new StringReader(xmlString))
            {
                imports = (List<ImportShellDto>)serializer.Deserialize(reader);
            }

            List<Shell> shells = new List<Shell>();

            foreach (var shell in imports)
            {
                if (!IsValid(shell))
                {
                    sb.AppendLine("Invalid data.");
                    continue;
                }

                shells.Add(new Shell { ShellWeight = shell.ShellWeight, Caliber = shell.Caliber });
                sb.AppendLine($"Successfully import shell caliber #{shell.Caliber} weight {shell.ShellWeight} kg.");
            }

            context.AddRange(shells);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            List<JsonImportGunDto> imports = JsonConvert.DeserializeObject<List<JsonImportGunDto>>(jsonString);
            StringBuilder sb = new StringBuilder();
            List<Gun> guns = new List<Gun>();

            List<CountryGun> countryGuns = new List<CountryGun>();

            foreach (var gun in imports)
            {
                if (!IsValid(gun))
                {
                    sb.AppendLine("Invalid data.");
                    continue;
                }

                bool isValidGunType = Enum.TryParse<GunType>(gun.GunType, out GunType currentGunType);

                if (!isValidGunType)
                {
                    sb.AppendLine("Invalid data.");
                    continue;
                }

                Gun currentGun = new Gun
                {
                    GunType = currentGunType,
                    BarrelLength = gun.BarrelLength,
                    GunWeight = gun.GunWeight,
                    ManufacturerId = gun.ManufacturerId,
                    NumberBuild = gun.NumberBuild,
                    ShellId = gun.ShellId,
                    Range = gun.Range,
                    CountriesGuns = new List<CountryGun>()
                };
                guns.Add(currentGun);

                foreach (var country in gun.Countries)
                {
                    countryGuns.Add(new CountryGun { CountryId = country.Id, Gun = currentGun });
                }

                sb.AppendLine($"Successfully import gun {currentGun.GunType.ToString()} with a total weight of {currentGun.GunWeight} kg. and barrel length of {currentGun.BarrelLength} m.");
            }

            context.Guns.AddRange(guns);
            context.CountriesGuns.AddRange(countryGuns);
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
