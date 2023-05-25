using Trucks.Data.Models;
using Trucks.DataProcessor.ExportDto;

namespace Trucks
{
    using AutoMapper;

    public class TrucksProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
        public TrucksProfile()
        {
            CreateMap<Truck, ExportTrucks>()
                .ForMember(x => x.RegistrationNumber, m => m.MapFrom(t => t.RegistrationNumber))
                .ForMember(x => x.Make, m => m.MapFrom(t => t.MakeType.ToString()));

            CreateMap<Despatcher, ExportDespatcher>()
                .ForMember(x => x.Name, m => m.MapFrom(d => d.Name))
                .ForMember(x => x.Trucks,
                    m => m.MapFrom(d => d.Trucks.ToArray().OrderBy(z => z.RegistrationNumber).ToArray()))
                .ForMember(x => x.TrucksCount, m => m.MapFrom(d => d.Trucks.Count));
        }
    }
}
