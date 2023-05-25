using System.Xml.Serialization;
using Footballers.Data.Models;
using Footballers.DataProcessor.ExportDto;

namespace Footballers
{
    using AutoMapper;

    // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE OR RENAME THIS CLASS
    public class FootballersProfile : Profile
    {
        public FootballersProfile()
        {
            CreateMap<Footballer, ExportFootballersDto>()
                .ForMember(f => f.Position, m => m.MapFrom(t => t.PositionType.ToString()));

            CreateMap<Coach, ExportCoachDto>()
                .ForMember(c => c.FootballersCount, m => m.MapFrom(oc => oc.Footballers.Count))
                .ForMember(c => c.Footballers, m => m.MapFrom(f => f.Footballers.ToArray().OrderBy(x => x.Name).ToArray()));
        }
    }
}
