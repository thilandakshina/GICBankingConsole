using AutoMapper;
using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Core.Domain.Models;

namespace GICBankingSystem.Core.Application.Mappings;

[ExcludeFromCodeCoverage]
public class InterestMappingProfile : Profile
{
    public InterestMappingProfile()
    {
        CreateMap<InterestEntity, InterestRuleDto>();
        CreateMap<InterestRuleDto, InterestEntity>();
    }
}
