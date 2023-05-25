using Artillery.Data.Models;
using Artillery.DataProcessor.ExportDto;

namespace Artillery
{
    using AutoMapper;

    public class ArtilleryProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public ArtilleryProfile()
        {
            CreateMap<Country, ExportGunsCountriesDto>();

            CreateMap<Gun, ExportGunsDto>()
                .ForMember(x => x.Countries, m => m.MapFrom(c => c.CountriesGuns
                    .Where(a => a.Country.ArmySize > 4500000)
                    .Select(c => new{ c.Country.CountryName, c.Country.ArmySize })
                    .OrderBy(x => x.ArmySize)));
        }
    }
}