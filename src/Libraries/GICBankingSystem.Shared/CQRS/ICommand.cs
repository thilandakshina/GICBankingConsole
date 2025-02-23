using MediatR;

namespace GICBankingSystem.Shared.CQRS;

public interface ICommand : ICommand<Unit>
{ 

}
public interface ICommand<out TResponse> : IRequest<TResponse>
{

}

