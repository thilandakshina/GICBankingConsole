using MediatR;

namespace GICBankingSystem.Shared.CQRS;

public interface IQuery<out TResponse> : IRequest<TResponse>
    where TResponse : notnull
{
}
