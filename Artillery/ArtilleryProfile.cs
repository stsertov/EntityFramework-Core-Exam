namespace Artillery
{
    using Artillery.Data.Models;
    using Artillery.DataProcessor.ExportDto;
    using AutoMapper;
    using System.Linq;

    class ArtilleryProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public ArtilleryProfile()
        {
            CreateMap<Gun, GunJsonExportDto>()
                .ForMember(x => x.GunType, y => y.MapFrom(g => g.GunType.ToString()))
                .ForMember(x => x.Range, y => y.MapFrom(g => g.Range > 3000 ? "Long-range" : "Regular range"));

            CreateMap<Shell, ShellJsonExportDto>()
                .ForMember(x => x.Guns, y => y
                .MapFrom(s => s.Guns
                .Where(g => g.GunType.ToString() == "AntiAircraftGun")
                .OrderByDescending(g => g.GunWeight)
                .ToArray()));


            CreateMap<Gun, XGunExportXmlDto>()
                .ForMember(x => x.Manufacturer, y => y.MapFrom(g => g.Manufacturer.ManufacturerName))
                .ForMember(x => x.GunType, y => y.MapFrom(g => g.GunType.ToString()))
                .ForMember(x => x.GunWeight, y => y.MapFrom(g => g.GunWeight.ToString()))
                .ForMember(x => x.BarrelLength, y => y.MapFrom(g => g.BarrelLength.ToString()))
                .ForMember(x => x.BarrelLengthV, y => y.MapFrom(g => g.BarrelLength))
                .ForMember(x => x.Range, y => y.MapFrom(g => g.Range.ToString()))
                .ForMember(x => x.Countries, y => y.MapFrom(g => g.CountriesGuns.Select(cg => cg.Country).Where(c => c.ArmySize > 4500000).OrderBy(c => c.ArmySize)));

            CreateMap<Country, XCountryExportXmlDto>()
                .ForMember(x => x.Name, y => y.MapFrom(c => c.CountryName))
                .ForMember(x => x.ArmySizeV, y => y.MapFrom(c => c.ArmySize))
                .ForMember(x => x.ArmySize, y => y.MapFrom(c => c.ArmySize.ToString()));
        }
    }
}