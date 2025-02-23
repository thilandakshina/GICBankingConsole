using AutoMapper;
using GICBankingSystem.Core.Application.DTOs;
using GICBankingSystem.Core.Domain.Enums;
using GICBankingSystem.Core.Domain.Models;

namespace GICBankingSystem.Core.Application.Mappings;

[ExcludeFromCodeCoverage]
public class TransactionMappingProfile : Profile
{
    public TransactionMappingProfile()
    {
        CreateMap<TransactionEntity, StatementLineDto>()
            .ForMember(dest => dest.Type,
                opt => opt.MapFrom(src => src.Type == TransactionType.Deposit ? "D" : "W"))
            .ForMember(dest => dest.TransactionId,
                opt => opt.MapFrom(src => src.TransactionId))
            .ForMember(dest => dest.Amount,
                opt => opt.MapFrom(src => src.Amount))
            .ForMember(dest => dest.CreatedDate,
                opt => opt.MapFrom(src => src.CreatedDate));

        CreateMap<TransactionDto, TransactionEntity>()
            .ForMember(dest => dest.Type,
                opt => opt.MapFrom(src => src.Type.ToUpper() == "D" ?
                    TransactionType.Deposit : TransactionType.Withdrawal))
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => Guid.NewGuid()));
    }
}
